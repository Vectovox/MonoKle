using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoKle.Input;
using MonoKle.Input.Keyboard;
using System;
using System.Collections.Generic;

namespace MonoKle.Console
{
    /// <summary>
    /// Class for a CLI-like input field.
    /// </summary>
    public class ConsoleInputField : KeyboardTextInput
    {
        private readonly List<string> _history;
        private readonly string _commandToken;
        private readonly KeyboardTyper _keyboardTyper;
        
        private string _line = string.Empty;
        private int _historyIndex = 0;

        public ConsoleInputField(string commandToken, int historyCapacity, ICharacterInput characterInput, KeyboardTyper keyboardTyper)
            : base(characterInput, keyboardTyper)
        {
            _commandToken = commandToken;
            _keyboardTyper = keyboardTyper;
            _history = new List<string>(historyCapacity);
            OnTextChange();
        }

        public int LineCursorPosition => CursorPosition + _commandToken.Length;

        public ReadOnlySpan<char> Line => _line.AsSpan();

        public Keys NextMemoryKey { get; set; } = Keys.Down;

        public Keys PreviousMemoryKey { get; set; } = Keys.Up;

        /// <summary>
        /// Remembers the current line.
        /// </summary>
        public void RememberCurrent() => Remember(Text);

        /// <summary>
        /// Remembers the given text.
        /// </summary>
        /// <param name="text">The text to remember.</param>
        public void Remember(string text)
        {
            // Remove duplicates from history
            _history.RemoveAll(t => t == text);
            // Clamp to capacity
            if (_history.Count == _history.Capacity)
            {
                _history.RemoveAt(0);
            }
            // Remember
            _history.Add(text);
        }

        public override void Update()
        {
            base.Update();

            if (_keyboardTyper.IsTyped(NextMemoryKey))
            {
                ChangeMemory(1);
            }
            else if (_keyboardTyper.IsTyped(PreviousMemoryKey))
            {
                ChangeMemory(-1);
            }
        }

        protected override void OnTextChange()
        {
            _historyIndex = _history.Count;
            _line = _commandToken + Text;
        }

        private void ChangeMemory(int delta)
        {
            if (_history.Count > 0)
            {
                int newIndex = _historyIndex + delta;
                newIndex = MathHelper.Clamp(newIndex, 0, _history.Count - 1);
                Text = _history[newIndex];
                // Overwrite history index after text update
                _historyIndex = newIndex;
            }
        }
    }
}
