using Microsoft.Xna.Framework.Input.Touch;

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
        IPressAction Tap { get; }

        /// <summary>
        /// Gets the <see cref="GestureType.DoubleTap"/> action.
        /// </summary>
        IPressAction DoubleTap { get; }

        /// <summary>
        /// Gets the <see cref="GestureType.Hold"/> action.
        /// </summary>
        IPressAction Hold { get; }

        /// <summary>
        /// Gets the <see cref="GestureType.FreeDrag"/> action.
        /// </summary>
        IDragAction Drag { get; }

        /// <summary>
        /// Gets the <see cref="GestureType.Pinch"/> action.
        /// </summary>
        IPinchAction Pinch { get; }

        /// <summary>
        /// Gets or sets whether mouse input can generate touch actions.
        /// Primarily intended for development purposes and may not support all actions.
        /// </summary>
        bool VirtualTouch { get; set; }

        /// <summary>
        /// Gets or sets the enabled touch gestures. Use bitmasking to select multiple gestures.
        /// </summary>
        GestureType EnabledGestures { get; set; }
    }
}
