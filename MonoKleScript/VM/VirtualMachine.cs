namespace MonoKle.Script.VM
{
    using MonoKle.Script.Common.Script;
    using MonoKle.Script.VM.Event;
    using System.Collections.Generic;
    using System;


    /// <summary>
    /// Virtual machine that executes compiled scripts.
    /// </summary>
    public class VirtualMachine : IVirtualMachine
    {
        private ScriptExecuter executer = new ScriptExecuter();
        private Dictionary<string, ByteScript> scriptByName = new Dictionary<string, ByteScript>();
        private Dictionary<string, ICollection<string>> scriptsByChannel = new Dictionary<string, ICollection<string>>();

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
        /// Executes all scripts on a given channel.
        /// </summary>
        /// <param name="channel">The channel to execute scripts from.</param>
        /// <returns>Result of the channel execution.</returns>
        public ChannelResult ExecuteChannel(string channel)
        {
            return this.ExecuteChannel(channel, new object[0]);
        }

        /// <summary>
        /// Executes all scripts on a given channel with the provided parameters.
        /// </summary>
        /// <param name="channel">The channel to execute scripts from.</param>
        /// <param name="parameters">Parameters to proide each script.</param>
        /// <returns>Result of the channel execution.</returns>
        public ChannelResult ExecuteChannel(string channel, object[] parameters)
        {
            ICollection<ExecutionResult> resultCollection = new LinkedList<ExecutionResult>();
            if(this.scriptsByChannel.ContainsKey(channel))
            {
                foreach(string s in this.scriptsByChannel[channel])
                {
                    resultCollection.Add(this.ExecuteScript(s, parameters));
                }
            }
            return new ChannelResult(channel, resultCollection);
        }

        /// <summary>
        /// Executes the script with the provided name.
        /// </summary>
        /// <param name="script">Name of the script to execute.</param>
        /// <returns>Result of the execution.</returns>
        public ExecutionResult ExecuteScript(string script)
        {
            return this.ExecuteScript(script, new object[0]);
        }

        /// <summary>
        /// Executes the script with the provided name.
        /// </summary>
        /// <param name="script">Name of the script to execute.</param>
        /// <param name="parameters">Parameters to give the script.</param>
        /// <returns>Result of the execution.</returns>
        public ExecutionResult ExecuteScript(string script, object[] arguments)
        {
            if (this.scriptByName.ContainsKey(script))
            {
                return this.executer.RunScript(this.scriptByName[script], arguments, this.scriptByName);
            }
            else
            {
                this.OnRuntimeError(new RuntimeErrorEventArgs("Script " + script + " not loaded"));
            }
            return ExecutionResult.CreateFail(script);
        }

        /// <summary>
        /// Loads the provided script and returns if successful.
        /// </summary>
        /// <param name="script">Script.</param>
        /// <returns>True if script was loaded, otherwise false.</returns>
        public bool LoadScript(ByteScript script)
        {
            if(this.scriptByName.ContainsKey(script.Header.Name) == false)
            {
                // Add script to dictionary
                this.scriptByName.Add(script.Header.Name, script);
                // Add script to channel
                if(script.Header.Channel != null && script.Header.Channel.Length > 0)
                {
                    if(this.scriptsByChannel.ContainsKey(script.Header.Channel) == false)
                    {
                        this.scriptsByChannel.Add(script.Header.Channel, new LinkedList<string>());
                    }
                    this.scriptsByChannel[script.Header.Channel].Add(script.Header.Name);
                }
                return true;
            }
            this.OnRuntimeError(new RuntimeErrorEventArgs("Script with name [" + script.Header.Name + "] already exists"));
            return false;
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
                if(this.LoadScript(s))
                {
                    nLoaded++;
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