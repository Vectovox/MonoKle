namespace MonoKle.Input
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// Keyboard input class.
    /// </summary>
    public class KeyboardInput
    {
        private HashSet<Keys> currentKeys;
        private Dictionary<Keys, double> heldTimerByKey;
        private HashSet<Keys> previousKeys;

        internal KeyboardInput()
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
            heldTimerByKey.TryGetValue(key, out ret);
            return ret;
        }

        /// <summary>
        /// Returns true if the specified key is down; else false.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <returns></returns>
        public bool IsKeyDown(Keys key)
        {
            return currentKeys.Contains(key);
        }

        /// <summary>
        /// Returns true if the specified key is held; else false.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <returns></returns>
        public bool IsKeyHeld(Keys key)
        {
            return IsKeyDown(key) && previousKeys.Contains(key);
        }

        /// <summary>
        /// Returns true if the specified key has been held for at least an amount of time.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <param name="timeHeld">The amount of time.</param>
        /// <returns></returns>
        public bool IsKeyHeld(Keys key, double timeHeld)
        {
            return IsKeyHeld(key) && GetKeyHeldTime(key) >= timeHeld;
        }

        /// <summary>
        /// Returns true if the specified key is pressed; else false.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <returns></returns>
        public bool IsKeyPressed(Keys key)
        {
            return IsKeyDown(key) && previousKeys.Contains(key) == false;
        }

        /// <summary>
        /// Returns true if the specified key is released; else false.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <returns></returns>
        public bool IsKeyReleased(Keys key)
        {
            return IsKeyUp(key) && previousKeys.Contains(key);
        }

        /// <summary>
        /// Returns true if the specified key is up; else false.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <returns></returns>
        public bool IsKeyUp(Keys key)
        {
            return currentKeys.Contains(key) == false;
        }

        /// <summary>
        /// Should be called once per frame.
        /// </summary>
        /// <param name="seconds">Time passed since last call.</param>
        internal void Update(double seconds)
        {
            previousKeys = currentKeys;
            currentKeys = new HashSet<Keys>(Keyboard.GetState().GetPressedKeys());

            // Remove and update old held keys
            ICollection c = new LinkedList<Keys>(heldTimerByKey.Keys);
            foreach (Keys k in c)
            {
                if (currentKeys.Contains(k))
                {
                    heldTimerByKey[k] += seconds;
                }
                else
                {
                    heldTimerByKey.Remove(k);
                }
            }

            // Add new held keys
            foreach (Keys k in currentKeys)
            {
                if (heldTimerByKey.ContainsKey(k) == false)
                {
                    heldTimerByKey.Add(k, 0);
                }
            }
        }
    }
}