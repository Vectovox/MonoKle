namespace MonoKle.Script.Common.Internal
{
    using System;

    internal static class CommonHelpers
    {
        internal static byte TypeToConstantType(Type t)
        {
            if(t == typeof(bool))
            {
                return ByteCodeValues.TYPE_BOOL;
            }
            else if(t == typeof(string))
            {
                return ByteCodeValues.TYPE_STRING;
            }
            else if(t == typeof(int))
            {
                return ByteCodeValues.TYPE_INT;
            }
            else if(t == typeof(float))
            {
                return ByteCodeValues.TYPE_FLOAT;
            }
            else if(t == typeof(object))
            {
                return ByteCodeValues.TYPE_OBJECT;
            }
            else
            {
                throw new Exception("DON'T GET HERE!");
            }
        }

        internal static Type ConstantTypeToType(byte type)
        {
            switch(type)
            {
                case ByteCodeValues.TYPE_BOOL:
                    return typeof(bool);
                case ByteCodeValues.TYPE_STRING:
                    return typeof(string);
                case ByteCodeValues.TYPE_INT:
                    return typeof(int);
                case ByteCodeValues.TYPE_FLOAT:
                    return typeof(float);
                case ByteCodeValues.TYPE_OBJECT:
                    return typeof(object);
                default:
                    throw new Exception("DON'T GET HERE!");
            }
        }

        internal static Type StringTypeToType(string stringType)
        {
            if(stringType.Equals("bool"))
            {
                return Type.GetType("System.Boolean");
            }
            else if(stringType.Equals("int"))
            {
                return Type.GetType("System.Int32");
            }
            else if(stringType.Equals("float"))
            {
                return Type.GetType("System.Single");
            }
            else if(stringType.Equals("void"))
            {
                return Type.GetType("System.Void");
            }
            else if(stringType.Equals("string"))
            {
                return Type.GetType("System.String");
            }
            else if(stringType.Equals("object"))
            {
                return Type.GetType("System.Object");
            }

            throw new ArgumentException("Invalid stringType entered.");
        }

        internal static bool IsTypeCompatibleToTarget(Type type, Type target)
        {
            if(target == typeof(float) && type == typeof(int))
            {
                return true;
            }
            else if(target == typeof(string))
            {
                return true;
            }

            return type == target;
        }

        internal static bool IsTypeArithmetic(Type type)
        {
            return type == typeof(int) || type == typeof(float);
        }
    }
}
