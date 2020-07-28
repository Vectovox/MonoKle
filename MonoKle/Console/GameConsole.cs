using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoKle.Asset;
using MonoKle.Attributes;
using MonoKle.Input.Keyboard;
using MonoKle.Logging;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoKle.Console
{
    /// <summary>
    /// Class that maintains and displays a console.
    /// </summary>
    public class GameConsole : IGameConsole, IUpdateable, IDrawable
    {
        private static readonly TimeSpan TypingActivationDelay = TimeSpan.FromMilliseconds(400);
        private static readonly TimeSpan TypingCycleDelay = TimeSpan.FromMilliseconds(2);

        private readonly InputField inputField;
        private readonly KeyboardTyper keyboard;
        private readonly LinkedList<TextEntry> textEntries = new LinkedList<TextEntry>();
        private readonly SpriteBatch spriteBatch;
        private readonly Texture2D whiteTexture;

        private int scrollOffset = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameConsole"/> class.
        /// </summary>
        /// <param name="area">The area.</param>
        /// <param name="graphicsDevice">The graphics device.</param>
        /// <param name="keyboardInput">The keyboard input.</param>
        /// <param name="whiteTexture">The background texture.</param>
        /// <param name="logger">The logger to use.</param>
        public GameConsole(MRectangleInt area, GraphicsDevice graphicsDevice, IKeyboard keyboardInput, Texture2D whiteTexture, Font font, Logger logger)
        {
            spriteBatch = new SpriteBatch(graphicsDevice);
            keyboard = new KeyboardTyper(keyboardInput, TypingActivationDelay, TypingCycleDelay);
            inputField = new InputField("-", "$ ", TimeSpan.FromSeconds(0.25), 10, new KeyboardCharacterInput(keyboard));

            Area = area;
            TextFont = font;
            this.whiteTexture = whiteTexture;
            logger.LogAddedEvent += LogAdded;
            logger.Log("GameConsole activated.", LogLevel.Debug);

            CommandBroker = new CommandBroker(this);
            CommandBroker.RegisterCallingAssembly();
        }

        public MRectangleInt Area { get; set; }

        public Color BackgroundColor { get; set; } = new Color(0, 0, 0, 0.7f);

        public CommandBroker CommandBroker { get; }

        public Color CommandTextColour { get; set; } = Color.LightGreen;

        public Color DefaultTextColour { get; set; } = Color.WhiteSmoke;

        public Color DisabledTextColour { get; set; } = Color.Gray;

        public Color ErrorTextColour { get; set; } = Color.Red;

        [PropertyVariable("c_isopen")]
        public bool IsOpen { get; set; }

        [PropertyVariable("c_size")]
        public uint Size { get; set; } = byte.MaxValue;

        public int TabLength { get; set; } = 4;

        public Font TextFont { get; set; }

        [PropertyVariable("c_textscale")]
        public float TextScale { get; set; } = 0.5f;

        public Keys ToggleKey { get; set; } = Keys.F1;

        public Color WarningTextColour { get; set; } = Color.Yellow;

        public void Clear() => textEntries.Clear();

        public void Draw(TimeSpan timeDelta)
        {
            if (IsOpen)
            {
                spriteBatch.Begin();

                // Draw background
                spriteBatch.Draw(whiteTexture, Area, BackgroundColor);

                // Draw input field
                var textPosition = new Vector2(Area.Left, Area.Bottom - TextFont.MeasureString(inputField.CursorDisplayText, TextScale).Y);
                spriteBatch.DrawString(TextFont, inputField.CursorDisplayText, textPosition, CommandTextColour, 0f, Vector2.Zero, TextScale, SpriteEffects.None, 0f);

                // Draw all lines
                foreach (var line in textEntries.Skip(scrollOffset))
                {
                    string stringToDraw = TextFont.WrapString(line.Text, Area.Width, TextScale);
                    var stringHeight = TextFont.MeasureString(stringToDraw, TextScale).Y;
                    textPosition.Y -= stringHeight;
                    spriteBatch.DrawString(TextFont, stringToDraw, textPosition, line.Color, 0f, Vector2.Zero, TextScale, SpriteEffects.None, 0f);

                    if (textPosition.Y + stringHeight < 0)
                    {
                        break;
                    }
                }

                spriteBatch.End();
            }
        }

        public void Update(TimeSpan timeDelta)
        {
            if (keyboard.Keyboard.IsKeyPressed(ToggleKey))
            {
                IsOpen = !IsOpen;
            }

            if (IsOpen)
            {
                UpdateKeyboard();
                inputField.Update(timeDelta);
            }
        }

        public void WriteError(string message) => WriteLine(message, ErrorTextColour);

        public void WriteLine(string text) => WriteLine(text, DefaultTextColour);

        public void WriteLine(string text, Color color)
        {
            var entryBuilder = new StringBuilder();

            // Convert string to entries, based on newlines
            foreach (string row in text.Split('\n'))
            {
                entryBuilder.Clear();
                int column = 0;

                // Add characters to entry
                foreach (char character in row)
                {
                    // Convert tab characters to spaces
                    if (character == '\t')
                    {
                        var spacesToAdd = TabLength - (column % TabLength);
                        entryBuilder.Append(' ', spacesToAdd);
                        column += spacesToAdd;
                    }
                    else
                    {
                        entryBuilder.Append(character);
                        column++;
                    }
                }

                textEntries.AddFirst(new TextEntry(entryBuilder.ToString(), color));

                while (textEntries.Count > Size)
                {
                    textEntries.RemoveLast();
                }
            }
        }

        private void AutoComplete()
        {
            string commandLine = inputField.Text.Trim();
            var commandParts = commandLine.Split(new char[] { ' ' });
            string commandString = commandParts.First();
            string argumentString = commandParts.Last();
            var hasEnteredCommand = CommandBroker.Commands.Any(command => command == argumentString);

            // Get possible completions
            var completions = hasEnteredCommand || commandParts.Length > 1
                ? CommandBroker.GetPositionalSuggestions(commandString)
                : CommandBroker.Commands.ToList();

            if (commandParts.Length > 1)
            {
                completions = completions.Where(completion => completion.StartsWith(argumentString)).ToList();
            }
            else if (!hasEnteredCommand)
            {
                completions = completions.Where(completion => completion.StartsWith(commandString)).ToList();
            }

            // Fill in input
            var completionText = LongestCommonStartString(completions);
            if (completionText.Any())
            {
                inputField.Text = commandLine[..(commandLine.Length - argumentString.Length)] + completionText;
            }

            // Print possible continuations
            if (completions.Count > 1)
            {
                WriteLine(inputField.DisplayText, CommandTextColour);
                completions.ForEach(completion => WriteLine("\t" + completion));
            }
        }

        private void CallCommand()
        {
            WriteLine(inputField.DisplayText, CommandTextColour);

            if (inputField.Text.Any())
            {
                inputField.RememberCurrent();

                if (CommandString.TryParse(inputField.Text, out var commandString))
                {
                    CommandBroker.Call(commandString);
                }
                else
                {
                    WriteLine("Command syntax incorrect. Try 'command [params] -arg val -flag'", WarningTextColour);
                }
            }

            inputField.Clear();
            scrollOffset = 0;
        }

        private void UpdateKeyboard()
        {
            if (keyboard.IsTyped(Keys.Enter))
            {
                CallCommand();
            }

            if (keyboard.IsTyped(Keys.Tab))
            {
                AutoComplete();
            }

            if (keyboard.IsTyped(Keys.PageUp))
            {
                ScrollUp();
            }

            if (keyboard.IsTyped(Keys.PageDown))
            {
                ScrollDown();
            }
        }

        private void LogAdded(object sender, LogAddedEventArgs e) => WriteLine(e.Log.ToString(), e.Log.Level switch
        {
            LogLevel.Warning => WarningTextColour,
            LogLevel.Error => ErrorTextColour,
            _ => DefaultTextColour,
        });

        private string LongestCommonStartString(ICollection<string> strings)
        {
            string longestCommonString = strings.OrderBy(s => s.Length).LastOrDefault() ?? "";
            strings.ForEach(s => longestCommonString = LongestCommonStartString(longestCommonString, s));
            return longestCommonString;
        }

        private string LongestCommonStartString(string first, string second)
        {
            var result = new StringBuilder();

            string shortest = first.Length > second.Length ? second : first;
            for (int index = 0; index < shortest.Length && first[index] == second[index]; index++)
            {
                result.Append(first[index]);
            }

            return result.ToString();
        }

        private void ScrollDown() => Scroll(-ScrollHeight);

        private void ScrollUp() => Scroll(ScrollHeight);

        private int ScrollHeight => (int)(Area.Height / TextFont.MeasureString("M", TextScale).Y) / 2;

        private void Scroll(int delta) => scrollOffset = MathHelper.Clamp(scrollOffset + delta, 0, textEntries.Count - 1);

        /// <summary>
        /// Class representing a text entry in the console.
        /// </summary>
        private class TextEntry
        {
            public TextEntry(string text, Color color)
            {
                Text = text;
                Color = color;
            }

            public Color Color { get; private set; }
            public string Text { get; private set; }
        }
    }
}
