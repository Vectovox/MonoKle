namespace MonoKle.Scripting
{
    using Microsoft.CSharp;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// Compiler for scripts.
    /// </summary>
    public class ScriptCompiler
    {
        private const string NAMESPACE = "MonoKleScriptNamespace";
        private const string EXECUTABLE_METHOD_NAME = "Run";

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptCompiler"/> class.
        /// </summary>
        public ScriptCompiler()
        {
            ReferencedAssemblies = new List<Assembly>();
            ReferencedAssemblies.Add(Assembly.GetAssembly(typeof(Script)));
        }

        /// <summary>
        /// Gets the referenced assemblies.
        /// </summary>
        /// <value>
        /// The referenced assemblies.
        /// </value>
        public List<Assembly> ReferencedAssemblies { get; private set; }

        /// <summary>
        /// Compiles the specified script.
        /// </summary>
        /// <param name="script">The script to compile.</param>
        public void Compile(IScriptCompilable script) => Compile(new List<IScriptCompilable> { script });

        /// <summary>
        /// Compiles the provided scipts.
        /// </summary>
        /// <param name="scripts">The scripts to compile.</param>
        public void Compile(IEnumerable<IScriptCompilable> scripts)
        {
            var provider = new CSharpCodeProvider(new Dictionary<string, string>
            { { "CompilerVersion", "v4.0" } }
            );
            CompilerParameters compilerParameters = MakeCompilerParameters();

            foreach (IScriptCompilable script in scripts)
            {
                script.CompilationDate = DateTime.UtcNow;
                script.InternalScript = null;
                script.Errors.Clear();

                var compilationResult = provider.CompileAssemblyFromSource(compilerParameters, PreProcessSource(script));

                // Set errors + warnings
                foreach (CompilerError e in compilationResult.Errors)
                {
                    script.Errors.Add(new ScriptCompilationError(e.ErrorText, e.Line, e.IsWarning));
                }

                if (!compilationResult.Errors.HasErrors)
                {
                    // Get the type of the implementation
                    var type = compilationResult.CompiledAssembly.DefinedTypes.Where(t => typeof(ScriptImplementation).IsAssignableFrom(t)).FirstOrDefault();

                    if (type != null)
                    {
                        // Instantiate the implementation
                        script.InternalScript = Activator.CreateInstance(type) as ScriptImplementation;
                        if (script.InternalScript != null)
                        {
                            // Set the execute method
                            InitializeImplementation(script.InternalScript);
                            if (script.InternalScript.ExecuteMethod == null)
                            {
                                script.Errors.Add(new ScriptCompilationError("MonoKle: Implementation does not define method: " + EXECUTABLE_METHOD_NAME, -1, false));
                            }
                        }
                        else
                        {
                            script.Errors.Add(new ScriptCompilationError("MonoKle: Could not activate implementation.", -1, false));
                        }
                    }
                    else
                    {
                        script.Errors.Add(new ScriptCompilationError("MonoKle: Implementation type error.", -1, false));
                    }
                }
            }
        }

        private void InitializeImplementation(ScriptImplementation implementation) => implementation.ExecuteMethod = implementation.GetType().GetMethod(EXECUTABLE_METHOD_NAME, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        private CompilerParameters MakeCompilerParameters()
        {
            var compilerParameters = new CompilerParameters
            {
                GenerateExecutable = false,
                GenerateInMemory = true,
                IncludeDebugInformation = false,
                OutputAssembly = "MonoKleTempScriptAssembly",
                CompilerOptions = "/optimize",
            };

            compilerParameters.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
            compilerParameters.ReferencedAssemblies.Add("mscorlib.dll");
            compilerParameters.ReferencedAssemblies.Add("System.dll");

            foreach (Assembly a in ReferencedAssemblies)
            {
                compilerParameters.ReferencedAssemblies.Add(a.Location);
            }

            return compilerParameters;
        }

        private string PreProcessSource(IScriptCompilable script)
        {
            var namespaces = ReferencedAssemblies.SelectMany(a => a.GetTypes().Where(t => t.IsPublic).Select(t => t.Namespace)).Distinct();

            var usingsBuilder = new StringBuilder();
            foreach (var ns in namespaces)
            {
                usingsBuilder.Append("using ");
                usingsBuilder.Append(ns);
                usingsBuilder.Append("; ");
            }

            string ret = $"namespace {ScriptCompiler.NAMESPACE} {{ {usingsBuilder.ToString()} public class {script.Name} : {nameof(ScriptImplementation)} {{ {script.Source.Code} }} }}";
            return ret;
        }
    }
}
