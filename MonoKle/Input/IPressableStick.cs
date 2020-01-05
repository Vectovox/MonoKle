namespace MonoKle.Input
{
    /// <summary>
    /// Interface for a pressable stick.
    /// </summary>
    /// <seealso cref="IDirectable" />
    public interface IPressableStick : IDirectable
    {
        /// <summary>
        /// Gets the button state.
        /// </summary>
        /// <value>
        /// The button state.
        /// </value>
        IPressable Button { get; }
    }
}
