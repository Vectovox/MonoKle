namespace MonoKle.Console.Command
{
    using Handlers;
    using System;

    /// <summary>
    /// Class for console command without arguments.
    /// </summary>
    public class ArgumentlessConsoleCommand : AbstractConsoleCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentlessConsoleCommand"/> class.
        /// </summary>
        /// <param name="name">The name of the command.</param>
        /// <param name="description">The description of the command.</param>
        /// <param name="arguments">The arguments for the command.</param>
        /// <param name="handler">The handler for the command.</param>
        /// <exception cref="System.ArgumentNullException">Handler must not be null</exception>
        public ArgumentlessConsoleCommand(string name, string description, DefaultConsoleCommandHandler handler)
            : base(name, description, CommandArguments.NoArguments)
        {
            if (handler == null)
            {
                throw new ArgumentNullException("Handler must not be null");
            }
            this.Handler = handler;
        }

        /// <summary>
        /// Gets a value indicating whether arguments are accepted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if arguments are accepted; otherwise, <c>false</c>.
        /// </value>
        public override bool AcceptsArguments => false;

        /// <summary>
        /// Gets a value indicating whether zero arguments are allowed.
        /// </summary>
        /// <value>
        /// <c>true</c> if zero arguments are allowed; otherwise, <c>false</c>.
        /// </value>
        public override bool AllowsZeroArguments => true;

        /// <summary>
        /// Gets the handler.
        /// </summary>
        /// <value>
        /// The handler.
        /// </value>
        public DefaultConsoleCommandHandler Handler { get; private set; }

        /// <summary>
        /// Calls the command with the specified arguments.
        /// </summary>
        /// <param name="arguments">The arguments. Passing null counts as passing zero arguments.</param>
        /// <returns></returns>
        public override bool Call(string[] arguments)
        {
            if (arguments == null || arguments.Length == 0)
            {
                this.Handler();
                return true;
            }

            return false;
        }
    }
}