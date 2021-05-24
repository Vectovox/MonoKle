using System;

namespace MonoKle
{
    /// <summary>
    /// A serializable and updateable timer that provides information on time left and whether the timer is done counting.
    /// </summary>
    [Serializable]
    public class Timer : IUpdateable
    {
        private bool _wasDone;

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer"/> class and sets it to the provided duration.
        /// </summary>
        /// <param name="duration">The <see cref="TimeSpan"/> duration to set the timer to.</param>
        public Timer(TimeSpan duration) : this(duration, false) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer"/> class and sets it to the provided duration.
        /// </summary>
        /// <param name="duration">The <see cref="TimeSpan"/> duration to set the timer to.</param>
        /// <param name="triggered">If true, the timer will start triggered (no duration left).</param>
        public Timer(TimeSpan duration, bool triggered)
        {
            Set(duration);
            if (triggered)
            {
                Trigger();
            }
        }

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
        public void Reset() => Reset(Duration);

        /// <summary>
        /// Resets with the time left to the provided value.
        /// </summary>
        /// <param name="timeLeft">The time left to reset to.</param>
        public void Reset(TimeSpan timeLeft) => TimeLeft = timeLeft;

        /// <summary>
        /// Gets the fraction of how much time has elapsed.
        /// </summary>
        public double FractionDone => Elapsed.TotalSeconds / Duration.TotalSeconds;

        /// <summary>
        /// Gets the fraction of how much time is left.
        /// </summary>
        public double FractionLeft => 1.0 - FractionDone;

        /// <summary>
        /// Sets the duration of the timer to the given value. Resets the timer as a side effect.
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
        public void Trigger() => Reset(TimeSpan.Zero);

        /// <summary>
        /// Gets whether the timer is triggered.
        /// If true, will invert after the subsequent <see cref="Update"/> or <see cref="UpdateDone(TimeSpan)"/>.
        /// </summary>
        public bool IsTriggered => !_wasDone && IsDone;

        public void Update(TimeSpan elapsedTime)
        {
            _wasDone = IsDone;
            TimeLeft -= elapsedTime;
            if (TimeLeft < TimeSpan.Zero)
            {
                TimeLeft = TimeSpan.Zero;
            }
        }

        /// <summary>
        /// Updates the timer and returns whether the timer got finished by the update.
        /// Same logic applies as <see cref="IsTriggered"/>.
        /// </summary>
        /// <param name="elapsedTime">The elapsed time.</param>
        /// <returns>True if done, otherwise false.</returns>
        public bool UpdateDone(TimeSpan elapsedTime)
        {
            Update(elapsedTime);
            return IsTriggered;
        }
    }
}
