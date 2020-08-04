using MonoKle.Console;
using MoreLinq;
using System.Collections.Generic;
using System.Linq;

namespace MonoKle.Engine.Commands
{
    [ConsoleCommand("vars", Description = "Lists the currently active variables.")]
    public class VarsCommand : IConsoleCommand
    {
        public void Call(IGameConsole console) => MGame.Variables.Variables.Identifiers
                .OrderBy(i => i)
                .ForEach(identifier => MGame.Console.WriteLine("\t" + identifier, MGame.Variables.Variables.CanSet(identifier) ? MGame.Console.DefaultTextColour : MGame.Console.DisabledTextColour));

        public ICollection<string> GetPositionalSuggestions() => new string[0];
    }
}
