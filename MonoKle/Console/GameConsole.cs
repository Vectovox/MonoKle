namespace MonoKle.Console
{
    using System.Collections.Generic;
    using System.Text;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using MonoKle.Assets.Font;
    using MonoKle.Core;
    using MonoKle.Logging;
    using MonoKle.Messaging;
    using MonoKle.Utilities;

    /// <summary>
    /// Class that maintains and displays a console.
    /// </summary>
    public class GameConsole
    {
        public const string CHANNEL_ID = "CONSOLE";
        private const string CURSOR_TOKEN = "_";

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
            // TODO: Break out all of these into either a settings struct or global script "variable"
            Logger.Global.LogAddedEvent += GameConsole_LogAddedEvent;
            this.UpdateCurrentLine();
            Logger.Global.Log("GameConsole activated!", LogLevel.Info);
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
        public string TextFont
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
        /// Writes the provided line.
        /// </summary>
        /// <param name="line">The line to write.</param>
        public void WriteLine(string line)
        {
            this.history.AddLast(line);
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
                Font font = this.TextFont == null ? MonoKleGame.FontManager.DefaultFont : MonoKleGame.FontManager.GetFont(this.TextFont);

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
            if (this.cursorTimer.Update(seconds))
            {
                this.drawCursor = !this.drawCursor;
                this.cursorTimer.Reset();
            }

            if (this.IsOpen)
            {
                // Check letters
                for (int i = 65; i <= 90; i++)
                {
                    if (MonoKleGame.Keyboard.IsKeyPressed((Keys)i))
                    {
                        bool upperCase = MonoKleGame.Keyboard.IsKeyHeld(Keys.LeftShift) || MonoKleGame.Keyboard.IsKeyHeld(Keys.RightShift);
                        this.AppendLine((char)(i + (upperCase ? +0 : 32)));
                    }
                }

                // Check numbers
                for (int i = 48; i <= 58; i++)
                {
                    if (MonoKleGame.Keyboard.IsKeyPressed((Keys)i))
                    {
                        this.AppendLine((char)i);
                    }
                }

                if (MonoKleGame.Keyboard.IsKeyPressed(Keys.Space))
                {
                    this.AppendLine(' ');
                }

                if (MonoKleGame.Keyboard.IsKeyPressed(Keys.OemPeriod))
                {
                    this.AppendLine('.');
                }

                if (MonoKleGame.Keyboard.IsKeyPressed(Keys.OemPlus))
                {
                    this.AppendLine('+');
                }

                if (MonoKleGame.Keyboard.IsKeyPressed(Keys.OemMinus))
                {
                    this.AppendLine('-');
                }

                // Check for eraser
                if (MonoKleGame.Keyboard.IsKeyPressed(Keys.Back))
                {
                    this.EraseCharacter();
                }

                // Check for if command is given
                if (MonoKleGame.Keyboard.IsKeyPressed(Keys.Enter))
                {
                    this.SendCommand();
                }
            }
        }

        private void AppendLine(char character)
        {
            this.currentLineBuilder.Append(character);
            this.UpdateCurrentLine();
        }

        private void AppendLine(string text)
        {
            this.currentLineBuilder.Append(text);
            this.UpdateCurrentLine();
        }

        private void GameConsole_LogAddedEvent(object sender, LogAddedEventArgs e)
        {
            this.WriteLine(e.Log.ToString());
        }

        private void EraseCharacter()
        {
            if (this.currentLineBuilder.Length > 0)
            {
                this.currentLineBuilder.Remove(this.currentLineBuilder.Length - 1, 1);
                this.UpdateCurrentLine();
            }
        }

        private void EraseLine()
        {
            this.currentLineBuilder.Clear();
            this.UpdateCurrentLine();
        }

        private void SendCommand()
        {
            string s = this.currentLine;
            this.EraseLine();
            this.WriteLine(s);
            MonoKleGame.MessagePasser.SendMessage(GameConsole.CHANNEL_ID, new MessageEventArgs(s), this);  // TODO: Break out into settings file (the message channel)
        }

        private void UpdateCurrentLine()
        {
            this.currentLine = this.currentLineBuilder.ToString();
            this.currentLineCursor = this.currentLine + GameConsole.CURSOR_TOKEN;
        }
    }
}