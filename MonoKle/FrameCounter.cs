using System;

namespace MonoKle
{
    /// <summary>
    /// Class that keeps track of time spent per update.
    /// </summary>
    public class FrameCounter
    {
        private const float FrameTimeUpdateFrequency = 0.5f;

        private TimeSpan _timeSpent;
        private int _updates;
        private DateTime _startTime;
        private bool _begun;

        public TimeSpan TimePerUpdate { get; private set; }

        public void Begin()
        {
            if (_begun)
            {
                throw new InvalidOperationException($"{nameof(Begin)} has already been called");
            }
            _startTime = DateTime.UtcNow;
            _begun = true;
        }

        public void End()
        {
            if (!_begun)
            {
                throw new InvalidOperationException($"{nameof(Begin)} has not been called");
            }

            _timeSpent += DateTime.UtcNow - _startTime;
            _updates++;

            if (_timeSpent.TotalSeconds > FrameTimeUpdateFrequency || _updates > 200)
            {
                TimePerUpdate = _timeSpent / _updates;
                _timeSpent = TimeSpan.Zero;
                _updates = 0;
            }

            _begun = false;
        }
    }
}
