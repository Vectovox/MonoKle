﻿using MonoKle.Console;
using System.Collections.Generic;

namespace MonoKle.Engine.Console.Commands
{
    [ConsoleCommand("echo", Description = "Prints the provided string to console.")]
    public class EchoCommand : IConsoleCommand
    {
        [ConsolePositional(0, Description = "The text to output.")]
        public string Text { get; set; } = string.Empty;

        public void Call(IGameConsole console) => console.WriteLine(Text);

        public ICollection<string> GetPositionalSuggestions() => new string[0];
    }
}
