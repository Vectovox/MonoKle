namespace MonoKle.Input
{
    /// <summary>
    /// Defines a method for character input.
    /// </summary>
    public interface ICharacterInput
    {
        /// <summary>
        /// Gets the currently typed character. If no character is typed then the default <see cref="char"/> value is returned.
        /// </summary>
        /// <returns>The typed character or the default <see cref="char"/> value.</returns>
        char GetChar();

        /// <summary>
        /// Gets the typed character using an out parameter. The return value indicates whether a character was typed or not.
        /// </summary>
        /// <param name="character">When this method returns, contains either a typed character or the default <see cref="char"/> value.</param>
        /// <returns>True if a character was typed; otherwise false.</returns>
        bool TryGetChar(out char character);
    }
}