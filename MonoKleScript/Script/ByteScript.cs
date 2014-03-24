namespace MonoKleScript.Script
{
    public class ByteScript
    {
        public byte UsedVariables { get; private set; }
        public byte[] ByteCode { get; private set; }
        public ScriptHeader Header { get; private set; }

        public ByteScript(ScriptHeader header, byte[] byteCode, byte usedVariables)
        {
            this.Header = header;
            this.ByteCode = byteCode;
            this.UsedVariables = usedVariables;
        }
    }
}
