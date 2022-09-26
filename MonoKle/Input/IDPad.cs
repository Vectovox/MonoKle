namespace MonoKle.Input
{
    /// <summary>
    /// Interface for a DPad.
    /// </summary>
    public interface IDPad
    {
        /// <summary>
        /// Gets the down direction.
        /// </summary>
        /// <value>
        /// The down direction.
        /// </value>
        IPressable Down { get; }

        /// <summary>
        /// Gets the left direction.
        /// </summary>
        /// <value>
        /// The left direction.
        /// </value>
        IPressable Left { get; }

        /// <summary>
        /// Gets the right direction.
        /// </summary>
        /// <value>
        /// The right direction.
        /// </value>
        IPressable Right { get; }

        /// <summary>
        /// Gets the up direction.
        /// </summary>
        /// <value>
        /// The up direction.
        /// </value>
        IPressable Up { get; }

        /// <summary>
        /// Gets the direction of the DPad.
        /// </summary>
        /// <returns>Point with X and Y components taking the values {-1, 0, 1}</returns>
        MPoint2 Direction();

        /// <summary>
        /// Gets the direction of the DPad. Only takes on values when pressed.
        /// </summary>
        /// <returns>Point with X and Y components taking the values {-1, 0, 1}</returns>
        MPoint2 DirectionPressed();
    }
}