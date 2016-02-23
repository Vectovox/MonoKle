namespace MonoKle.Input.Conversion
{
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// Abstract implementation of <see cref="IKeyConverter"/>.
    /// </summary>
    public abstract class AbstractKeyConverter : IKeyConverter
    {
        /// <summary>
        /// Converts the specified key to its character representation. If conversion failed, returns the default <see cref="char" /> value.
        /// </summary>
        /// <param name="key">The key to convert.</param>
        /// <param name="shift">Indicates shift modifier.</param>
        /// <param name="altgr">Indicates altgr modifier.</param>
        /// <returns>
        /// Character representation of the key; the default <see cref="char" /> value if no representation was found.
        /// </returns>
        public abstract char Convert(Keys key, bool shift, bool altgr);

        /// <summary>
        /// Converts the specified key to its character representation, stored in an out parameter. Return value indicates if conversion succeeded.
        /// </summary>
        /// <param name="key">The key to convert.</param>
        /// <param name="shift">Indicates shift modifier.</param>
        /// <param name="altgr">Indicates altgr modifier.</param>
        /// <param name="value">The value containing the converted value if conversion succeeded; otherwise it contains the default <see cref="char" /> value.</param>
        /// <returns>
        /// True if conversion succeeded; otherwise false.
        /// </returns>
        public bool Convert(Keys key, bool shift, bool altgr, out char value)
        {
            value = this.Convert(key, shift, altgr);
            return value != default(char);
        }
    }
}