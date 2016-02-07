namespace MonoKle.Console
{
    using Attributes;
    using Command;
    using Core.Geometry;
    using Input;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using MonoKle.Asset.Font;
    using MonoKle.Core;
    using MonoKle.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Class that maintains and displays a console.
    /// </summary>
    public class GameConsole : IGameConsole, IMComponent
    {
        private const double KEY_TYPED_CYCLE_INTERVAL = 0.02;

        private const double KEY_TYPED_TIMEROFFSET = 0.5;

        private int drawingOffset = 0;
        private GraphicsDevice graphicsDevice;
        private InputField inputField = new InputField("_", ">> ", 0.25, 10);
        private KeyboardInput keyboard;
        private LinkedList<Line> lines = new LinkedList<Line>();
        private SpriteBatch spriteBatch;
        private Texture2D whiteTexture;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameConsole"/> class.
        /// </summary>
        /// <param name="area">The area.</param>
        /// <param name="graphicsDevice">The graphics device.</param>
        /// <param name="keyboard">The keyboard.</param>
        /// <param name="whiteTexture">The background texture.</param>
        public GameConsole(MRectangleInt area, GraphicsDevice graphicsDevice, KeyboardInput keyboard, Texture2D whiteTexture)
        {
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = new SpriteBatch(graphicsDevice);
            this.keyboard = keyboard;

            this.Area = area;
            this.Size = byte.MaxValue;
            this.whiteTexture = whiteTexture;
            this.BackgroundColor = new Color(0, 0, 0, 0.7f);
            this.DefaultTextColour = Color.WhiteSmoke;
            this.WarningTextColour = Color.Yellow;
            this.ErrorTextColour = Color.Red;
            this.CommandTextColour = Color.LightGreen;
            this.DisabledTextColour = Color.Gray;
            this.TextScale = 0.5f;
            this.TabLength = 4;
            Logger.Global.LogAddedEvent += LogAdded;
            Logger.Global.Log("GameConsole activated.", LogLevel.Debug);

            this.SetupBroker();
        }

        /// <summary>
        /// Gets or sets the area in which the console will be drawn.
        /// </summary>
        public MRectangleInt Area
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        public Color BackgroundColor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the command broker. Used for executing console commands.
        /// </summary>
        /// <value>
        /// The command broker.
        /// </value>
        public CommandBroker CommandBroker
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the command text colour.
        /// </summary>
        /// <value>
        /// The command text colour.
        /// </value>
        public Color CommandTextColour
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the color that the text will be drawn with if no other colour is specified.
        /// </summary>
        public Color DefaultTextColour
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the disabled text colour.
        /// </summary>
        /// <value>
        /// The disabled text colour.
        /// </value>
        public Color DisabledTextColour
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the error text colour.
        /// </summary>
        /// <value>
        /// The error text colour.
        /// </value>
        public Color ErrorTextColour
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets wether the console is open.
        /// </summary>
        [PropertyVariableAttribute("c_isopen")]
        public bool IsOpen
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the maximum amount of entries to keep.
        /// </summary>
        [PropertyVariableAttribute("c_size")]
        public int Size
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the length of the tabs.
        /// </summary>
        /// <value>
        /// The length of the tabs.
        /// </value>
        public int TabLength
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the string identifier of the text font. If null, the default font will be used.
        /// </summary>
        public Font TextFont
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the scale for the font.
        /// </summary>
        [PropertyVariableAttribute("c_textscale")]
        public float TextScale
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the toggle key.
        /// </summary>
        /// <value>
        /// The toggle key.
        /// </value>
        public Keys ToggleKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the warning text colour.
        /// </summary>
        /// <value>
        /// The warning text colour.
        /// </value>
        public Color WarningTextColour
        {
            get;
            set;
        }

        /// <summary>
        /// Clears all history.
        /// </summary>
        public void Clear()
        {
            this.lines.Clear();
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public void Draw(double seconds)
        {
            if (this.IsOpen)
            {
                Font font = this.TextFont;
                spriteBatch.Begin();
                spriteBatch.Draw(this.whiteTexture, this.Area, this.BackgroundColor);

                string drawnLine = this.inputField.GetText(true);
                Vector2 textPos = new Vector2(this.Area.Left, this.Area.Bottom - font.MeasureString(drawnLine, this.TextScale).Y);
                LinkedListNode<Line> node = lines.Find(lines.ElementAtOrDefault(lines.Count - this.drawingOffset - 1));
                StringWrapper wrapper = new StringWrapper();

                font.DrawString(this.spriteBatch, drawnLine, textPos, this.CommandTextColour, 0f, Vector2.Zero, this.TextScale, SpriteEffects.None, 0f);
                while (textPos.Y > 0 && node != null)
                {
                    string toDraw = wrapper.WrapWidth(node.Value.Text, font, Area.Width, this.TextScale);
                    textPos.Y -= font.MeasureString(toDraw, this.TextScale).Y;
                    font.DrawString(this.spriteBatch, toDraw, textPos, node.Value.Color, 0f, Vector2.Zero, this.TextScale, SpriteEffects.None, 0f);
                    node = node.Previous;
                }
                spriteBatch.End();
            }
        }

        /// <summary>
        /// Updates the specified seconds.
        /// </summary>
        /// <param name="seconds">The seconds elapsed.</param>
        public void Update(double seconds)
        {
            if (this.keyboard.IsKeyPressed(this.ToggleKey))
            {
                this.IsOpen = !this.IsOpen; ;
            }

            if (this.IsOpen)
            {
                DoKeyboardInput();

                this.inputField.Update(seconds);
            }
        }

        /// <summary>
        /// Writes the provided text with the colour <see cref="GameConsole.DefaultTextColour"/>.
        /// </summary>
        /// <param name="text">The text to write.</param>
        public void WriteLine(string text)
        {
            this.WriteLine(text, this.DefaultTextColour);
        }

        /// <summary>
        /// Writes the provided text with the given color.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <param name="color">Color of the line.</param>
        public void WriteLine(string text, Color color)
        {
            // Divide into separate rows for \n
            string[] rows = text.Split('\n');

            StringBuilder sb = new StringBuilder();
            foreach (string r in rows)
            {
                // Add tabs
                sb.Clear();
                int column = 0;
                for (int i = 0; i < r.Length; i++)
                {
                    if (r[i] == '\t')
                    {
                        int amnt = this.TabLength - (column % this.TabLength);
                        for (int j = 0; j < amnt; j++)
                        {
                            sb.Append(' ');
                            column++;
                        }
                    }
                    else
                    {
                        sb.Append(r[i]);
                        column++;
                    }
                }

                this.lines.AddLast(new Line(sb.ToString(), color));
            }

            this.TrimLines();
        }

        private void AutoComplete()
        {
            string current = this.inputField.GetInput();

            if (current.Length > 0)
            {
                IList<IConsoleCommand> matches = this.CommandBroker.Commands.Where(o => o.Name.StartsWith(current)).ToList();
                if (matches.Count == 1)
                {
                    this.inputField.Set(matches.First().Name);
                }
                else if (matches.Count > 1)
                {
                    this.WriteLine("Matches for: " + current);
                    foreach (IConsoleCommand c in matches)
                    {
                        this.WriteLine("\t" + c.Name);
                    }
                }
            }
        }

        private void CommandClear()
        {
            this.Clear();
        }

        private void CommandEcho(string[] arguments)
        {
            this.WriteLine(arguments[0]);
        }

        private void CommandHelp(string[] args)
        {
            IConsoleCommand command = this.CommandBroker.GetCommand(args[0]);
            if (command != null)
            {
                if (command.Description != null)
                {
                    this.WriteLine(command.Description + "\n");
                }

                StringBuilder usageBuilder = new StringBuilder("Usage: ");
                if (command.AcceptsArguments && command.AllowsZeroArguments)
                {
                    usageBuilder.Append(command.Name.ToUpper());
                    usageBuilder.Append("\n       ");
                }
                usageBuilder.Append(command.Name.ToUpper());
                foreach (string a in command.Arguments.Arguments)
                {
                    usageBuilder.Append(" [");
                    usageBuilder.Append(a);
                    usageBuilder.Append(']');
                }
                this.WriteLine(usageBuilder.ToString());

                if (command.Arguments.Length > 0)
                {
                    usageBuilder.Clear();
                    usageBuilder.Append("\n");

                    int maxLength = command.Arguments.ArgumentDescriptionMap.Keys.Max(o => o.Length);
                    foreach (string a in command.Arguments.ArgumentDescriptionMap.Keys)
                    {
                        usageBuilder.Append("\t");
                        usageBuilder.Append(a);
                        for (int i = 0; i < maxLength - a.Length; i++)
                        {
                            usageBuilder.Append(' ');
                        }
                        usageBuilder.Append(" - ");
                        usageBuilder.AppendLine(command.Arguments.GetArgumentDescription(a));
                    }

                    this.WriteLine(usageBuilder.ToString());
                }
                else
                {
                    this.WriteLine("");
                }
            }
            else
            {
                this.WriteLine("There is no such command to get help for.");
            }
        }

        private void CommandHelpList()
        {
            this.WriteLine("For more information on a specific command, type HELP [command].");
            List<IConsoleCommand> commands = this.CommandBroker.Commands.ToList();
            commands.Sort((a, b) => a.Name.CompareTo(b.Name));
            int maxLength = commands.Max(o => o.Name.Length);

            StringBuilder sb = new StringBuilder();
            foreach (IConsoleCommand c in commands)
            {
                sb.Clear();
                sb.Append("\t");
                sb.Append(c.Name);
                if (c.Description != null)
                {
                    for (int i = 0; i < maxLength - c.Name.Length; i++)
                    {
                        sb.Append(' ');
                    }
                    sb.Append(" \t");
                    sb.Append(c.Description);
                }
                this.WriteLine(sb.ToString());
            }
            this.WriteLine("");
        }

        private void DoCommand()
        {
            this.WriteLine(this.inputField.GetText(), this.CommandTextColour);

            string[] split = this.inputField.GetInput().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (split.Length > 0)
            {
                string command = split[0];
                string[] arguments = new string[split.Length - 1];
                Array.Copy(split, 1, arguments, 0, arguments.Length);

                if (this.CommandBroker.Call(command, arguments) == false)
                {
                    this.WriteLine("'" + command + "' is not a recognized command or has an invalid amount of arguments.", this.WarningTextColour);
                }
                this.inputField.Remember();
            }

            this.inputField.Clear();
        }

        private void DoKeyboardInput()
        {
            // Check letters
            for (int i = 65; i <= 90; i++)
            {
                if (this.keyboard.IsKeyTyped((Keys)i, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
                {
                    bool upperCase = this.keyboard.IsKeyHeld(Keys.LeftShift) || this.keyboard.IsKeyHeld(Keys.RightShift);
                    this.inputField.Type((char)(i + (upperCase ? +0 : 32)));
                }
            }

            // Check numbers
            for (int i = 48; i <= 58; i++)
            {
                if (this.keyboard.IsKeyTyped((Keys)i, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
                {
                    this.inputField.Type((char)i);
                }
            }

            if (this.keyboard.IsKeyTyped(Keys.Space, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
            {
                this.inputField.Type(' ');
            }

            if (this.keyboard.IsKeyTyped(Keys.OemPeriod, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
            {
                this.inputField.Type('.');
            }

            if (this.keyboard.IsKeyTyped(Keys.OemPlus, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
            {
                this.inputField.Type('+');
            }

            if (this.keyboard.IsKeyTyped(Keys.OemMinus, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
            {
                if (this.keyboard.IsKeyDown(Keys.LeftShift) || this.keyboard.IsKeyDown(Keys.RightShift))
                {
                    this.inputField.Type('_');
                }
                else
                {
                    this.inputField.Type('-');
                }
            }

            // Check for eraser
            if (this.keyboard.IsKeyTyped(Keys.Back, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
            {
                this.inputField.Erase();
            }

            if (this.keyboard.IsKeyTyped(Keys.Delete, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
            {
                this.inputField.Delete();
            }

            // Check for if command is given
            if (this.keyboard.IsKeyTyped(Keys.Enter, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
            {
                this.DoCommand();
            }

            // Autocomplete
            if (this.keyboard.IsKeyTyped(Keys.Tab, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
            {
                this.AutoComplete();
            }

            // History
            if (this.keyboard.IsKeyTyped(Keys.Up, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
            {
                this.inputField.PreviousMemory();
            }

            if (this.keyboard.IsKeyTyped(Keys.Down, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
            {
                this.inputField.NextMemory();
            }

            // Cursor
            if (this.keyboard.IsKeyTyped(Keys.Left, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
            {
                this.inputField.CursorLeft();
            }

            if (this.keyboard.IsKeyTyped(Keys.Right, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
            {
                this.inputField.CursorRight();
            }

            // Scrolling
            if (this.keyboard.IsKeyTyped(Keys.PageUp, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
            {
                this.ScrollUp();
            }

            if (this.keyboard.IsKeyTyped(Keys.PageDown, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
            {
                this.ScrollDown();
            }

            if (this.keyboard.IsKeyTyped(Keys.Home, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
            {
                this.inputField.CursorHome();
            }

            if (this.keyboard.IsKeyTyped(Keys.End, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
            {
                this.inputField.CursorEnd();
            }
        }

        private void LogAdded(object sender, LogAddedEventArgs e)
        {
            Color c = this.DefaultTextColour;
            if (e.Log.Level == LogLevel.Warning)
            {
                c = this.WarningTextColour;
            }
            else if (e.Log.Level == LogLevel.Error)
            {
                c = this.ErrorTextColour;
            }
            this.WriteLine(e.Log.ToString(), c);
        }

        private void Scroll(int delta)
        {
            this.drawingOffset += delta;
            this.drawingOffset = Math.Min(this.drawingOffset, this.lines.Count - 1);
            this.drawingOffset = Math.Max(this.drawingOffset, 0);
        }

        private void ScrollDown()
        {
            float height = this.TextFont.MeasureString("M", this.TextScale).Y;
            int maxRows = (int)(this.Area.Height / height);
            this.Scroll(-maxRows / 2);
        }

        private void ScrollUp()
        {
            float height = this.TextFont.MeasureString("M", this.TextScale).Y;
            int maxRows = (int)(this.Area.Height / height);
            this.Scroll(maxRows / 2);
        }

        private void SetupBroker()
        {
            this.CommandBroker = new CommandBroker();
            this.CommandBroker.Register(new ArgumentlessConsoleCommand("clear", "Clears the console output.", this.CommandClear));
            this.CommandBroker.Register(new ConsoleCommand("help",
                "Provides help about commands. If no argument is passed, a list of commands will be provided.",
                new CommandArguments(new string[] { "command" }, new string[] { "The command to get help for" }),
                this.CommandHelp,
                this.CommandHelpList));
            this.CommandBroker.Register(new ConsoleCommand("echo",
                "Prints the provided argument.",
                new CommandArguments(new string[] { "print" }, new string[] { "The argument to print" }),
                this.CommandEcho));
        }

        private void TrimLines()
        {
            while (this.lines.Count > this.Size)
            {
                this.lines.RemoveFirst();
            }
        }
    }
}