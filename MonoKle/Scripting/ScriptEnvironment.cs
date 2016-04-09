using System.Collections.Generic;
using System.Reflection;

namespace MonoKle.Scripting
{
    public class ScriptEnvironment
    {
        private ScriptCompiler compiler = new ScriptCompiler();
        private Dictionary<string, Script> scriptById = new Dictionary<string, Script>();

        public int Count => scriptById.Count;

        public Script this[string id] => this.scriptById[id];

        public List<Assembly> ReferencedAssemblies { get { return this.compiler.ReferencedAssemblies; } }

        public bool Add(Script script)
        {
            if(!this.scriptById.ContainsKey(script.Name))
            {
                this.scriptById.Add(script.Name, script);
                return true;
            }
            return false;
        }

        public bool Compile(string script)
        {
            if (scriptById.ContainsKey(script))
            {
                this.compiler.Compile(this[script]);
                return true;
            }
            return false;
        }

        public int CompileAll()
        {
            this.compiler.Compile(this.scriptById.Values);
            return this.scriptById.Values.Count;
        }

        public bool Contains(string scriptName) => scriptById.ContainsKey(scriptName);

        public List<string> ScriptNames => new List<string>(scriptById.Keys);
    }
}