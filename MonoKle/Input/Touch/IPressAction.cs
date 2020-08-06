using System;

namespace MonoKle.Input.Touch
{
    /// <summary>
    /// Interface for a press action.
    /// </summary>
    public interface IPressAction : ITouchAction
    {
        /// <summary>
        /// Gets the screen coordinate of the action. Throws if <see cref="IsTriggered"/> is false.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if action is not valid. See <see cref="IsTriggered"/>.</exception>
        MPoint2 Coordinate { get; }

        /// <summary>
        /// Returns <see cref="IsTriggered"/>. If true, populates the out parameter
        /// with <see cref="Coordinate"/>.
        /// </summary>
        /// <param name="coordinate">The coordinate the action was at.</param>
        /// <returns>True if action is valid.</returns>
        bool TryGetCoordinate(out MPoint2 coordinate);
    }
}
