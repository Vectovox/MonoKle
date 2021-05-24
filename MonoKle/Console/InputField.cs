using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoKle.Input.Keyboard;
using System;
using System.Collections.Generic;

namespace MonoKle.Console
{
    /// <summary>
    /// Class for a CLI-like input field.
    /// </summary>
    public class InputField : KeyboardTextInput
    {
        private readonly List<string> _history;

        private readonly Timer _cursorTimer;

        private readonly string _commandToken;
        private readonly string _cursorToken;

        private string _displayTextCursor = "";
        private string _displayTextCursorNoToken = "";

        private int _historyIndex = 0;
        private bool _isBlinking;

        public InputField(string cursorToken, string commandToken, TimeSpan cursorBlinkRate, int historyCapacity, KeyboardCharacterInput characterInput)
            : base(characterInput)
        {
            _cursorToken = cursorToken;
            _commandToken = commandToken;
            _history = new List<string>(historyCapacity);
            _cursorTimer = new Timer(cursorBlinkRate);
            OnTextChange();
        }

        public string DisplayText { get; private set; } = "";

        public string CursorDisplayText => _isBlinking ? _displayTextCursorNoToken : _displayTextCursor;

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
            if (_history.Count == _history.Capacity)
            {
                _history.RemoveAt(0);
            }
            _history.Add(text);
        }

        public void Update(TimeSpan timeDelta)
        {
            Update();

            if (_cursorTimer.Update(timeDelta))
            {
                _isBlinking = !_isBlinking;
                _cursorTimer.Reset();
            }

            if (CharacterInput.KeyboardTyper.IsTyped(NextMemoryKey))
            {
                ChangeMemory(1);
            }

            if (CharacterInput.KeyboardTyper.IsTyped(PreviousMemoryKey))
            {
                ChangeMemory(-1);
            }
        }

        protected override void OnCursorChange() => UpdateDisplayText();

        protected override void OnTextChange()
        {
            _historyIndex = _history.Count;
            UpdateDisplayText();
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

        private void UpdateDisplayText()
        {
            var beforeCursor = _commandToken + Text[..CursorPosition];
            var afterCursor = Text[CursorPosition..];
            DisplayText = beforeCursor + afterCursor;
            _displayTextCursor = beforeCursor + _cursorToken + afterCursor;
            _displayTextCursorNoToken = beforeCursor + " " + afterCursor;
        }
    }
}
