namespace MonoKle.Networking.TCP
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    public abstract class TcpServer : AbstractServer
    {
        public IPEndPoint LocalEndpoint { get; private set; }

        private TcpListener listener;

        private LinkedList<TcpClient> connectionList = new LinkedList<TcpClient>();

        public TcpServer(IPEndPoint localEndpoint)
        {
            this.LocalEndpoint = localEndpoint;
        }

        protected override void OnStarting()
        {
            base.OnStarting();
            this.listener = new TcpListener(this.LocalEndpoint);
            this.listener.Start();
        }

        protected override void DoWork()
        {
            if(this.listener.Pending())
            {
                TcpClient connection = this.listener.AcceptTcpClient();
                if (this.ValidateConnection(connection))
                {
                    Thread t = new Thread(ConnectionWorker);
                    t.Start(connection);
                    this.OnConnectionEstablished(connection);
                }
            }
            else
            {
                Thread.Sleep(500);
            }
        }

        protected override void OnStopping()
        {
            base.OnStopping();
            this.listener.Stop();
            foreach(TcpClient c in connectionList)
            {
                c.Close();
            }
        }

        protected abstract void DoConnectionWork(TcpClient connection);

        private void ConnectionWorker(object connection)
        {
            using(TcpClient c = connection as TcpClient)
            {
                this.DoConnectionWork(c);
            }
        }

        protected virtual bool ValidateConnection(TcpClient connection)
        {
            return true;
        }

        protected virtual void OnConnectionEstablished(TcpClient connection)
        {
        }
    }
}
