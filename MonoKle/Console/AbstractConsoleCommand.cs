namespace MonoKle.Console {
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Abstract console command class.
    /// </summary>
    public abstract class AbstractConsoleCommand : IConsoleCommand {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractConsoleCommand"/> class.
        /// </summary>
        /// <param name="name">The name of the command.</param>
        /// <param name="description">The description of the command.</param>
        /// <param name="arguments">The arguments for the command.</param>
        /// <exception cref="ArgumentNullException">
        /// Name must not be null.
        /// or
        /// Arguments must not be null.
        /// </exception>
        public AbstractConsoleCommand(string name, string description, CommandArguments arguments) {
            if (name == null) {
                throw new ArgumentNullException("Name must not be null.");
            }
            if (arguments == null) {
                throw new ArgumentNullException("Arguments must not be null.");
            }
            Name = name.ToLower();
            Description = description;
            Arguments = arguments;
        }

        /// <summary>
        /// Gets a value indicating whether arguments are accepted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if arguments are accepted; otherwise, <c>false</c>.
        /// </value>
        public abstract bool AcceptsArguments { get; }

        /// <summary>
        /// Gets a value indicating whether zero arguments are allowed.
        /// </summary>
        /// <value>
        /// <c>true</c> if zero arguments are allowed; otherwise, <c>false</c>.
        /// </value>
        public abstract bool AllowsZeroArguments { get; }

        /// <summary>
        /// Gets the arguments.
        /// </summary>
        /// <value>
        /// The arguments.
        /// </value>
        public CommandArguments Arguments { get; private set; }

        /// <summary>
        /// Gets the description of the command.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the lowercase name of the command.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Calls the command with the specified arguments.
        /// </summary>
        /// <param name="arguments">The arguments. Passing null counts as passing zero arguments.</param>
        /// <returns></returns>
        public abstract bool Call(string[] arguments);

        /// <summary>
        /// Calls the command without passing any arguments.
        /// </summary>
        /// <returns></returns>
        public bool Call() => Call(null);

        /// <summary>
        /// Gets input suggestions for arguments at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public ICollection<string> GetInputSuggestions(int index) {
            if (index >= 0 && index < Arguments.Length) {
                return DoGetInputSuggestions(index);
            }
            return new string[0];
        }

        /// <summary>
        /// Does the get input suggestions.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        protected virtual ICollection<string> DoGetInputSuggestions(int index) => new string[0];
    }
}