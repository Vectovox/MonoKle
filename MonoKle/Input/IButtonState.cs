namespace MonoKle.Input
{
    /// <summary>
    /// Provides an interface for a button state.
    /// </summary>
    public interface IButtonState
    {
        /// <summary>
        /// Gets the held time.
        /// </summary>
        /// <value>
        /// The held time.
        /// </value>
        double HeldTime { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is down.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is down; otherwise, <c>false</c>.
        /// </value>
        bool IsDown { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is held.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is held; otherwise, <c>false</c>.
        /// </value>
        bool IsHeld { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is pressed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is pressed; otherwise, <c>false</c>.
        /// </value>
        bool IsPressed { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is released.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is released; otherwise, <c>false</c>.
        /// </value>
        bool IsReleased { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is up.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is up; otherwise, <c>false</c>.
        /// </value>
        bool IsUp { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has been held for at least the provided amount of seconds.
        /// </summary>
        /// <param name="seconds">The seconds to have been held.</param>
        /// <returns>True if held for at least the provided amount of seconds; otherwise false.</returns>
        bool IsHeldFor(double seconds);
    }
}