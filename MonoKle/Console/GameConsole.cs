namespace MonoKle.Console
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using MonoKle.Assets.Font;
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
        private const double KEY_TYPED_CYCLE_INTERVAL = 0.02;

        private const double KEY_TYPED_TIMEROFFSET = 0.5;

        private string currentLine;

        private StringBuilder currentLineBuilder = new StringBuilder();

        private string currentLineCursor;

        private Timer cursorTimer = new Timer(0.25);

        // TODO: Break out into settings.
        private bool drawCursor;

        private GraphicsDevice graphicsDevice;

        private LinkedList<Line> history = new LinkedList<Line>();

        private SpriteBatch spriteBatch;

        internal GameConsole(Rectangle area, GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = new SpriteBatch(graphicsDevice);

            this.Area = area;
            this.Size = byte.MaxValue;
            this.BackgroundColor = new Color(0, 0, 0, 0.7f);
            this.DefaultTextColour = Color.WhiteSmoke;
            this.WarningTextColour = Color.Yellow;
            this.ErrorTextColour = Color.Red;
            this.TextScale = 0.5f;
            this.CursorToken = "_";
            this.TabToken = ' ';
            this.TabLength = 4;
            this.CommandToken = ">> ";
            // TODO: Break out all of these into either a settings struct or global script "variable"
            Logger.Global.LogAddedEvent += LogAdded;
            this.InputUpdateCurrentLine();
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
        /// Gets or sets the string identifier of the background texture. If null, the default background will be used.
        /// </summary>
        public string BackgroundTexture
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
        /// Gets or sets the command token.
        /// </summary>
        /// <value>
        /// The command token.
        /// </value>
        public string CommandToken { get; set; }

        /// <summary>
        /// Gets or sets the cursor token.
        /// </summary>
        /// <value>
        /// The cursor token.
        /// </value>
        public string CursorToken
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
        public bool IsOpen
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the maximum amount of entries to keep in history.
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
        /// Gets or sets the tab token.
        /// </summary>
        /// <value>
        /// The tab token.
        /// </value>
        public char TabToken
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
            this.history.Clear();
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
            StringBuilder sb = new StringBuilder();
            int column = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '\t')
                {
                    int amnt = this.TabLength - (column % this.TabLength);
                    for (int j = 0; j < amnt; j++)
                    {
                        sb.Append(this.TabToken);
                        column++;
                    }
                }
                else
                {
                    sb.Append(text[i]);
                    if (text[i] == '\n')
                    {
                        column = 0;
                    }
                    else
                    {
                        column++;
                    }
                }
            }

            this.history.AddLast(new Line(sb.ToString(), color));
            this.TrimHistory();
        }

        internal void Draw()
        {
            if (this.IsOpen)
            {
                Texture2D background = this.BackgroundTexture == null ?
                    MonoKleGame.TextureStorage.WhiteTexture : MonoKleGame.TextureStorage.GetAsset(this.BackgroundTexture);
                Font font = this.TextFont;
                spriteBatch.Begin();
                spriteBatch.Draw(background, this.Area, this.BackgroundColor);

                string drawnLine = this.drawCursor ? this.currentLineCursor : this.currentLine;
                Vector2 textPos = new Vector2(this.Area.Left, this.Area.Bottom - font.MeasureString(drawnLine, this.TextScale).Y);
                LinkedListNode<Line> node = history.Last;
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

        internal void Update(double seconds)
        {
            if (MonoKleGame.Keyboard.IsKeyPressed(this.ToggleKey))
            {
                this.IsOpen = !this.IsOpen;
                this.drawCursor = false;
            }

            if (this.IsOpen)
            {
                if (this.cursorTimer.Update(seconds))
                {
                    this.drawCursor = !this.drawCursor;
                    this.cursorTimer.Reset();
                }

                // Check letters
                for (int i = 65; i <= 90; i++)
                {
                    if (MonoKleGame.Keyboard.IsKeyTyped((Keys)i, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
                    {
                        bool upperCase = MonoKleGame.Keyboard.IsKeyHeld(Keys.LeftShift) || MonoKleGame.Keyboard.IsKeyHeld(Keys.RightShift);
                        this.InputAppend((char)(i + (upperCase ? +0 : 32)));
                    }
                }

                // Check numbers
                for (int i = 48; i <= 58; i++)
                {
                    if (MonoKleGame.Keyboard.IsKeyTyped((Keys)i, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
                    {
                        this.InputAppend((char)i);
                    }
                }

                if (MonoKleGame.Keyboard.IsKeyTyped(Keys.Space, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
                {
                    this.InputAppend(' ');
                }

                if (MonoKleGame.Keyboard.IsKeyTyped(Keys.OemPeriod, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
                {
                    this.InputAppend('.');
                }

                if (MonoKleGame.Keyboard.IsKeyTyped(Keys.OemPlus, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
                {
                    this.InputAppend('+');
                }

                if (MonoKleGame.Keyboard.IsKeyTyped(Keys.OemMinus, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
                {
                    this.InputAppend('-');
                }

                // Check for eraser
                if (MonoKleGame.Keyboard.IsKeyTyped(Keys.Back, GameConsole.KEY_TYPED_TIMEROFFSET, GameConsole.KEY_TYPED_CYCLE_INTERVAL))
                {
                    this.InputEraseCharacter();
                }

                // Check for if command is given
                if (MonoKleGame.Keyboard.IsKeyPressed(Keys.Enter))
                {
                    this.SendCommand();
                }

                // Autocomplete
                if (MonoKleGame.Keyboard.IsKeyPressed(Keys.Tab))
                {
                    this.AutoComplete();
                }
            }
        }

        private void AutoComplete()
        {
            string current = this.InputText();

            if (current.Length > 0)
            {
                IList<CommandBroker.Command> matches = this.CommandBroker.Commands.Where(o => o.Name.StartsWith(current)).ToList();
                if (matches.Count == 1)
                {
                    this.InputSet(matches.First().Name);
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

        private void InputAppend(char character)
        {
            this.currentLineBuilder.Append(character);
            this.InputUpdateCurrentLine();
        }

        private void InputAppend(string text)
        {
            this.currentLineBuilder.Append(text);
            this.InputUpdateCurrentLine();
        }

        private void InputEraseAll()
        {
            this.currentLineBuilder.Clear();
            this.InputUpdateCurrentLine();
        }

        private void InputEraseCharacter()
        {
            if (this.currentLineBuilder.Length > 0)
            {
                this.currentLineBuilder.Remove(this.currentLineBuilder.Length - 1, 1);
                this.InputUpdateCurrentLine();
            }
        }

        private void InputSet(string text)
        {
            this.currentLineBuilder.Clear();
            this.currentLineBuilder.Append(text);
            this.InputUpdateCurrentLine();
        }

        private string InputText()
        {
            return this.currentLineBuilder.ToString();
        }

        private void InputUpdateCurrentLine()
        {
            this.currentLine = this.CommandToken + this.currentLineBuilder.ToString();
            this.currentLineCursor = this.currentLine + this.CursorToken;
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

        private void SendCommand()
        {
            this.WriteLine(this.currentLine);

            string[] split = this.InputText().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (split.Length > 0)
            {
                string command = split[0];
                string[] arguments = new string[split.Length - 1];
                Array.Copy(split, 1, arguments, 0, arguments.Length);

                if (this.CommandBroker.Call(command, arguments) == false)
                {
                    this.WriteLine("'" + command + "' is not a recognized command or has an invalid amount of arguments.");
                }
            }

            this.InputEraseAll();
        }

        private void SetupBroker()
        {
            this.CommandBroker = new CommandBroker();
            this.CommandBroker.Register("clear", this.CommandClear);
            this.CommandBroker.Register("help", this.CommandHelp);
            this.CommandBroker.Register("echo", 1, this.CommandEcho);
        }

        private void TrimHistory()
        {
            while (this.history.Count > this.Size)
            {
                this.history.RemoveFirst();
            }
        }

        private class Line
        {
            public Line(string text, Color color)
            {
                this.Text = text;
                this.Color = color;
            }

            public Color Color { get; private set; }
            public string Text { get; private set; }
        }
    }
}