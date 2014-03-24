namespace MonoKleScript.Script
{
    using System;

    public struct ScriptHeader
    {
        public Type returnType;
        public string channel;
        public string name;
        public ScriptVariable[] arguments;

        public ScriptHeader(string name, Type returnType, string channel, ScriptVariable[] arguments)
        {
            this.returnType = returnType;
            this.channel = channel;
            this.name = name;
            this.arguments = arguments;
        }
    }
}
