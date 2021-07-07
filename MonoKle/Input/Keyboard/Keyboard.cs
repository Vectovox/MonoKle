using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoKle.Input.Keyboard
{
    /// <summary>
    /// Class providing polling-based keyboard.
    /// </summary>
    public class Keyboard : IKeyboard, IUpdateable
    {
        private readonly Keys[] _possibleKeys;
        private readonly Button[] _keyArray;
        private readonly Keys[] _keysDown;
        private int _keysDownLength;

        // Cached funcs for allocation performance
        private readonly Func<Keys, bool> _isKeyHeldFunc;
        private readonly Func<Keys, bool> _isKeyDownFunc;
        private readonly Func<Keys, bool> _isKeyPressedFunc;
        private readonly Func<Keys, bool> _isKeyReleasedFunc;
        private readonly Func<Keys, bool> _isKeyUpFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="Keyboard"/> class.
        /// </summary>
        public Keyboard()
        {
            _possibleKeys = Enum.GetValues(typeof(Keys)).Cast<Keys>().ToArray();
            _keyArray = new Button[(int)_possibleKeys.Max() + 1];
            _keysDown = new Keys[_possibleKeys.Length];

            foreach (Keys key in _possibleKeys)
            {
                _keyArray[(int)key] = new Button();
            }

            _isKeyHeldFunc = new Func<Keys, bool>(IsKeyHeld);
            _isKeyDownFunc = new Func<Keys, bool>(IsKeyDown);
            _isKeyPressedFunc = new Func<Keys, bool>(IsKeyPressed);
            _isKeyReleasedFunc = new Func<Keys, bool>(IsKeyReleased);
            _isKeyUpFunc = new Func<Keys, bool>(IsKeyUp);
        }

        /// <summary>
        /// Queries whether the specified keys are down.
        /// </summary>
        /// <param name="keys">The keys to query.</param>
        /// <param name="behavior">The query behavior.</param>
        /// <returns>
        /// True if the keys are down; otherwise false.
        /// </returns>
        public bool AreKeysDown(IEnumerable<Keys> keys, CollectionQueryBehavior behavior) => behavior == CollectionQueryBehavior.All
            ? keys.All(_isKeyDownFunc)
            : keys.Any(_isKeyDownFunc);

        /// <summary>
        /// Queries whether the specified keys are held.
        /// </summary>
        /// <param name="keys">The keys to query.</param>
        /// <param name="behavior">The query behavior.</param>
        /// <returns>
        /// True if the keys are held; otherwise false.
        /// </returns>
        public bool AreKeysHeld(IEnumerable<Keys> keys, CollectionQueryBehavior behavior) => behavior == CollectionQueryBehavior.All
            ? keys.All(_isKeyHeldFunc)
            : keys.Any(_isKeyHeldFunc);

        /// <summary>
        /// Queries whether the specified keys are held for the given amount of time.
        /// </summary>
        /// <param name="keys">The keys to query.</param>
        /// <param name="timeHeld">The amount of time.</param>
        /// <param name="behavior">The query behavior.</param>
        /// <returns>
        /// True if the keys are held; otherwise false.
        /// </returns>
        public bool AreKeysHeld(IEnumerable<Keys> keys, TimeSpan timeHeld, CollectionQueryBehavior behavior)
        {
            if (behavior == CollectionQueryBehavior.All)
            {
                return keys.All(o => IsKeyHeld(o, timeHeld));
            }
            else
            {
                return keys.Any(o => IsKeyHeld(o, timeHeld));
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
        public bool AreKeysPressed(IEnumerable<Keys> keys, CollectionQueryBehavior behavior) => behavior == CollectionQueryBehavior.All
            ? keys.All(_isKeyPressedFunc)
            : keys.Any(_isKeyPressedFunc);

        /// <summary>
        /// Queries whether the specified keys are released.
        /// </summary>
        /// <param name="keys">The keys to query.</param>
        /// <param name="behavior">The query behavior.</param>
        /// <returns>
        /// True if keys are released; otherwise false.
        /// </returns>
        public bool AreKeysReleased(IEnumerable<Keys> keys, CollectionQueryBehavior behavior) => behavior == CollectionQueryBehavior.All
            ? keys.All(_isKeyReleasedFunc)
            : keys.Any(_isKeyReleasedFunc);

        /// <summary>
        /// Queries whether the specified keys are up.
        /// </summary>
        /// <param name="keys">The keys to query.</param>
        /// <param name="behavior">The query behavior.</param>
        /// <returns>
        /// True if the keys are up; otherwise false.
        /// </returns>
        public bool AreKeysUp(IEnumerable<Keys> keys, CollectionQueryBehavior behavior) => behavior == CollectionQueryBehavior.All
            ? keys.All(_isKeyUpFunc)
            : keys.Any(_isKeyUpFunc);

        /// <summary>
        /// Provides the time, in seconds, that the specified key has been continously held.
        /// </summary>
        /// <param name="key">The key to query.</param>
        /// <returns>
        /// Seconds that the specified key has been held.
        /// </returns>
        public TimeSpan GetKeyHeldTime(Keys key) => GetKeyState(key).HeldTime;

        /// <summary>
        /// Gets the keys that are down.
        /// </summary>
        /// <returns>
        /// Collection of keys down.
        /// </returns>
        public Span<Keys> GetKeysDown() => new Span<Keys>(_keysDown, 0, _keysDownLength);

        /// <summary>
        /// Gets the state of the provided key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// State of the key.
        /// </returns>
        public IPressable GetKeyState(Keys key) => _keyArray[(int)key];

        /// <summary>
        /// Queries whether the specified key is down.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <returns>
        /// True if key is down; otherwise false.
        /// </returns>
        public bool IsKeyDown(Keys key) => GetKeyState(key).IsDown;

        /// <summary>
        /// Queries whether the specified key is held.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <returns>
        /// True if key is held; otherwise false.
        /// </returns>
        public bool IsKeyHeld(Keys key) => GetKeyState(key).IsHeld;

        /// <summary>
        /// Queries whether the specified key has been held for at least the given amount of time.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <param name="timeHeld">The amount of time.</param>
        /// <returns>
        /// True if key has been held for the specified amount of time; otherwise false.
        /// </returns>
        public bool IsKeyHeld(Keys key, TimeSpan timeHeld) => GetKeyState(key).IsHeldFor(timeHeld);

        /// <summary>
        /// Queries whether the specified key is pressed.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <returns>
        /// True if the key is pressed; otherwise false.
        /// </returns>
        public bool IsKeyPressed(Keys key) => GetKeyState(key).IsPressed;

        /// <summary>
        /// Queries whether the specified key is released.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <returns>
        /// True if the key is released; otherwise false.
        /// </returns>
        public bool IsKeyReleased(Keys key) => GetKeyState(key).IsReleased;

        /// <summary>
        /// Queries whether the specified key is up.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <returns>
        /// True if the specified key is up; otherwise false.
        /// </returns>
        public bool IsKeyUp(Keys key) => GetKeyState(key).IsUp;

        public void Update(TimeSpan timeDelta)
        {
            // Reset keys that are down
            _keysDownLength = 0;

            var keyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            foreach (var key in _possibleKeys)
            {
                var keyDown = keyboardState.IsKeyDown(key);

                // Update this key
                _keyArray[(int)key].Update(keyDown, timeDelta);

                // Register key as down
                if (keyDown)
                {
                    _keysDown[_keysDownLength++] = key;
                }
            }
        }
    }
}
