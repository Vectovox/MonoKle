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
        private readonly List<string> history;

        private readonly Timer cursorTimer;

        private readonly string commandToken;
        private readonly string cursorToken;

        private string displayTextCursor = "";
        private string displayTextCursorNoToken = "";

        private int historyIndex = 0;
        private bool isBlinking;

        public InputField(string cursorToken, string commandToken, TimeSpan cursorBlinkRate, int historyCapacity, KeyboardCharacterInput characterInput)
            : base(characterInput)
        {
            this.cursorToken = cursorToken;
            this.commandToken = commandToken;
            history = new List<string>(historyCapacity);
            cursorTimer = new Timer(cursorBlinkRate);
            OnTextChange();
        }

        public string DisplayText { get; private set; } = "";

        public string CursorDisplayText => isBlinking ? displayTextCursorNoToken : displayTextCursor;

        public Keys NextMemoryKey { get; set; } = Keys.Down;

        public Keys PreviousMemoryKey { get; set; } = Keys.Up;

        /// <summary>
        /// Remembers the current line.
        /// </summary>
        public void RememberCurrent() => Remember(Text);

        /// <summary>
        /// Remembers the given text.
        /// </summary>
        /// <param name="input">The text to remember.</param>
        public void Remember(string text)
        {
            if (history.Count == history.Capacity)
            {
                history.RemoveAt(0);
            }
            history.Add(text);
        }

        public void Update(TimeSpan timeDelta)
        {
            Update();

            if (cursorTimer.UpdateDone(timeDelta))
            {
                isBlinking = !isBlinking;
                cursorTimer.Reset();
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
            historyIndex = history.Count;
            UpdateDisplayText();
        }

        private void ChangeMemory(int delta)
        {
            if (history.Count > 0)
            {
                int newIndex = historyIndex + delta;
                newIndex = MathHelper.Clamp(newIndex, 0, history.Count - 1);
                Text = history[newIndex];
                // Overwrite history index after text update
                historyIndex = newIndex;
            }
        }

        private void UpdateDisplayText()
        {
            var beforeCursor = commandToken + Text[..CursorPosition];
            var afterCursor = Text[CursorPosition..];
            DisplayText = beforeCursor + afterCursor;
            displayTextCursor = beforeCursor + cursorToken + afterCursor;
            displayTextCursorNoToken = beforeCursor + " " + afterCursor;
        }
    }
}
