using Microsoft.Xna.Framework;
using System.Text;

namespace MonoKle.Input
{
    /// <summary>
    /// Implementation of <see cref="ITextInput"/> that relies on manual editing by calling
    /// the appropriate methods.
    /// </summary>
    public class SimpleTextInput : ITextInput
    {
        private int _cursorPos;
        private string _text = "";
        private readonly StringBuilder _textBuilder = new StringBuilder();

        public int CursorPosition
        {
            get => _cursorPos;
            set => CursorSet(value);
        }

        public string Text
        {
            get => _text;
            set
            {
                _textBuilder.Clear();
                _textBuilder.Append(value);
                UpdatePublicText();
                CursorEnd();
                OnTextChange();
            }
        }

        public void Clear() => Text = "";

        public override string ToString() => Text;

        public void CursorBeginning() => CursorSet(0);

        public void CursorEnd() => CursorSet(_textBuilder.Length);

        public void CursorBackward() => CursorMove(-1);

        public void CursorForward() => CursorMove(1);

        public void CursorMove(int amount) => CursorSet(_cursorPos + amount);

        public void CursorSet(int position)
        {
            _cursorPos = MathHelper.Clamp(position, 0, _textBuilder.Length);
            OnCursorChange();
        }

        public void Delete()
        {
            if (Text.Length > 0 && _cursorPos < Text.Length)
            {
                _textBuilder.Remove(_cursorPos, 1);
                UpdatePublicText();
                OnTextChange();
            }
        }

        public void Erase()
        {
            if (Text.Length > 0 && _cursorPos > 0)
            {
                _textBuilder.Remove(_cursorPos - 1, 1);
                UpdatePublicText();
                CursorBackward();
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

        public void Type(char character) => Type(character.ToString());

        public void Type(string text)
        {
            _textBuilder.Insert(_cursorPos, text);
            UpdatePublicText();
            CursorMove(text.Length);
            OnTextChange();
        }

        private void UpdatePublicText() => _text = _textBuilder.ToString();
    }
}