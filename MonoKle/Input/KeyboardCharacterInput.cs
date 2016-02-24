namespace MonoKle.Input
{
    using Conversion;
    using Microsoft.Xna.Framework.Input;
    using System;

    /// <summary>
    /// Class querying for characters via keyboard input.
    /// </summary>
    public class KeyboardCharacterInput : AbstractCharacterInput
    {
        private IKeyConverter converter;
        private KeyboardTyper keyboardTyper;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardCharacterInput" /> class, using <see cref="EnglishKeyConverter" /> as key converter.
        /// </summary>
        /// <param name="keyboardTyper">The keyboard typer.</param>
        public KeyboardCharacterInput(KeyboardTyper keyboardTyper)
            : this(keyboardTyper, new EnglishKeyConverter())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardCharacterInput"/> class, using the provided key converter.
        /// </summary>
        /// <param name="keyboardTyper">The keyboard typer.</param>
        /// <param name="converter">The input converter.</param>
        public KeyboardCharacterInput(KeyboardTyper keyboardTyper, IKeyConverter converter)
        {
            if (keyboardTyper == null)
            {
                throw new ArgumentNullException("Keyboard typer must not be null.");
            }
            if (converter == null)
            {
                throw new ArgumentNullException("Key converter must not be null.");
            }
            this.keyboardTyper = keyboardTyper;
            this.Converter = converter;
        }

        /// <summary>
        /// Gets or sets the keyboard input converter.
        /// </summary>
        /// <value>
        /// The keyboard input converter.
        /// </value>
        public IKeyConverter Converter
        {
            get { return this.converter; }
            set { this.converter = value; }
        }

        /// <summary>
        /// Gets or sets the underlying keyboard typer.
        /// </summary>
        /// <value>
        /// The keyboard typer.
        /// </value>
        public KeyboardTyper KeyboardTyper
        {
            get { return this.keyboardTyper; }
        }

        /// <summary>
        /// Gets the currently typed character. If no character is typed then the default <see cref="char" /> value is returned.
        /// </summary>
        /// <returns>
        /// The typed character or the default <see cref="char" /> value.
        /// </returns>
        public override char GetChar()
        {
            bool shift = this.keyboardTyper.KeyboardInput.IsKeyDown(Keys.LeftShift) || this.keyboardTyper.KeyboardInput.IsKeyDown(Keys.RightShift);
            bool altgr = this.keyboardTyper.KeyboardInput.IsKeyDown(Keys.RightAlt);

            char c;
            foreach (Keys k in this.keyboardTyper.KeyboardInput.GetKeysDown())
            {
                if (this.converter.Convert(k, shift, altgr, out c)
                    && this.keyboardTyper.IsTyped(k))
                {
                    return c;
                }
            }

            return default(char);
        }
    }
}