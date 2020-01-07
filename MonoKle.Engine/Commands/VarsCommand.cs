using MonoKle.Console;
using MoreLinq;
using System.Collections.Generic;
using System.Linq;

namespace MonoKle.Engine.Commands
{
    [ConsoleCommand("vars", Description = "Lists the currently active variables.")]
    public class VarsCommand : IConsoleCommand
    {
        public void Call(IGameConsole console) => MBackend.Variables.Variables.Identifiers
                .OrderBy(i => i)
                .ForEach(identifier => MBackend.Console.WriteLine("\t" + identifier, MBackend.Variables.Variables.CanSet(identifier) ? MBackend.Console.DefaultTextColour : MBackend.Console.DisabledTextColour));

        public ICollection<string> GetPositionalSuggestions() => new string[0];
    }
}
