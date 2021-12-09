using System;

namespace MonoKle.Input
{
    /// <summary>
    /// Class providing the state of a button.
    /// </summary>
    /// <seealso cref="IPressable" />
    public class Button : IPressable
    {
        private bool _hasBeenUp = true;
        private bool _wasDown = false;
        private TimeSpan _lastHeldTime = TimeSpan.Zero;

        public TimeSpan HeldTime => IsDown
            ? _lastHeldTime
            : TimeSpan.Zero;

        public bool IsDown { get; private set; }

        public bool IsHeld => IsDown && _wasDown;

        public bool IsPressed => IsDown && !_wasDown;

        public bool IsReleased => !IsDown && _wasDown;

        public bool IsUp => !IsDown;

        public bool IsHeldFor(TimeSpan duration) => HeldTime >= duration;

        public bool IsHeldForOnce(TimeSpan duration)
        {
            if (IsHeldFor(duration) && _hasBeenUp)
            {
                _hasBeenUp = false;
                return true;
            }
            return false;
        }

        public bool IsReleasedAfter(TimeSpan duration) => IsReleased && _lastHeldTime >= duration;

        public virtual void Update(bool down, TimeSpan deltaTime)
        {
            // Update down state
            _wasDown = IsDown;
            IsDown = down;
            // Set held time
            if (IsPressed)
            {
                // Reset
                _lastHeldTime = deltaTime;
            }
            else if (IsDown)
            {
                // Increase
                _lastHeldTime += deltaTime;
            }
            _hasBeenUp = _hasBeenUp || !down;
        }
    }
}
