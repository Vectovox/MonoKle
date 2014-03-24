namespace MonoKleScript.Compiler
{
    using System;
    
    public static class CompilerHelper
    {
        public static Type StringTypeToType(string stringType)
        {
            if (stringType.Equals("bool"))
            {
                return Type.GetType("System.Boolean");
            }
            else if (stringType.Equals("int"))
            {
                return Type.GetType("System.Int32");
            }
            else if (stringType.Equals("float"))
            {
                return Type.GetType("System.Single");
            }
            else if (stringType.Equals("void"))
            {
                return Type.GetType("System.Void");
            }
            else if (stringType.Equals("string"))
            {
                return Type.GetType("System.String");
            }

            throw new ArgumentException("Invalid stringType entered.");
        }
    }
}
