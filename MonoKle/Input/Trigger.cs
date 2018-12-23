using System;

namespace MonoKle.Input {
    /// <summary>
    /// Class providing the state of a trigger.
    /// </summary>
    /// <seealso cref="ITrigger" />
    public class Trigger : ITrigger {
        private Button buttonState = new Button();

        /// <summary>
        /// Gets the held time.
        /// </summary>
        /// <value>
        /// The held time.
        /// </value>
        public TimeSpan HeldTime => buttonState.HeldTime;

        /// <summary>
        /// Gets a value indicating whether this instance is down.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is down; otherwise, <c>false</c>.
        /// </value>
        public bool IsDown => buttonState.IsDown;

        /// <summary>
        /// Gets a value indicating whether this instance is held.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is held; otherwise, <c>false</c>.
        /// </value>
        public bool IsHeld => buttonState.IsHeld;

        /// <summary>
        /// Gets a value indicating whether this instance is pressed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is pressed; otherwise, <c>false</c>.
        /// </value>
        public bool IsPressed => buttonState.IsPressed;

        /// <summary>
        /// Gets a value indicating whether this instance is released.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is released; otherwise, <c>false</c>.
        /// </value>
        public bool IsReleased => buttonState.IsReleased;

        /// <summary>
        /// Gets a value indicating whether this instance is up.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is up; otherwise, <c>false</c>.
        /// </value>
        public bool IsUp => buttonState.IsUp;

        /// <summary>
        /// Gets the continuous state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public float State { get; private set; }

        public bool IsHeldFor(TimeSpan duration) => buttonState.IsHeldFor(duration);

        /// <summary>
        /// Updates the state of the <see cref="Trigger"/>.
        /// </summary>
        /// <param name="state">Continuous state.</param>
        /// <param name="deltaTime">Time since last update.</param>
        public virtual void Update(float state, TimeSpan deltaTime) {
            State = state;
            buttonState.Update(state != 0, deltaTime);
        }
    }
}
