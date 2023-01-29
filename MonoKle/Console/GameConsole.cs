using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoKle.Asset;
using MonoKle.Configuration;
using MonoKle.Graphics;
using MonoKle.Input.Keyboard;
using MonoKle.Input.Mouse;
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

        private readonly ConsoleInputField _inputField;
        private readonly KeyboardTyper _keyboard;
        private readonly NativeCharacterInput _characterInput;
        private readonly SpriteBatch _spriteBatch;
        private readonly IMouse _mouse;
        private readonly MTexture _whiteTexture;
        private readonly Timer _caretTimer = new(TimeSpan.FromMilliseconds(100));

        private int _scrollOffset = 0;
        private bool _isOpen = false;
        private bool _showCaret = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameConsole"/> class.
        /// </summary>
        public GameConsole(GameWindow window, MRectangleInt area, GraphicsDevice graphicsDevice, IKeyboard keyboard, IMouse mouse,
            MTexture whiteTexture, FontInstance font, GameConsoleLogData logData)
        {
            _spriteBatch = new SpriteBatch(graphicsDevice);
            _keyboard = new KeyboardTyper(keyboard, TypingActivationDelay, TypingCycleDelay);
            _characterInput = new NativeCharacterInput(window)
            {
                Enabled = false,
            };
            _inputField = new ConsoleInputField("$ ", 10, _characterInput, new KeyboardTyper(keyboard));

            Area = area;
            _mouse = mouse;
            TextFont = font;
            TextSize = 16;
            _whiteTexture = whiteTexture;
            Log = logData;

            CommandBroker = new CommandBroker(this);
            CommandBroker.RegisterCallingAssembly();
        }

        public GameConsoleLogData Log { get; }

        public MRectangleInt Area { get; set; }

        public Color BackgroundColor { get; set; } = new Color(0, 0, 0, 0.7f);

        public Color CommandTextColour { get; set; } = Color.LightGreen;

        public CommandBroker CommandBroker { get; }

        [CVar("console_isopen")]
        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                _isOpen = value;
                _characterInput.Enabled = value;
            }
        }

        [CVar("console_capacity")]
        public uint Capacity { get => Log.Capacity; set => Log.Capacity = value; }

        public FontInstance TextFont { get; set; }

        [CVar("console_textsize")]
        public int TextSize { get => TextFont.Size; set => TextFont.Size = value; }

        [CVar("console_togglekey")]
        public Keys ToggleKey { get; set; } = Keys.F1;

        public void Draw(TimeSpan timeDelta)
        {
            if (IsOpen)
            {
                _spriteBatch.Begin();

                // Draw background
                _spriteBatch.Draw(_whiteTexture, Area, BackgroundColor);

                // Draw input field
                // First text
                var inputTextSize = TextFont.Measure(_inputField.Line);
                var textPosition = new Vector2(Area.Left, Area.Bottom - inputTextSize.Y);
                TextFont.Draw(_spriteBatch, _inputField.Line, textPosition, CommandTextColour);
                // Then caret
                if (_showCaret)
                {
                    var caretPositionText = _inputField.Line[0.._inputField.LineCursorPosition];
                    var caretTextSize = TextFont.Measure(caretPositionText);
                    var caretPosition = new Vector2(Area.Left + caretTextSize.X, Area.Bottom - inputTextSize.Y);
                    TextFont.Draw(_spriteBatch, "_", caretPosition, Color.White);
                }

                // Draw log lines
                foreach (var line in Log.TextEntries.Skip(_scrollOffset))
                {
                    var stringToDraw = TextFont.Wrap(line.Text, Area.Width);
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
                UpdateMouse();
                _inputField.Update();
                if (_caretTimer.Update(timeDelta, true))
                {
                    _showCaret = !_showCaret;
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
                Log.WriteLine(_inputField.Line.ToString(), CommandTextColour);
                completions.ForEach(completion => Log.WriteLine("\t" + completion));
            }
        }

        private void CallCommand()
        {
            Log.WriteLine(_inputField.Line.ToString(), CommandTextColour);

            if (_inputField.Text.Any())
            {
                _inputField.RememberCurrent();

                if (CommandString.TryParse(_inputField.Text.Trim(), out var commandString))
                {
                    CommandBroker.Call(commandString);
                }
                else
                {
                    Log.WriteWarning("Command syntax incorrect. Try 'command [params] -arg val -flag'");
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
            else if (_keyboard.IsTyped(Keys.Tab))
            {
                AutoComplete();
            }
            else if (_keyboard.IsTyped(Keys.PageUp))
            {
                ScrollUp();
            }
            else if (_keyboard.IsTyped(Keys.PageDown))
            {
                ScrollDown();
            }
        }

        private void UpdateMouse()
        {
            if (_mouse.ScrollDirection == Input.MouseScrollDirection.Down)
            {
                ScrollDown();
            }
            else if (_mouse.ScrollDirection == Input.MouseScrollDirection.Up)
            {
                ScrollUp();
            }
        }

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

        private void Scroll(int delta) => _scrollOffset = MathHelper.Clamp(_scrollOffset + delta, 0, Log.Count - 1);
    }
}
