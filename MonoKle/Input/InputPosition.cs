namespace MonoKle.Input
{
    /// <summary>
    /// Class representing an input position.
    /// </summary>
    /// <seealso cref="IInputPosition" />
    public class InputPosition : IInputPosition
    {
        private MPoint2 current;

        private MPoint2 delta;

        private MPoint2 previous;

        /// <summary>
        /// Gets the delta value.
        /// </summary>
        /// <value>
        /// The delta value.
        /// </value>
        public MPoint2 DeltaValue => this.delta;

        /// <summary>
        /// Gets the previous value.
        /// </summary>
        /// <value>
        /// The previous value.
        /// </value>
        public MPoint2 PreviousValue => this.previous;

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public MPoint2 Value => this.current;

        /// <summary>
        /// Updates the current position.
        /// </summary>
        /// <param name="position">The current position.</param>
        /// <param name="seconds">The delta time in seconds since last update.</param>
        public void Update(MPoint2 position, double seconds)
        {
            this.previous = this.current;
            this.current = position;
            this.delta = this.current - this.previous;
        }
    }
}