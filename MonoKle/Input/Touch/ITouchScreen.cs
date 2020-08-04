using Microsoft.Xna.Framework.Input.Touch;
using System;

namespace MonoKle.Input.Touch
{
    /// <summary>
    /// Interface for a touch screen with methods to control and check
    /// touch-based input.
    /// </summary>
    public interface ITouchScreen
    {
        /// <summary>
        /// Gets the <see cref="GestureType.Tap"/> action.
        /// </summary>
        ITouchAction Tap { get; }

        /// <summary>
        /// Gets the <see cref="GestureType.DoubleTap"/> action.
        /// </summary>
        ITouchAction DoubleTap { get; }

        /// <summary>
        /// Gets the <see cref="GestureType.Hold"/> action.
        /// </summary>
        ITouchAction Hold { get; }

        /// <summary>
        /// Returns the action associated with the provided <see cref="GestureType"/>.
        /// </summary>
        /// <param name="gestureType">The gesture type to return.</param>
        /// <exception cref="ArgumentException">Thrown on unsupported gestures.</exception>
        ITouchAction GetAction(GestureType gestureType);

        /// <summary>
        /// Gets or sets whether mouse input can generate touch actions.
        /// Primarily intended for development purposes and may not support all actions.
        /// </summary>
        bool VirtualTouch { get; set; }
    }
}
