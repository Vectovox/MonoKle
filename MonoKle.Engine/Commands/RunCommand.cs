using MonoKle.Console;
using MonoKle.Scripting;
using System.Collections.Generic;
using System.Linq;

namespace MonoKle.Engine.Commands
{
    [ConsoleCommand("run", Description = "Runs the specified script.")]
    public class RunCommand : IConsoleCommand
    {
        [ConsolePositional(0, Description = "The script to run.", IsRequired = true)]
        public string Script { get; set; }

        [ConsolePositional(1, Description = "The script arguments.", IsRequired = false)]
        public string Value { get; set; }

        public void Call(IGameConsole console)
        {
            if (MBackend.ScriptEnvironment.Contains(Script))
            {
                IScript script = MBackend.ScriptEnvironment[Script];
                if (script.CanExecute)
                {
                    var sc = new StringConverter();
                    object[] arguments = Value.Split(" ").Select(a => sc.ToAny(a)).ToArray();
                    ScriptExecution result = script.Execute(arguments);

                    if (result.Success)
                    {
                        if (script.ReturnsValue)
                        {
                            console.WriteLine($"Execution result: {result.Result}");
                        }
                    }
                    else
                    {
                        console.WriteError($"Error executing script '{script.Name}': {result.Message}");
                    }
                }
                else
                {
                    console.WriteError($"Can not execute script '{script.Name}'.");
                }
            }
            else
            {
                console.WriteError($"Provided script '{Script}' does not exist.");
            }
        }

        public ICollection<string> GetPositionalSuggestions() => MBackend.ScriptEnvironment.ScriptNames;
    }
}
