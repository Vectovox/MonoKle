using MonoKle.Console;
using MonoKle.Scripting;
using System.Collections.Generic;

namespace MonoKle.Engine.Commands
{
    [ConsoleCommand("source", Description = "Prints the source for the given script.")]
    public class SourceCommand : IConsoleCommand
    {
        [ConsolePositional(0, Description = "Name of the script to print source for.", IsRequired = true)]
        public string Script { get; set; } = string.Empty;

        public void Call(IGameConsole console)
        {
            if (MBackend.ScriptEnvironment.Contains(Script))
            {
                IScript script = MBackend.ScriptEnvironment[Script];
                console.WriteLine($"Printing source for '{script.Name}' from '{script.Source.Date}': ");
                console.WriteLine("> " + script.Source.Code.Replace("\n", "\n> "));
            }
            else
            {
                console.WriteError($"Provided script '{Script}' does not exist.");
            }
        }

        public ICollection<string> GetPositionalSuggestions() => MBackend.ScriptEnvironment.ScriptNames;
    }
}
