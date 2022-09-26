using MonoKle.Console;
using System;
using System.Collections.Generic;

namespace MonoKle.Engine.Commands
{
    [ConsoleCommand("unused_textures", Description = "Lists unused textures.")]
    public class UnusedTexturesCommand : IConsoleCommand
    {
        public void Call(IGameConsole console)
        {
            foreach(var identifier in MGame.Asset.Texture.UnusedIdentifiers())
            {
                console.WriteLine(identifier);
            }
        }

        public ICollection<string> GetPositionalSuggestions(IGameConsole console) => Array.Empty<string>();
    }
}
