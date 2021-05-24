using System;

namespace MonoKle.Input.Touch
{
    /// <summary>
    /// Interface for a drag action.
    /// </summary>
    public interface IDragAction : IPressAction
    {
        /// <summary>
        /// Gets the screen delta of the action. Throws if <see cref="ITouchAction.IsTriggered"/> is false.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if action is not valid. See <see cref="ITouchAction.IsTriggered"/>.</exception>
        MPoint2 Delta { get; }

        /// <summary>
        /// Returns <see cref="ITouchAction.IsTriggered"/>. If true, populates the out parameter
        /// with <see cref="Delta"/>.
        /// </summary>
        /// <param name="coordinate">The delta of the action.</param>
        /// <returns>True if action is valid.</returns>
        bool TryGetDelta(out MPoint2 coordinate);
    }
}
