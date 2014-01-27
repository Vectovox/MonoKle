namespace MonoKle.Scripting.Script
{
    using System;

    internal struct Argument
    {
        public string name;
        public Type type;

        public Argument(string name, Type type)
        {
            this.name = name;
            this.type = type;
        }
    }
}
