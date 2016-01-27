namespace MonoKle.Console
{
    using Core;
    using System;
    using System.Collections.Generic;
    using System.Text;

    internal class InputField
    {
        private string command;
        private string commandToken;
        private int cursorPos = 0;
        private string cursorText;
        private Timer cursorTimer;
        private string cursorToken;
        private List<string> history = new List<string>();
        private int historyCapacity;
        private int historyIndex = -1;
        private bool showCursor;
        private string text;

        private StringBuilder textBuilder = new StringBuilder();

        public InputField(string cursorToken, string commandToken, double cursorBlinkRate, int historyCapacity)
        {
            this.cursorToken = cursorToken;
            this.commandToken = commandToken;
            this.cursorTimer = new Timer(cursorBlinkRate);
            this.historyCapacity = historyCapacity;
            this.UpdateCommand();
            this.UpdateText();
        }

        public void Clear()
        {
            this.textBuilder.Clear();
            this.UpdateCommand();
            this.cursorPos = 0;
            this.UpdateText();
        }

        public void CursorLeft()
        {
            this.CursorLeft(1);
        }

        public void CursorLeft(int amount)
        {
            this.cursorPos -= amount;
            this.cursorPos = Math.Max(this.cursorPos, 0);
            this.UpdateText();
            this.showCursor = true;
            this.cursorTimer.Reset();
        }

        public void CursorRight()
        {
            this.CursorRight(1);
        }

        public void CursorRight(int amount)
        {
            this.cursorPos += amount;
            this.cursorPos = Math.Min(this.cursorPos, this.GetInput().Length);
            this.UpdateText();
            this.showCursor = true;
            this.cursorTimer.Reset();
        }

        public void Delete()
        {
            if (this.GetInput().Length > 0 && this.cursorPos < this.GetInput().Length)
            {
                this.textBuilder.Remove(this.cursorPos, 1);
                this.UpdateCommand();
                this.UpdateText();
            }
        }

        public void Erase()
        {
            if (this.GetInput().Length > 0 && this.cursorPos > 0)
            {
                this.textBuilder.Remove(this.cursorPos - 1, 1);
                this.UpdateCommand();
                this.CursorLeft();
                this.UpdateText();
            }
        }

        public string GetInput()
        {
            return this.command;
        }

        public string GetText(bool useCursor)
        {
            if (useCursor && this.showCursor)
            {
                return this.cursorText;
            }
            return this.text;
        }

        public string GetText()
        {
            return this.GetText(false);
        }

        public void NextMemory()
        {
            if (this.history.Count > 0)
            {
                int newIndex = this.historyIndex + 1;
                newIndex = Math.Min(newIndex, this.history.Count - 1);
                this.Set(history[newIndex]);
                this.historyIndex = newIndex;
            }
        }

        public void PreviousMemory()
        {
            if (this.history.Count > 0)
            {
                int newIndex = this.historyIndex - 1;
                newIndex = Math.Max(newIndex, 0);
                this.Set(history[newIndex]);
                this.historyIndex = newIndex;
            }
        }

        public void Remember()
        {
            this.Remember(this.GetInput());
        }

        public void Remember(string input)
        {
            this.history.Add(input);
            while (this.history.Count > historyCapacity)
            {
                this.history.RemoveAt(0);
            }
        }

        public void Set(string text)
        {
            this.textBuilder.Clear();
            this.textBuilder.Append(text);
            this.UpdateCommand();
            this.cursorPos = text.Length;
            this.UpdateText();
        }

        public void Type(char character)
        {
            this.Type(character.ToString());
        }

        public void Type(string text)
        {
            this.textBuilder.Insert(this.cursorPos, text);
            this.UpdateCommand();
            this.CursorRight(text.Length);
            this.UpdateText();
        }

        public void Update(double seconds)
        {
            if (this.cursorTimer.Update(seconds))
            {
                this.showCursor = !this.showCursor;
                this.cursorTimer.Reset();
            }
        }

        private void UpdateCommand()
        {
            this.command = this.textBuilder.ToString();
        }

        private void UpdateText()
        {
            string left, right;

            if (this.command.Length == 0)
            {
                left = this.commandToken + this.command;
                right = "";
            }
            else
            {
                left = this.commandToken + this.command.Substring(0, this.cursorPos);
                right = this.command.Substring(this.cursorPos, this.command.Length - this.cursorPos);
            }

            this.text = left + " " + right;
            this.cursorText = left + cursorToken + right;
            this.historyIndex = this.history.Count;
        }
    }
}