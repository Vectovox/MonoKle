namespace MonoKle.Input.Keyboard
{
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// Class for making strings from keyboard input using common text editor behavior.
    /// </summary>
    public class KeyboardTextInput : AbstractTextInput
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardTextInput"/> class.
        /// </summary>
        /// <param name="characterInput">The character input.</param>
        public KeyboardTextInput(KeyboardCharacterInput characterInput)
        {
            CharacterInput = characterInput;
            CursorBeginningKey = Keys.Home;
            CursorEndKey = Keys.End;
            CursorLeftKey = Keys.Left;
            CursorRightKey = Keys.Right;
            DeleteKey = Keys.Delete;
            EraseKey = Keys.Back;
        }

        /// <summary>
        /// Gets or sets the character input.
        /// </summary>
        /// <value>
        /// The character input.
        /// </value>
        public KeyboardCharacterInput CharacterInput { get; private set; }

        /// <summary>
        /// Gets or sets the cursor beginning key.
        /// </summary>
        /// <value>
        /// The cursor beginning key.
        /// </value>
        public Keys CursorBeginningKey { get; set; }

        /// <summary>
        /// Gets or sets the cursor end key.
        /// </summary>
        /// <value>
        /// The cursor end key.
        /// </value>
        public Keys CursorEndKey { get; set; }

        /// <summary>
        /// Gets or sets the cursor left key.
        /// </summary>
        /// <value>
        /// The cursor left key.
        /// </value>
        public Keys CursorLeftKey { get; set; }

        /// <summary>
        /// Gets or sets the cursor right key.
        /// </summary>
        /// <value>
        /// The cursor right key.
        /// </value>
        public Keys CursorRightKey { get; set; }

        /// <summary>
        /// Gets or sets the delete key.
        /// </summary>
        /// <value>
        /// The delete key.
        /// </value>
        public Keys DeleteKey { get; set; }

        /// <summary>
        /// Gets or sets the erase key.
        /// </summary>
        /// <value>
        /// The erase key.
        /// </value>
        public Keys EraseKey { get; set; }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public override void Update()
        {
            if (CharacterInput.GetChar(out var typedCharacter))
            {
                base.Type(typedCharacter);
            }
            if (CharacterInput.KeyboardTyper.IsTyped(EraseKey))
            {
                base.Erase();
            }
            if (CharacterInput.KeyboardTyper.IsTyped(DeleteKey))
            {
                base.Delete();
            }
            if (CharacterInput.KeyboardTyper.IsTyped(CursorBeginningKey))
            {
                base.CursorBeginning();
            }
            if (CharacterInput.KeyboardTyper.IsTyped(CursorEndKey))
            {
                base.CursorEnd();
            }
            if (CharacterInput.KeyboardTyper.IsTyped(CursorLeftKey))
            {
                base.CursorLeft();
            }
            if (CharacterInput.KeyboardTyper.IsTyped(CursorRightKey))
            {
                base.CursorRight();
            }
        }
    }
}