using Microsoft.Xna.Framework.Input;

namespace MonoKle.Input.Keyboard
{
    /// <summary>
    /// Class for making strings from keyboard input using common text editor behavior.
    /// </summary>
    public class KeyboardTextInput : SimpleTextInput
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardTextInput"/> class.
        /// </summary>
        /// <param name="characterInput">The character input.</param>
        /// <param name="keyboardTyper">Keyboard typer for cursor changes.</param>
        public KeyboardTextInput(ICharacterInput characterInput, KeyboardTyper keyboardTyper)
        {
            CharacterInput = characterInput;
            KeyboardTyper = keyboardTyper;
            CursorBeginningKey = Keys.Home;
            CursorEndKey = Keys.End;
            CursorLeftKey = Keys.Left;
            CursorRightKey = Keys.Right;
            DeleteKey = Keys.Delete;
            EraseKey = Keys.Back;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardTextInput"/> class.
        /// </summary>
        /// <param name="keyboardTyper">Keyboard typer.</param>
        public KeyboardTextInput(KeyboardTyper keyboardTyper) : this(new KeyboardCharacterInput(keyboardTyper), keyboardTyper)
        {
        }

        /// <summary>
        /// Gets the character input.
        /// </summary>
        public ICharacterInput CharacterInput { get; set; }

        /// <summary>
        /// Gets the keyboard typer
        /// </summary>
        public KeyboardTyper KeyboardTyper { get; }

        /// <summary>
        /// Gets or sets the cursor beginning key.
        /// </summary>
        public Keys CursorBeginningKey { get; set; }

        /// <summary>
        /// Gets or sets the cursor end key.
        /// </summary>
        public Keys CursorEndKey { get; set; }

        /// <summary>
        /// Gets or sets the cursor left key.
        /// </summary>
        public Keys CursorLeftKey { get; set; }

        /// <summary>
        /// Gets or sets the cursor right key.
        /// </summary>
        public Keys CursorRightKey { get; set; }

        /// <summary>
        /// Gets or sets the delete key.
        /// </summary>
        public Keys DeleteKey { get; set; }

        /// <summary>
        /// Gets or sets the erase key.
        /// </summary>
        public Keys EraseKey { get; set; }

        /// <summary>
        /// Updates this instance to poll the keyboard state.
        /// </summary>
        public void Update()
        {
            if (CharacterInput.TryGetChar(out var typedCharacter))
            {
                Type(typedCharacter);
            }
            else if (KeyboardTyper.IsTyped(EraseKey))
            {
                Erase();
            }
            else if (KeyboardTyper.IsTyped(DeleteKey))
            {
                Delete();
            }
            else if (KeyboardTyper.IsTyped(CursorBeginningKey))
            {
                CursorBeginning();
            }
            else if (KeyboardTyper.IsTyped(CursorEndKey))
            {
                CursorEnd();
            }
            else if (KeyboardTyper.IsTyped(CursorLeftKey))
            {
                CursorBackward();
            }
            else if (KeyboardTyper.IsTyped(CursorRightKey))
            {
                CursorForward();
            }
        }
    }
}