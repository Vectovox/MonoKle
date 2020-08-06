namespace MonoKle.Input
{
    /// <summary>
    /// Input position properties.
    /// </summary>
    public interface IInputPosition
    {
        /// <summary>
        /// Gets the delta value.
        /// </summary>
        /// <value>
        /// The delta value.
        /// </value>
        MPoint2 Delta { get; }

        /// <summary>
        /// Gets the previous value.
        /// </summary>
        /// <value>
        /// The previous value.
        /// </value>
        MPoint2 PreviousCoordinate { get; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        MPoint2 Coordinate { get; }
    }
}