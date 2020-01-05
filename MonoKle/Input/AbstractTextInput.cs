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
                return cursorPos;
            }

            set
            {
                CursorSet(value);
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
                return text;
            }

            set
            {
                textBuilder.Clear();
                textBuilder.Append(value);
                UpdatePublicText();
                CursorEnd();
                OnTextChange();
            }
        }

        /// <summary>
        /// Clears the current text.
        /// </summary>
        public void Clear() => Text = "";

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString() => Text;

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Sets the cursor to before the text.
        /// </summary>
        protected void CursorBeginning() => CursorSet(0);

        /// <summary>
        /// Sets the cursor to after the text.
        /// </summary>
        protected void CursorEnd() => CursorSet(textBuilder.Length);

        /// <summary>
        /// Moves the cursor left.
        /// </summary>
        protected void CursorLeft() => CursorMove(-1);

        /// <summary>
        /// Moves the cursor the specified delta.
        /// </summary>
        /// <param name="amount">The amount of characters to move. Negative values indicate movement towards the beginning of the text.</param>
        protected void CursorMove(int amount) => CursorSet(cursorPos + amount);

        /// <summary>
        /// Moves the cursor right.
        /// </summary>
        protected void CursorRight() => CursorMove(1);

        /// <summary>
        /// Sets the cursor position.
        /// </summary>
        /// <param name="position">The position.</param>
        protected void CursorSet(int position)
        {
            cursorPos = MathHelper.Clamp(position, 0, textBuilder.Length);
            OnCursorChange();
        }

        /// <summary>
        /// Deletes the character after the cursor.
        /// </summary>
        protected void Delete()
        {
            if (Text.Length > 0 && cursorPos < Text.Length)
            {
                textBuilder.Remove(cursorPos, 1);
                UpdatePublicText();
                OnTextChange();
            }
        }

        /// <summary>
        /// Erases the character before the cursor.
        /// </summary>
        protected void Erase()
        {
            if (Text.Length > 0 && cursorPos > 0)
            {
                textBuilder.Remove(cursorPos - 1, 1);
                UpdatePublicText();
                CursorLeft();
                OnTextChange();
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
        protected void Type(char character) => Type(character.ToString());

        /// <summary>
        /// Types the specified text at the current cursor position.
        /// </summary>
        /// <param name="text">The text to type.</param>
        protected void Type(string text)
        {
            textBuilder.Insert(cursorPos, text);
            UpdatePublicText();
            CursorMove(text.Length);
            OnTextChange();
        }

        private void UpdatePublicText() => text = textBuilder.ToString();
    }
}