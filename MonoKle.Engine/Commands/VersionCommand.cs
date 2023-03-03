using MonoKle.Console;
using System;
using System.Collections.Generic;

namespace MonoKle.Engine.Commands
{
    [ConsoleCommand(Name, Description = "Displays version information.")]
    public class VersionCommand : IConsoleCommand
    {
        public const string Name = "version";

        public void Call(IGameConsole console)
        {
            if (string.IsNullOrWhiteSpace(ConfigData.InternalVersion))
            {
                console.Log.AddLine($"Version: {ConfigData.ProductVersion}", console.CommandTextColour);
            }
            else
            {
                console.Log.AddLine($"Version: {ConfigData.ProductVersion} | {ConfigData.InternalVersion}",
                    console.CommandTextColour);
            }
            console.Log.AddLine($"MonoKle version: {ThisAssembly.AssemblyInformationalVersion} [{ThisAssembly.AssemblyConfiguration}]",
                console.CommandTextColour);
        }

        public ICollection<string> GetPositionalSuggestions(IGameConsole console) => Array.Empty<string>();
    }
}
