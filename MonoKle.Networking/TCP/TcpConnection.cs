namespace MonoKle.Networking.TCP
{
    using System;
    using System.Net.Sockets;
    using System.Runtime.Serialization.Formatters.Binary;

    public class TcpConnection : AbstractConnection, IDisposable
    {
        private TcpClient client;
        private NetworkStream stream;
        private BinaryFormatter formatter = new BinaryFormatter();

        public TcpConnection(TcpClient client)
        {
            this.client = client;
            this.stream = client.GetStream();
        }

        public override bool IsConnected
        {
            get
            {
                return this.client.Connected;
            }
        }

        public int ReceiveRaw(byte[] buffer, int offset, int size)
        {
            return this.client.GetStream().Read(buffer, offset, size);
        }

        public T ReceiveObject<T>()
        {
            return (T)this.formatter.Deserialize(this.stream);
        }

        public object ReceiveObject()
        {
            return this.formatter.Deserialize(this.stream);
        }

        public void SendRaw(byte[] data, int offset, int size)
        {
            this.client.GetStream().Write(data, offset, size);
        }

        public void SendObject(object data)
        {
            this.formatter.Serialize(this.client.GetStream(), data);
        }

        //public void SendObjectWithType(object data)
        //{
        //    this.formatter.Serialize(this.client.GetStream(), data.GetType());
        //    this.formatter.Serialize(this.client.GetStream(), data);
        //}

        //public void SendObjectWithType<T>(T data)
        //{
        //    this.formatter.Serialize(this.client.GetStream(), typeof(T));
        //    this.formatter.Serialize(this.client.GetStream(), data);
        //}

        public void Dispose()
        {
            this.client.Close();
        }
    }
}
