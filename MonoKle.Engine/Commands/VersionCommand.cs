using MonoKle.Console;
using System;
using System.Collections.Generic;

namespace MonoKle.Engine.Commands
{
    [ConsoleCommand("version", Description = "Displays the version numbers.")]
    public class VersionCommand : IConsoleCommand
    {
        public void Call(IGameConsole console) => console.WriteLine($"{ThisAssembly.AssemblyInformationalVersion}");

        public ICollection<string> GetPositionalSuggestions(IGameConsole console) => Array.Empty<string>();
    }
}
