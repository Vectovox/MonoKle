using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MonoKle.Scripting
{
    /// <summary>
    /// Compiler for scripts.
    /// </summary>
    public class ScriptCompiler
    {
        private const string NAMESPACE = "MonoKleScriptNamespace";

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptCompiler"/> class.
        /// </summary>
        public ScriptCompiler()
        {
            this.ReferencedAssemblies = new List<Assembly>();
            this.ReferencedAssemblies.Add(Assembly.GetAssembly(typeof(Script)));
            this.ReferencedAssemblies.Add(Assembly.GetAssembly(typeof(Exception)));
            this.ReferencedAssemblies.Add(Assembly.GetAssembly(typeof(Stopwatch)));
        }

        public List<Assembly> ReferencedAssemblies { get; private set; }

        public void Compile(Script script) => this.Compile(new List<Script> { script });

        public void Compile(IEnumerable<Script> scripts)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters compilerParameters = this.MakeCompilerParameters();

            foreach (Script script in scripts)
            {
                script.InternalScript = null;
                script.Errors.Clear();

                var compilationResult = provider.CompileAssemblyFromSource(compilerParameters, this.PreProcessSource(script));

                // Set errors + warnings
                foreach (CompilerError e in compilationResult.Errors)
                {
                    script.Errors.Add(new ScriptCompilationError() { Message = e.ErrorText, Line = e.Line, IsWarning = e.IsWarning });
                }

                if (!compilationResult.Errors.HasErrors)
                {
                    var type = compilationResult.CompiledAssembly.DefinedTypes.Where(t => typeof(ExecutableScript).IsAssignableFrom(t)).FirstOrDefault();

                    if (type != null)
                    {
                        script.InternalScript = (ExecutableScript)Activator.CreateInstance(type);
                        script.InternalScript.Initialize();
                    }
                }
            }
        }

        private CompilerParameters MakeCompilerParameters()
        {
            CompilerParameters compilerParameters = new CompilerParameters
            {
                GenerateExecutable = false,
                GenerateInMemory = true,
                IncludeDebugInformation = false,
                OutputAssembly = "MonoKleTempScriptAssembly",
                CompilerOptions = "/optimize"
            };
            foreach (Assembly a in ReferencedAssemblies)
            {
                compilerParameters.ReferencedAssemblies.Add(a.Location);
            }

            return compilerParameters;
        }

        private string PreProcessSource(Script script)
        {
            var namespaces = ReferencedAssemblies.SelectMany(a => a.GetTypes().Where(t => t.IsPublic).Select(t => t.Namespace)).Distinct();

            StringBuilder usingsBuilder = new StringBuilder();
            foreach (var ns in namespaces)
            {
                usingsBuilder.Append("using ");
                usingsBuilder.Append(ns);
                usingsBuilder.Append("; ");
            }

            string ret = $"namespace {ScriptCompiler.NAMESPACE} {{ {usingsBuilder.ToString()} public class {script.Name} : {nameof(ExecutableScript)} {{ {script.Source.SoureCode} }} }}";
            return ret;
        }
    }
}