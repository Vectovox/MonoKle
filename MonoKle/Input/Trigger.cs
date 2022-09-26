using System;

namespace MonoKle.Input
{
    /// <summary>
    /// Class providing the state of a trigger.
    /// </summary>
    /// <seealso cref="ITrigger" />
    public class Trigger : ITrigger
    {
        private readonly Button _buttonState = new();

        public TimeSpan HeldTime => _buttonState.HeldTime;

        public bool IsDown => _buttonState.IsDown;

        public bool IsHeld => _buttonState.IsHeld;

        public bool IsPressed => _buttonState.IsPressed;

        public bool IsReleased => _buttonState.IsReleased;

        public bool IsUp => _buttonState.IsUp;

        public float State { get; private set; }

        public bool IsHeldFor(TimeSpan duration) => _buttonState.IsHeldFor(duration);

        public bool IsHeldForOnce(TimeSpan duration) => _buttonState.IsHeldForOnce(duration);

        public bool IsReleasedAfter(TimeSpan duration) => _buttonState.IsReleasedAfter(duration);

        /// <summary>
        /// Updates the state of the <see cref="Trigger"/>.
        /// </summary>
        /// <param name="state">Continuous state.</param>
        /// <param name="deltaTime">Time since last update.</param>
        public virtual void Update(float state, TimeSpan deltaTime)
        {
            State = state;
            _buttonState.Update(state != 0, deltaTime);
        }
    }
}
