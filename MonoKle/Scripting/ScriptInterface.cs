namespace MonoKle.Scripting
{
    using MonoKle.Scripting.Script;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ScriptInterface
    {
        private Dictionary<string, LinkedList<ByteScript>> scriptsByChannel = new Dictionary<string, LinkedList<ByteScript>>();
        private Dictionary<string, ByteScript> scriptByName = new Dictionary<string, ByteScript>();

        private CompilerEngine compilerEngine = new CompilerEngine();
        private VirtualMachine vm = new VirtualMachine();

        internal ScriptInterface(){}

        public object[] CallChannel(string channel, object[] args)
        {
            LinkedList<ByteScript> scripts = scriptsByChannel[channel];
            object[] ret = new object[scripts.Count];

            for (int i = 0; i < ret.Length; i++)
            {

            }

            return ret;
        }

        public Result CallScript(string name, params object[] arguments)
        {
            if (scriptByName.ContainsKey(name))
            {
                MonoKleGame.Logger.AddLog("Running script: " + name, Logging.LogLevel.Debug);
                return vm.RunScript(scriptByName[name], arguments);
            }
            else
            {
                MonoKleGame.Logger.AddLog("Tried to run non-existing script: " + name, Logging.LogLevel.Error);
                return Result.Fail;
            }
        }

        /// <summary>
        /// Clears all loaded scripts from memory, returning the amount that was cleared.
        /// <returns>Amount of cleared scripts.</returns>
        /// </summary>
        public int ClearScripts()
        {
            int amount = scriptByName.Keys.Count;
            this.scriptByName.Clear();
            this.scriptsByChannel.Clear();
            MonoKleGame.Logger.AddLog("Cleared " + amount + " scripts.", Logging.LogLevel.Debug);
            return amount;
        }

        /// <summary>
        /// Clears all loaded sources from memory, returning the amount that was cleared.
        /// <returns>Amount of cleared sources.</returns>
        /// </summary>
        public int ClearSources()
        {
            int amount = this.compilerEngine.ClearSources();
            MonoKleGame.Logger.AddLog("Cleared " + amount + " sources.", Logging.LogLevel.Debug);
            return amount;
        }

        /// <summary>
        /// Compiles the loaded source files and loads them. Existing loaded scripts will be disgarded.
        /// </summary>
        public void CompileSource()
        {
            this.ClearScripts();
            
            int fails = 0;
            MonoKleGame.Logger.AddLog("Compiling " + this.compilerEngine.GetSourcesAmount() + " scripts...", Logging.LogLevel.Info);

            IEnumerable<ByteScript> scripts = this.compilerEngine.Compile(out fails);

            foreach (ByteScript script in scripts)
            {
                if (this.scriptByName.ContainsKey(script.Header.name))
                {
                    MonoKleGame.Logger.AddLog("Script (" + script.Header.name + ") has already been defined!", Logging.LogLevel.Error);
                    fails++;
                }
                else
                {
                    this.scriptByName.Add(script.Header.name, script);
                    MonoKleGame.Logger.AddLog("Script loaded :" + script.Header.name, Logging.LogLevel.Trace);
                    if (script.Header.channel != null)
                    {
                        if (this.scriptsByChannel.ContainsKey(script.Header.channel) == false)
                        {
                            this.scriptsByChannel.Add(script.Header.channel, new LinkedList<ByteScript>());
                        }
                        this.scriptsByChannel[script.Header.channel].AddLast(script);
                    }
                }
            }

            MonoKleGame.Logger.AddLog("...done [" + scripts.Count() + " successes |" + fails + " failures ]", Logging.LogLevel.Info);
        }

        public void LoadSource(string path)
        {
            this.LoadSource(path, false);
        }

        public void LoadSource(string path, bool recurse)
        {
            ScriptFileReader reader = new ScriptFileReader();
            IEnumerable<Source> sources = reader.GetScriptSources(path, recurse);
            int added = this.compilerEngine.AddSources(sources);
            MonoKleGame.Logger.AddLog("Loaded " + added + " script sources.", Logging.LogLevel.Info);
        }
    }
}
