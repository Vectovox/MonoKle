﻿using MonoKle.Console;
using System;
using System.Collections.Generic;

namespace MonoKle.Engine.Commands
{
    [ConsoleCommand("exit", Description = "Terminates the application.")]
    public class ExitCommand : IConsoleCommand
    {
        public void Call(IGameConsole console) => MGame.GameInstance.Exit();

        public ICollection<string> GetPositionalSuggestions(IGameConsole console) => Array.Empty<string>();
    }
}
