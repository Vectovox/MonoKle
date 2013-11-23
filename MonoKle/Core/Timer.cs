namespace MonoKle.Core
{
    using System;

    /// <summary>
    /// An updateable timer that can return if the timer is done counting.
    /// </summary>
    public class Timer
    {
        private double maxTimer = 0;
        private double timer = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer"/> class and sets it to the provided duration.
        /// </summary>
        /// <param name="duration">The <see cref="TimeSpan"/> duration to set the timer to.</param>
        public Timer(TimeSpan duration)
            : this(duration.TotalSeconds)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer"/> class and sets it to the provided duration.
        /// </summary>
        /// <param name="seconds">The duration, in seconds, to set the timer to.</param>
        public Timer(double seconds)
        {
            this.Set(seconds);
        }

        /// <summary>
        /// Gets a value indicating the total duration, in seconds, of the timer.
        /// </summary>
        public double Duration
        {
            get { return this.maxTimer; }
        }

        /// <summary>
        /// Gets a value indicating whether the timer is done counting. True if it is done, else false.
        /// </summary>
        public bool IsDone
        {
            get { return this.timer <= 0; }
        }

        /// <summary>
        /// Gets a value indicating the time left, in seconds, of the timer.
        /// </summary>
        public double TimeLeft
        {
            get { return this.timer; }
        }

        /// <summary>
        /// Resets the timer to the last set duration.
        /// </summary>
        public void Reset()
        {
            this.timer = this.maxTimer;
        }

        /// <summary>
        /// Sets the timer to the given duration.
        /// </summary>
        /// <param name="duration">The duration to set to.</param>
        public void Set(TimeSpan duration)
        {
            this.Set(duration.TotalSeconds);
        }

        /// <summary>
        /// Sets the timer to the given duration.
        /// </summary>
        /// <param name="seconds">The duration, in seconds, to set the timer to.</param>
        public void Set(double seconds)
        {
            this.maxTimer = seconds;
            this.timer = seconds;
        }

        /// <summary>
        /// Updates the timer with the given elapsed time.
        /// </summary>
        /// <param name="elapsedTime">The elapsed time to count down.</param>
        /// <returns>True if the timer is done counting.</returns>
        public bool Update(TimeSpan elapsedTime)
        {
            return this.Update(elapsedTime.TotalSeconds);
        }

        /// <summary>
        /// Updates the timer with the given elapsed time.
        /// </summary>
        /// <param name="elapsedSeconds">The elapsed time, in seconds, to count down.</param>
        /// <returns>True if the timer is done counting.</returns>
        public bool Update(double elapsedSeconds)
        {
            if (this.IsDone == false)
            {
                this.timer -= elapsedSeconds;
                if (this.timer < 0)
                {
                    this.timer = 0;
                }
            }

            return this.IsDone;
        }
    }
}