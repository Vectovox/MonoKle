using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoKle.Asset;
using MonoKle.Configuration;
using MonoKle.Graphics;
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

        private readonly InputField _inputField;
        private readonly KeyboardTyper _keyboard;
        private readonly LinkedList<TextEntry> _textEntries = new LinkedList<TextEntry>();
        private readonly SpriteBatch _spriteBatch;
        private readonly MTexture _whiteTexture;

        private int _scrollOffset = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameConsole"/> class.
        /// </summary>
        /// <param name="area">The area.</param>
        /// <param name="graphicsDevice">The graphics device.</param>
        /// <param name="keyboardInput">The keyboard input.</param>
        /// <param name="whiteTexture">The background texture.</param>
        /// <param name="logger">The logger to use.</param>
        public GameConsole(MRectangleInt area, GraphicsDevice graphicsDevice, IKeyboard keyboardInput, MTexture whiteTexture, FontInstance font, Logger logger)
        {
            _spriteBatch = new SpriteBatch(graphicsDevice);
            _keyboard = new KeyboardTyper(keyboardInput, TypingActivationDelay, TypingCycleDelay);
            _inputField = new InputField("-", "$ ", TimeSpan.FromSeconds(0.25), 10, new KeyboardCharacterInput(_keyboard));

            Area = area;
            TextFont = font;
            TextSize = 16;
            _whiteTexture = whiteTexture;
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

        [CVar("c_isopen")]
        public bool IsOpen { get; set; }

        [CVar("c_size")]
        public uint Size { get; set; } = byte.MaxValue;

        public int TabLength { get; set; } = 4;

        public FontInstance TextFont { get; set; }

        [CVar("c_textsize")]
        public int TextSize { get => TextFont.Size; set => TextFont.Size = value; }

        public Keys ToggleKey { get; set; } = Keys.F1;

        public Color WarningTextColour { get; set; } = Color.Yellow;

        public void Clear() => _textEntries.Clear();

        public void Draw(TimeSpan timeDelta)
        {
            if (IsOpen)
            {
                _spriteBatch.Begin();

                // Draw background
                _spriteBatch.Draw(_whiteTexture, Area, BackgroundColor);

                // Draw input field
                var textPosition = new Vector2(Area.Left, Area.Bottom - TextFont.Measure(_inputField.CursorDisplayText).Y);
                TextFont.Draw(_spriteBatch, _inputField.CursorDisplayText, textPosition, CommandTextColour);

                // Draw all lines
                foreach (var line in _textEntries.Skip(_scrollOffset))
                {
                    string stringToDraw = TextFont.Wrap(line.Text, Area.Width);
                    var stringHeight = TextFont.Measure(stringToDraw).Y;
                    textPosition.Y -= stringHeight;
                    TextFont.Draw(_spriteBatch, stringToDraw, textPosition, line.Color);

                    if (textPosition.Y + stringHeight < 0)
                    {
                        break;
                    }
                }

                _spriteBatch.End();
            }
        }

        public void Update(TimeSpan timeDelta)
        {
            if (_keyboard.Keyboard.IsKeyPressed(ToggleKey))
            {
                IsOpen = !IsOpen;
            }

            if (IsOpen)
            {
                UpdateKeyboard();
                _inputField.Update(timeDelta);
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

                _textEntries.AddFirst(new TextEntry(entryBuilder.ToString(), color));

                while (_textEntries.Count > Size)
                {
                    _textEntries.RemoveLast();
                }
            }
        }

        private void AutoComplete()
        {
            string commandLine = _inputField.Text.Trim();
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
                _inputField.Text = commandLine[..(commandLine.Length - argumentString.Length)] + completionText;
            }

            // Print possible continuations
            if (completions.Count > 1)
            {
                WriteLine(_inputField.DisplayText, CommandTextColour);
                completions.ForEach(completion => WriteLine("\t" + completion));
            }
        }

        private void CallCommand()
        {
            WriteLine(_inputField.DisplayText, CommandTextColour);

            if (_inputField.Text.Any())
            {
                _inputField.RememberCurrent();

                if (CommandString.TryParse(_inputField.Text, out var commandString))
                {
                    CommandBroker.Call(commandString);
                }
                else
                {
                    WriteLine("Command syntax incorrect. Try 'command [params] -arg val -flag'", WarningTextColour);
                }
            }

            _inputField.Clear();
            _scrollOffset = 0;
        }

        private void UpdateKeyboard()
        {
            if (_keyboard.IsTyped(Keys.Enter))
            {
                CallCommand();
            }

            if (_keyboard.IsTyped(Keys.Tab))
            {
                AutoComplete();
            }

            if (_keyboard.IsTyped(Keys.PageUp))
            {
                ScrollUp();
            }

            if (_keyboard.IsTyped(Keys.PageDown))
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

        private int ScrollHeight => (int)(Area.Height / TextFont.Measure("M").Y) / 2;

        private void Scroll(int delta) => _scrollOffset = MathHelper.Clamp(_scrollOffset + delta, 0, _textEntries.Count - 1);

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
