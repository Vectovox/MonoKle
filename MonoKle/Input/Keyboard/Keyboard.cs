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

        public bool WasActivated => _keysDownLength > 0;

        public bool AreKeysDown(IEnumerable<Keys> keys, CollectionQueryBehavior behavior) => behavior == CollectionQueryBehavior.All
            ? keys.All(_isKeyDownFunc)
            : keys.Any(_isKeyDownFunc);

        public bool AreKeysHeld(IEnumerable<Keys> keys, CollectionQueryBehavior behavior) => behavior == CollectionQueryBehavior.All
            ? keys.All(_isKeyHeldFunc)
            : keys.Any(_isKeyHeldFunc);

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

        public bool AreKeysPressed(IEnumerable<Keys> keys, CollectionQueryBehavior behavior) => behavior == CollectionQueryBehavior.All
            ? keys.All(_isKeyPressedFunc)
            : keys.Any(_isKeyPressedFunc);

        public bool AreKeysReleased(IEnumerable<Keys> keys, CollectionQueryBehavior behavior) => behavior == CollectionQueryBehavior.All
            ? keys.All(_isKeyReleasedFunc)
            : keys.Any(_isKeyReleasedFunc);

        public bool AreKeysUp(IEnumerable<Keys> keys, CollectionQueryBehavior behavior) => behavior == CollectionQueryBehavior.All
            ? keys.All(_isKeyUpFunc)
            : keys.Any(_isKeyUpFunc);

        public TimeSpan GetKeyHeldTime(Keys key) => GetKeyState(key).HeldTime;

        public Span<Keys> GetKeysDown() => new(_keysDown, 0, _keysDownLength);

        public IPressable GetKeyState(Keys key) => _keyArray[(int)key];

        public bool IsKeyDown(Keys key) => GetKeyState(key).IsDown;

        public bool IsKeyHeld(Keys key) => GetKeyState(key).IsHeld;

        public bool IsKeyHeld(Keys key, TimeSpan timeHeld) => GetKeyState(key).IsHeldFor(timeHeld);

        public bool IsKeyPressed(Keys key) => GetKeyState(key).IsPressed;

        public bool IsKeyReleased(Keys key) => GetKeyState(key).IsReleased;

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
