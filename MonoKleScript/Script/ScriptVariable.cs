namespace MonoKleScript.Script
{
    using System;

    public struct ScriptVariable
    {
        public string name;
        public Type type;

        public ScriptVariable(string name, Type type)
        {
            this.name = name;
            this.type = type;
        }
    }
}
