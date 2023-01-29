using MonoKle.Console;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoKle.Engine.Commands
{
    [ConsoleCommand("vars", Description = "Lists the currently active variables.")]
    public class VarsCommand : IConsoleCommand
    {
        public void Call(IGameConsole console) => MGame.Variables.System.Identifiers
                .OrderBy(i => i)
                .ForEach(PrintIdentifier);

        private static void PrintIdentifier(string identifier)
        {
            var color = MGame.Variables.System.CanSet(identifier)
                ? MGame.Console.Log.DefaultTextColour
                : MGame.Console.Log.DisabledTextColour;
            string text = $"\t{identifier} : {MGame.Variables.System.GetValue(identifier)}";
            MGame.Console.Log.WriteLine(text, color);
        }

        public ICollection<string> GetPositionalSuggestions(IGameConsole console) => Array.Empty<string>();
    }
}
