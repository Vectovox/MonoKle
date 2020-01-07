using Microsoft.Xna.Framework;
using MonoKle.Console;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoKle.Engine.Commands
{
    [ConsoleCommand("scripts", Description = "Lists the known scripts.")]
    public class ScriptsCommand : IConsoleCommand
    {
        public void Call(IGameConsole console)
        {
            var scripts = MBackend.ScriptEnvironment.ScriptNames.Select(s => MBackend.ScriptEnvironment[s]).OrderBy(s => s.Name);
            foreach (var s in scripts)
            {
                var sb = new StringBuilder("\t");
                if (s.ReturnsValue)
                {
                    sb.Append(s.ReturnType.Name);
                    sb.Append(" ");
                }
                sb.Append(s.Name);

                if (s.IsOutdated)
                {
                    sb.Append(" -outdated-");
                }
                if (s.Errors.Count != 0)
                {
                    sb.Append($" -{s.Errors.Count} errors-");
                }

                Color c = console.DefaultTextColour;
                if (s.IsOutdated)
                    c = console.WarningTextColour;
                if (s.Errors.Count != 0)
                    c = console.ErrorTextColour;

                console.WriteLine(sb.ToString(), c);
            }
        }

        public ICollection<string> GetPositionalSuggestions() => new string[0];
    }
}
