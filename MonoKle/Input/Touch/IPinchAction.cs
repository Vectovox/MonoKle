using System;

namespace MonoKle.Input.Touch
{
    /// <summary>
    /// Interface for a pinch action.
    /// </summary>
    public interface IPinchAction : ITouchAction
    {
        /// <summary>
        /// Gets the screen coordinate of the action origin. Throws if <see cref="ITouchAction.IsTriggered"/> is false.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if action is not valid. See <see cref="ITouchAction.IsTriggered"/>.</exception>
        MPoint2 Origin { get; }

        /// <summary>
        /// Gets the pinch factor.
        /// </summary>
        /// <remarks>
        /// A factor that is 0 if no change was made, negative if pinching
        /// inwards, and positive if pinching outwards.
        /// </remarks>
        float PinchFactor { get; }

        /// <summary>
        /// Returns <see cref="ITouchAction.IsTriggered"/>. If true, populates the out parameters
        /// with <see cref="Origin"/> and <see cref="PinchFactor"/>.
        /// </summary>
        /// <param name="origin">The coordinate of the action origin.</param>
        /// <param name="pinchFactor">The pinch factor of the action.</param>
        /// <returns>True if action is valid.</returns>
        bool TryGetValues(out MPoint2 origin, out float pinchFactor);
    }
}
