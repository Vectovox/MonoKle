using System;

namespace MonoKle
{
    /// <summary>
    /// Class that keeps track of time spent per update.
    /// </summary>
    public class FrameCounter
    {
        private const int UpdateFrequencyMs = 500;

        private TimeSpan _timeSpent;
        private int _frames;
        private DateTime _beginTime;

        private DateTime _globalTimerStart = DateTime.UtcNow;
        
        public bool IsActive { get; private set; }
        public TimeSpan FrameTime { get; private set; }
        public int TheoreticalFramesPerSecond { get; private set; }
        public int FramesPerSecond { get; private set; }

        public void Begin()
        {
            if (IsActive)
            {
                throw new InvalidOperationException($"{nameof(Begin)} has already been called");
            }

            IsActive = true;
            _beginTime = DateTime.UtcNow;
        }

        public TimeSpan End()
        {
            if (!IsActive)
            {
                throw new InvalidOperationException($"{nameof(Begin)} has not been called");
            }

            var beginEndDelta = DateTime.UtcNow - _beginTime;
            _timeSpent += beginEndDelta;
            _frames++;

            var globalTimeDelta = DateTime.UtcNow - _globalTimerStart;
            if (globalTimeDelta.TotalMilliseconds >= UpdateFrequencyMs)
            {
                // Update metrics
                FrameTime = _timeSpent / _frames;
                TheoreticalFramesPerSecond = (int)(_frames / _timeSpent.TotalSeconds);
                FramesPerSecond = (int)(_frames / globalTimeDelta.TotalSeconds);
                
                // Reset measurements
                _timeSpent = TimeSpan.Zero;
                _frames = 0;
                _globalTimerStart = DateTime.UtcNow;
            }

            IsActive = false;

            return beginEndDelta;
        }
    }
}
