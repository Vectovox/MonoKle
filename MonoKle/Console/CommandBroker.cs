namespace MonoKle.Console
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Broker for console commands.
    /// </summary>
    public class CommandBroker
    {
        private Dictionary<string, Command> dictionary = new Dictionary<string, Command>();

        /// <summary>
        /// Gets the registered commands.
        /// </summary>
        /// <value>
        /// The registered commands.
        /// </value>
        public ICollection<Command> Commands { get { return this.dictionary.Values; } }

        /// <summary>
        /// Calls the specified command. Verifies a correct arguments length.
        /// </summary>
        /// <param name="command">The command to call.</param>
        /// <returns>True if command is registered and correct argument length was provided; otherwise false.</returns>
        public bool Call(string command)
        {
            return this.Call(command, new string[0]);
        }

        /// <summary>
        /// Calls the specified command. Verifies a correct arguments length.
        /// </summary>
        /// <param name="command">The command to call.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>True if command is registered and correct argument length was provided; otherwise false.</returns>
        public bool Call(string command, string[] arguments)
        {
            command = command.ToLower();
            if (this.dictionary.ContainsKey(command))
            {
                Command c = this.dictionary[command];
                if (arguments.Length == 0 && c.DefaultHandler != null)
                {
                    c.DefaultHandler();
                    return true;
                }
                else if (c.ArgumentLength == arguments.Length)
                {
                    c.Handler(arguments);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Clears all registered commands.
        /// </summary>
        public void Clear()
        {
            this.dictionary.Clear();
        }

        /// <summary>
        /// Gets the specified command if it exists, otherwise null.
        /// </summary>
        /// <param name="command">The command to retrieve.</param>
        /// <returns></returns>
        public CommandBroker.Command Get(string command)
        {
            command = command.ToLower();
            if (this.dictionary.ContainsKey(command))
            {
                return this.dictionary[command];
            }
            return null;
        }

        /// <summary>
        /// Registers the specified argumentless command.
        /// </summary>
        /// <param name="command">The command to register.</param>
        /// <param name="handler">The handler to execute the command when no arguments are passed.</param>
        /// <returns>True if command was registered; otherwise false.</returns>
        public bool Register(string command, DefaultConsoleCommandHandler handler)
        {
            return this.Register(command.ToLower(), 0, null, handler);
        }

        /// <summary>
        /// Registers the specified argumentless command.
        /// </summary>
        /// <param name="command">The command to register.</param>
        /// <param name="description">The description of the command. May be null.</param>
        /// <param name="handler">The handler to execute the command when no arguments are passed.</param>
        /// <returns>True if command was registered; otherwise false.</returns>
        public bool Register(string command, string description, DefaultConsoleCommandHandler handler)
        {
            return this.Register(command.ToLower(), description, new string[0], null, handler);
        }

        /// <summary>
        /// Registers the specified command. If the handler for zero arguments is not null, it will be used whenever zero arguments are provided.
        /// </summary>
        /// <param name="command">The command to register.</param>
        /// <param name="arguments">The amount of arguments.</param>
        /// <param name="handler">The handler to execute the command when arguments are passed. May be null.</param>
        /// <param name="defaultHandler">The handler for executing the command provided zero arguments. May be null.</param>
        /// <returns>True if command was registered; otherwise false.</returns>
        public bool Register(string command, int arguments, ConsoleCommandHandler handler, DefaultConsoleCommandHandler defaultHandler)
        {
            return this.Register(command.ToLower(), null, arguments, handler, defaultHandler);
        }

        /// <summary>
        /// Registers the specified command. If the handler for zero arguments is not null, it will be used whenever zero arguments are provided.
        /// </summary>
        /// <param name="command">The command to register.</param>
        /// <param name="arguments">The amount of arguments.</param>
        /// <param name="handler">The handler to execute the command when arguments are passed.</param>
        /// <returns>True if command was registered; otherwise false.</returns>
        public bool Register(string command, int arguments, ConsoleCommandHandler handler)
        {
            return this.Register(command.ToLower(), null, arguments, handler, null);
        }

        /// <summary>
        /// Registers the specified command. If the handler for zero arguments is not null, it will be used whenever zero arguments are provided.
        /// </summary>
        /// <param name="command">The command to register.</param>
        /// <param name="description">The description of the command. May be null.</param>
        /// <param name="arguments">The amount of arguments.</param>
        /// <param name="handler">The handler to execute the command when arguments are passed.</param>
        /// <returns>True if command was registered; otherwise false.</returns>
        public bool Register(string command, string description, int arguments, ConsoleCommandHandler handler)
        {
            return this.Register(command.ToLower(), description, arguments, handler, null);
        }

        /// <summary>
        /// Registers the specified command. If the handler for zero arguments is not null, it will be used whenever zero arguments are provided.
        /// </summary>
        /// <param name="command">The command to register.</param>
        /// <param name="description">The description of the command. May be null.</param>
        /// <param name="arguments">The amount of arguments.</param>
        /// <param name="handler">The handler to execute the command when arguments are passed. May be null.</param>
        /// <param name="defaultHandler">The handler for executing the command provided zero arguments. May be null.</param>
        /// <returns>True if command was registered; otherwise false.</returns>
        public bool Register(string command, string description, int arguments, ConsoleCommandHandler handler, DefaultConsoleCommandHandler defaultHandler)
        {
            return this.Register(command.ToLower(), description, new string[arguments], handler, defaultHandler);
        }

        /// <summary>
        /// Registers the specified command. If the handler for zero arguments is not null, it will be used whenever zero arguments are provided.
        /// </summary>
        /// <param name="command">The command to register.</param>
        /// <param name="description">The description of the command. May be null.</param>
        /// <param name="arguments">The argument descriptions. A count of zero means zero arguments.</param>
        /// <param name="handler">The handler to execute the command when arguments are passed.</param>
        /// <returns>True if command was registered; otherwise false.</returns>
        public bool Register(string command, string description, ICollection<string> arguments,
            ConsoleCommandHandler handler)
        {
            return this.Register(command.ToLower(), description, arguments, handler, null);
        }

        /// <summary>
        /// Registers the specified command. If the handler for zero arguments is not null, it will be used whenever zero arguments are provided.
        /// </summary>
        /// <param name="command">The command to register.</param>
        /// <param name="description">The description of the command. May be null.</param>
        /// <param name="arguments">The argument descriptions. A count of zero means zero arguments.</param>
        /// <param name="handler">The handler to execute the command when arguments are passed. May be null.</param>
        /// <param name="defaultHandler">The handler for executing the command provided zero arguments. May be null.</param>
        /// <returns>True if command was registered; otherwise false.</returns>
        public bool Register(string command, string description, ICollection<string> arguments,
            ConsoleCommandHandler handler, DefaultConsoleCommandHandler defaultHandler)
        {
            command = command.ToLower();
            if (this.dictionary.ContainsKey(command) == false)
            {
                this.dictionary.Add(command, new Command(command, description, arguments, handler, defaultHandler));
                return true;
            }

            return false;
        }

        /// <summary>
        /// Unregisters the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>True if command was unregistered; otherwise false.</returns>
        public bool Unregister(string command)
        {
            return this.dictionary.Remove(command.ToLower());
        }

        /// <summary>
        /// Container for console command information.
        /// </summary>
        public class Command
        {
            private ICollection<string> arguments;

            /// <summary>
            /// Initializes a new instance of the <see cref="Command"/> class.
            /// </summary>
            /// <param name="name">The name of the command.</param>
            /// <param name="description">Description of the command. May be null.</param>
            /// <param name="arguments">The argument descriptions. A count of zero means zero arguments.</param>
            /// <param name="handler">The handler to execute the command when arguments are passed. May be null.</param>
            /// <param name="defaultHandler">Handler to use if zero arguments are provided. May be null.</param>
            public Command(string name, string description, ICollection<string> arguments, ConsoleCommandHandler handler, DefaultConsoleCommandHandler defaultHandler)
            {
                if (name == null)
                {
                    throw new ArgumentNullException("Name may not be null.");
                }
                if (arguments == null)
                {
                    throw new ArgumentNullException("Arguments may not be null.");
                }
                if(arguments.Count != 0 && handler == null || arguments.Count == 0 && Handler != null)
                {
                    throw new ArgumentException("Mismatch between arguments and handler. Either provide no handler and zero arguments or a handler and non-zero arguments.");
                }
                if(handler == null && defaultHandler == null)
                {
                    throw new ArgumentException("Both handler and default handler may not be null.");
                }

                this.Name = name;
                this.Handler = handler;
                this.DefaultHandler = defaultHandler;
                this.arguments = arguments;
                this.Description = description;
            }

            /// <summary>
            /// Gets the amount of arguments.
            /// </summary>
            /// <value>
            /// The amount of arguments.
            /// </value>
            public int ArgumentLength { get { return this.Arguments.Count; } }

            /// <summary>
            /// Gets the arguments.
            /// </summary>
            /// <value>
            /// The arguments.
            /// </value>
            public ICollection<string> Arguments { get { return new List<string>(this.arguments); } }

            /// <summary>
            /// Gets the default handler to execute upon no provided arguments.
            /// </summary>
            /// <value>
            /// The default handler.
            /// </value>
            public DefaultConsoleCommandHandler DefaultHandler { get; private set; }

            /// <summary>
            /// Gets or sets the description.
            /// </summary>
            /// <value>
            /// The description.
            /// </value>
            public string Description { get; private set; }

            /// <summary>
            /// Gets the handler to execute the command.
            /// </summary>
            /// <value>
            /// The handler.
            /// </value>
            public ConsoleCommandHandler Handler { get; private set; }

            /// <summary>
            /// Gets the name of the command.
            /// </summary>
            /// <value>
            /// The name.
            /// </value>
            public string Name { get; private set; }
        }
    }
}