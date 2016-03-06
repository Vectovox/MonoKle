namespace MonoKle.Input.Keyboard
{
    using Microsoft.Xna.Framework.Input;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Class providing polling-based keyboard.
    /// </summary>
    public class Keyboard : IKeyboard, IUpdateable
    {
        private KeyState[] keyArray;

        /// <summary>
        /// Initializes a new instance of the <see cref="Keyboard"/> class.
        /// </summary>
        public Keyboard()
        {
            var values = Enum.GetValues(typeof(Keys));
            this.keyArray = new KeyState[(int)values.GetValue(values.GetUpperBound(0)) + 1];
            foreach (Keys k in values)
            {
                this.keyArray[(int)k] = new KeyState(k);
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
            return this.GetKeyState(key).HeldTime;
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
        /// Gets the state of the provided key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// State of the key.
        /// </returns>
        public IPressable GetKeyState(Keys key)
        {
            return this.keyArray[(int)key];
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
            return this.GetKeyState(key).IsDown;
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
            return this.GetKeyState(key).IsHeld;
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
            return this.GetKeyState(key).IsHeldFor(timeHeld);
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
            return this.GetKeyState(key).IsPressed;
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
            return this.GetKeyState(key).IsReleased;
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
            return this.GetKeyState(key).IsUp;
        }

        /// <summary>
        /// Updates the component with the specified seconds since last update.
        /// </summary>
        /// <param name="seconds">The amount of seconds since last update.</param>
        public void Update(double seconds)
        {
            var keyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            foreach (KeyState s in this.keyArray)
            {
                s?.Update(keyboardState.IsKeyDown(s.Key), seconds);
            }
        }

        /// <summary>
        /// Class containing the status of a key. Used to avoid boxing.
        /// </summary>
        private class KeyState : Button
        {
            public readonly Keys Key;

            public KeyState(Keys key)
            {
                this.Key = key;
            }
        }
    }
}