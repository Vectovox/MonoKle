namespace MonoKle.Input.Touch
{
    /// <summary>
    /// Interface providing polling input for a touch device.
    /// </summary>
    public interface ITouchInput
    {
        /// <summary>
        /// Gets the position relative the upper left corner of the device input.
        /// </summary>
        IInputPosition Position { get; }

        /// <summary>
        /// Gets the screen press state.
        /// </summary>
        IPressable Press { get; }
    }
}
