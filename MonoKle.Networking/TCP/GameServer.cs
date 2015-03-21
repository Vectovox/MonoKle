namespace MonoKle.Networking.TCP
{
    using System.Net;
    using System.Net.Sockets;

    public class GameServer : TcpServer
    {
        public GameServer(IPEndPoint localEndpoint) : base(localEndpoint) { }

        protected override void DoConnectionWork(TcpClient connection)
        {
            while(connection.Connected)
            {

            }
        }
    }
}
