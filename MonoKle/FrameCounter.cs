using System;

namespace MonoKle
{
    /// <summary>
    /// Class that keeps track of time spent per update.
    /// </summary>
    public class FrameCounter
    {
        private const int UpdateFrequencyMs = 500;
        private const int UpdateFrameCutoff = 200;

        private TimeSpan _timeSpent;
        private int _updates;
        private DateTime _startTime;
        
        public bool IsActive { get; private set; }
        public TimeSpan FrameTime { get; private set; }
        public int FramesPerSecond { get; private set; }

        public void Begin()
        {
            if (IsActive)
            {
                throw new InvalidOperationException($"{nameof(Begin)} has already been called");
            }
            _startTime = DateTime.UtcNow;
            IsActive = true;
        }

        public TimeSpan End()
        {
            if (!IsActive)
            {
                throw new InvalidOperationException($"{nameof(Begin)} has not been called");
            }

            var delta = DateTime.UtcNow - _startTime;
            _timeSpent += delta;
            _updates++;

            if (_timeSpent.TotalMilliseconds >= UpdateFrequencyMs || _updates >= UpdateFrameCutoff)
            {
                FrameTime = _timeSpent / _updates;
                FramesPerSecond = (int)(_updates / _timeSpent.TotalSeconds);
                _timeSpent = TimeSpan.Zero;
                _updates = 0;
            }

            IsActive = false;

            return delta;
        }
    }
}
