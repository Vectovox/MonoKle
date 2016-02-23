namespace MonoKle.Input
{
    using Microsoft.Xna.Framework;
    using System.Text;

    /// <summary>
    /// Abstract implementation of <see cref="ITextInput"/>.
    /// </summary>
    public abstract class AbstractTextInput : ITextInput
    {
        private int cursorPos;
        private string text = "";
        private StringBuilder textBuilder = new StringBuilder();

        /// <summary>
        /// Gets or sets the cursor position.
        /// </summary>
        /// <value>
        /// The cursor position.
        /// </value>
        public int CursorPosition
        {
            get
            {
                return this.cursorPos;
            }

            set
            {
                this.CursorSet(value);
            }
        }

        /// <summary>
        /// Gets the current text.
        /// </summary>
        /// <value>
        /// The current text.
        /// </value>
        public string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                this.textBuilder.Clear();
                this.textBuilder.Append(value);
                this.UpdatePublicText();
                this.CursorEnd();
                this.OnTextChange();
            }
        }

        /// <summary>
        /// Clears the current text.
        /// </summary>
        public void Clear()
        {
            this.Text = "";
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => this.Text;

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Sets the cursor to before the text.
        /// </summary>
        protected void CursorBeginning()
        {
            this.CursorSet(0);
        }

        /// <summary>
        /// Sets the cursor to after the text.
        /// </summary>
        protected void CursorEnd()
        {
            this.CursorSet(this.textBuilder.Length);
        }

        /// <summary>
        /// Moves the cursor left.
        /// </summary>
        protected void CursorLeft()
        {
            this.CursorMove(-1);
        }

        /// <summary>
        /// Moves the cursor the specified delta.
        /// </summary>
        /// <param name="amount">The amount of characters to move. Negative values indicate movement towards the beginning of the text.</param>
        protected void CursorMove(int amount)
        {
            this.CursorSet(this.cursorPos + amount);
        }

        /// <summary>
        /// Moves the cursor right.
        /// </summary>
        protected void CursorRight()
        {
            this.CursorMove(1);
        }

        /// <summary>
        /// Sets the cursor position.
        /// </summary>
        /// <param name="position">The position.</param>
        protected void CursorSet(int position)
        {
            this.cursorPos = MathHelper.Clamp(position, 0, this.textBuilder.Length);
            this.OnCursorChange();
        }

        /// <summary>
        /// Deletes the character after the cursor.
        /// </summary>
        protected void Delete()
        {
            if (this.Text.Length > 0 && this.cursorPos < this.Text.Length)
            {
                this.textBuilder.Remove(this.cursorPos, 1);
                this.UpdatePublicText();
                this.OnTextChange();
            }
        }

        /// <summary>
        /// Erases the character before the cursor.
        /// </summary>
        protected void Erase()
        {
            if (this.Text.Length > 0 && this.cursorPos > 0)
            {
                this.textBuilder.Remove(this.cursorPos - 1, 1);
                this.UpdatePublicText();
                this.CursorLeft();
                this.OnTextChange();
            }
        }

        /// <summary>
        /// Called when cursor changes.
        /// </summary>
        protected virtual void OnCursorChange()
        {
        }

        /// <summary>
        /// Called when text changes.
        /// </summary>
        protected virtual void OnTextChange()
        {
        }

        /// <summary>
        /// Types the specified character at the current cursor position.
        /// </summary>
        /// <param name="character">The character to type.</param>
        protected void Type(char character)
        {
            this.Type(character.ToString());
        }

        /// <summary>
        /// Types the specified text at the current cursor position.
        /// </summary>
        /// <param name="text">The text to type.</param>
        protected void Type(string text)
        {
            this.textBuilder.Insert(this.cursorPos, text);
            this.UpdatePublicText();
            this.CursorMove(text.Length);
            this.OnTextChange();
        }

        private void UpdatePublicText()
        {
            this.text = this.textBuilder.ToString();
        }
    }
}