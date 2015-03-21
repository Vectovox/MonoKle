namespace MonoKle.Networking.TCP
{
    using System.Net;
    using System.Net.Sockets;

    public class TcpGameServer : TcpServer
    {
        public TcpGameServer(IPEndPoint localEndpoint) : base(localEndpoint) { }

        protected override void DoConnectionWork(TcpConnection connection)
        {
            while (connection.IsConnected)
            {
            }
        }
    }
}
