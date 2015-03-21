namespace MonoKle.Networking
{
    using System.Threading;

    /// <summary>
    /// Abstract server class providing start, stop, and worker logic.
    /// </summary>
    public abstract class AbstractServer
    {
        public bool Running { get; private set; }
        private Thread workerThread;

        public void Start()
        {
            this.OnStarting();
            this.Running = true;
            this.workerThread = new Thread(ServerWorker);
            this.workerThread.Start();
            this.OnStarted();
        }

        public void Stop()
        {
            this.OnStopping();
            this.Running = false;
            this.workerThread.Join();
            this.OnStopped();
        }

        private void ServerWorker()
        {
            while (this.Running)
            {
                this.DoWork();
            }
        }

        protected abstract void DoWork();

        protected virtual void OnStarted() { }

        protected virtual void OnStarting() { }

        protected virtual void OnStopped() { }

        protected virtual void OnStopping() { }
    }
}