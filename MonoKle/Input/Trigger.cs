namespace MonoKle.Input
{
    /// <summary>
    /// Class providing the state of a trigger.
    /// </summary>
    /// <seealso cref="ITrigger" />
    public class Trigger : ITrigger
    {
        private Button buttonState = new Button();

        private float triggerState;

        /// <summary>
        /// Gets the held time.
        /// </summary>
        /// <value>
        /// The held time.
        /// </value>
        public double HeldTime => this.buttonState.HeldTime;

        /// <summary>
        /// Gets a value indicating whether this instance is down.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is down; otherwise, <c>false</c>.
        /// </value>
        public bool IsDown => this.buttonState.IsDown;

        /// <summary>
        /// Gets a value indicating whether this instance is held.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is held; otherwise, <c>false</c>.
        /// </value>
        public bool IsHeld => this.buttonState.IsHeld;

        /// <summary>
        /// Gets a value indicating whether this instance is pressed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is pressed; otherwise, <c>false</c>.
        /// </value>
        public bool IsPressed => this.buttonState.IsPressed;

        /// <summary>
        /// Gets a value indicating whether this instance is released.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is released; otherwise, <c>false</c>.
        /// </value>
        public bool IsReleased => this.buttonState.IsReleased;

        /// <summary>
        /// Gets a value indicating whether this instance is up.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is up; otherwise, <c>false</c>.
        /// </value>
        public bool IsUp => this.buttonState.IsUp;

        /// <summary>
        /// Gets the continuous state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public float State => this.triggerState;

        /// <summary>
        /// Gets a value indicating whether this instance has been held for at least the provided amount of seconds.
        /// </summary>
        /// <param name="seconds">The seconds to have been held.</param>
        /// <returns>
        /// True if held for at least the provided amount of seconds; otherwise false.
        /// </returns>
        public bool IsHeldFor(double seconds) => this.buttonState.IsHeldFor(seconds);

        /// <summary>
        /// Updates the state of the <see cref="Trigger"/>.
        /// </summary>
        /// <param name="state">Continuous state.</param>
        /// <param name="deltaTime">Time in seconds since last update.</param>
        public virtual void Update(float state, double deltaTime)
        {
            this.triggerState = state;
            this.buttonState.Update(state != 0, deltaTime);
        }
    }
}