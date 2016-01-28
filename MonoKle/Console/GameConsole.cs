namespace MonoKle.Console
{
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
    public class GameConsole
    {
        // TODO: Break constants and things in class out into settings
        private const double KEY_TYPED_CYCLE_INTERVAL = 0.02;

        private const double KEY_TYPED_TIMEROFFSET = 0.5;

        private int drawingOffset = 0;
        private GraphicsDevice graphicsDevice;
        private InputField input = new InputField("|", ">> ", 0.25, 10);
        private LinkedList<Line> lines = new LinkedList<Line>();
        private SpriteBatch spriteBatch;
        private KeyboardInput keyboard;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameConsole"/> class.
        /// </summary>
        /// <param name="area">The area.</param>
        /// <param name="graphicsDevice">The graphics device.</param>
        /// <param name="keyboard">The keyboard.</param>
        /// <param name="backgroundTexture">The background texture.</param>
        public GameConsole(Rectangle area, GraphicsDevice graphicsDevice, KeyboardInput keyboard, Texture2D backgroundTexture)
        {
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = new SpriteBatch(graphicsDevice);
            this.keyboard = keyboard;

            this.Area = area;
            this.Size = byte.MaxValue;
            this.BackgroundTexture = backgroundTexture;
            this.BackgroundColor = new Color(0, 0, 0, 0.7f);
            this.DefaultTextColour = Color.WhiteSmoke;
            this.WarningTextColour = Color.Yellow;
            this.ErrorTextColour = Color.Red;
            this.TextScale = 0.5f;
            this.TabLength = 4;
            Logger.Global.LogAddedEvent += LogAdded;
            Logger.Global.Log("GameConsole activated!", LogLevel.Info);

            this.SetupBroker();
        }

        /// <summary>
        /// Gets or sets the area in which the console will be drawn.
        /// </summary>
        public Rectangle Area
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
        /// Gets or sets the background texture.
        /// </summary>
        public Texture2D BackgroundTexture
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
        /// Gets or sets the color that the text will be drawn with if no other colour is specified.
        /// </summary>
        public Color DefaultTextColour
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
        /// Gets or sets the maximum amount of previous commands to keep in history.
        /// </summary>
        public int HistorySize
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets wether the console is open.
        /// </summary>
        public bool IsOpen
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the maximum amount of entries to keep.
        /// </summary>
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

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public void Draw()
        {
            if (this.IsOpen)
            {
                Font font = this.TextFont;
                spriteBatch.Begin();
                spriteBatch.Draw(this.BackgroundTexture, this.Area, this.BackgroundColor);

                string drawnLine = this.input.GetText(true);
                Vector2 textPos = new Vector2(this.Area.Left, this.Area.Bottom - font.MeasureString(drawnLine, this.TextScale).Y);
                LinkedListNode<Line> node = lines.Find(lines.ElementAtOrDefault(lines.Count - this.drawingOffset - 1));
                StringWrapper wrapper = new StringWrapper();

                font.DrawString(this.spriteBatch, drawnLine, textPos, this.DefaultTextColour, 0f, Vector2.Zero, this.TextScale, SpriteEffects.None, 0f);
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
                // Check letters
                for (int i = 65; i <= 90; i++)
                {
                    if (this.keyboard.IsKeyTyped((Keys)i, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
                    {
                        bool upperCase = this.keyboard.IsKeyHeld(Keys.LeftShift) || this.keyboard.IsKeyHeld(Keys.RightShift);
                        this.input.Type((char)(i + (upperCase ? +0 : 32)));
                    }
                }

                // Check numbers
                for (int i = 48; i <= 58; i++)
                {
                    if (this.keyboard.IsKeyTyped((Keys)i, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
                    {
                        this.input.Type((char)i);
                    }
                }

                if (this.keyboard.IsKeyTyped(Keys.Space, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
                {
                    this.input.Type(' ');
                }

                if (this.keyboard.IsKeyTyped(Keys.OemPeriod, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
                {
                    this.input.Type('.');
                }

                if (this.keyboard.IsKeyTyped(Keys.OemPlus, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
                {
                    this.input.Type('+');
                }

                if (this.keyboard.IsKeyTyped(Keys.OemMinus, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
                {
                    this.input.Type('-');
                }

                // Check for eraser
                if (this.keyboard.IsKeyTyped(Keys.Back, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
                {
                    this.input.Erase();
                }

                if (this.keyboard.IsKeyTyped(Keys.Delete, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
                {
                    this.input.Delete();
                }

                // Check for if command is given
                if (this.keyboard.IsKeyTyped(Keys.Enter, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
                {
                    this.SendCommand();
                }

                // Autocomplete
                if (this.keyboard.IsKeyTyped(Keys.Tab, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
                {
                    this.AutoComplete();
                }

                // History
                if (this.keyboard.IsKeyTyped(Keys.Up, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
                {
                    this.input.PreviousMemory();
                }

                if (this.keyboard.IsKeyTyped(Keys.Down, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
                {
                    this.input.NextMemory();
                }

                // Cursor
                if (this.keyboard.IsKeyTyped(Keys.Left, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
                {
                    this.input.CursorLeft();
                }

                if (this.keyboard.IsKeyTyped(Keys.Right, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
                {
                    this.input.CursorRight();
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
                    this.ScrollTop();
                }

                if (this.keyboard.IsKeyTyped(Keys.End, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
                {
                    this.ScrollBottom();
                }

                this.input.Update(seconds);
            }
        }

        private void AutoComplete()
        {
            string current = this.input.GetInput();

            if (current.Length > 0)
            {
                IList<CommandBroker.Command> matches = this.CommandBroker.Commands.Where(o => o.Name.StartsWith(current)).ToList();
                if (matches.Count == 1)
                {
                    this.input.Set(matches.First().Name);
                }
                else if (matches.Count > 1)
                {
                    this.WriteLine("Matches for: " + current);
                    foreach (CommandBroker.Command c in matches)
                    {
                        this.WriteLine("\t" + c.Name);
                    }
                }
            }
        }

        private void CommandClear(string[] arguments)
        {
            this.Clear();
        }

        private void CommandEcho(string[] arguments)
        {
            this.WriteLine(arguments[0]);
        }

        private void CommandHelp(string[] arguments)
        {
            this.WriteLine("Listing availabe commands:");
            this.WriteLine("\tCommand\t\tArguments");
            this.WriteLine("============================");
            foreach (CommandBroker.Command c in this.CommandBroker.Commands)
            {
                this.WriteLine("\t" + c.Name + "\t\t" + c.ArgumentLength);
            }
        }

        private void LogAdded(object sender, LogAddedEventArgs e)
        {
            Color c = this.DefaultTextColour;
            if (e.Log.Level == LogLevel.Warning)
            {
                c = this.WarningTextColour;
            }
            else if (e.Log.Level == LogLevel.Info)
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

        private void ScrollBottom()
        {
            this.Scroll(-this.drawingOffset);
        }

        private void ScrollDown()
        {
            float height = this.TextFont.MeasureString("M", this.TextScale).Y;
            int maxRows = (int)(this.Area.Height / height);
            this.Scroll(-maxRows / 2);
        }

        private void ScrollTop()
        {
            this.Scroll(this.lines.Count - this.drawingOffset);
        }

        private void ScrollUp()
        {
            float height = this.TextFont.MeasureString("M", this.TextScale).Y;
            int maxRows = (int)(this.Area.Height / height);
            this.Scroll(maxRows / 2);
        }

        private void SendCommand()
        {
            this.WriteLine(this.input.GetText());

            string[] split = this.input.GetInput().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (split.Length > 0)
            {
                string command = split[0];
                string[] arguments = new string[split.Length - 1];
                Array.Copy(split, 1, arguments, 0, arguments.Length);

                if (this.CommandBroker.Call(command, arguments) == false)
                {
                    this.WriteLine("'" + command + "' is not a recognized command or has an invalid amount of arguments.");
                }
                this.input.Remember();
            }

            this.input.Clear();
        }

        private void SetupBroker()
        {
            this.CommandBroker = new CommandBroker();
            this.CommandBroker.Register("clear", this.CommandClear);
            this.CommandBroker.Register("help", this.CommandHelp);
            this.CommandBroker.Register("echo", 1, this.CommandEcho);
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