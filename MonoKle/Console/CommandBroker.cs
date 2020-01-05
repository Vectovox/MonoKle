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
        public ICollection<IConsoleCommand> Commands => dictionary.Values;

        /// <summary>
        /// Calls the specified command without arguments.
        /// </summary>
        /// <param name="command">The command to call.</param>
        /// <returns>True if command is registered and correct argument length was provided; otherwise false.</returns>
        public bool Call(string command) => Call(command, new string[0]);

        /// <summary>
        /// Calls the specified command with the given arguments.
        /// </summary>
        /// <param name="command">The command to call.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>True if command is registered and correct argument length was provided; otherwise false.</returns>
        public bool Call(string command, string[] arguments)
        {
            command = command.ToLower();
            if (dictionary.ContainsKey(command))
            {
                IConsoleCommand c = dictionary[command];
                return c.Call(arguments);
            }
            return false;
        }

        /// <summary>
        /// Clears all registered commands.
        /// </summary>
        public void Clear() => dictionary.Clear();

        /// <summary>
        /// Gets the specified command if it exists, otherwise null.
        /// </summary>
        /// <param name="command">The command to retrieve.</param>
        /// <returns></returns>
        public IConsoleCommand GetCommand(string command)
        {
            command = command.ToLower();
            if (dictionary.ContainsKey(command))
            {
                return dictionary[command];
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

            if (dictionary.ContainsKey(command.Name) == false)
            {
                dictionary.Add(command.Name, command);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Unregisters the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>True if command was unregistered; otherwise false.</returns>
        public bool Unregister(string command) => dictionary.Remove(command.ToLower());

        /// <summary>
        /// Unregisters the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>True if command was unregistered; otherwise false.</returns>
        public bool Unregister(IConsoleCommand command) => Unregister(command.Name);
    }
}