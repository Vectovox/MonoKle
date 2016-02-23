namespace MonoKle.Input
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
            this.CharacterInput = characterInput;
            this.CursorBeginningKey = Keys.Home;
            this.CursorEndKey = Keys.End;
            this.CursorLeftKey = Keys.Left;
            this.CursorRightKey = Keys.Right;
            this.DeleteKey = Keys.Delete;
            this.EraseKey = Keys.Back;
        }

        /// <summary>
        /// Gets or sets the character input.
        /// </summary>
        /// <value>
        /// The character input.
        /// </value>
        public KeyboardCharacterInput CharacterInput { get; set; }

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
            char typedCharacter;
            if (this.CharacterInput.GetChar(out typedCharacter))
            {
                base.Type(typedCharacter);
            }
            if (this.CharacterInput.Input.IsKeyTyped(this.EraseKey, this.CharacterInput.TypingStartTime, this.CharacterInput.TypingCycleInterval))
            {
                base.Erase();
            }
            if (this.CharacterInput.Input.IsKeyTyped(this.DeleteKey, this.CharacterInput.TypingStartTime, this.CharacterInput.TypingCycleInterval))
            {
                base.Delete();
            }
            if (this.CharacterInput.Input.IsKeyTyped(this.CursorBeginningKey, this.CharacterInput.TypingStartTime, this.CharacterInput.TypingCycleInterval))
            {
                base.CursorBeginning();
            }
            if (this.CharacterInput.Input.IsKeyTyped(this.CursorEndKey, this.CharacterInput.TypingStartTime, this.CharacterInput.TypingCycleInterval))
            {
                base.CursorEnd();
            }
            if (this.CharacterInput.Input.IsKeyTyped(this.CursorLeftKey, this.CharacterInput.TypingStartTime, this.CharacterInput.TypingCycleInterval))
            {
                base.CursorLeft();
            }
            if (this.CharacterInput.Input.IsKeyTyped(this.CursorRightKey, this.CharacterInput.TypingStartTime, this.CharacterInput.TypingCycleInterval))
            {
                base.CursorRight();
            }
        }
    }
}