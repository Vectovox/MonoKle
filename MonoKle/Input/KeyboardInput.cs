namespace MonoKle.Input
{
    using Microsoft.Xna.Framework.Input;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Class providing polling-based keyboard input.
    /// </summary>
    public class KeyboardInput : IKeyboardInput, IMUpdateable
    {
        private KeyStatus[] keyArray;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardInput"/> class.
        /// </summary>
        public KeyboardInput()
        {
            var values = Enum.GetValues(typeof(Keys));
            this.keyArray = new KeyStatus[(int)values.GetValue(values.GetUpperBound(0)) + 1];
            foreach (Keys k in values)
            {
                this.keyArray[(int)k] = new KeyStatus(k);
            }
        }

        /// <summary>
        /// Queries whether the specified keys are down.
        /// </summary>
        /// <param name="keys">The keys to query.</param>
        /// <param name="behavior">The query behavior.</param>
        /// <returns>
        /// True if the keys are down; otherwise false.
        /// </returns>
        public bool AreKeysDown(IEnumerable<Keys> keys, CollectionQueryBehavior behavior)
        {
            if (behavior == CollectionQueryBehavior.All)
            {
                return keys.All(o => this.IsKeyDown(o));
            }
            else
            {
                return keys.Any(o => this.IsKeyDown(o));
            }
        }

        /// <summary>
        /// Queries whether the specified keys are held.
        /// </summary>
        /// <param name="keys">The keys to query.</param>
        /// <param name="behavior">The query behavior.</param>
        /// <returns>
        /// True if the keys are held; otherwise false.
        /// </returns>
        public bool AreKeysHeld(IEnumerable<Keys> keys, CollectionQueryBehavior behavior)
        {
            if (behavior == CollectionQueryBehavior.All)
            {
                return keys.All(o => this.IsKeyHeld(o));
            }
            else
            {
                return keys.Any(o => this.IsKeyHeld(o));
            }
        }

        /// <summary>
        /// Queries whether the specified keys are held for the given amount of time.
        /// </summary>
        /// <param name="keys">The keys to query.</param>
        /// <param name="timeHeld">The amount of time.</param>
        /// <param name="behavior">The query behavior.</param>
        /// <returns>
        /// True if the keys are held; otherwise false.
        /// </returns>
        public bool AreKeysHeld(IEnumerable<Keys> keys, double timeHeld, CollectionQueryBehavior behavior)
        {
            if (behavior == CollectionQueryBehavior.All)
            {
                return keys.All(o => this.IsKeyHeld(o, timeHeld));
            }
            else
            {
                return keys.Any(o => this.IsKeyHeld(o, timeHeld));
            }
        }

        /// <summary>
        /// Queries whether the specified keys are pressed.
        /// </summary>
        /// <param name="keys">The keys to query.</param>
        /// <param name="behavior">The query behavior.</param>
        /// <returns>
        /// True if keys are pressed; otherwise false.
        /// </returns>
        public bool AreKeysPressed(IEnumerable<Keys> keys, CollectionQueryBehavior behavior)
        {
            if (behavior == CollectionQueryBehavior.All)
            {
                return keys.All(o => this.IsKeyPressed(o));
            }
            else
            {
                return keys.Any(o => this.IsKeyPressed(o));
            }
        }

        /// <summary>
        /// Queries whether the specified keys are released.
        /// </summary>
        /// <param name="keys">The keys to query.</param>
        /// <param name="behavior">The query behavior.</param>
        /// <returns>
        /// True if keys are released; otherwise false.
        /// </returns>
        public bool AreKeysReleased(IEnumerable<Keys> keys, CollectionQueryBehavior behavior)
        {
            if (behavior == CollectionQueryBehavior.All)
            {
                return keys.All(o => this.IsKeyReleased(o));
            }
            else
            {
                return keys.Any(o => this.IsKeyReleased(o));
            }
        }

        /// <summary>
        /// Queries whether the specified keys are up.
        /// </summary>
        /// <param name="keys">The keys to query.</param>
        /// <param name="behavior">The query behavior.</param>
        /// <returns>
        /// True if the keys are up; otherwise false.
        /// </returns>
        public bool AreKeysUp(IEnumerable<Keys> keys, CollectionQueryBehavior behavior)
        {
            if (behavior == CollectionQueryBehavior.All)
            {
                return keys.All(o => this.IsKeyUp(o));
            }
            else
            {
                return keys.Any(o => this.IsKeyUp(o));
            }
        }

        /// <summary>
        /// Provides the time, in seconds, that the specified key has been continously held.
        /// </summary>
        /// <param name="key">The key to query.</param>
        /// <returns>
        /// Seconds that the specified key has been held.
        /// </returns>
        public double GetKeyHeldTime(Keys key)
        {
            return this.GetStatus(key).HeldTime;
        }

        /// <summary>
        /// Gets the keys that are down.
        /// </summary>
        /// <returns>
        /// Collection of keys down.
        /// </returns>
        public IEnumerable<Keys> GetKeysDown()
        {
            return this.keyArray.Where(o => o != null && o.IsDown).Select(o => o.Key);
        }

        /// <summary>
        /// Queries whether the specified key is down.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <returns>
        /// True if key is down; otherwise false.
        /// </returns>
        public bool IsKeyDown(Keys key)
        {
            return this.GetStatus(key).IsDown;
        }

        /// <summary>
        /// Queries whether the specified key is held.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <returns>
        /// True if key is held; otherwise false.
        /// </returns>
        public bool IsKeyHeld(Keys key)
        {
            return this.GetStatus(key).IsHeld;
        }

        /// <summary>
        /// Queries whether the specified key has been held for at least the given amount of time.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <param name="timeHeld">The amount of time.</param>
        /// <returns>
        /// True if key has been held for the specified amount of time; otherwise false.
        /// </returns>
        public bool IsKeyHeld(Keys key, double timeHeld)
        {
            return this.GetStatus(key).HeldFor(timeHeld);
        }

        /// <summary>
        /// Queries whether the specified key is pressed.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <returns>
        /// True if the key is pressed; otherwise false.
        /// </returns>
        public bool IsKeyPressed(Keys key)
        {
            return this.GetStatus(key).IsPressed;
        }

        /// <summary>
        /// Queries whether the specified key is released.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <returns>
        /// True if the key is released; otherwise false.
        /// </returns>
        public bool IsKeyReleased(Keys key)
        {
            return this.GetStatus(key).IsReleased;
        }

        /// <summary>
        /// Queries whether the specified key is up.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <returns>
        /// True if the specified key is up; otherwise false.
        /// </returns>
        public bool IsKeyUp(Keys key)
        {
            return this.GetStatus(key).IsUp;
        }

        /// <summary>
        /// Updates the component with the specified seconds since last update.
        /// </summary>
        /// <param name="seconds">The amount of seconds since last update.</param>
        public void Update(double seconds)
        {
            var keyboardState = Keyboard.GetState();
            foreach (KeyStatus s in this.keyArray)
            {
                s?.SetDown(keyboardState.IsKeyDown(s.Key), seconds);
            }
        }

        private KeyStatus GetStatus(Keys key)
        {
            return this.keyArray[(int)key];
        }

        /// <summary>
        /// Class containing the status of a key. Used to avoid boxing.
        /// </summary>
        private class KeyStatus
        {
            public readonly Keys Key;

            private bool isDown;

            private bool wasDown;

            private double heldTime;

            public bool IsHeld => this.isDown && this.wasDown;

            public bool IsPressed => this.isDown && !this.wasDown;

            public bool IsReleased => !this.isDown && this.wasDown;

            public bool IsDown => this.isDown;

            public bool IsUp => !this.isDown;

            public double HeldTime => this.heldTime;

            public bool HeldFor(double seconds) => this.heldTime >= seconds;

            public KeyStatus(Keys key)
            {
                this.Key = key;
            }

            public void SetDown(bool down, double deltaTime)
            {
                this.wasDown = this.isDown;
                this.isDown = down;
                this.heldTime = down ? this.heldTime + deltaTime : 0;
            }
        }
    }
}