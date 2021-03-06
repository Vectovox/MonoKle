﻿using MonoKle.Console;
using System.Collections.Generic;

namespace MonoKle.Engine.Console.Commands
{
    [ConsoleCommand("clear", Description = "Clears the console output.")]
    public class ClearCommand : IConsoleCommand
    {
        public void Call(IGameConsole console) => console.Clear();

        public ICollection<string> GetPositionalSuggestions() => new string[0];
    }
}
