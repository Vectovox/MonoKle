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
            if (MGame.Variables.Variables.Unbind(Variable) == false)
            {
                MGame.Console.Log.WriteError("Could not remove variable since it does not exist.");
            }
        }

        public ICollection<string> GetPositionalSuggestions(IGameConsole console) => MGame.Variables.Variables.Identifiers;
    }
}
