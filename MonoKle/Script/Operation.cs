namespace MonoKle.Script
{
    using System.Text;

    internal struct Operation
    {
        public string stringToken;
        public byte byteCode;
        public byte nArguments;

        internal Operation(string stringToken, byte byteCode, byte nArguments)
        {
            this.stringToken = stringToken;
            this.byteCode = byteCode;
            this.nArguments = nArguments;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.stringToken);
            sb.Append("(");
            sb.Append(this.byteCode);
            sb.Append(")");
            sb.Append("[");
            sb.Append(this.nArguments);
            sb.Append("]");
            return sb.ToString();
        }
    }
}
