using MonoKle.Console;
using MoreLinq;
using System.Collections.Generic;
using System.Linq;

namespace MonoKle.Engine.Commands
{
    [ConsoleCommand("state", Description = "Switches to or removes states. No arguments lists them.")]
    public class StateCommand : IConsoleCommand
    {
        [ConsolePositional(0, Description = "State to switch to.", IsRequired = false)]
        public string State { get; set; }

        [ConsoleFlag("r", Description = "Removes the given state instead of switching.")]
        public bool Remove { get; set; }

        [ConsoleFlag("l", Description = "Lists states.")]
        public bool List { get; set; }

        public void Call(IGameConsole console)
        {
            if (List)
            {
                MGame.StateSystem.Identifiers
                    .OrderBy(i => i)
                    .ForEach(PrintIdentifier);
            }
            else if (State == null)
            {
                MGame.Console.Log.AddLine($"Current state: {MGame.StateSystem.Current}");
            }
            else
            {
                if (Remove)
                {
                    MGame.StateSystem.RemoveState(State);
                }
                else
                {
                    MGame.StateSystem.SwitchState(State);
                }
            }
        }

        private static void PrintIdentifier(string identifier) => MGame.Console.Log.AddLine($"\t{identifier}");

        public ICollection<string> GetPositionalSuggestions(IGameConsole console) => MGame.StateSystem.Identifiers;
    }
}
