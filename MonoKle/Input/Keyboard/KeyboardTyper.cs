using Microsoft.Xna.Framework.Input;
using System;

namespace MonoKle.Input.Keyboard
{
    /// <summary>
    /// Class for making keyboard input behave like common text editing typing behaviour.
    /// </summary>
    public class KeyboardTyper
    {
        private readonly int[] _cycleArray;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardTyper"/> class with decent repetition delay values.
        /// </summary>
        /// <param name="keyboard">The keyboard input to use.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="keyboard"/> is null.</exception>
        public KeyboardTyper(IKeyboard keyboard) : this(keyboard, TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(50))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardTyper"/> class.
        /// </summary>
        /// <param name="keyboard">The keyboard.</param>
        /// <param name="activationDelay">The activation delay.</param>
        /// <param name="repeatDelay">The repeat delay.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="keyboard"/> is null.</exception>
        public KeyboardTyper(IKeyboard keyboard, TimeSpan activationDelay, TimeSpan repeatDelay)
        {
            Keyboard = keyboard ?? throw new ArgumentNullException($"{nameof(keyboard)} must not be null.");
            RepeatActivationDelay = activationDelay;
            RepeatDelay = repeatDelay;
            var values = Enum.GetValues(typeof(Keys));
            _cycleArray = new int[(int)values.GetValue(values.GetUpperBound(0)) + 1];
        }

        /// <summary>
        /// Gets the keyboard used.
        /// </summary>
        public IKeyboard Keyboard { get; private set; }

        /// <summary>
        /// Gets or sets the duration that a key needs to be held before it is repeated.
        /// </summary>
        public TimeSpan RepeatActivationDelay { get; set; }

        /// <summary>
        /// Gets or sets the duration between consecutive repeated keys.
        /// </summary>
        public TimeSpan RepeatDelay { get; set; }

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

                // Check if we are repeating the key
                if (timeHeld >= RepeatActivationDelay)
                {
                    // Check if enough time has passed to repeat the key
                    var previousCycle = _cycleArray[(int)key];
                    var currentCycle = RepeatDelay == TimeSpan.Zero
                        ? previousCycle
                        : (int)((timeHeld - RepeatActivationDelay).Ticks / RepeatDelay.Ticks);

                    if (previousCycle != currentCycle)
                    {
                        _cycleArray[(int)key] = currentCycle;
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
                // Cancel repetition
                _cycleArray[(int)key] = -1;
            }

            return false;
        }
    }
}
