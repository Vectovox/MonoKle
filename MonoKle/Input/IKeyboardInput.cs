namespace MonoKle.Input
{
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// Interface providing polling functionality for keyboard input.
    /// </summary>
    public interface IKeyboardInput
    {
        /// <summary>
        /// Returns the time the specified key has been continously held.
        /// </summary>
        /// <param name="key">The key to query.</param>
        /// <returns></returns>
        double GetKeyHeldTime(Keys key);

        /// <summary>
        /// Returns whether the specified key is down.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <returns>True if key is down; otherwise false.</returns>
        bool IsKeyDown(Keys key);

        /// <summary>
        /// Returns whether the specified key is held.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <returns>True if key is held; otherwise false.</returns>
        bool IsKeyHeld(Keys key);

        /// <summary>
        /// Returns whether the specified key has been held for at least the specified amount of time.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <param name="timeHeld">The amount of time.</param>
        /// <returns>True if key has been held for the specified amount of time; otherwise false.</returns>
        bool IsKeyHeld(Keys key, double timeHeld);

        /// <summary>
        /// Cyclically checks whether the specified key is held, starting the cycle at the given time.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <param name="startTime">The amount of time before checking cycles. Zero is instantaneous.</param>
        /// <param name="cycleInterval">The cycle interval.</param>
        /// <returns>True if key is held; otherwise false.</returns>
        bool IsKeyHeld(Keys key, double startTime, double cycleInterval);

        /// <summary>
        /// Returns whether the specified key is pressed.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <returns>True if the key is pressed; otherwise false.</returns>
        bool IsKeyPressed(Keys key);

        /// <summary>
        /// Returns whether the specified key is released.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <returns>True if the key is released; otherwise false.</returns>
        bool IsKeyReleased(Keys key);

        /// <summary>
        /// Determines whether the specified key is typed, following standard text editing behaviour. Initial key press is typed,
        /// then every cycle that the key is held after a given offset.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="startTime">The amount of time before checking cycles. Zero is instantaneous.</param>
        /// <param name="cycleInterval">The cycle interval.</param>
        /// <returns>True if key is typed; otherwise false.</returns>
        bool IsKeyTyped(Keys key, double startTime, double cycleInterval);

        /// <summary>
        /// Returns whether the specified key is up.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <returns>True if the specified key is up; otherwise false.</returns>
        bool IsKeyUp(Keys key);
    }
}