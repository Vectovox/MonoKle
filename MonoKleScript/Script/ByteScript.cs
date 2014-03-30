namespace MonoKleScript.Script
{
    public class ByteScript
    {
        public byte[] ByteCode { get; private set; }
        public ScriptHeader Header { get; private set; }

        public ByteScript(ScriptHeader header, byte[] byteCode)
        {
            this.Header = header;
            this.ByteCode = byteCode;
        }
    }
}
