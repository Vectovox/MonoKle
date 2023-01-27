using MonoKle.Console;
using System;
using System.Collections.Generic;

namespace MonoKle.Engine.Console.Commands
{
    [ConsoleCommand("clear", Description = "Clears the console output.")]
    public class ClearCommand : IConsoleCommand
    {
        public void Call(IGameConsole console) => console.Log.Clear();

        public ICollection<string> GetPositionalSuggestions(IGameConsole console) => Array.Empty<string>();
    }
}
