namespace MonoKle.Console
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using Input.Keyboard;
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
            this.cursorTimer = new Timer(cursorBlinkRate);
            this.NextMemoryKey = Keys.Down;
            this.PreviousMemoryKey = Keys.Up;
            this.historyCapacity = historyCapacity;
            this.OnTextChange();
        }

        public string DisplayText => this.displayText;

        public string DisplayTextCursor => this.showCursor ? this.displayTextCursor : this.displayTextCursorNoToken;

        public Keys NextMemoryKey { get; set; }
        public Keys PreviousMemoryKey { get; set; }

        public void Remember()
        {
            this.Remember(base.Text);
        }

        public void Remember(string input)
        {
            this.history.Add(input);
            while (this.history.Count > historyCapacity)
            {
                this.history.RemoveAt(0);
            }
        }

        public void Update(double seconds)
        {
            this.Update();

            if (this.cursorTimer.Update(seconds))
            {
                this.showCursor = !this.showCursor;
                this.cursorTimer.Reset();
            }

            if (this.CharacterInput.KeyboardTyper.IsTyped(this.NextMemoryKey))
            {
                this.ChangeMemory(1);
            }

            if (this.CharacterInput.KeyboardTyper.IsTyped(this.PreviousMemoryKey))
            {
                this.ChangeMemory(-1);
            }
        }

        protected override void OnCursorChange()
        {
            this.UpdateDisplayText();
        }

        protected override void OnTextChange()
        {
            this.historyIndex = this.history.Count;
            this.UpdateDisplayText();
        }

        private void ChangeMemory(int delta)
        {
            if (this.history.Count > 0)
            {
                int newIndex = this.historyIndex + delta;
                newIndex = MathHelper.Clamp(newIndex, 0, this.history.Count - 1);
                base.Text = history[newIndex];
                this.historyIndex = newIndex;
            }
        }

        private void UpdateDisplayText()
        {
            string left, right;

            if (base.Text.Length == 0)
            {
                left = this.commandToken + base.Text;
                right = "";
            }
            else
            {
                left = this.commandToken + base.Text.Substring(0, base.CursorPosition);
                right = base.Text.Substring(base.CursorPosition, base.Text.Length - base.CursorPosition);
            }

            this.displayText = left + right;
            this.displayTextCursor = left + cursorToken + right;
            this.displayTextCursorNoToken = left + " " + right;
        }
    }
}