namespace MonoKle.Script
{
    using Microsoft.CSharp;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;

    public class MScriptCompiler
    {
        private const string MainClassName = "MScriptMainClass";
        private const string ScriptPrefix = "public class " + MainClassName + "{";
        private const string ScriptSuffix = "}";

        private List<string> sourceList = new List<string>();

        private object mainInstance;
        private Dictionary<string, MethodInfo> scriptByID;

        private bool Compiled { get; private set; }
        
        public void AddSource(string source)
        {
            this.sourceList.Add(source);
            this.Compiled = false;
        }

        public object CallScript(string scriptID, params object[] parameters)
        {
            if (scriptByID.ContainsKey(scriptID))
            {
                return scriptByID[scriptID].Invoke(this.mainInstance, parameters);
            }
            else
            {
                return null;
            }
        }

        public CompilationResult CompileScripts()
        {
            string s = this.CreateScriptString();
            CompilerResults r = this.Compile(s);
            CompilationResult result = this.CreateResult(r);
            
            if(result.Success)
            {
                this.mainInstance = this.CreateMainInstance(r.CompiledAssembly);
                this.scriptByID = this.CreateScriptDictionary(this.mainInstance.GetType());
                this.Compiled = true;
            }

            return result;
        }

        public IEnumerable<string> GetScriptIDs()
        {
            return this.scriptByID.Keys;
        }

        private Dictionary<string, MethodInfo> CreateScriptDictionary(Type t)
        {
            Dictionary<string, MethodInfo> ret = new Dictionary<string, MethodInfo>();
            foreach(MethodInfo i in t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                ret.Add(i.Name, i);
            }
            return ret;
        }

        private object CreateMainInstance(Assembly a)
        {
            Type t = a.GetType(MScriptCompiler.MainClassName);
            return Activator.CreateInstance(t);
        }

        private string CreateScriptString()
        {
            StringBuilder sb = new StringBuilder(MScriptCompiler.ScriptPrefix);
            foreach (string s in sourceList)
            {
                sb.Append(s);
            }
            sb.Append(MScriptCompiler.ScriptSuffix);
            return sb.ToString();
        }

        private CompilationResult CreateResult(CompilerResults result)
        {
            List<string> errorList = new List<string>();
            foreach (CompilerError e in result.Errors)
            {
                errorList.Add(e.ToString());
            }

            return new CompilationResult(errorList);
        }

        private CompilerResults Compile(string s)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = true;
            return provider.CompileAssemblyFromSource(parameters, s);
        }
    }
}
