using System;

namespace MonoKle
{
    /// <summary>
    /// A serializable and updateable timer that provides information on time left and whether the timer is done counting.
    /// </summary>
    [Serializable]
    public class Timer : IUpdateable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Timer"/> class and sets it to the provided duration.
        /// </summary>
        /// <param name="duration">The <see cref="TimeSpan"/> duration to set the timer to.</param>
        public Timer(TimeSpan duration) => Set(duration);

        /// <summary>
        /// Gets a value indicating the total duration of the timer.
        /// </summary>
        public TimeSpan Duration { get; private set; }

        /// <summary>
        /// Gets the time left on the timer.
        /// </summary>
        /// <returns>Amount of time left.</returns>
        public TimeSpan TimeLeft { get; private set; }

        /// <summary>
        /// Gets whether the timer is done counting or not.
        /// </summary>
        /// <returns>True if it is done, else false.</returns>
        public bool IsDone => TimeLeft == TimeSpan.Zero;

        /// <summary>
        /// Gets the elapsed time.
        /// </summary>
        public TimeSpan Elapsed => Duration - TimeLeft;

        /// <summary>
        /// Resets the time left to <see cref="Duration"/>.
        /// </summary>
        public void Reset() => TimeLeft = Duration;

        /// <summary>
        /// Sets the timer to the given duration. Reset the timer as a side effect.
        /// </summary>
        /// <param name="duration">The duration to set to.</param>
        public void Set(TimeSpan duration)
        {
            Duration = duration;
            Reset();
        }

        /// <summary>
        /// Trigger the <see cref="Timer"/>, setting its duration to zero. Effectively an inverse <see cref="Reset"/>.
        /// </summary>
        public void Trigger() => TimeLeft = TimeSpan.Zero;

        public void Update(TimeSpan elapsedTime)
        {
            TimeLeft -= elapsedTime;
            if (TimeLeft < TimeSpan.Zero)
            {
                TimeLeft = TimeSpan.Zero;
            }
        }

        /// <summary>
        /// Updates the timer and returns whether the timer is done after the update.
        /// </summary>
        /// <param name="elapsedTime">The elapsed time.</param>
        /// <returns>True if done, otherwise false.</returns>
        public bool UpdateDone(TimeSpan elapsedTime)
        {
            Update(elapsedTime);
            return IsDone;
        }
    }
}
