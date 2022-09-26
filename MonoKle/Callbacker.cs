using System;
using System.Collections.Concurrent;
using System.Threading;

namespace MonoKle
{
    /// <summary>
    /// Class that provides methods of adding callbacks and calling them. With the intended usage of letting different threads
    /// add and consume the logic, e.g. executing logic on the UI thread.
    /// </summary>
    public class Callbacker
    {
        private readonly ConcurrentQueue<(Action, ManualResetEvent)> _operationQueue = new();

        /// <summary>
        /// Adds the given action as a callback.
        /// </summary>
        /// <param name="action">The action to add.</param>
        public void Add(Action action)
        {
            _operationQueue.Enqueue((action, new ManualResetEvent(true)));
        }

        /// <summary>
        /// Adds the given action as a callback, waiting (blocking) until it has been called.
        /// </summary>
        /// <param name="action">The action to add.</param>
        public void AddWait(Action action)
        {
            var resetEvent = new ManualResetEvent(false);
            _operationQueue.Enqueue((action, resetEvent));
            resetEvent.WaitOne();
        }

        /// <summary>
        /// Calls one callback according to FIFO.
        /// </summary>
        /// <returns>True if a callback was called; otherwise false.</returns>
        public bool CallOne()
        {
            if (_operationQueue.TryDequeue(out var item))
            {
                item.Item1();
                item.Item2.Set();
                return true;
            }
            return false;
        }
    }
}
