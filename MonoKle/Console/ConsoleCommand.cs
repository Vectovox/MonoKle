namespace MonoKle.Console {
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Class for console command with arguments.
    /// </summary>
    public class ConsoleCommand : AbstractConsoleCommand {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentlessConsoleCommand"/> class.
        /// </summary>
        /// <param name="name">The name of the command.</param>
        /// <param name="description">The description of the command.</param>
        /// <param name="arguments">The arguments for the command.</param>
        /// <param name="handler">The handler for the command.</param>
        public ConsoleCommand(string name, string description,
            CommandArguments arguments, ConsoleCommandHandler handler) : this(name, description, arguments, handler, null) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentlessConsoleCommand"/> class.
        /// </summary>
        /// <param name="name">The name of the command.</param>
        /// <param name="description">The description of the command.</param>
        /// <param name="arguments">The arguments for the command.</param>
        /// <param name="handler">The handler for the command.</param>
        /// <param name="defaultHandler">The default handler for the command if no argument is passed. May be null.</param>
        public ConsoleCommand(string name, string description,
            CommandArguments arguments, ConsoleCommandHandler handler, DefaultConsoleCommandHandler defaultHandler)
            : this(name, description, arguments, handler, defaultHandler, null) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentlessConsoleCommand"/> class.
        /// </summary>
        /// <param name="name">The name of the command.</param>
        /// <param name="description">The description of the command.</param>
        /// <param name="arguments">The arguments for the command.</param>
        /// <param name="handler">The handler for the command.</param>
        /// <param name="defaultHandler">The default handler for the command if no argument is passed. May be null.</param>
        /// <param name="suggestionHandler">Handler for providing input suggestions to arguments at a specified index.</param>
        public ConsoleCommand(string name, string description,
            CommandArguments arguments, ConsoleCommandHandler handler, DefaultConsoleCommandHandler defaultHandler,
            ConsoleCommandSuggestionHandler suggestionHandler) : base(name, description, arguments) {
            if (handler == null) {
                throw new ArgumentNullException("Handler must not be null");
            }
            Handler = handler;
            DefaultHandler = defaultHandler;
            SuggestionHandler = suggestionHandler;
        }

        /// <summary>
        /// Gets a value indicating whether arguments are accepted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if arguments are accepted; otherwise, <c>false</c>.
        /// </value>
        public override bool AcceptsArguments => true;

        /// <summary>
        /// Gets a value indicating whether zero arguments are allowed.
        /// </summary>
        /// <value>
        /// <c>true</c> if zero arguments are allowed; otherwise, <c>false</c>.
        /// </value>
        public override bool AllowsZeroArguments => DefaultHandler != null;

        /// <summary>
        /// Gets the default handler.
        /// </summary>
        /// <value>
        /// The default handler.
        /// </value>
        public DefaultConsoleCommandHandler DefaultHandler { get; private set; }

        /// <summary>
        /// Gets the handler.
        /// </summary>
        /// <value>
        /// The handler.
        /// </value>
        public ConsoleCommandHandler Handler { get; private set; }

        /// <summary>
        /// Gets the suggestion handler.
        /// </summary>
        /// <value>
        /// The suggestion handler.
        /// </value>
        public ConsoleCommandSuggestionHandler SuggestionHandler { get; private set; }

        /// <summary>
        /// Calls the command with the specified arguments.
        /// </summary>
        /// <param name="arguments">The arguments. Passing null counts as passing zero arguments.</param>
        /// <returns></returns>
        public override bool Call(string[] arguments) {
            if (arguments != null && arguments.Length > 0) {
                if (arguments.Length >= Arguments.Length) {
                    Handler(arguments);
                    return true;
                }
            } else if (DefaultHandler != null) {
                DefaultHandler();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Does the get input suggestions.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        protected override ICollection<string> DoGetInputSuggestions(int index) {
            if (SuggestionHandler != null) {
                return SuggestionHandler(index);
            }
            return new string[0];
        }
    }
}