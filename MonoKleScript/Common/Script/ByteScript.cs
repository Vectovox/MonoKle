namespace MonoKle.Script.Common.Script
{
    /// <summary>
    /// Container for a compiled script.
    /// </summary>
    public class ByteScript
    {
        /// <summary>
        /// Bytecode of the script.
        /// </summary>
        /// <returns></returns>
        public byte[] ByteCode { get; private set; }

        /// <summary>
        /// Header of the script.
        /// </summary>
        /// <returns></returns>
        public ScriptHeader Header { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="ByteScript"/>.
        /// </summary>
        /// <param name="header">Header of the script.</param>
        /// <param name="byteCode">Bytecode of the script.</param>
        public ByteScript(ScriptHeader header, byte[] byteCode)
        {
            this.Header = header;
            this.ByteCode = byteCode;
        }
    }
}
