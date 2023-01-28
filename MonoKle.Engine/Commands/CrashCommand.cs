using MonoKle.Console;
using System;
using System.Collections.Generic;

namespace MonoKle.Engine.Commands
{
    [ConsoleCommand("crash", Description = "Crashses the application.")]
    public class CrashCommand : IConsoleCommand
    {
        public void Call(IGameConsole console) => MGame.GameInstance.ShouldCrash = true;

        public ICollection<string> GetPositionalSuggestions(IGameConsole console) => Array.Empty<string>();
    }
}
