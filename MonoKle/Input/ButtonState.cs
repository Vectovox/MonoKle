namespace MonoKle.Input
{
    /// <summary>
    /// Class providing the state of a button.
    /// </summary>
    /// <seealso cref="MonoKle.Input.IButtonState" />
    public class ButtonState : IButtonState
    {
        private double heldTime;
        private bool isDown;

        private bool wasDown;

        /// <summary>
        /// Gets the held time.
        /// </summary>
        /// <value>
        /// The held time.
        /// </value>
        public double HeldTime => this.heldTime;

        /// <summary>
        /// Gets a value indicating whether this instance is down.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is down; otherwise, <c>false</c>.
        /// </value>
        public bool IsDown => this.isDown;

        /// <summary>
        /// Gets a value indicating whether this instance is held.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is held; otherwise, <c>false</c>.
        /// </value>
        public bool IsHeld => this.isDown && this.wasDown;

        /// <summary>
        /// Gets a value indicating whether this instance is pressed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is pressed; otherwise, <c>false</c>.
        /// </value>
        public bool IsPressed => this.isDown && !this.wasDown;

        /// <summary>
        /// Gets a value indicating whether this instance is released.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is released; otherwise, <c>false</c>.
        /// </value>
        public bool IsReleased => !this.isDown && this.wasDown;

        /// <summary>
        /// Gets a value indicating whether this instance is up.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is up; otherwise, <c>false</c>.
        /// </value>
        public bool IsUp => !this.isDown;

        /// <summary>
        /// Gets a value indicating whether this instance has been held for at least the provided amount of seconds.
        /// </summary>
        /// <param name="seconds">The seconds to have been held.</param>
        /// <returns>
        /// True if held for at least the provided amount of seconds; otherwise false.
        /// </returns>
        public bool IsHeldFor(double seconds) => this.heldTime >= seconds;

        /// <summary>
        /// Updates the state.
        /// </summary>
        /// <param name="down">True if button is down.</param>
        /// <param name="deltaTime">Time in seconds since last update.</param>
        public void SetIsDown(bool down, double deltaTime)
        {
            this.wasDown = this.isDown;
            this.isDown = down;
            this.heldTime = down ? this.heldTime + deltaTime : 0;
        }
    }
}