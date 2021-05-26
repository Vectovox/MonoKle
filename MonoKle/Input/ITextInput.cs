using System.Collections.Generic;

namespace MonoKle.Input
{
    /// <summary>
    /// Defines text input methods.
    /// </summary>
    public interface ITextInput
    {
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
        /// Gets or sets whether the cursor is enabled.
        /// </summary>
        bool CursorEnabled { get; set; }

        /// <summary>
        /// Gets or sets the characters that are included. If empty, all characters are considered included.
        /// </summary>
        HashSet<char> IncludedCharacters { get; set; }

        /// <summary>
        /// Gets or sets the characters that are excluded. If empty, no characters are considered excluded.
        /// </summary>
        HashSet<char> ExcludedCharacters { get; set; }

        /// <summary>
        /// Gets or sets the maximum amount of characters allowed.
        /// </summary>
        int MaxLength { get; set; }

        /// <summary>
        /// Clears the current text.
        /// </summary>
        void Clear();

        /// <summary>
        /// Sets the cursor to before the text.
        /// </summary>
        void CursorBeginning();

        /// <summary>
        /// Sets the cursor to after the text.
        /// </summary>
        void CursorEnd();

        /// <summary>
        /// Moves the cursor towards the beginning.
        /// </summary>
        void CursorBackward();
        
        /// <summary>
        /// Moves the cursor towards the end.
        /// </summary>
        void CursorForward();

        /// <summary>
        /// Moves the cursor the specified delta.
        /// </summary>
        /// <param name="amount">The amount of characters to move. Negative values indicate movement towards the beginning of the text.</param>
        void CursorMove(int amount);

        /// <summary>
        /// Sets the cursor position.
        /// </summary>
        /// <param name="position">The position.</param>
        void CursorSet(int position);

        /// <summary>
        /// Removes the character after the cursor.
        /// </summary>
        void Delete();

        /// <summary>
        /// Removes the character before the cursor.
        /// </summary>
        void Erase();

        /// <summary>
        /// Types the specified character at the current cursor position.
        /// </summary>
        /// <remarks>
        /// If character is not available due to <see cref="IncludedCharacters"/> or <see cref="ExcludedCharacters"/>
        /// it will be skipped. The text will only become as long as <see cref="MaxLength"/>, after which characters are
        /// swallowed.
        /// </remarks>
        /// <param name="character">The character to type.</param>
        void Type(char character);

        /// <summary>
        /// Types the specified string at the current cursor position.
        /// </summary>
        /// <param name="text">The text to type.</param>
        /// <remarks>
        /// If character is not available due to <see cref="IncludedCharacters"/> or <see cref="ExcludedCharacters"/>
        /// it will be skipped. The text will only become as long as <see cref="MaxLength"/>, after which characters are
        /// swallowed.
        /// </remarks>
        void Type(string text);
    }
}