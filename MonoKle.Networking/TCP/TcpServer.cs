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
                TcpClient client = this.listener.AcceptTcpClient();
                TcpConnection connection = new TcpConnection(client);
                if (this.ValidateConnection(connection))
                {
                    new Thread(ConnectionWorker).Start(connection);
                    this.OnConnectionEstablished(connection);
                }
                else
                {
                    connection.Dispose();
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
        }

        protected abstract void DoConnectionWork(TcpConnection connection);

        private void ConnectionWorker(object connection)
        {
            using (TcpConnection c = connection as TcpConnection)
            {
                this.DoConnectionWork(c);
            }
        }

        protected virtual bool ValidateConnection(TcpConnection connection)
        {
            return true;
        }

        protected virtual void OnConnectionEstablished(TcpConnection connection)
        {
        }
    }
}
