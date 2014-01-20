namespace MonoKle.Script
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal class Script
    {
        public Type ReturnType { get; private set; }
        public byte[] ByteCode { get; private set; }
        public string Channel { get; private set; }
        public string Name { get; private set; }
        public byte NVariables { get; private set; }

        internal Script(string name, Type returnType, string channel, byte[] byteCode, byte nVariables)
        {
            this.ByteCode = byteCode;
            this.ReturnType = returnType;
            this.Channel = channel;
            this.Name = name;
            this.NVariables = nVariables;
        }
    }
}
