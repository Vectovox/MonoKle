using MonoKle.Console;
using System.Collections.Generic;
using System.Linq;

namespace MonoKle.Engine.Commands
{
    [ConsoleCommand("reload_texture", Description = "Reloads the given texture asset.")]
    public class ReloadTextureCommand : IConsoleCommand
    {
        [ConsolePositional(0, Description = "The texture identifier to reload.", IsRequired = true)]
        public string Identifier { get; set; }

        public void Call(IGameConsole console)
        {
            MGame.Asset.Texture.Reload(Identifier);
        }

        public ICollection<string> GetPositionalSuggestions(IGameConsole console) => MGame.Asset.Texture.Identifiers.ToList();
    }
}
