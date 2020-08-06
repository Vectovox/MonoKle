using System;

namespace MonoKle.Input.Touch
{
    /// <summary>
    /// Interface for a touch-based action.
    /// </summary>
    public interface ITouchAction
    {
        /// <summary>
        /// Gets whether the action has been triggered and is valid for consumption.
        /// </summary>
        bool IsTriggered { get; }
    }
}
