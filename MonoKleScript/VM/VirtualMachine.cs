namespace MonoKleScript.VM
{
    using System.Collections.Generic;

    using MonoKleScript.Script;
    using MonoKleScript.VM.Event;

    public class VirtualMachine : IVirtualMachine
    {
        private ScriptExecuter executer = new ScriptExecuter();
        private Dictionary<string, ByteScript> scriptByName = new Dictionary<string, ByteScript>();

        public VirtualMachine()
        {
            this.executer.RuntimeError += executer_RuntimeError;
            this.executer.Print += executer_Print;
        }

        public event PrintEventHandler Print;

        public event RuntimeErrorEventHandler RuntimeError;

        public Result ExecuteScript(string script)
        {
            return this.ExecuteScript(script, new object[0]);
        }

        public Result ExecuteScript(string script, object[] arguments)
        {
            if (this.scriptByName.ContainsKey(script))
            {
                return this.executer.RunScript(this.scriptByName[script], arguments, this.scriptByName);
            }
            else
            {
                this.OnRuntimeError(new RuntimeErrorEventArgs("Script " + script + " not loaded"));
            }
            return Result.Fail;
        }

        public int LoadScripts(ICollection<ByteScript> scripts)
        {
            int nLoaded = 0;
            foreach (ByteScript s in scripts)
            {
                if (this.scriptByName.ContainsKey(s.Header.name) == false)
                {
                    nLoaded++;
                    this.scriptByName.Add(s.Header.name, s);
                }
                else
                {
                    this.OnRuntimeError(new RuntimeErrorEventArgs("Script with name [" + s.Header.name + "] already exists"));
                }
            }
            return nLoaded;
        }

        private void executer_Print(object sender, PrintEventArgs e)
        {
            this.OnPrint(e);
        }

        private void executer_RuntimeError(object sender, RuntimeErrorEventArgs e)
        {
            this.OnRuntimeError(e);
        }

        private void OnPrint(PrintEventArgs e)
        {
            var l = Print;
            if (l != null)
            {
                l(this, e);
            }
        }

        private void OnRuntimeError(RuntimeErrorEventArgs e)
        {
            var l = RuntimeError;
            if (l != null)
            {
                l(this, e);
            }
        }
    }
}