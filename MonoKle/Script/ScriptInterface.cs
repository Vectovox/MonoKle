namespace MonoKle.Script
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ScriptInterface
    {
        private Dictionary<string, LinkedList<Script>> scriptsByChannel = new Dictionary<string, LinkedList<Script>>();
        private Dictionary<string, Script> scriptByName = new Dictionary<string, Script>();

        private ScriptCompiler compiler = new ScriptCompiler();
        private VirtualMachine vm = new VirtualMachine();

        internal ScriptInterface(){}

        public object[] CallChannel(string channel, object[] args)
        {
            LinkedList<Script> scripts = scriptsByChannel[channel];
            object[] ret = new object[scripts.Count];

            for (int i = 0; i < ret.Length; i++)
            {

            }

            return ret;
        }

        public Result CallScript(string name)
        {
            if (scriptByName.ContainsKey(name))
            {
                MonoKleGame.Logger.AddLog("Running script: " + name, Logging.LogLevel.Debug);
                return vm.RunScript(scriptByName[name]);
            }
            else
            {
                MonoKleGame.Logger.AddLog("Tried to run non-existing script: " + name, Logging.LogLevel.Error);
                return Result.Fail;
            }
        }

        public void LoadScripts(string path)
        {
            this.LoadScripts(path, false);
        }

        public void LoadScripts(string path, bool recurse)
        {
            ScriptReader reader = new ScriptReader();
            IEnumerable<string> scripts = reader.GetScripts(path, recurse);
            foreach (string source in scripts)
            {
                Script script = this.compiler.Compile(source);
                if (script == null)
                {
                    MonoKleGame.Logger.AddLog("Script in path (" + path + ") did not compile!", Logging.LogLevel.Error);
                }
                else
                {
                    if (this.scriptByName.ContainsKey(script.Name))
                    {
                        MonoKleGame.Logger.AddLog("Script (" + script.Name + ") has already been defined!", Logging.LogLevel.Error);
                    }
                    else
                    {
                        this.scriptByName.Add(script.Name, script);
                        if (script.Channel != null)
                        {
                            if (this.scriptsByChannel.ContainsKey(script.Channel) == false)
                            {
                                this.scriptsByChannel.Add(script.Channel, new LinkedList<Script>());
                            }
                            this.scriptsByChannel[script.Channel].AddLast(script);
                        }
                    }
                }
            }
        }
    }
}
