using MonoKle.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoKle.Engine.Console.Commands
{
    [ConsoleCommand("help", Description = "Provides help about a given commands. If no argument is passed, a list of commands will be provided.")]
    public class HelpCommand : IConsoleCommand
    {
        [ConsolePositional(0, Description = "Command to get help for.")]
        public string Command { get; set; } = string.Empty;

        public void Call(IGameConsole console)
        {
            if (Command.Length == 0)
            {
                PrintHelpList(console);
            }
            else
            {
                PrintHelp(console, Command);
            }
        }

        public ICollection<string> GetPositionalSuggestions() => new string[0];

        private static void PrintHelp(IGameConsole console, string command)
        {
            if (console.CommandBroker.Contains(command))
            {
                var commandInfo = console.CommandBroker.GetInformation(command);

                // First line describes usage example of all possible options
                var usageBuilder = new StringBuilder($"{commandInfo.Command.Name}: {commandInfo.Command.Name} ");

                foreach (var positional in commandInfo.Positionals)
                {
                    usageBuilder.Append($"[{positional.Position}]");
                    usageBuilder.Append(" ");
                }

                foreach (var argument in commandInfo.Arguments)
                {
                    usageBuilder.Append($"-{argument.Name}");
                    usageBuilder.Append(" val ");
                }

                foreach (var flag in commandInfo.Flags)
                {
                    usageBuilder.Append($"-{flag.Name}");
                    usageBuilder.Append(" ");
                }

                // Now start indented detailed information of desciption and each option
                usageBuilder.AppendLine();
                usageBuilder.Append($"\t{commandInfo.Command.Description}");

                // Record the longest option length for later indentation
                var longestOptionLength = 0;
                if (commandInfo.Positionals.Count > 0)
                {
                    longestOptionLength = Math.Max(longestOptionLength, commandInfo.Positionals.Select(p => p.Position.ToString().Length).Max());
                }
                if (commandInfo.Arguments.Count > 0)
                {
                    longestOptionLength = Math.Max(longestOptionLength, commandInfo.Arguments.Max(n => n.Name.Length));
                }
                if (commandInfo.Flags.Count > 0)
                {
                    longestOptionLength = Math.Max(longestOptionLength, commandInfo.Flags.Max(n => n.Name.Length));
                }
                usageBuilder.AppendLine();

                if (commandInfo.Positionals.Count > 0)
                {
                    var longestPositionalLength = commandInfo.Positionals.Select(p => p.Position.ToString().Length).Max();

                    usageBuilder.AppendLine();
                    usageBuilder.AppendLine("\tPositional arguments:");
                    foreach (var positional in commandInfo.Positionals)
                    {
                        usageBuilder.Append("\t\t");
                        usageBuilder.Append(' ', longestOptionLength - positional.Position.ToString().Length);
                        usageBuilder.Append($" {positional.Position}\t{positional.Description}");
                        if (!positional.IsRequired)
                        {
                            usageBuilder.Append(" (optional)");
                        }
                        usageBuilder.AppendLine();
                    }
                }
                else if (commandInfo.Arguments.Count > 0)
                {
                    usageBuilder.AppendLine();
                }

                if (commandInfo.Arguments.Count > 0)
                {
                    var longestNamedlength = commandInfo.Arguments.Max(n => n.Name.Length);

                    usageBuilder.AppendLine();
                    usageBuilder.AppendLine("\tArguments:");
                    foreach (var named in commandInfo.Arguments)
                    {
                        usageBuilder.Append("\t\t");
                        usageBuilder.Append(new string(' ', longestOptionLength - named.Name.Length));
                        usageBuilder.Append($"-{named.Name}\t{named.Description}");
                        if (!named.IsRequired)
                        {
                            usageBuilder.Append(" (optional)");
                        }
                        usageBuilder.AppendLine();
                    }
                }
                else if (commandInfo.Flags.Count > 0)
                {
                    usageBuilder.AppendLine();
                }

                if (commandInfo.Flags.Count > 0)
                {
                    var longestFlagLength = commandInfo.Flags.Max(n => n.Name.Length);

                    usageBuilder.AppendLine();
                    usageBuilder.AppendLine("\tFlags:");
                    foreach (var flag in commandInfo.Flags)
                    {
                        usageBuilder.Append("\t\t");
                        usageBuilder.Append(new string(' ', longestOptionLength - flag.Name.Length));
                        usageBuilder.Append($"-{flag.Name}\t{flag.Description}");
                        usageBuilder.AppendLine();
                    }
                }

                console.WriteLine(usageBuilder.ToString());
            }
            else
            {
                console.WriteLine("There is no such command to get help for.");
            }
        }

        private static void PrintHelpList(IGameConsole console)
        {
            console.WriteLine("These are the registered commands. Type 'help' to see this list.");
            console.WriteLine("For more information on a specific command, type 'help [command]'.");

            var commands = console.CommandBroker.Commands.OrderBy(c => c).ToList();

            console.WriteLine("");

            // Go over all commands in order
            var longestCommandLength = commands.Max(o => o.Length);
            var sb = new StringBuilder();
            foreach (var command in commands)
            {
                // Reuse stringbuilder per each command
                sb.Clear();

                sb.Append("\t");
                sb.Append(new string(' ', longestCommandLength - command.Length));
                sb.Append(command);
                sb.Append(" \t");
                sb.Append(console.CommandBroker.GetInformation(command).Command.Description);

                // Print to console
                console.WriteLine(sb.ToString());
            }

            // Add newline
            console.WriteLine("");
        }
    }
}
