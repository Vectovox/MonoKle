namespace MonoKle.Input
{
    /// <summary>
    /// Abstract implementation of <see cref="ICharacterInput"/>.
    /// </summary>
    public abstract class AbstractCharacterInput : ICharacterInput
    {
        /// <summary>
        /// Gets the currently typed character. If no character is typed then the default <see cref="char" /> value is returned.
        /// </summary>
        /// <returns>
        /// The typed character or the default <see cref="char" /> value.
        /// </returns>
        public abstract char GetChar();

        /// <summary>
        /// Gets the typed character using an out parameter. The return value indicates whether a character was typed or not.
        /// </summary>
        /// <param name="character">When this method returns, contains either a typed character or the default <see cref="char" /> value.</param>
        /// <returns>
        /// True if a character was typed; otherwise false.
        /// </returns>
        public bool GetChar(out char character)
        {
            character = this.GetChar();
            return character != default(char);
        }
    }
}