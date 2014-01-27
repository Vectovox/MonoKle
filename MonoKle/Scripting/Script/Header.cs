namespace MonoKle.Scripting.Script
{
    using System;

    internal struct Header
    {
        public Type returnType;
        public string channel;
        public string name;
        public Argument[] arguments;

        public Header(string name, Type returnType, string channel, Argument[] arguments)
        {
            this.returnType = returnType;
            this.channel = channel;
            this.name = name;
            this.arguments = arguments;
        }
    }
}
