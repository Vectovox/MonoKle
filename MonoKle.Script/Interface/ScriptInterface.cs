namespace MonoKle.Script.Interface
{
    using MonoKle.Script.Compiler;
    using MonoKle.Script.Interface.Event;
    using MonoKle.Script.IO;
    using MonoKle.Script.VM;
    using MonoKle.Script.VM.Event;

    /// <summary>
    /// Interface for loading and executing scripts.
    /// </summary>
    public class ScriptInterface
    {
        private VirtualMachine vm;
        private ScriptReader reader;
        private CompilationEnvironment compiler;

        public ScriptInterface()
        {
            ScriptCompiler c = new ScriptCompiler();
            this.compiler = new CompilationEnvironment(c);
            this.vm = new VirtualMachine();
            this.reader = new ScriptReader();
            this.vm.Print += OnPrint;
            this.vm.RuntimeError += OnRuntimeError;
        }

        /// <summary>
        /// Fired when a script fails to execute properly.
        /// </summary>
        public event RuntimeErrorEventHandler RuntimeError;

        /// <summary>
        /// Fired when compilation of a script fails.
        /// </summary>
        public event CompilationErrorEventHandler CompilationError;
        
        /// <summary>
        /// Fired when a executing script wants to print.
        /// </summary>
        public event PrintEventHandler Print;

        private void OnCompilationError(object sender, CompilationErrorEventArgs e)
        {
            var v = this.CompilationError;
            if(v != null)
            {
                v(this, e);
            }
        }

        private void OnRuntimeError(object sender, RuntimeErrorEventArgs e)
        {
            var v = this.RuntimeError;
            if(v != null)
            {
                v(this, e);
            }
        }

        private void OnPrint(object sender, PrintEventArgs e)
        {
            var v = this.Print;
            if(v != null)
            {
                v(this, e);
            }
        }

        /// <summary>
        /// Executes the script with the provided name.
        /// </summary>
        /// <param name="scriptName">Name of the script to execute.</param>
        /// <returns>Result of the execution.</returns>
        public ExecutionResult ExecuteScript(string scriptName)
        {
            return this.vm.ExecuteScript(scriptName);
        }

        /// <summary>
        /// Executes the script with the provided name.
        /// </summary>
        /// <param name="scriptName">Name of the script to execute.</param>
        /// <param name="parameters">Parameters to give the script.</param>
        /// <returns>Result of the execution.</returns>
        public ExecutionResult ExecuteScript(string scriptName, object[] parameters)
        {
            return this.vm.ExecuteScript(scriptName, parameters);
        }

        /// <summary>
        /// Executes all scripts on a given channel.
        /// </summary>
        /// <param name="channelName">The channel to execute scripts from.</param>
        /// <returns>Result of the channel execution.</returns>
        public ChannelResult ExecuteChannel(string channelName)
        {
            return this.vm.ExecuteChannel(channelName);
        }

        /// <summary>
        /// Executes all scripts on a given channel with the provided parameters.
        /// </summary>
        /// <param name="channelName">The channel to execute scripts from.</param>
        /// <param name="parameters">Parameters to proide each script.</param>
        /// <returns>Result of the channel execution.</returns>
        public ChannelResult ExecuteChannel(string channelName, object[] parameters)
        {
            return this.vm.ExecuteChannel(channelName, parameters);
        }

        /// <summary>
        /// Loads scripts in the given path. Both files and folders are accepted.
        /// </summary>
        /// <param name="path">Path to load scripts from.</param>
        /// <param name="recurse">If true, loading will be done recursively.</param>
        /// <returns>Amount of script sources added.</returns>
        public int AddScriptSources(string path, bool recurse)
        {
            return this.compiler.LoadSources(this.reader.ReadScriptSources(path, recurse).Sources);
        }

        /// <summary>
        /// Compiles added sources.
        /// </summary>
        /// <returns>Amount of compiled sources.</returns>
        public int CompileSources()
        {
            int counter = 0;
            foreach(ICompilationResult r in this.compiler.Compile())
            {
                if(r.Success)
                {
                    this.vm.LoadScript(r.Script);
                }
                else
                {
                    this.OnCompilationError(this, new CompilationErrorEventArgs(r.ErrorMessages, r.ScriptName));
                }
            }
            return counter;
        }
    }
}
