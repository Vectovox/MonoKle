namespace MonoKle.Networking.TCP
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using System.Linq;

    public abstract class TcpServer : AbstractServer
    {
        public int ListeningPort { get; private set; }

        private TcpListener listener;

        public TcpServer(int port)
        {
            this.ListeningPort = port;
        }

        protected override void OnStarting()
        {
            base.OnStarting();
            this.listener = new TcpListener(IPAddress.Any, this.ListeningPort);
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
                lock (this.connections)
                {
                    this.connections.Add(c);
                }
                this.DoConnectionWork(c);
                lock (this.connections)
                {
                    this.connections.Remove(c);
                }
            }
        }

        private HashSet<TcpConnection> connections = new HashSet<TcpConnection>();

        protected void BroadcastRaw(TcpConnection sender, byte[] data, int offset, int size)
        {
            IEnumerable<TcpConnection> cons = null;
            lock(this.connections)
            {
                cons = this.connections.ToList();
            }
            
            foreach(TcpConnection c in cons)
            {
                if (c != sender)
                {
                    c.SendRaw(data, offset, size);
                }
            }
        }

        protected void BroadcastRaw(byte[] data, int offset, int size)
        {
            this.BroadcastRaw(null, data, offset, size);
        }

        protected void BroadcastObject(TcpConnection sender, object o)
        {
            IEnumerable<TcpConnection> cons = null;
            lock (this.connections)
            {
                cons = this.connections.ToList();
            }
            foreach (TcpConnection c in cons)
            {
                if(c != sender)
                {
                    c.SendObject(o);
                }
            }
        }

        protected void BroadcastObject(object o)
        {
            this.BroadcastObject(null, o);
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
