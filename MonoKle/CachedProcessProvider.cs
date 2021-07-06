using System;
using System.Diagnostics;

namespace MonoKle
{
    /// <summary>
    /// Class that provides the current process. Process is cached for performance.
    /// </summary>
    public class CachedProcessProvider
    {
        private const int SecondsBetweenRefresh = 5;

        private Process _process = Process.GetCurrentProcess();
        private DateTime _lastRefresh = DateTime.UtcNow;

        public Process Process => GetCurrentProcess();

        public Process GetCurrentProcess()
        {
            if ((DateTime.UtcNow - _lastRefresh).TotalSeconds >= SecondsBetweenRefresh)
            {
                _process.Refresh();
                _lastRefresh = DateTime.UtcNow;
            }
            return _process;
        }
    }
}
