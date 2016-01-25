namespace MonoKle.Console
{
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
        public ICollection<string> Commands { get { return this.dictionary.Keys; } }

        /// <summary>
        /// Calls the specified command. Verifies a correct arguments length.
        /// </summary>
        /// <param name="command">The command to call.</param>
        /// <returns>True if command is registered and correct argument length was provided; otherwise false.</returns>
        public bool Call(string command)
        {
            if (this.dictionary.ContainsKey(command))
            {
                Command c = this.dictionary[command];
                if (c.ArgumentLenth == 0)
                {
                    c.Handler(new string[0]);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Calls the specified command. Verifies a correct arguments length.
        /// </summary>
        /// <param name="command">The command to call.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>True if command is registered and correct argument length was provided; otherwise false.</returns>
        public bool Call(string command, string[] arguments)
        {
            if (this.dictionary.ContainsKey(command))
            {
                Command c = this.dictionary[command];
                if (c.ArgumentLenth == arguments.Length)
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
        /// Registers the specified argumentless command.
        /// </summary>
        /// <param name="command">The command to register.</param>
        /// <param name="handler">The handler to execute the command.</param>
        /// <returns>True if command was registered; otherwise false.</returns>
        public bool Register(string command, ConsoleCommandHandler handler)
        {
            return this.Register(command, 0, handler);
        }

        /// <summary>
        /// Registers the specified command.
        /// </summary>
        /// <param name="command">The command to register.</param>
        /// <param name="argumentLength">Amount of arguments to provide.</param>
        /// <param name="handler">The handler to execute the command.</param>
        /// <returns>True if command was registered; otherwise false.</returns>
        public bool Register(string command, byte argumentLength, ConsoleCommandHandler handler)
        {
            if (this.dictionary.ContainsKey(command) == false)
            {
                this.dictionary.Add(command, new Command() { Handler = handler, ArgumentLenth = argumentLength });
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
            return this.dictionary.Remove(command);
        }

        private class Command
        {
            public byte ArgumentLenth;
            public ConsoleCommandHandler Handler;
        }
    }
}