using System;

namespace MonoKle
{
    /// <summary>
    /// Interface for updateable components.
    /// </summary>
    public interface IUpdateable
    {
        /// <summary>
        /// Updates the object with the specified time delta.
        /// </summary>
        /// <param name="timeDelta">Time since last update.</param>
        void Update(TimeSpan timeDelta);
    }
}
