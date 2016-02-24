namespace MonoKle.Input
{
    using Microsoft.Xna.Framework.Input;
    using System;

    /// <summary>
    /// Class for making keyboard input behave like common text editing typing behaviour.
    /// </summary>
    public class KeyboardTyper
    {
        /// <summary>
        /// Gets the keyboard input.
        /// </summary>
        /// <value>
        /// The keyboard input.
        /// </value>
        public IKeyboardInput KeyboardInput { get; private set; }

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
        /// <param name="keyboardInput">The keyboard input.</param>
        /// <param name="activationDelay">The activation delay.</param>
        /// <param name="repeatDelay">The repeat delay.</param>
        /// <exception cref="System.ArgumentNullException">Input must not be null.</exception>
        public KeyboardTyper(IKeyboardInput keyboardInput, double activationDelay, double repeatDelay)
        {
            if (keyboardInput == null)
            {
                throw new ArgumentNullException("Input must not be null.");
            }
            this.KeyboardInput = keyboardInput;
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
            if(this.KeyboardInput.IsKeyDown(key))
            {
                double timeHeld = this.KeyboardInput.GetKeyHeldTime(key);

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
                    return this.KeyboardInput.IsKeyPressed(key);
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