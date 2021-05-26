using Microsoft.Xna.Framework;
using System.Collections.Generic;
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

        public bool CursorEnabled { get; set; } = true;

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

        public int MaxLength { get; set; } = int.MaxValue;
        
        public HashSet<char> IncludedCharacters { get; set; } = new HashSet<char>(0);
        
        public HashSet<char> ExcludedCharacters { get; set; } = new HashSet<char>(0);

        public void Clear() => Text = "";

        public override string ToString() => Text;

        public void CursorBeginning() => CursorSet(0);

        public void CursorEnd() => CursorSet(_textBuilder.Length);

        public void CursorBackward() => CursorMove(-1);

        public void CursorForward() => CursorMove(1);

        public void CursorMove(int amount) => CursorSet(_cursorPos + amount);

        public void CursorSet(int position)
        {
            // Record where it was
            var oldPos = _cursorPos;

            // Do the change
            if (CursorEnabled)
            {
                _cursorPos = MathHelper.Clamp(position, 0, _textBuilder.Length);
            }
            else
            {
                _cursorPos = _textBuilder.Length;
            }

            // Notify if changed
            if (_cursorPos != oldPos)
            {
                OnCursorChange();
            }
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

        public void Type(char character)
        {
            if (InternalType(character))
            {
                UpdatePublicText();
                OnTextChange();
                OnCursorChange();
            }
        }

        public void Type(string text)
        {
            var anyTyped = false;
            foreach (var c in text)
            {
                anyTyped |= InternalType(c);
            }

            if (anyTyped)
            {
                UpdatePublicText();
                OnTextChange();
                OnCursorChange();
            }
        }

        private bool InternalType(char character)
        {
            // Include and Exclude character
            if (IncludedCharacters.Count != 0 && !IncludedCharacters.Contains(character))
            {
                return false;
            }
            if (ExcludedCharacters.Count != 0 && ExcludedCharacters.Contains(character))
            {
                return false;
            }
            if (_textBuilder.Length >= MaxLength)
            {
                return false;
            }
            // Type the character
            _textBuilder.Insert(_cursorPos, character);
            _cursorPos++;
            return true;
        }

        private void UpdatePublicText() => _text = _textBuilder.ToString();
    }
}