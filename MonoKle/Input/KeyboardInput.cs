namespace MonoKle.Input
{
    using Microsoft.Xna.Framework.Input;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Class providing polling functionality for keyboard input.
    /// </summary>
    public class KeyboardInput : IKeyboardInput, IMUpdateable
    {
        private HashSet<Keys> currentKeys;
        private Dictionary<Keys, double> heldTimerByKey;
        private HashSet<Keys> previousKeys;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardInput"/> class.
        /// </summary>
        public KeyboardInput()
        {
            currentKeys = new HashSet<Keys>();
            previousKeys = new HashSet<Keys>();
            heldTimerByKey = new Dictionary<Keys, double>();
        }

        /// <summary>
        /// Returns the time the specified key has been continously held.
        /// </summary>
        /// <param name="key">The key to query.</param>
        /// <returns></returns>
        public double GetKeyHeldTime(Keys key)
        {
            double ret = 0;
            this.heldTimerByKey.TryGetValue(key, out ret);
            return ret;
        }

        /// <summary>
        /// Returns whether the specified key is down.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <returns>True if key is down; otherwise false.</returns>
        public bool IsKeyDown(Keys key)
        {
            return this.currentKeys.Contains(key);
        }

        /// <summary>
        /// Returns whether the specified key is held.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <returns>True if key is held; otherwise false.</returns>
        public bool IsKeyHeld(Keys key)
        {
            return this.IsKeyDown(key) && this.previousKeys.Contains(key);
        }

        /// <summary>
        /// Returns whether the specified key has been held for at least the specified amount of time.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <param name="timeHeld">The amount of time.</param>
        /// <returns>True if key has been held for the specified amount of time; otherwise false.</returns>
        public bool IsKeyHeld(Keys key, double timeHeld)
        {
            return this.IsKeyHeld(key) && this.GetKeyHeldTime(key) >= timeHeld;
        }

        /// <summary>
        /// Cyclically checks whether the specified key is held, starting the cycle at the given time.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <param name="startTime">The amount of time before checking cycles. Zero is instantaneous.</param>
        /// <param name="cycleInterval">The cycle interval.</param>
        /// <returns>True if key is held; otherwise false.</returns>
        public bool IsKeyHeld(Keys key, double startTime, double cycleInterval)
        {
            if (this.IsKeyHeld(key, startTime) && this.GetKeyHeldTime(key) >= startTime + cycleInterval)
            {
                this.heldTimerByKey[key] = startTime;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns whether the specified key is pressed.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <returns>True if the key is pressed; otherwise false.</returns>
        public bool IsKeyPressed(Keys key)
        {
            return this.IsKeyDown(key) && this.previousKeys.Contains(key) == false;
        }

        /// <summary>
        /// Returns whether the specified key is released.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <returns>True if the key is released; otherwise false.</returns>
        public bool IsKeyReleased(Keys key)
        {
            return this.IsKeyUp(key) && this.previousKeys.Contains(key);
        }

        /// <summary>
        /// Determines whether the specified key is typed, following standard text editing behaviour. Initial key press is typed,
        /// then every cycle that the key is held after a given offset.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="startTime">The amount of time before checking cycles. Zero is instantaneous.</param>
        /// <param name="cycleInterval">The cycle interval.</param>
        /// <returns>True if key is typed; otherwise false.</returns>
        public bool IsKeyTyped(Keys key, double startTime, double cycleInterval)
        {
            return this.IsKeyPressed(key) || this.IsKeyHeld(key, startTime, cycleInterval);
        }

        /// <summary>
        /// Returns whether the specified key is up.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <returns>True if the specified key is up; otherwise false.</returns>
        public bool IsKeyUp(Keys key)
        {
            return this.currentKeys.Contains(key) == false;
        }

        /// <summary>
        /// Updates the component with the specified seconds since last update.
        /// </summary>
        /// <param name="seconds">The amount of seconds since last update.</param>
        public void Update(double seconds)
        {
            this.previousKeys = this.currentKeys;
            this.currentKeys = new HashSet<Keys>(Keyboard.GetState().GetPressedKeys());

            // Remove and update old held keys
            ICollection c = new LinkedList<Keys>(this.heldTimerByKey.Keys);
            foreach (Keys k in c)
            {
                if (this.currentKeys.Contains(k))
                {
                    this.heldTimerByKey[k] += seconds;
                }
                else
                {
                    this.heldTimerByKey.Remove(k);
                }
            }

            // Add new held keys
            foreach (Keys k in this.currentKeys)
            {
                if (this.heldTimerByKey.ContainsKey(k) == false)
                {
                    this.heldTimerByKey.Add(k, 0);
                }
            }
        }
    }
}