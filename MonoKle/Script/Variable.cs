namespace MonoKle.Script
{
    using System;
    internal struct Variable
    {
        public Type type;
        public byte number;

        public Variable(Type type, byte number)
        {
            this.type = type;
            this.number = number;
        }
    }
}