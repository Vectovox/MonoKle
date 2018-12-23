using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoKle.Asset.Font;
using MonoKle.Attributes;
using MonoKle.Input.Keyboard;
using MonoKle.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoKle.Console {
    /// <summary>
    /// Class that maintains and displays a console.
    /// </summary>
    public class GameConsole : IGameConsole, IUpdateable, IDrawable {
        private static readonly TimeSpan TypingActivationDelay = TimeSpan.FromMilliseconds(400);
        private static readonly TimeSpan TypingCycleDelay = TimeSpan.FromMilliseconds(2);

        private int scrollOffset = 0;
        private GraphicsDevice graphicsDevice;
        private InputField inputField;
        private KeyboardTyper keyboard;
        private LinkedList<Line> lines = new LinkedList<Line>();
        private SpriteBatch spriteBatch;
        private Texture2D whiteTexture;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameConsole"/> class.
        /// </summary>
        /// <param name="area">The area.</param>
        /// <param name="graphicsDevice">The graphics device.</param>
        /// <param name="keyboardInput">The keyboard input.</param>
        /// <param name="whiteTexture">The background texture.</param>
        /// <param name="logger">The logger to use.</param>
        public GameConsole(MRectangleInt area, GraphicsDevice graphicsDevice, IKeyboard keyboardInput, Texture2D whiteTexture, Logger logger) {
            this.graphicsDevice = graphicsDevice;
            spriteBatch = new SpriteBatch(graphicsDevice);
            keyboard = new KeyboardTyper(keyboardInput, GameConsole.TypingActivationDelay, GameConsole.TypingCycleDelay);

            inputField = new InputField("_", ">> ", 0.25, 10, new KeyboardCharacterInput(keyboard));

            Area = area;
            Size = byte.MaxValue;
            this.whiteTexture = whiteTexture;
            BackgroundColor = new Color(0, 0, 0, 0.7f);
            DefaultTextColour = Color.WhiteSmoke;
            WarningTextColour = Color.Yellow;
            ErrorTextColour = Color.Red;
            CommandTextColour = Color.LightGreen;
            DisabledTextColour = Color.Gray;
            TextScale = 0.5f;
            TabLength = 4;
            logger.LogAddedEvent += LogAdded;
            logger.Log("GameConsole activated.", LogLevel.Debug);

            SetupBroker();
        }

        /// <summary>
        /// Gets or sets the area in which the console will be drawn.
        /// </summary>
        public MRectangleInt Area {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        public Color BackgroundColor {
            get;
            set;
        }

        /// <summary>
        /// Gets the command broker. Used for executing console commands.
        /// </summary>
        /// <value>
        /// The command broker.
        /// </value>
        public CommandBroker CommandBroker {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the command text colour.
        /// </summary>
        /// <value>
        /// The command text colour.
        /// </value>
        public Color CommandTextColour {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the color that the text will be drawn with if no other colour is specified.
        /// </summary>
        public Color DefaultTextColour {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the disabled text colour.
        /// </summary>
        /// <value>
        /// The disabled text colour.
        /// </value>
        public Color DisabledTextColour {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the error text colour.
        /// </summary>
        /// <value>
        /// The error text colour.
        /// </value>
        public Color ErrorTextColour {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets wether the console is open.
        /// </summary>
        [PropertyVariable("c_isopen")]
        public bool IsOpen {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the maximum amount of entries to keep.
        /// </summary>
        [PropertyVariable("c_size")]
        public int Size {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the length of the tabs.
        /// </summary>
        /// <value>
        /// The length of the tabs.
        /// </value>
        public int TabLength {
            get; set;
        }

        /// <summary>
        /// Gets or sets the string identifier of the text font. If null, the default font will be used.
        /// </summary>
        public Font TextFont {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the scale for the font.
        /// </summary>
        [PropertyVariable("c_textscale")]
        public float TextScale {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the toggle key.
        /// </summary>
        /// <value>
        /// The toggle key.
        /// </value>
        public Keys ToggleKey {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the warning text colour.
        /// </summary>
        /// <value>
        /// The warning text colour.
        /// </value>
        public Color WarningTextColour {
            get;
            set;
        }

        /// <summary>
        /// Clears all history.
        /// </summary>
        public void Clear() => lines.Clear();

        public void Draw(TimeSpan timeDelta) {
            if (IsOpen) {
                Font font = TextFont;
                spriteBatch.Begin();
                spriteBatch.Draw(whiteTexture, Area, BackgroundColor);

                string drawnLine = inputField.DisplayTextCursor;
                var textPos = new Vector2(Area.Left, Area.Bottom - font.MeasureString(drawnLine, TextScale).Y);
                LinkedListNode<Line> node = lines.Find(lines.ElementAtOrDefault(lines.Count - scrollOffset - 1));

                spriteBatch.DrawString(font, drawnLine, textPos, CommandTextColour, 0f, Vector2.Zero, TextScale, SpriteEffects.None, 0f);
                while (textPos.Y > 0 && node != null) {
                    string toDraw = font.WrapString(node.Value.Text, Area.Width, TextScale);
                    textPos.Y -= font.MeasureString(toDraw, TextScale).Y;
                    spriteBatch.DrawString(font, toDraw, textPos, node.Value.Color, 0f, Vector2.Zero, TextScale, SpriteEffects.None, 0f);
                    node = node.Previous;
                }
                spriteBatch.End();
            }
        }

        public void Update(TimeSpan timeDelta) {
            if (keyboard.Keyboard.IsKeyPressed(ToggleKey)) {
                IsOpen = !IsOpen;
            }

            if (IsOpen) {
                DoKeyboardInput();
                inputField.Update(timeDelta);
            }
        }

        /// <summary>
        /// Writes the provided text with the colour <see cref="DefaultTextColour"/>.
        /// </summary>
        /// <param name="text">The text to write.</param>
        public void WriteLine(string text) => WriteLine(text, DefaultTextColour);

        /// <summary>
        /// Writes the provided text with the given color.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <param name="color">Color of the line.</param>
        public void WriteLine(string text, Color color) {
            // Divide into separate rows for \n
            string[] rows = text.Split('\n');

            var sb = new StringBuilder();
            foreach (string r in rows) {
                // Add tabs
                sb.Clear();
                int column = 0;
                for (int i = 0; i < r.Length; i++) {
                    if (r[i] == '\t') {
                        int amnt = TabLength - (column % TabLength);
                        for (int j = 0; j < amnt; j++) {
                            sb.Append(' ');
                            column++;
                        }
                    } else {
                        sb.Append(r[i]);
                        column++;
                    }
                }

                lines.AddLast(new Line(sb.ToString(), color));
            }

            TrimLines();
        }

        private void AutoComplete() {
            string current = inputField.Text.Trim();

            if (current.Length > 0) {
                string[] split = current.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string commandString = split[0];
                string matchingOn = split[split.Length - 1];
                string completedPart = current.Substring(0, current.Length - matchingOn.Length);
                ICollection<string> matches = null;

                if (split.Length == 1) {
                    matches = MatchInput(matchingOn, CommandBroker.Commands.Select(o => o.Name).ToArray());
                } else {
                    IConsoleCommand command = CommandBroker.GetCommand(commandString);
                    if (command != null) {
                        matches = MatchInput(matchingOn, command.GetInputSuggestions(split.Length - 2));
                    }
                }

                if (matches != null) {
                    if (matches.Count == 1) {
                        inputField.Text = completedPart + matches.First();
                    } else if (matches.Count > 1) {
                        WriteLine(inputField.DisplayText, CommandTextColour);
                        foreach (string m in matches) {
                            WriteLine("\t" + m);
                        }
                        WriteLine("");
                        inputField.Text = completedPart + LongestCommonStartString(matches);
                    }
                }
            }
        }

        private void CommandClear() => Clear();

        private void CommandEcho(string[] arguments) => WriteLine(arguments[0]);

        private void CommandHelp(string[] args) {
            IConsoleCommand command = CommandBroker.GetCommand(args[0]);
            if (command != null) {
                if (command.Description != null) {
                    WriteLine(command.Description + "\n");
                }

                var usageBuilder = new StringBuilder("Usage: ");
                if (command.AcceptsArguments && command.AllowsZeroArguments) {
                    usageBuilder.Append(command.Name.ToUpper());
                    usageBuilder.Append("\n       ");
                }
                usageBuilder.Append(command.Name.ToUpper());
                foreach (string a in command.Arguments.Arguments) {
                    usageBuilder.Append(" [");
                    usageBuilder.Append(a);
                    usageBuilder.Append(']');
                }
                WriteLine(usageBuilder.ToString());

                if (command.Arguments.Length > 0) {
                    usageBuilder.Clear();
                    usageBuilder.Append("\n");

                    int maxLength = command.Arguments.ArgumentDescriptionMap.Keys.Max(o => o.Length);
                    foreach (string a in command.Arguments.ArgumentDescriptionMap.Keys) {
                        usageBuilder.Append("\t");
                        usageBuilder.Append(a);
                        for (int i = 0; i < maxLength - a.Length; i++) {
                            usageBuilder.Append(' ');
                        }
                        usageBuilder.Append(" - ");
                        usageBuilder.AppendLine(command.Arguments.GetArgumentDescription(a));
                    }

                    WriteLine(usageBuilder.ToString());
                } else {
                    WriteLine("");
                }
            } else {
                WriteLine("There is no such command to get help for.");
            }
        }

        private void CommandHelpList() {
            WriteLine("For more information on a specific command, type HELP [command].");
            var commands = CommandBroker.Commands.ToList();
            commands.Sort((a, b) => a.Name.CompareTo(b.Name));
            int maxLength = commands.Max(o => o.Name.Length);

            var sb = new StringBuilder();
            foreach (IConsoleCommand c in commands) {
                sb.Clear();
                sb.Append("\t");
                sb.Append(c.Name);
                if (c.Description != null) {
                    for (int i = 0; i < maxLength - c.Name.Length; i++) {
                        sb.Append(' ');
                    }
                    sb.Append(" \t");
                    sb.Append(c.Description);
                }
                WriteLine(sb.ToString());
            }
            WriteLine("");
        }

        private ICollection<string> CommandHelpSuggestions(int index) => CommandBroker.Commands.Select(o => o.Name).ToArray();

        private void DoCommand() {
            WriteLine(inputField.DisplayText, CommandTextColour);

            string[] split = inputField.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (split.Length > 0) {
                string command = split[0];
                string[] arguments = new string[split.Length - 1];
                Array.Copy(split, 1, arguments, 0, arguments.Length);

                if (CommandBroker.Call(command, arguments) == false) {
                    WriteLine("'" + command + "' is not a recognized command or has an invalid amount of arguments.", WarningTextColour);
                }
                inputField.Remember();
            }

            inputField.Clear();
        }

        private void DoKeyboardInput() {
            // Check for if command is given
            if (keyboard.IsTyped(Keys.Enter)) {
                DoCommand();
            }

            // Autocomplete
            if (keyboard.IsTyped(Keys.Tab)) {
                AutoComplete();
            }

            // Scrolling
            if (keyboard.IsTyped(Keys.PageUp)) {
                ScrollUp();
            }

            if (keyboard.IsTyped(Keys.PageDown)) {
                ScrollDown();
            }
        }

        private void LogAdded(object sender, LogAddedEventArgs e) {
            Color c = DefaultTextColour;
            if (e.Log.Level == LogLevel.Warning) {
                c = WarningTextColour;
            } else if (e.Log.Level == LogLevel.Error) {
                c = ErrorTextColour;
            }
            WriteLine(e.Log.ToString(), c);
        }

        private string LongestCommonStartString(ICollection<string> strings) {
            if (strings.Count > 0) {
                string longest = strings.OrderByDescending(s => s.Length).First();
                foreach (string s in strings) {
                    longest = LongestCommonStartString(longest, s);
                }
                return longest;
            }
            return "";
        }

        private string LongestCommonStartString(string a, string b) {
            var result = new StringBuilder();

            string shortest = a.Length > b.Length ? b : a;
            for (int i = 0; i < shortest.Length; i++) {
                if (a[i] == b[i]) {
                    result.Append(a[i]);
                } else {
                    break;
                }
            }

            return result.ToString();
        }

        private string[] MatchInput(string input, ICollection<string> options) => options.Where(o => o.StartsWith(input)).ToArray();

        private void Scroll(int delta) {
            scrollOffset += delta;
            scrollOffset = Math.Min(scrollOffset, lines.Count - 1);
            scrollOffset = Math.Max(scrollOffset, 0);
        }

        private void ScrollDown() {
            float height = TextFont.MeasureString("M", TextScale).Y;
            int maxRows = (int)(Area.Height / height);
            Scroll(-maxRows / 2);
        }

        private void ScrollUp() {
            float height = TextFont.MeasureString("M", TextScale).Y;
            int maxRows = (int)(Area.Height / height);
            Scroll(maxRows / 2);
        }

        private void SetupBroker() {
            CommandBroker = new CommandBroker();
            CommandBroker.Register(new ArgumentlessConsoleCommand("clear", "Clears the console output.", CommandClear));
            CommandBroker.Register(
                new ConsoleCommand("help", "Provides help about commands. If no argument is passed, a list of commands will be provided.",
                new CommandArguments(new string[] { "command" }, new string[] { "The command to get help for" }),
                CommandHelp,
                CommandHelpList,
                CommandHelpSuggestions));
            CommandBroker.Register(
                new ConsoleCommand("echo", "Prints the provided argument.",
                new CommandArguments(new string[] { "print" }, new string[] { "The argument to print" }),
                CommandEcho));
        }

        private void TrimLines() {
            while (lines.Count > Size) {
                lines.RemoveFirst();
            }
        }
    }
}
