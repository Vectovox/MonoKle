using System;
using System.Diagnostics;

namespace MonoKle
{
    /// <summary>
    /// Class that provides the current process. Process is cached for performance.
    /// </summary>
    public class CachedProcessProvider
    {
        private Process _process = Process.GetCurrentProcess();
        private DateTime _lastRefresh = DateTime.UtcNow;

        public Process Process => GetCurrentProcess();

        public Process GetCurrentProcess()
        {
            if ((DateTime.UtcNow - _lastRefresh).TotalSeconds >= 2)
            {
                _process = Process.GetCurrentProcess();
                _lastRefresh = DateTime.UtcNow;
            }
            return _process;
        }
    }
}
