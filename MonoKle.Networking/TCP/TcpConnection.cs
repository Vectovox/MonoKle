namespace MonoKle.Networking.TCP
{
    using System;
    using System.Net.Sockets;
    using System.Runtime.Serialization.Formatters.Binary;

    public class TcpConnection : IDisposable
    {
        public TcpClient TcpClient { get { return this.client; } }
        private TcpClient client;
        private NetworkStream stream;
        private BinaryFormatter formatter = new BinaryFormatter();

        public TcpConnection(TcpClient client)
        {
            if (client == null)
            {
                throw new ArgumentNullException("Client must not be null.");
            }
            this.client = client;
            try
            {
                this.stream = client.GetStream();
            }
            catch (Exception e)
            {
                throw new ArgumentException("Could not access stream from TcpClient.", e);
            }
        }

        public bool IsConnected
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
            try
            {
                return (T)this.formatter.Deserialize(this.stream);
            }
            catch(Exception e)
            {
                return default(T);
            }
        }

        public bool ReceiveObject<T>(out T result)
        {
            try
            {
                result = (T)this.formatter.Deserialize(this.stream);
            }
            catch (Exception e)
            {
                result = default(T);
                return false;
            }
            return true;
        }

        public object ReceiveObject()
        {
            try
            {
                return this.formatter.Deserialize(this.stream);
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public bool ReceiveObject(out object result)
        {
            try
            {
                result = this.formatter.Deserialize(this.stream);
            }
            catch (Exception e)
            {
                result = null;
                return false;
            }
            return true;
        }

        public bool SendRaw(byte[] data, int offset, int size)
        {
            try
            {
                this.stream.Write(data, offset, size);
            }
            catch(Exception e)
            {
                return false;
            }
            return true;
        }

        public bool SendObject(object data)
        {
            try
            {
                this.formatter.Serialize(this.stream, data);
            }
            catch(Exception e)
            {
                return false;
            }
            return true;
        }

        public void Dispose()
        {
            this.client.Close();
        }
    }
}
