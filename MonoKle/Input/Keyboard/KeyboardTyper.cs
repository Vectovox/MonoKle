namespace MonoKle.Input.Keyboard
{
    using Microsoft.Xna.Framework.Input;
    using System;

    /// <summary>
    /// Class for making keyboard input behave like common text editing typing behaviour.
    /// </summary>
    public class KeyboardTyper
    {
        /// <summary>
        /// Gets the keyboard.
        /// </summary>
        /// <value>
        /// The keyboard.
        /// </value>
        public IKeyboard Keyboard { get; private set; }

        /// <summary>
        /// Gets or sets the activation delay.
        /// </summary>
        /// <value>
        /// The activation delay.
        /// </value>
        public double ActivationDelay { get; set; }

        /// <summary>
        /// Gets or sets the repeat delay.
        /// </summary>
        /// <value>
        /// The repeat delay.
        /// </value>
        public double RepeatDelay { get; set; }

        private int[] cycleArray;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardTyper"/> class.
        /// </summary>
        /// <param name="keyboard">The keyboard.</param>
        /// <param name="activationDelay">The activation delay.</param>
        /// <param name="repeatDelay">The repeat delay.</param>
        /// <exception cref="System.ArgumentNullException">Input must not be null.</exception>
        public KeyboardTyper(IKeyboard keyboard, double activationDelay, double repeatDelay)
        {
            if (keyboard == null)
            {
                throw new ArgumentNullException("Input must not be null.");
            }
            this.Keyboard = keyboard;
            this.ActivationDelay = activationDelay;
            this.RepeatDelay = repeatDelay;
            var values = Enum.GetValues(typeof(Keys));
            this.cycleArray = new int[(int)values.GetValue(values.GetUpperBound(0)) + 1];
        }

        /// <summary>
        /// Determines whether the specified key is typed.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public bool IsTyped(Keys key)
        {
            if(this.Keyboard.IsKeyDown(key))
            {
                double timeHeld = this.Keyboard.GetKeyHeldTime(key);

                if (timeHeld >= this.ActivationDelay)
                {
                    int previousCycle = this.cycleArray[(int)key];
                    int currentCycle = this.RepeatDelay == 0 ? previousCycle : (int)((timeHeld - this.ActivationDelay) / this.RepeatDelay);

                    if (previousCycle != currentCycle)
                    {
                        this.cycleArray[(int)key] = currentCycle;
                        return true;
                    }
                }
                else
                {
                    return this.Keyboard.IsKeyPressed(key);
                }
            }
            else
            {
                this.cycleArray[(int)key] = -1;
            }

            return false;
        }
    }
}