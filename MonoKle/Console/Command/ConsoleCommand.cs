namespace MonoKle.Console.Command
{
    using System;

    /// <summary>
    /// Class for console command with arguments.
    /// </summary>
    public class ConsoleCommand : AbstractConsoleCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentlessConsoleCommand"/> class.
        /// </summary>
        /// <param name="name">The name of the command.</param>
        /// <param name="description">The description of the command.</param>
        /// <param name="arguments">The arguments for the command.</param>
        /// <param name="handler">The handler for the command.</param>
        public ConsoleCommand(string name, string description,
            CommandArguments arguments, ConsoleCommandHandler handler) : this(name, description, arguments, handler, null)
        {
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
            CommandArguments arguments, ConsoleCommandHandler handler, DefaultConsoleCommandHandler defaultHandler) : base(name, description, arguments)
        {
            if (handler == null)
            {
                throw new ArgumentNullException("Handler must not be null");
            }
            this.Handler = handler;
            this.DefaultHandler = defaultHandler;
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
        public override bool AllowsZeroArguments { get { return this.DefaultHandler != null; } }

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
        /// Calls the command with the specified arguments.
        /// </summary>
        /// <param name="arguments">The arguments. Passing null counts as passing zero arguments.</param>
        /// <returns></returns>
        public override bool Call(string[] arguments)
        {
            if (arguments != null && arguments.Length > 0)
            {
                this.Handler(arguments);
                return true;
            }
            else if (this.DefaultHandler != null)
            {
                this.DefaultHandler();
                return true;
            }

            return false;
        }
    }
}