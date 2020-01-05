using System;

namespace MonoKle
{
    /// <summary>
    /// Interface for drawable components.
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// Draws the specified component with the specified time delta since last drawal.
        /// </summary>
        /// <param name="timeDelta">Time since last update.</param>
        void Draw(TimeSpan timeDelta);
    }
}
