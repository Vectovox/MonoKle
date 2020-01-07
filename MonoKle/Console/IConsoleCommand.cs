using System.Collections.Generic;

namespace MonoKle.Console
{
    /// <summary>
    /// Interface for a console command.
    /// </summary>
    public interface IConsoleCommand
    {
        /// <summary>
        /// Calls the command with the specified arguments.
        /// </summary>
        /// <param name="console">The console that the command is run in.</param>
        void Call(IGameConsole console);

        /// <summary>
        /// Gets input suggestions for arguments.
        /// </summary>
        ICollection<string> GetPositionalSuggestions();
    }
}