namespace MonoKle.Input
{
    public interface IActivatableInput
    {
        /// <summary>
        /// Gets whether the input method was activated by the player.
        /// </summary>
        /// <remarks>
        /// Useful for checking if any input has been made, e.g. when switching from and to gamepad mode.
        /// </remarks>
        bool WasActivated { get; }
    }
}
