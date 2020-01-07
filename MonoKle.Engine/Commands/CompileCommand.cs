using MonoKle.Console;
using MonoKle.Scripting;
using System.Collections.Generic;

namespace MonoKle.Engine.Commands
{
    [ConsoleCommand("compile", Description = "Compiles the specified script. If no script is provided, all outdated will be compiled.")]
    public class CompileCommand : IConsoleCommand
    {
        [ConsolePositional(0, Description = "The specific script to compile.", IsRequired = false)]
        public string Script { get; set; } = string.Empty;

        public void Call(IGameConsole console)
        {
            if (string.IsNullOrWhiteSpace(Script))
            {
                int amount = MBackend.ScriptEnvironment.CompileOutdated();
                console.WriteLine($"Compiled {amount} scripts.");
            }
            else if (MBackend.ScriptEnvironment.Compile(Script))
            {
                IScript script = MBackend.ScriptEnvironment[Script];
                if (script.Errors.Count == 0)
                {
                    console.WriteLine($"Script '{script.Name}' compiled.");
                }
                else
                {
                    console.WriteError($"Script '{script.Name}' compiled with {script.Errors.Count} errors:\n  {string.Join("\n  ", script.Errors)}");
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
