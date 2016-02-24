namespace MonoKle.Input
{
    using Microsoft.Xna.Framework.Input;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Class providing polling-based keyboard input.
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
        /// Queries whether the specified keys are down.
        /// </summary>
        /// <param name="keys">The keys to query.</param>
        /// <param name="behavior">The query behavior.</param>
        /// <returns>
        /// True if the keys are down; otherwise false.
        /// </returns>
        public bool AreKeysDown(ICollection<Keys> keys, CollectionQueryBehavior behavior)
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
        public bool AreKeysHeld(ICollection<Keys> keys, CollectionQueryBehavior behavior)
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
        public bool AreKeysHeld(ICollection<Keys> keys, double timeHeld, CollectionQueryBehavior behavior)
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
        /// Cyclically queries whether the specified keys are held, starting the cycle at the given time.
        /// </summary>
        /// <param name="keys">Keys to query.</param>
        /// <param name="startTime">The amount of time before query cycles. Zero is instantaneous.</param>
        /// <param name="cycleInterval">The cycle interval.</param>
        /// <param name="behavior">The query behavior.</param>
        /// <returns>
        /// True if keys are held; otherwise false.
        /// </returns>
        public bool AreKeysHeld(ICollection<Keys> keys, double startTime, double cycleInterval, CollectionQueryBehavior behavior)
        {
            if (behavior == CollectionQueryBehavior.All)
            {
                return keys.All(o => this.IsKeyHeld(o, startTime, cycleInterval));
            }
            else
            {
                return keys.Any(o => this.IsKeyHeld(o, startTime, cycleInterval));
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
        public bool AreKeysPressed(ICollection<Keys> keys, CollectionQueryBehavior behavior)
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
        public bool AreKeysReleased(ICollection<Keys> keys, CollectionQueryBehavior behavior)
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
        public bool AreKeysUp(ICollection<Keys> keys, CollectionQueryBehavior behavior)
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
            double ret = 0;
            this.heldTimerByKey.TryGetValue(key, out ret);
            return ret;
        }

        /// <summary>
        /// Gets the keys that are down.
        /// </summary>
        /// <returns>
        /// Collection of keys down.
        /// </returns>
        public ICollection<Keys> GetKeysDown()
        {
            return new List<Keys>(this.currentKeys);
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
            return this.currentKeys.Contains(key);
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
            return this.IsKeyDown(key) && this.previousKeys.Contains(key);
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
            return this.IsKeyHeld(key) && this.GetKeyHeldTime(key) >= timeHeld;
        }

        /// <summary>
        /// Cyclically queries whether the specified key is held, starting the cycle at the given time.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <param name="startTime">The amount of time before query cycles. Zero is instantaneous.</param>
        /// <param name="cycleInterval">The query interval.</param>
        /// <returns>
        /// True if key is held; otherwise false.
        /// </returns>
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
        /// Queries whether the specified key is pressed.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <returns>
        /// True if the key is pressed; otherwise false.
        /// </returns>
        public bool IsKeyPressed(Keys key)
        {
            return this.IsKeyDown(key) && this.previousKeys.Contains(key) == false;
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
            return this.IsKeyUp(key) && this.previousKeys.Contains(key);
        }

        /// <summary>
        /// Determines whether the specified key is typed, following standard text editing behaviour. Initial key press is typed,
        /// then every cycle that the key is held after a given offset.
        /// </summary>
        /// <param name="key">The key to query.</param>
        /// <param name="startTime">The amount of time before query cycles. Zero is instantaneous.</param>
        /// <param name="cycleInterval">The query interval.</param>
        /// <returns>
        /// True if key is typed; otherwise false.
        /// </returns>
        public bool IsKeyTyped(Keys key, double startTime, double cycleInterval)
        {
            return this.IsKeyPressed(key) || this.IsKeyHeld(key, startTime, cycleInterval);
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