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
        private const double DefaultTypingCycleInterval = 0.05f;
        private const double DefaultTypingStartTime = 0.5f;
        private IKeyConverter converter;
        private IKeyboardInput input;
        private double typingCycleInterval;
        private double typingStartTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardCharacterInput"/> class, using <see cref="EnglishKeyConverter"/> as key converter and
        /// default values for typing times.
        /// </summary>
        /// <param name="keyboardInput">The keyboard input.</param>
        public KeyboardCharacterInput(IKeyboardInput keyboardInput)
            : this(keyboardInput, new EnglishKeyConverter(), KeyboardCharacterInput.DefaultTypingStartTime, KeyboardCharacterInput.DefaultTypingCycleInterval)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardCharacterInput"/> class, using <see cref="EnglishKeyConverter"/> as key converter.
        /// </summary>
        /// <param name="keyboardInput">The keyboard input.</param>
        /// <param name="typingStartTime">The typing start time.</param>
        /// <param name="typingCycleInterval">The typing cycle interval.</param>
        public KeyboardCharacterInput(IKeyboardInput keyboardInput, double typingStartTime, double typingCycleInterval)
            : this(keyboardInput, new EnglishKeyConverter(), typingStartTime, typingCycleInterval)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardCharacterInput"/> class, using the provided key converter.
        /// </summary>
        /// <param name="keyboardInput">The keyboard input.</param>
        /// <param name="converter">The input converter.</param>
        /// <param name="typingStartTime">The typing start time.</param>
        /// <param name="typingCycleInterval">The typing cycle interval.</param>
        public KeyboardCharacterInput(IKeyboardInput keyboardInput, IKeyConverter converter, double typingStartTime, double typingCycleInterval)
        {
            if (keyboardInput == null)
            {
                throw new ArgumentNullException("Keyboard input must not be null.");
            }
            if (converter == null)
            {
                throw new ArgumentNullException("Key converter must not be null.");
            }
            this.Input = keyboardInput;
            this.Converter = converter;
            this.TypingStartTime = typingStartTime;
            this.TypingCycleInterval = typingCycleInterval;
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
        /// Gets or sets the underlying keyboard input method.
        /// </summary>
        /// <value>
        /// The keyboard input method.
        /// </value>
        public IKeyboardInput Input
        {
            get { return this.input; }
            set { this.input = value; }
        }

        /// <summary>
        /// Gets or sets the typing cycle interval.
        /// </summary>
        /// <value>
        /// The typing cycle interval.
        /// </value>
        public double TypingCycleInterval
        {
            get { return this.typingCycleInterval; }
            set { this.typingCycleInterval = value; }
        }

        /// <summary>
        /// Gets or sets the typing start time.
        /// </summary>
        /// <value>
        /// The typing start time.
        /// </value>
        public double TypingStartTime
        {
            get { return this.typingStartTime; }
            set { this.typingStartTime = value; }
        }

        /// <summary>
        /// Gets the currently typed character. If no character is typed then the default <see cref="char" /> value is returned.
        /// </summary>
        /// <returns>
        /// The typed character or the default <see cref="char" /> value.
        /// </returns>
        public override char GetChar()
        {
            bool shift = this.input.IsKeyDown(Keys.LeftShift) || this.input.IsKeyDown(Keys.RightShift);
            bool altgr = this.input.IsKeyDown(Keys.RightAlt);

            char c;
            foreach (Keys k in this.input.GetKeysDown())
            {
                // Note that we check conversion first in order to not consume IsKeyTyped for other keys.
                if (this.converter.Convert(k, shift, altgr, out c)
                    && this.input.IsKeyTyped(k, this.typingStartTime, this.typingCycleInterval))
                {
                    return c;
                }
            }

            return default(char);
        }
    }
}