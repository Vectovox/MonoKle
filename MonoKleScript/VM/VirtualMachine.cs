namespace MonoKleScript.VM
{
    using MonoKleScript.Common.Script;
    using MonoKleScript.VM.Event;
    using System.Collections.Generic;

    /// <summary>
    /// Virtual machine that executes compiled scripts.
    /// </summary>
    public class VirtualMachine : IVirtualMachine
    {
        private ScriptExecuter executer = new ScriptExecuter();
        private Dictionary<string, ByteScript> scriptByName = new Dictionary<string, ByteScript>();

        /// <summary>
        /// Creates a new instance of <see cref="VirtualMachine"/>.
        /// </summary>
        public VirtualMachine()
        {
            this.executer.RuntimeError += executer_RuntimeError;
            this.executer.Print += executer_Print;
        }

        /// <summary>
        /// Event fired when a script wants to print.
        /// </summary>
        public event PrintEventHandler Print;

        /// <summary>
        /// Event fired when executing a script resulted in a runtime error.
        /// </summary>
        public event RuntimeErrorEventHandler RuntimeError;

        /// <summary>
        /// Executes the script with the provided name.
        /// </summary>
        /// <param name="script">Name of the script to execute.</param>
        /// <returns>Result of the execution.</returns>
        public Result ExecuteScript(string script)
        {
            return this.ExecuteScript(script, new object[0]);
        }

        /// <summary>
        /// Executes the script with the provided name.
        /// </summary>
        /// <param name="script">Name of the script to execute.</param>
        /// <param name="parameters">Parameters to give the script.</param>
        /// <returns>Result of the execution.</returns>
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

        /// <summary>
        /// Loads the provided scripts and returns the amount loaded.
        /// </summary>
        /// <param name="scripts">Collection of scripts.</param>
        /// <returns>Amount of loaded scripts.</returns>
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