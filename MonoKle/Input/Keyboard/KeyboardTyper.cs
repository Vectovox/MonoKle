using Microsoft.Xna.Framework.Input;
using System;

namespace MonoKle.Input.Keyboard
{
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
        public TimeSpan ActivationDelay { get; set; }

        /// <summary>
        /// Gets or sets the repeat delay.
        /// </summary>
        /// <value>
        /// The repeat delay.
        /// </value>
        public TimeSpan RepeatDelay { get; set; }

        private int[] cycleArray;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardTyper"/> class.
        /// </summary>
        /// <param name="keyboard">The keyboard.</param>
        /// <param name="activationDelay">The activation delay.</param>
        /// <param name="repeatDelay">The repeat delay.</param>
        /// <exception cref="ArgumentNullException">Input must not be null.</exception>
        public KeyboardTyper(IKeyboard keyboard, TimeSpan activationDelay, TimeSpan repeatDelay)
        {
            Keyboard = keyboard ?? throw new ArgumentNullException("Input must not be null.");
            ActivationDelay = activationDelay;
            RepeatDelay = repeatDelay;
            var values = Enum.GetValues(typeof(Keys));
            cycleArray = new int[(int)values.GetValue(values.GetUpperBound(0)) + 1];
        }

        /// <summary>
        /// Determines whether the specified key is typed.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public bool IsTyped(Keys key)
        {
            if (Keyboard.IsKeyDown(key))
            {
                TimeSpan timeHeld = Keyboard.GetKeyHeldTime(key);

                if (timeHeld >= ActivationDelay)
                {
                    int previousCycle = cycleArray[(int)key];
                    int currentCycle = RepeatDelay == TimeSpan.Zero ? previousCycle : (int)((timeHeld - ActivationDelay).Ticks / RepeatDelay.Ticks);

                    if (previousCycle != currentCycle)
                    {
                        cycleArray[(int)key] = currentCycle;
                        return true;
                    }
                }
                else
                {
                    return Keyboard.IsKeyPressed(key);
                }
            }
            else
            {
                cycleArray[(int)key] = -1;
            }

            return false;
        }
    }
}
