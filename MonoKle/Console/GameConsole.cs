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
        private Timer cursorTimer = new Timer(0.25); // TODO: Break out into settings.
        private bool drawCursor;
        private GraphicsDevice graphicsDevice;
        private LinkedList<string> history = new LinkedList<string>();
        private SpriteBatch spriteBatch;

        internal GameConsole(Rectangle area, GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = new SpriteBatch(graphicsDevice);

            this.Area = area;
            this.Size = byte.MaxValue;
            this.BackgroundColor = new Color(0, 0, 0, 0.7f);
            this.TextColor = Color.White;
            this.TextScale = 0.5f;
            this.CursorToken = "_";
            this.TabToken = "  ";
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
        /// Gets or sets the tab token.
        /// </summary>
        /// <value>
        /// The tab token.
        /// </value>
        public string TabToken
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the color that the text will be drawn with.
        /// </summary>
        public Color TextColor
        {
            get;
            set;
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
        /// Clears all history.
        /// </summary>
        public void Clear()
        {
            this.history.Clear();
        }

        /// <summary>
        /// Writes the provided line.
        /// </summary>
        /// <param name="line">The line to write.</param>
        public void WriteLine(string line)
        {
            string formattedLine = line.Replace("\t", this.TabToken);
            this.history.AddLast(formattedLine);
            this.TrimHistory();
        }

        private void TrimHistory()
        {
            while (this.history.Count > this.Size)
            {
                this.history.RemoveLast();
            }
        }

        internal void Draw()
        {
            if (this.IsOpen)
            {
                Texture2D background = this.BackgroundTexture == null ? MonoKleGame.TextureManager.WhiteTexture : MonoKleGame.TextureManager.GetTexture(BackgroundTexture);
                Font font = this.TextFont;
                spriteBatch.Begin();
                spriteBatch.Draw(background, this.Area, this.BackgroundColor);

                string drawnLine = this.drawCursor ? this.currentLineCursor : this.currentLine;
                Vector2 textPos = new Vector2(this.Area.Left, this.Area.Bottom - font.MeasureString(drawnLine, this.TextScale).Y);
                LinkedListNode<string> node = history.Last;
                StringWrapper wrapper = new StringWrapper();

                font.DrawString(this.spriteBatch, drawnLine, textPos, this.TextColor, 0f, Vector2.Zero, this.TextScale, SpriteEffects.None, 0f);
                while (textPos.Y > 0 && node != null)
                {
                    string toDraw = wrapper.WrapWidth(node.Value, font, Area.Width, this.TextScale);
                    textPos.Y -= font.MeasureString(toDraw, this.TextScale).Y;
                    font.DrawString(this.spriteBatch, toDraw, textPos, this.TextColor, 0f, Vector2.Zero, this.TextScale, SpriteEffects.None, 0f);
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

        private void CommandHelp(string[] arguments)
        {
            this.WriteLine("Listing availabe commands:");
            foreach (CommandBroker.Command c in this.CommandBroker.Commands)
            {
                this.WriteLine("\t" + c.Name + "\t(" + c.ArgumentLength + ")");
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
            this.WriteLine(e.Log.ToString());
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
        }
    }
}