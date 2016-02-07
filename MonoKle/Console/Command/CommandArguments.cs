namespace MonoKle.Console.Command
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Class for command arguments.
    /// </summary>
    public class CommandArguments
    {
        /// <summary>
        /// The no arguments instance.
        /// </summary>
        public static readonly CommandArguments NoArguments = new CommandArguments(0);

        private const string NullArgumentsMessage = "Arguments must not be null.";

        private Dictionary<string, string> arguments;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandArguments"/> class.
        /// </summary>
        /// <param name="arguments">The arguments and their descriptions mapped.</param>
        public CommandArguments(Dictionary<string, string> arguments)
        {
            this.CheckArguments(arguments);
            this.arguments = new Dictionary<string, string>(arguments);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandArguments"/> class.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        public CommandArguments(ICollection<string> arguments)
        {
            this.CheckArguments(arguments);
            this.arguments = new Dictionary<string, string>();
            foreach (string s in arguments)
            {
                this.arguments.Add(s, null);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandArguments"/> class.
        /// </summary>
        /// <param name="nArguments">The amount of arguments.</param>
        /// <exception cref="System.ArgumentException">Amount of arguments must be positive.</exception>
        public CommandArguments(int nArguments)
        {
            if (nArguments < 0)
            {
                throw new ArgumentException("Amount of arguments must be positive.");
            }
            this.arguments = new Dictionary<string, string>();
            for (int i = 1; i <= nArguments; i++)
            {
                this.arguments.Add("arg" + i, null);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandArguments"/> class.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <param name="argumentDescriptions">The argument descriptions.</param>
        /// <exception cref="System.ArgumentException">Arguments length and argument descriptions length must match.</exception>
        public CommandArguments(string[] arguments, string[] argumentDescriptions)
        {
            this.CheckArguments(arguments);
            this.CheckArguments(argumentDescriptions);
            if (arguments.Length != argumentDescriptions.Length)
            {
                throw new ArgumentException("Arguments length and argument descriptions length must match.");
            }

            this.arguments = new Dictionary<string, string>();
            for (int i = 0; i < arguments.Length; i++)
            {
                this.arguments.Add(arguments[i], argumentDescriptions[i]);
            }
        }

        /// <summary>
        /// Gets a mapping between each argument and its description.
        /// </summary>
        /// <value>
        /// The argument description map.
        /// </value>
        public Dictionary<string, string> ArgumentDescriptionMap { get { return new Dictionary<string, string>(this.arguments); } }

        /// <summary>
        /// Gets the arguments.
        /// </summary>
        /// <value>
        /// The arguments.
        /// </value>
        public ICollection<string> Arguments { get { return this.arguments.Keys; } }

        /// <summary>
        /// Gets the amount of arguments.
        /// </summary>
        /// <value>
        /// The amount of arguments.
        /// </value>
        public int Length { get { return this.Arguments.Count; } }

        /// <summary>
        /// Gets the argument description for the specified argument.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <returns></returns>
        public string GetArgumentDescription(string argument)
        {
            if (this.arguments.ContainsKey(argument))
            {
                return this.arguments[argument];
            }
            return null;
        }

        private void CheckArguments(object o)
        {
            if (o == null)
            {
                throw new ArgumentNullException(CommandArguments.NullArgumentsMessage);
            }
        }
    }
}