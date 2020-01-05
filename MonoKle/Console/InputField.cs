namespace MonoKle.Console
{
    using Input.Keyboard;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using System;
    using System.Collections.Generic;

    internal class InputField : KeyboardTextInput
    {
        private string commandToken;
        private Timer cursorTimer;
        private string cursorToken;
        private string displayText;
        private string displayTextCursor;
        private string displayTextCursorNoToken;
        private List<string> history = new List<string>();
        private int historyCapacity;
        private int historyIndex = -1;
        private bool showCursor;

        public InputField(string cursorToken, string commandToken, double cursorBlinkRate, int historyCapacity, KeyboardCharacterInput characterInput)
            : base(characterInput)
        {
            this.cursorToken = cursorToken;
            this.commandToken = commandToken;
            cursorTimer = new Timer(TimeSpan.FromSeconds(cursorBlinkRate));
            NextMemoryKey = Keys.Down;
            PreviousMemoryKey = Keys.Up;
            this.historyCapacity = historyCapacity;
            OnTextChange();
        }

        public string DisplayText => displayText;

        public string DisplayTextCursor => showCursor ? displayTextCursor : displayTextCursorNoToken;

        public Keys NextMemoryKey { get; set; }
        public Keys PreviousMemoryKey { get; set; }

        public void Remember() => Remember(Text);

        public void Remember(string input)
        {
            history.Add(input);
            while (history.Count > historyCapacity)
            {
                history.RemoveAt(0);
            }
        }

        public void Update(TimeSpan timeDelta)
        {
            Update();

            if (cursorTimer.UpdateDone(timeDelta))
            {
                showCursor = !showCursor;
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
                base.Text = history[newIndex];
                historyIndex = newIndex;
            }
        }

        private void UpdateDisplayText()
        {
            string left, right;

            if (base.Text.Length == 0)
            {
                left = commandToken + base.Text;
                right = "";
            }
            else
            {
                left = commandToken + base.Text.Substring(0, base.CursorPosition);
                right = base.Text.Substring(base.CursorPosition, base.Text.Length - base.CursorPosition);
            }

            displayText = left + right;
            displayTextCursor = left + cursorToken + right;
            displayTextCursorNoToken = left + " " + right;
        }
    }
}
