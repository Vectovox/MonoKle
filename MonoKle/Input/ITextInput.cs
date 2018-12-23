namespace MonoKle.Input {
    /// <summary>
    /// Defines text input methods.
    /// </summary>
    public interface ITextInput {
        /// <summary>
        /// Gets or sets the cursor position.
        /// </summary>
        /// <value>
        /// The cursor position.
        /// </value>
        int CursorPosition { get; set; }

        /// <summary>
        /// Gets or sets the current text.
        /// </summary>
        /// <value>
        /// The current text.
        /// </value>
        string Text { get; set; }

        /// <summary>
        /// Clears the current text.
        /// </summary>
        void Clear();

        /// <summary>
        /// Updates this instance.
        /// </summary>
        void Update();
    }
}