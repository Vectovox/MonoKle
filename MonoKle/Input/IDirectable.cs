namespace MonoKle.Input
{
    /// <summary>
    /// Provides a input direction property.
    /// </summary>
    public interface IDirectable
    {
        /// <summary>
        /// Gets the direction.
        /// </summary>
        /// <value>
        /// The direction.
        /// </value>
        MVector2 Direction { get; }
    }
}
