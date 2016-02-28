namespace MonoKle.Console
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Broker for console commands.
    /// </summary>
    public class CommandBroker
    {
        private Dictionary<string, IConsoleCommand> dictionary = new Dictionary<string, IConsoleCommand>();

        /// <summary>
        /// Gets the registered commands.
        /// </summary>
        /// <value>
        /// The registered commands.
        /// </value>
        public ICollection<IConsoleCommand> Commands { get { return this.dictionary.Values; } }

        /// <summary>
        /// Calls the specified command without arguments.
        /// </summary>
        /// <param name="command">The command to call.</param>
        /// <returns>True if command is registered and correct argument length was provided; otherwise false.</returns>
        public bool Call(string command)
        {
            return this.Call(command, new string[0]);
        }

        /// <summary>
        /// Calls the specified command with the given arguments.
        /// </summary>
        /// <param name="command">The command to call.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>True if command is registered and correct argument length was provided; otherwise false.</returns>
        public bool Call(string command, string[] arguments)
        {
            command = command.ToLower();
            if (this.dictionary.ContainsKey(command))
            {
                IConsoleCommand c = this.dictionary[command];
                return c.Call(arguments);
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
        public IConsoleCommand GetCommand(string command)
        {
            command = command.ToLower();
            if (this.dictionary.ContainsKey(command))
            {
                return this.dictionary[command];
            }
            return null;
        }

        /// <summary>
        /// Registers the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        public bool Register(IConsoleCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("Command must not be null.");
            }

            if (this.dictionary.ContainsKey(command.Name) == false)
            {
                this.dictionary.Add(command.Name, command);
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
        /// Unregisters the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>True if command was unregistered; otherwise false.</returns>
        public bool Unregister(IConsoleCommand command)
        {
            return this.Unregister(command.Name);
        }
    }
}