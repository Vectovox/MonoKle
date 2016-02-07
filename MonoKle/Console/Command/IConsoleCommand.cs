namespace MonoKle.Console.Command
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface for a console command.
    /// </summary>
    public interface IConsoleCommand
    {
        /// <summary>
        /// Gets a value indicating whether arguments are accepted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if arguments are accepted; otherwise, <c>false</c>.
        /// </value>
        bool AcceptsArguments { get; }

        /// <summary>
        /// Gets a value indicating whether zero arguments are allowed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if zero arguments are allowed; otherwise, <c>false</c>.
        /// </value>
        bool AllowsZeroArguments { get; }

        /// <summary>
        /// Gets the arguments.
        /// </summary>
        /// <value>
        /// The arguments.
        /// </value>
        CommandArguments Arguments { get; }

        /// <summary>
        /// Gets the description of the command.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        string Description { get; }

        /// <summary>
        /// Gets the lowercase name of the command.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Calls the command without passing any arguments.
        /// </summary>
        /// <returns></returns>
        bool Call();

        /// <summary>
        /// Calls the command with the specified arguments.
        /// </summary>
        /// <param name="arguments">The arguments. Passing null counts as passing zero arguments.</param>
        /// <returns></returns>
        bool Call(string[] arguments);

        /// <summary>
        /// Gets input suggestions for arguments at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        ICollection<string> GetInputSuggestions(int index);
    }
}