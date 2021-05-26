namespace MonoKle.Input
{
    /// <summary>
    /// Abstract implementation of <see cref="ICharacterInput"/>.
    /// </summary>
    public abstract class AbstractCharacterInput : ICharacterInput
    {
        public abstract char GetChar();

        public bool GetChar(out char character)
        {
            character = GetChar();
            return character != default(char);
        }
    }
}