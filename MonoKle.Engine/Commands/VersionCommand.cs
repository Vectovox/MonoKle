using MonoKle.Console;
using System.Collections.Generic;
using System.Reflection;

namespace MonoKle.Engine.Commands
{
    [ConsoleCommand("version", Description = "Displays the version numbers.")]
    public class VersionCommand : IConsoleCommand
    {
        public void Call(IGameConsole console)
        {
            console.WriteLine("       MonoKle Version:\t" + Assembly.GetAssembly(typeof(MVector2)).GetName().Version);
            console.WriteLine("MonoKle Engine Version:\t" + Assembly.GetAssembly(typeof(MBackend)).GetName().Version);
        }

        public ICollection<string> GetPositionalSuggestions() => new string[0];
    }
}
