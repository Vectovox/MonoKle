using MonoKle.Console;
using System.Collections.Generic;

namespace MonoKle.Engine.Commands
{
    [ConsoleCommand("rem", Description = "Removes the provided variable.")]
    public class RemCommand : IConsoleCommand
    {
        [ConsolePositional(0, Description = "The variable to remove.", IsRequired = true)]
        public string Variable { get; set; }

        public void Call(IGameConsole console)
        {
            if (MBackend.Variables.Variables.Remove(Variable) == false)
            {
                MBackend.Console.WriteError("Could not remove variable since it does not exist.");
            }
        }

        public ICollection<string> GetPositionalSuggestions() => MBackend.Variables.Variables.Identifiers;
    }
}
