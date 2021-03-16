﻿using MonoKle.Console;
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
                .ForEach(PrintIdentifier);

        private static void PrintIdentifier(string identifier)
        {
            var color = MGame.Variables.Variables.CanSet(identifier) ? MGame.Console.DefaultTextColour : MGame.Console.DisabledTextColour;
            string text = $"\t{identifier} : {MGame.Variables.Variables.GetValue(identifier)}";
            MGame.Console.WriteLine(text, color);
        }

        public ICollection<string> GetPositionalSuggestions() => new string[0];
    }
}
