using System;

namespace MonoKle
{
    /// <summary>
    /// Timer that allows multiple cycles (triggers) per update. For instance useful
    /// when creating frame rate independent systems.
    /// </summary>
    public class CyclicTimer
    {
        private TimeSpan _elapsed = TimeSpan.Zero;
        private TimeSpan _duration;

        /// <summary>
        /// Creates a new instance of <see cref="CyclicTimer"/> with the given duration.
        /// </summary>
        /// <param name="duration">The duration to use. Must not be zero.</param>
        /// <exception cref="ArgumentException">Thrown if duration is zero.</exception>
        public CyclicTimer(TimeSpan duration) => Duration = duration;

        /// <summary>
        /// Gets or sets the duration of the timer. Must not be zero.
        /// </summary>
        public TimeSpan Duration
        {
            get => _duration;
            set
            {
                if (value == TimeSpan.Zero)
                {
                    throw new ArgumentException("Duration must be greated than zero.");
                }
                _duration = value;
            }
        }

        /// <summary>
        /// Updates the <see cref="CyclicTimer"/>, returning the amount of cycles
        /// that are triggered.
        /// </summary>
        /// <param name="timeDelta">The amount of time passed.</param>
        /// <returns>The amount of cycles.</returns>
        public int Update(TimeSpan timeDelta)
        {
            _elapsed += timeDelta;

            var triggers = 0;
            while (_elapsed >= _duration)
            {
                _elapsed -= _duration;
                triggers++;
            }

            return triggers;
        }

        /// <summary>
        /// Resets the timer to its intial state of zero elapsed time.
        /// </summary>
        public void Reset() => _elapsed = TimeSpan.Zero;
    }
}
