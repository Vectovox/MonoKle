using Microsoft.Xna.Framework;
using System;

namespace MonoKle.Input.Keyboard
{
    /// <summary>
    /// Class querying for characters via native text input.
    /// </summary>
    public class NativeCharacterInput : AbstractCharacterInput, IDisposable
    {
        public static readonly char[] SPECIAL_CHARACTERS = { '\r', '\n', '\a', '\b', '\t', '\f', '\v' };

        private char _lastChar;
        private bool _disposedValue;
        private GameWindow? _window;

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeCharacterInput" /> class.
        /// </summary>
        public NativeCharacterInput(GameWindow window)
        {
            _window = window;
            _window.TextInput += Window_TextInput;
        }

        /// <summary>
        /// Gets or sets whether characters should be captured.
        /// </summary>
        public bool Enabled { get; set; }

        private void Window_TextInput(object? sender, TextInputEventArgs e)
        {
            if (!Enabled)
            {
                // NOOP if disabled
                return;
            }

            // Update input character
            _lastChar = e.Character;

            // Special case: Special characters are ignored
            for (int i = 0; i < SPECIAL_CHARACTERS.Length; i++)
            {
                if (_lastChar == SPECIAL_CHARACTERS[i])
                {
                    _lastChar = default;
                    return;
                }
            }
        }

        public override char GetChar()
        {
            var result = _lastChar;
            _lastChar = default;
            return result;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _window!.TextInput -= Window_TextInput;
                }
                _window = null;
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}