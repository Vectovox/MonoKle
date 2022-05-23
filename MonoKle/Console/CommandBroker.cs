using MoreLinq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace MonoKle.Console
{
    /// <summary>
    /// Broker for console command registration and execution.
    /// </summary>
    public sealed class CommandBroker
    {
        private readonly IGameConsole _console;
        private readonly Dictionary<string, IConsoleCommand> _typeCommands = new Dictionary<string, IConsoleCommand>();

        /// <summary>
        /// Gets the registered commands.
        /// </summary>
        public IEnumerable<string> Commands => _typeCommands.Keys;

        public CommandBroker(IGameConsole console) => _console = console;

        /// <summary>
        /// Interprets the provided string and runs the appropriate command.
        /// </summary>
        /// <param name="commandString">The command string to call.</param>
        /// <returns>True if successfully called; false if string was not parseable or command failed.</returns>
        public bool Call(string commandString) => CommandString.TryParse(commandString, out var command) && Call(command);

        /// <summary>
        /// Calls the appropriate command given the specified command string.
        /// </summary>
        /// <param name="commandString">The command string to call.</param>
        /// <returns>True if the command was called succesfully; otherwise false.</returns>
        public bool Call(CommandString commandString)
        {
            var instance = GetCommandInstance(commandString.Command);

            // Check that the command exists
            if (instance == null)
            {
                _console.WriteError($"{commandString.Command} is not a recognized command.");
                return false;
            }

            if (!AssignArguments(instance, commandString))
            {
                _console.WriteError($"Arguments not provided correctly.");
                return false;
            }

            try
            {
                instance.Call(_console);
            }
            catch (Exception e)
            {
                _console.WriteError($"Command {commandString.Command} returned an error: {e.Message}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Calls the given command, using the provided positionals.
        /// </summary>
        /// <remarks>
        /// The positionals will be evaluated using <see cref="object.ToString"/>.
        /// </remarks>
        /// <param name="command">The identifier of the command to call.</param>
        /// <param name="positionals">The positional arguments to call with.</param>
        /// <returns>True if the command was called succesfully; otherwise false.</returns>
        public bool Call(string command, params object[] positionals) => Call($"{command} {string.Join(' ', positionals)}");

        /// <summary>
        /// Clears all registered commands.
        /// </summary>
        public void Clear() => _typeCommands.Clear();

        /// <summary>
        /// Returns whether the provided command has been registered.
        /// </summary>
        /// <param name="command">The command to check for.</param>
        public bool Contains(string command) => _typeCommands.ContainsKey(command);

        /// <summary>
        /// Gets the specified command if it exists, otherwise null.
        /// </summary>
        /// <param name="command">The command to retrieve.</param>
        public ICollection<string> GetPositionalSuggestions(string command) =>
            GetCommandInstance(command)?.GetPositionalSuggestions() ?? Array.Empty<string>();

        /// <summary>
        /// Gets information about the provided command.
        /// </summary>
        /// <param name="command">The command to get information for.</param>
        public CommandInformation GetInformation(string command)
        {
            var instance = GetCommandInstance(command);
            if (instance == null)
            {
                throw new ArgumentException($"Command {command} is not valid.");
            }

            var type = instance.GetType();

            return new CommandInformation(GetCommandAttribute(type), GetPositionals(type).Select(p => p.Argument).ToList(),
                GetFlags(type).Select(p => p.Argument).ToList(), GetArguments(type).Select(p => p.Argument).ToList());
        }

        /// <summary>
        /// Registers all valid <see cref="IConsoleCommand"/> types in the calling assembly for execution.
        /// </summary>
        /// <returns>The amount of registered commands.</returns>
        public int RegisterCallingAssembly() => Assembly.GetCallingAssembly().GetTypes().Where(IsInstantiableType).Sum(command =>
        {
            try
            {
                Register(command);
            }
            catch
            {
                return 0;
            }
            return 1;
        });

        /// <summary>
        /// Registers the specified command for execution. Type must be of <see cref="IConsoleCommand"/>.
        /// </summary>
        /// <param name="type">The type to register.</param>
        /// <exception cref="ArgumentException"></exception>
        public void Register(Type type)
        {
            AssertInstantiable(type);
            var commandInstance = (IConsoleCommand)Activator.CreateInstance(type);
            Register(commandInstance);
        }

        /// <summary>
        /// Registers the specified command for execution. Type must be of <see cref="IConsoleCommand"/>.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public void Register<T>() where T : IConsoleCommand, new() => Register(typeof(T));

        /// <summary>
        /// Registers the specified command for execution.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public void Register(IConsoleCommand command)
        {
            var type = command.GetType();
            var commandAttribute = GetCommandAttribute(type);

            // Sanity check command
            if (commandAttribute == null)
            {
                throw new ArgumentException($"Tried to register command {type.Name} without the {nameof(ConsoleCommandAttribute)} attribute.");
            }

            // Check that positionals have no gaps
            int positionalCounter = 0;
            bool wasRequired = true;
            foreach (var positional in GetPositionals(type).OrderBy(positional => positional.Argument.Position))
            {
                if (positional.Argument.Position != positionalCounter++)
                {
                    throw new ArgumentException($"Expected positional argument on position {positionalCounter} in command {commandAttribute.Name}.");
                }

                if (!wasRequired && positional.Argument.IsRequired)
                {
                    throw new ArgumentException($"Required positional argument on position {positionalCounter} followed an optional in command {commandAttribute.Name}.");
                }

                wasRequired = positional.Argument.IsRequired;
            }

            if (_typeCommands.ContainsKey(commandAttribute.Name))
            {
                throw new ArgumentException($"A command already exists with the name {commandAttribute.Name}.");
            }

            _typeCommands.Add(commandAttribute.Name, command);
        }

        /// <summary>
        /// Unregisters the provided command type.
        /// </summary>
        /// <param name="type">The type to unregister.</param>
        /// <returns>True if unregistered, otherwise false.</returns>
        /// <exception cref="ArgumentException"></exception>
        public bool Unregister(Type type)
        {
            if (!typeof(IConsoleCommand).IsAssignableFrom(type))
            {
                throw new ArgumentException($"Type not of {nameof(IConsoleCommand)}.");
            }

            var commandAttribute = GetCommandAttribute(type);

            if (commandAttribute == null)
            {
                throw new ArgumentException($"Tried to unregister command {type.Name} without the {nameof(ConsoleCommandAttribute)} attribute.");
            }

            return _typeCommands.Remove(commandAttribute.Name);
        }

        private void AssertInstantiable(Type type)
        {
            if (!IsInstantiableType(type))
            {
                throw new ArgumentException($"Type must be concrete implementation of {nameof(IConsoleCommand)} with parameterless constructor.");
            }
        }

        private bool IsInstantiableType(Type type) =>
            !type.IsAbstract && type.IsClass && type.GetConstructor(Type.EmptyTypes) != null && typeof(IConsoleCommand).IsAssignableFrom(type);

        /// <summary>
        /// Unregisters the provided command type.
        /// </summary>
        /// <returns>True if unregistered, otherwise false.</returns>
        public bool Unregister<T>() where T : IConsoleCommand, new() => Unregister(typeof(T));

        private IConsoleCommand? GetCommandInstance(string command) => _typeCommands.ContainsKey(command)
            ? _typeCommands[command]
            : null;

        private ConsoleCommandAttribute GetCommandAttribute(Type type) => (ConsoleCommandAttribute)type.GetCustomAttributes(typeof(ConsoleCommandAttribute), false)
                                                                                                       .FirstOrDefault();

        private IEnumerable<ArgumentProperty<T>> GetArgumentProperties<T>(Type type) =>
            type.GetProperties()
                .Select(prop => (prop, prop.GetCustomAttributes(typeof(T), false).Cast<T>()))
                .Where(t => t.Item2.Any())
                .Select(t => new ArgumentProperty<T>(t.prop, t.Item2.Single()));

        private IEnumerable<ArgumentProperty<ConsolePositionalAttribute>> GetPositionals(Type type) => GetArgumentProperties<ConsolePositionalAttribute>(type);
        private IEnumerable<ArgumentProperty<ConsoleFlagAttribute>> GetFlags(Type type) => GetArgumentProperties<ConsoleFlagAttribute>(type);
        private IEnumerable<ArgumentProperty<ConsoleArgumentAttribute>> GetArguments(Type type) => GetArgumentProperties<ConsoleArgumentAttribute>(type);

        private bool AssignArguments(IConsoleCommand command, CommandString commandString)
        {
            var type = command.GetType();
            var flags = GetFlags(type).ToList();
            var positionals = GetPositionals(type).ToList();
            var namedArguments = GetArguments(type).ToList();

            // Assign positional arguments
            foreach (var positional in positionals)
            {
                bool argumentExists = commandString.PositionalArguments.Count > positional.Argument.Position;

                if (!argumentExists)
                {
                    if (positional.Argument.IsRequired)
                    {
                        _console.WriteError($"Required argument on position '{positional.Argument.Position}' not found.");
                        return false;
                    }

                    // Assign default value
                    AssignDefaultArgument(command, positional.Property);
                }

                if (argumentExists && !AssignArgument(command, positional.Property, commandString.PositionalArguments[positional.Argument.Position]))
                {
                    _console.WriteError($"Could not assign '{commandString.PositionalArguments[positional.Argument.Position]}' to argument on position '{positional.Argument.Position}'.");
                    return false;
                }
            }

            // Assign flags
            flags.Where(flag => commandString.Flags.Contains(flag.Argument.Name)).ForEach(flag => AssignArgument(command, flag.Property, "true"));

            // Assign named arguments
            foreach (var argument in namedArguments)
            {
                bool namedArgumentExists = commandString.NamedArguments.ContainsKey(argument.Argument.Name);

                if (!namedArgumentExists)
                {
                    if (argument.Argument.IsRequired)
                    {
                        _console.WriteError($"Required argument '{argument.Argument.Name}' not found.");
                        return false;
                    }

                    // Assign default value
                    AssignDefaultArgument(command, argument.Property);
                }

                if (namedArgumentExists && !AssignArgument(command, argument.Property, commandString.NamedArguments[argument.Argument.Name]))
                {
                    _console.WriteError($"Could not assign '{commandString.NamedArguments[argument.Argument.Name]}' to '{argument.Argument.Name}'.");
                    return false;
                }
            }

            // Check for unknown arguments provided
            if (commandString.NamedArguments.Keys.Any(name => !namedArguments.Any(prop => prop.Argument.Name == name)) ||
                commandString.Flags.Any(name => !flags.Any(prop => prop.Argument.Name == name)) ||
                positionals.Any() && commandString.PositionalArguments.Count > positionals.Max(pos => pos.Argument.Position) + 1)
            {
                _console.WriteError("Unknown arguments provided.");
                return false;
            }

            return true;
        }

        private void AssignDefaultArgument(IConsoleCommand instance, PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType == typeof(int))
            {
                propertyInfo.SetValue(instance, default(int));
            }
            else if (propertyInfo.PropertyType == typeof(float))
            {
                propertyInfo.SetValue(instance, default(float));
            }
            else if (propertyInfo.PropertyType == typeof(bool))
            {
                propertyInfo.SetValue(instance, default(bool));
            }
            else if (propertyInfo.PropertyType.IsValueType)
            {
                propertyInfo.SetValue(instance, Activator.CreateInstance(propertyInfo.PropertyType));
            }
            else
            {
                propertyInfo.SetValue(instance, null);
            }
        }

        private bool AssignArgument(IConsoleCommand instance, PropertyInfo propertyInfo, string value)
        {
            if (propertyInfo.PropertyType == typeof(int) && int.TryParse(value, out int intResult))
            {
                propertyInfo.SetValue(instance, intResult);
                return true;
            }
            else if (propertyInfo.PropertyType == typeof(float) && float.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out float floatResult))
            {
                propertyInfo.SetValue(instance, floatResult);
                return true;
            }
            else if (propertyInfo.PropertyType == typeof(bool) && bool.TryParse(value, out bool boolResult))
            {
                propertyInfo.SetValue(instance, boolResult);
                return true;
            }
            else if (propertyInfo.PropertyType == typeof(string))
            {
                propertyInfo.SetValue(instance, value);
                return true;
            }
            else if (propertyInfo.PropertyType.IsValueType)
            {
                try
                {
                    var typeConverter = TypeDescriptor.GetConverter(propertyInfo.PropertyType);
                    object converted = typeConverter.ConvertFromInvariantString(null, value);
                    propertyInfo.SetValue(instance, converted);
                    return true;
                }
                catch (Exception)
                {
                }
            }

            return false;
        }

        private class ArgumentProperty<T>
        {
            public PropertyInfo Property { get; }
            public T Argument { get; }

            public ArgumentProperty(PropertyInfo property, T argument)
            {
                Property = property;
                Argument = argument;
            }
        }

        /// <summary>
        /// Class containing the attributes of a command.
        /// </summary>
        public class CommandInformation
        {
            /// <summary>
            /// Gets the command information itself.
            /// </summary>
            public ConsoleCommandAttribute Command { get; }

            /// <summary>
            /// Gets the positional argument attributes.
            /// </summary>
            public IReadOnlyList<ConsolePositionalAttribute> Positionals { get; }

            /// <summary>
            /// Gets the flag attributes.
            /// </summary>
            public IReadOnlyList<ConsoleFlagAttribute> Flags { get; }

            /// <summary>
            /// Gets the named argument attributes.
            /// </summary>
            public IReadOnlyList<ConsoleArgumentAttribute> Arguments { get; }

            /// <summary>
            /// Gets the amount of total options;
            /// </summary>
            public int TotalOptions => Positionals.Count + Flags.Count + Arguments.Count;

            public CommandInformation(ConsoleCommandAttribute command, IReadOnlyList<ConsolePositionalAttribute> positionals, IReadOnlyList<ConsoleFlagAttribute> flags, IReadOnlyList<ConsoleArgumentAttribute> arguments)
            {
                Command = command;
                Positionals = positionals;
                Flags = flags;
                Arguments = arguments;
            }
        }
    }
}