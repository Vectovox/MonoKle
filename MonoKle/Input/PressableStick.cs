namespace MonoKle.Input
{
    /// <summary>
    /// Class for pressable sticks.
    /// </summary>
    public class PressableStick : IPressableStick
    {
        private MVector2 direction;
        private Button buttonState = new Button();

        /// <summary>
        /// Gets the direction.
        /// </summary>
        /// <value>
        /// The direction.
        /// </value>
        public MVector2 Direction => this.direction;

        /// <summary>
        /// Gets the button state.
        /// </summary>
        /// <value>
        /// The button state.
        /// </value>
        public IPressable Button => this.buttonState;

        /// <summary>
        /// Updates the specified down.
        /// </summary>
        /// <param name="down">True if button is down.</param>
        /// <param name="direction">The direction of the button.</param>
        /// <param name="deltaTime">Time in seconds since last update.</param>
        public virtual void Update(bool down, MVector2 direction, double deltaTime)
        {
            this.buttonState.Update(down, deltaTime);
            this.direction = direction;
        }
    }
}