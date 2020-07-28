using MonoKle.Console;
using MoreLinq;
using System.Collections.Generic;
using System.Linq;

namespace MonoKle.Engine.Commands
{
    [ConsoleCommand("vars", Description = "Lists the currently active variables.")]
    public class VarsCommand : IConsoleCommand
    {
        public void Call(IGameConsole console) => MonoKleGame.Variables.Variables.Identifiers
                .OrderBy(i => i)
                .ForEach(identifier => MonoKleGame.Console.WriteLine("\t" + identifier, MonoKleGame.Variables.Variables.CanSet(identifier) ? MonoKleGame.Console.DefaultTextColour : MonoKleGame.Console.DisabledTextColour));

        public ICollection<string> GetPositionalSuggestions() => new string[0];
    }
}
