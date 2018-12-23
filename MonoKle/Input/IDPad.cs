namespace MonoKle.Input {
    /// <summary>
    /// Interface for a DPad.
    /// </summary>
    public interface IDPad {
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
    }
}