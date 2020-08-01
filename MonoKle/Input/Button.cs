using System;

namespace MonoKle.Input
{
    /// <summary>
    /// Class providing the state of a button.
    /// </summary>
    /// <seealso cref="IPressable" />
    public class Button : IPressable
    {
        public TimeSpan HeldTime { get; private set; }

        public bool IsDown { get; private set; }

        public bool IsHeld => IsDown && _wasDown;

        public bool IsPressed => IsDown && !_wasDown;

        public bool IsReleased => !IsDown && _wasDown;

        public bool IsUp => !IsDown;

        public bool IsHeldFor(TimeSpan duration) => HeldTime >= duration;

        private bool _hasBeenUp = true;

        private bool _wasDown;

        public bool IsHeldForOnce(TimeSpan duration)
        {
            if (IsHeldFor(duration) && _hasBeenUp)
            {
                _hasBeenUp = false;
                return true;
            }
            return false;
        }

        public virtual void Update(bool down, TimeSpan deltaTime)
        {
            _wasDown = IsDown;
            IsDown = down;
            HeldTime = down ? HeldTime + deltaTime : TimeSpan.Zero;
            _hasBeenUp = _hasBeenUp || !down;
        }
    }
}
