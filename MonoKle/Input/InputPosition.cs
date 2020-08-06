namespace MonoKle.Input
{
    /// <summary>
    /// Class representing an input position.
    /// </summary>
    /// <seealso cref="IInputPosition" />
    public class InputPosition : IInputPosition
    {
        /// <summary>
        /// Gets the change in position since last update.
        /// </summary>
        public MPoint2 Delta { get; private set; }

        /// <summary>
        /// Gets the previous <see cref="Coordinate"/>.
        /// </summary>
        public MPoint2 PreviousCoordinate { get; private set; }

        /// <summary>
        /// Gets the current coordinate.
        /// </summary>
        public MPoint2 Coordinate { get; private set; }

        /// <summary>
        /// Updates the current coordinate.
        /// </summary>
        /// <param name="coordinate">The current coordinate.</param>
        public void Update(MPoint2 coordinate)
        {
            PreviousCoordinate = Coordinate;
            Coordinate = coordinate;
            Delta = Coordinate - PreviousCoordinate;
        }
    }
}
