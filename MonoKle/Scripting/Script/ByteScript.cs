namespace MonoKle.Scripting.Script
{
    internal class ByteScript
    {
        public byte UsedVariables { get; private set; }
        public byte[] ByteCode { get; private set; }
        public Header Header { get; private set; }

        internal ByteScript(Header header, byte[] byteCode, byte usedVariables)
        {
            this.Header = header;
            this.ByteCode = byteCode;
            this.UsedVariables = usedVariables;
        }
    }
}
