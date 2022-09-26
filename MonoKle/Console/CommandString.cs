using System.Collections.Generic;
using System.Text;

namespace MonoKle.Console
{
    public class CommandString
    {
        private const char JOIN_CHARACTER = '"';
        private const char SPLITTER_CHARACTER = ' ';
        private const char FLAG_CHARACTER = '-';

        /// <summary>
        /// Gets the command to execute.
        /// </summary>
        public string Command { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the positional arguments to the command.
        /// </summary>
        public IReadOnlyList<string> PositionalArguments => positionalArguments;
        private readonly List<string> positionalArguments = new(0);

        /// <summary>
        /// Gets the flags that are set for the command.
        /// </summary>
        public IReadOnlyCollection<string> Flags => flags;
        private readonly HashSet<string> flags = new(0);

        /// <summary>
        /// Gets the named arguments that are set for the command.
        /// </summary>
        public IReadOnlyDictionary<string, string> NamedArguments => namedArguments;
        private readonly Dictionary<string, string> namedArguments = new();

        public static bool TryParse(string line, out CommandString commandString)
        {
            CommandString builtString = new();

            if (line.Length == 0)
            {
                return FailedParseResult(out commandString);
            }

            bool hasCommand = false;
            bool positionalsOver = false;
            string namedArgument = "";
            bool inNamedArgument = false;

            foreach (var token in GetParseTokens(line))
            {
                // Flag character indicates we are done with positional arguments
                if (token.StartsWith(FLAG_CHARACTER))
                {
                    if (!hasCommand || token.Length == 1)
                    {
                        // Invalid state if no command was given before flag token
                        // Invalid token if it is only a flag character
                        return FailedParseResult(out commandString);
                    }
                    positionalsOver = true;
                }

                if (!hasCommand)
                {
                    // First token is always command
                    builtString.Command = token;
                    hasCommand = true;
                }
                else if (positionalsOver)
                {
                    // Flag/named tokens
                    // Check if we already have gotten a name and should add its value
                    if (inNamedArgument)
                    {
                        if (token.StartsWith(FLAG_CHARACTER))
                        {
                            if (!TryAddFlag(builtString, namedArgument))
                            {
                                return FailedParseResult(out commandString);
                            }
                            namedArgument = token.Remove(0, 1);
                        }
                        else
                        {
                            if (!TryAddNamedArgument(builtString, namedArgument, token))
                            {
                                return FailedParseResult(out commandString);
                            }
                            inNamedArgument = false;
                        }
                    }
                    else
                    {
                        namedArgument = token.Remove(0, 1);
                        inNamedArgument = true;
                    }
                }
                else
                {
                    // Any token until flag/named tokens
                    builtString.positionalArguments.Add(token);
                }
            }

            // If our last named argument was not finished, treat it as a flag
            if (inNamedArgument)
            {
                if (!TryAddFlag(builtString, namedArgument))
                {
                    return FailedParseResult(out commandString);
                }
            }

            commandString = builtString;
            return true;
        }

        /// <summary>
        /// Helper that returns false and sets the default out value.
        /// </summary>
        private static bool FailedParseResult(out CommandString commandString)
        {
            commandString = new CommandString();
            return false;
        }

        /// <summary>
        /// Helper that tries to add a flag and returns whether it was successful or not.
        /// </summary>
        private static bool TryAddFlag(CommandString commandString, string flag)
        {
            if (HasKey(commandString, flag))
            {
                return false;
            }
            commandString.flags.Add(flag);
            return true;
        }

        /// <summary>
        /// Helper that tries to add a flag and returns whether it was successful or not.
        /// </summary>
        private static bool TryAddNamedArgument(CommandString commandString, string argument, string value)
        {
            if (HasKey(commandString, argument))
            {
                return false;
            }
            commandString.namedArguments[argument] = value;
            return true;
        }

        private static bool HasKey(CommandString commandString, string key) => commandString.namedArguments.ContainsKey(key) || commandString.flags.Contains(key);

        /// <summary>
        /// Helper that consumes each character one by one to produce each individual input token.
        /// </summary>
        private static IEnumerable<string> GetParseTokens(string line)
        {
            StringBuilder builder = new();
            bool inToken = false;
            foreach (char c in line)
            {
                // Split when not in a single token
                if (c == SPLITTER_CHARACTER && !inToken)
                {
                    if (builder.Length > 0)
                    {
                        yield return builder.ToString();
                        builder.Clear();
                    }
                }
                else if (c == JOIN_CHARACTER)
                {
                    // Toggle that we are in a single token
                    inToken = !inToken;
                }
                else
                {
                    builder.Append(c);
                }
            }

            if (builder.Length > 0)
            {
                yield return builder.ToString();
            }
        }
    }
}