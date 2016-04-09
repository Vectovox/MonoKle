using System;
using System.Collections.Generic;

namespace MonoKle.Scripting
{
    public class Script
    {
        public IScriptSource Source { get; private set; }

        public List<ScriptCompilationError> Errors { get; private set; }

        public ExecutableScript InternalScript { get; set; }

        public bool CanExecute { get { return InternalScript != null; } }

        public string Name { get; private set; }

        public Script(string name, IScriptSource source)
        {
            this.Source = source;
            this.Name = name;
            this.Errors = new List<ScriptCompilationError>();
        }

        public ScriptExecution Execute(params object[] parameters)
        {
            if(this.CanExecute)
            {
                return this.InternalScript.Execute(parameters);
            }

            return new ScriptExecution(null, false, "Script can not execute.");
        }
    }
}
