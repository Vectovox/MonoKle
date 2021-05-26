using Microsoft.Xna.Framework.Input;
using System;

namespace MonoKle.Input.Keyboard
{
    /// <summary>
    /// Class querying for characters via keyboard input.
    /// </summary>
    public class KeyboardCharacterInput : AbstractCharacterInput
    {
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
            KeyboardTyper = keyboardTyper ?? throw new ArgumentNullException("Keyboard typer must not be null.");
            Converter = converter ?? throw new ArgumentNullException("Key converter must not be null.");
        }

        /// <summary>
        /// Gets or sets the keyboard input converter.
        /// </summary>
        public IKeyConverter Converter { get; set; }

        /// <summary>
        /// Gets or sets the underlying keyboard typer.
        /// </summary>
        public KeyboardTyper KeyboardTyper { get; }

        public override char GetChar()
        {
            bool shift = KeyboardTyper.Keyboard.IsKeyDown(Keys.LeftShift) || KeyboardTyper.Keyboard.IsKeyDown(Keys.RightShift);
            bool altgr = KeyboardTyper.Keyboard.IsKeyDown(Keys.RightAlt);

            foreach (Keys k in KeyboardTyper.Keyboard.GetKeysDown())
            {
                if (Converter.Convert(k, shift, altgr, out var c)
                    && KeyboardTyper.IsTyped(k))
                {
                    return c;
                }
            }

            return default;
        }
    }
}