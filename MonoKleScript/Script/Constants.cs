using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace MonoKleScript.Script
{
    public class Constants
    {
        public const byte COMPILER_TOKEN_FUNCTION = 0x00;
        public const byte COMPILER_TOKEN_OPERAND = 0x01;
        public const byte COMPILER_TOKEN_OPERATOR = 0x02;
        public const byte COMPILER_TOKEN_GROUPING_LEFT = 0x03;
        public const byte COMPILER_TOKEN_GROUPING_RIGHT = 0x04;
        public const byte COMPILER_TOKEN_LOGICOPERATOR = 0x05;

        public const string SCRIPT_START = "script";
        public const string SCRIPT_END = "endscript";
        public const string SCRIPT_EXTENSION = ".ms";
        public const string SCRIPT_COMMENT = ";";
        public const string SCRIPT_STRING_TOKEN = "\"";

        public const string SCRIPT_OPERATION_SET_VARIABLE = "set";
        public const string SCRIPT_OPERATION_RETURN = "return";
        public const string SCRIPT_OPERATION_PRINT = "print";

        public const string SCRIPT_OPERATOR_GROUPLEFT = "(";
        public const string SCRIPT_OPERATOR_GROUPRIGHT = ")";
        public const string SCRIPT_OPERATOR_ADD = "+";
        public const string SCRIPT_OPERATOR_SUBTRACT = "-";
        public const string SCRIPT_OPERATOR_MULTIPLY = "*";
        public const string SCRIPT_OPERATOR_DIVIDE = "/";
        public const string SCRIPT_OPERATOR_MODULO = "%";
        public const string SCRIPT_OPERATOR_POWER = "^";
        public const string SCRIPT_OPERATOR_LOGIC_EQUAL = "=";
        public const string SCRIPT_OPERATOR_LOGIC_NOTEQUAL = "!=";
        public const string SCRIPT_OPERATOR_LOGIC_LARGER = ">";
        public const string SCRIPT_OPERATOR_LOGIC_LARGEREQUAL = ">=";
        public const string SCRIPT_OPERATOR_LOGIC_SMALLER = "<";
        public const string SCRIPT_OPERATOR_LOGIC_SMALLEREQUAL = "<=";

        public const string SCRIPT_OPERATOR_GROUPLEFT_REGEX = "\\(";
        public const string SCRIPT_OPERATOR_GROUPRIGHT_REGEX = "\\)";
        public const string SCRIPT_OPERATOR_ADD_REGEX = "\\+";
        public const string SCRIPT_OPERATOR_SUBTRACT_REGEX = "-";
        public const string SCRIPT_OPERATOR_MULTIPLY_REGEX = "\\*";
        public const string SCRIPT_OPERATOR_DIVIDE_REGEX = "/";
        public const string SCRIPT_OPERATOR_MODULO_REGEX = "%";
        public const string SCRIPT_OPERATOR_POWER_REGEX = "\\^";
        public const string SCRIPT_OPERATOR_LOGIC_EQUAL_REGEX = "=";
        public const string SCRIPT_OPERATOR_LOGIC_NOTEQUAL_REGEX = "!=";
        public const string SCRIPT_OPERATOR_LOGIC_LARGER_REGEX = ">";
        public const string SCRIPT_OPERATOR_LOGIC_LARGEREQUAL_REGEX = ">=";
        public const string SCRIPT_OPERATOR_LOGIC_SMALLER_REGEX = "<";
        public const string SCRIPT_OPERATOR_LOGIC_SMALLEREQUAL_REGEX = "<=";

        public const string SCRIPT_TYPE_ALLOWEDTYPES_REGEX = "(bool|int|float|string)";
        

        public const string REGEX_START_MATCH = "(^|\\s+)" + SCRIPT_START + "(\\s+|$)";
        public const string REGEX_END_MATCH = "(^|\\s+)" + SCRIPT_END + "(\\s+|$)";
        
        public const string SCRIPT_NAMES_ALLOWEDNAMES_REGEX = "[A-Za-z0-9_]+";
        
        public const string SCRIPT_HEADER_TYPE_ALLOWEDTYPES_REGEX = "(void|bool|int|float|string)";
        public const string SCRIPT_HEADER_ARGUMENT_REGEX = SCRIPT_TYPE_ALLOWEDTYPES_REGEX + "\\s+" + SCRIPT_NAMES_ALLOWEDNAMES_REGEX;
        public const string SCRIPT_HEADER_ARGUMENTS_REGEX = "\\(\\s*(|" + SCRIPT_HEADER_ARGUMENT_REGEX + "|"
            + SCRIPT_HEADER_ARGUMENT_REGEX + "(\\s*" + SCRIPT_ARGUMENT_SEPARATOR + "\\s*" + SCRIPT_HEADER_ARGUMENT_REGEX + ")*)\\s*\\)";
        
        public const string SCRIPT_HEADER_SPECIFICATION_REGEX = "^\\s*" + SCRIPT_START + "\\s+" + SCRIPT_HEADER_TYPE_ALLOWEDTYPES_REGEX
            + "\\s+" + SCRIPT_NAMES_ALLOWEDNAMES_REGEX + "\\s*" + SCRIPT_HEADER_ARGUMENTS_REGEX + "\\s*($|" + SCRIPT_CHANNEL_PREFIX + "\\s*" +
            SCRIPT_NAMES_ALLOWEDNAMES_REGEX + "\\s*$)";

        public const string SCRIPT_HEADER_TYPE_MATCH_REGEX = "(?<=^\\s*" + SCRIPT_START + "\\s+)" + SCRIPT_HEADER_TYPE_ALLOWEDTYPES_REGEX;
        public const string SCRIPT_HEADER_NAME_MATCH_REGEX = "(?<=^\\s*" + SCRIPT_START + "\\s+" + SCRIPT_HEADER_TYPE_ALLOWEDTYPES_REGEX
            + "\\s+)" + SCRIPT_NAMES_ALLOWEDNAMES_REGEX;
        public const string SCRIPT_HEADER_ARGUMENTS_MATCH_REGEX = "(?<=\\(\\s*).*(?=\\s*\\))";
        public const string SCRIPT_HEADER_CHANNEL_MATCH_REGEX = "(?<=" + SCRIPT_CHANNEL_PREFIX + "\\s*)" + SCRIPT_NAMES_ALLOWEDNAMES_REGEX;

        public const string SCRIPT_ARGUMENT_SEPARATOR = ",";
        public const string SCRIPT_CHANNEL_PREFIX = ">";

        public const string SCRIPT_FUNCTION_ARGUMENTS_REGEX = "\\(\\s*(|" + SCRIPT_NAMES_ALLOWEDNAMES_REGEX + "|"
            + SCRIPT_NAMES_ALLOWEDNAMES_REGEX + "(\\s*" + SCRIPT_ARGUMENT_SEPARATOR + "\\s*" + SCRIPT_NAMES_ALLOWEDNAMES_REGEX + ")*)\\s*\\)";

        
        public const int SCRIPT_MAX_VARIABLES = byte.MaxValue;
        public const int SCRIPT_MAX_ARGUMENTS = byte.MaxValue;

        public const byte OP_NONE = 0x00;

        public const byte OP_RETURN_VOID = 0x01;
        public const byte OP_RETURN_VALUE = 0x02;

        // Arithmetic operators
        public const byte OP_ADD = 0x03;
        public const byte OP_SUBTRACT = 0x04;
        public const byte OP_MULTIPLY = 0x05;
        public const byte OP_DIVIDE = 0x06;
        public const byte OP_MODULO = 0x07;
        public const byte OP_POWER = 0x08;
        public const byte OP_NEGATE = 0x09;

        // Logical operators
        public const byte OP_EQUAL = 0x10;
        public const byte OP_NOTEQUAL = 0x11;
        public const byte OP_LARGER = 0x12;
        public const byte OP_LARGEREQUAL = 0x13;
        public const byte OP_SMALLER = 0x14;
        public const byte OP_SMALLEREQUAL = 0x15;
        public const byte OP_NOT = 0x16;
        public const byte OP_AND = 0x17;
        public const byte OP_OR = 0x18;

        public const byte OP_PRINT = 0x20;

        // Function
        public const byte OP_CALLFUNCTION = 0xD0;

        // Initiate/Get/Set variable operations
        public const byte OP_INIVAR = 0xE0;
        public const byte OP_SETVAR = 0xE1;
        public const byte OP_GETVAR = 0xE2;
        public const byte OP_REMVAR = 0xE3;

        // Constant operations
        public const byte OP_CONST_BOOL = 0xFF;
        public const byte OP_CONST_INT = 0xFE;
        public const byte OP_CONST_FLOAT = 0xFD;
        public const byte OP_CONST_STRING = 0xFC;
        public const byte OP_CONST_OBJECT = 0xFB;

        // Types        
        public const byte TYPE_BOOL = 0x01;
        public const byte TYPE_INT = 0x02;
        public const byte TYPE_FLOAT = 0x03;
        public const byte TYPE_STRING = 0x04;
        public const byte TYPE_OBJECT = 0x05;


        private static Dictionary<string, byte> opCodeByToken = null;

        public static bool TokenHasOPCode(string token)
        {
            if (opCodeByToken == null)
            {
                InitializeOpCodeByToken();
            }
            return opCodeByToken.ContainsKey(token);
        }

        public static bool IsSupportedVariableType(string variable)
        {
            return Regex.IsMatch(variable, "^(int|float|string|bool)$"); // TODO: Break out into constants
        }

        public static bool IsMathTypeCompatible(Type left, Type right)
        {
            if(left == typeof(int))
            {
                if(right == typeof(int) || right == typeof(float))
                {
                    return true;
                }
            }
            else if(left == typeof(float))
            {
                if (right == typeof(int) || right == typeof(float))
                {
                    return true;
                }
            }

            return left == right;
        }

        public static bool IsReturnTypeCompatible(Type type, Type target)
        {
            if (target == typeof(int))
            {
                if (type != typeof(int))
                {
                    return false;
                }
            }
            else if (target == typeof(float))
            {
                if (type != typeof(int) && type != typeof(float))
                {
                    return false;
                }
            }
            else if (target != type)
            {
                return false;
            }

            return true;
        }

        public static byte TokenToOPCode(string token)
        {
            if(opCodeByToken == null)
            {
                InitializeOpCodeByToken();
            }
            if(opCodeByToken.ContainsKey(token))
            {
                return opCodeByToken[token];
            }
            return 0;
        }

        public static byte TypeToByte(Type type)
        {
            if (type == typeof(bool))
            {
                return Constants.TYPE_BOOL;
            }
            else if (type == typeof(int))
            {
                return Constants.TYPE_INT;
            }
            else if (type == typeof(float))
            {
                return Constants.TYPE_FLOAT;
            }
            else if (type == typeof(string))
            {
                return Constants.TYPE_STRING;
            }
            else
            {
                return Constants.TYPE_OBJECT;
            }
        }

        public static Type ByteToType(byte type)
        {
            switch(type)
            {
                case TYPE_BOOL:
                    return typeof(bool);
                case TYPE_INT:
                    return typeof(int);
                case TYPE_FLOAT:
                    return typeof(float);
                case TYPE_STRING:
                    return typeof(string);
                case TYPE_OBJECT:
                    return typeof(object);
            }
            return null;
        }

        private static void InitializeOpCodeByToken()
        {
            opCodeByToken = new Dictionary<string, byte>();
            // Arithmetic
            opCodeByToken.Add(SCRIPT_OPERATOR_ADD,OP_ADD);
            opCodeByToken.Add(SCRIPT_OPERATOR_SUBTRACT, OP_SUBTRACT);
            opCodeByToken.Add(SCRIPT_OPERATOR_MULTIPLY, OP_MULTIPLY);
            opCodeByToken.Add(SCRIPT_OPERATOR_DIVIDE, OP_DIVIDE);
            opCodeByToken.Add(SCRIPT_OPERATOR_MODULO, OP_MODULO);
            opCodeByToken.Add(SCRIPT_OPERATOR_POWER, OP_POWER);
            // Logic
            opCodeByToken.Add(SCRIPT_OPERATOR_LOGIC_SMALLER, OP_SMALLER);
            opCodeByToken.Add(SCRIPT_OPERATOR_LOGIC_SMALLEREQUAL, OP_SMALLEREQUAL);
            opCodeByToken.Add(SCRIPT_OPERATOR_LOGIC_LARGER, OP_LARGER);
            opCodeByToken.Add(SCRIPT_OPERATOR_LOGIC_LARGEREQUAL, OP_LARGEREQUAL);
            opCodeByToken.Add(SCRIPT_OPERATOR_LOGIC_EQUAL, OP_EQUAL);
            opCodeByToken.Add(SCRIPT_OPERATOR_LOGIC_NOTEQUAL, OP_NOTEQUAL);
        }

        //public static string RegexifyToken(string token)
        //{
        //    if (Regex.IsMatch(token, "(\\.|\\^|\\$|\\*|\\+|\\?|\\(|\\)|\\[|\\{|\\\\|\\|)"))
        //    {
        //        return "\\" + token;
        //    }
        //    return token;
        //}

        public static string TypeAlias(string type)
        {
            if (type.Equals("bool"))
            {
                return "System.Boolean";
            }
            else if (type.Equals("int"))
            {
                return "System.Int32";
            }
            else if (type.Equals("float"))
            {
                return "System.Single";
            }
            else if (type.Equals("void"))
            {
                return "System.Void";
            }
            else if (type.Equals("string"))
            {
                return "System.String";
            }
            return "";
        }

        //public static bool IsOperand(string token)
        //{
        //    return IsOperator(token) == false;
        //}

        //public static bool IsOperator(string token)
        //{
        //    return Regex.IsMatch(token, "^("
        //        + SCRIPT_OPERATOR_ADD_REGEX + "|" + SCRIPT_OPERATOR_DIVIDE_REGEX + "|" + SCRIPT_OPERATOR_MODULO_REGEX + "|" + SCRIPT_OPERATOR_MULTIPLY_REGEX
        //        + "|" + SCRIPT_OPERATOR_SUBTRACT_REGEX + "|" + SCRIPT_OPERATOR_GROUPLEFT_REGEX + "|" + SCRIPT_OPERATOR_GROUPRIGHT_REGEX
        //        + "|" + SCRIPT_OPERATOR_POWER_REGEX + "|" + SCRIPT_OPERATOR_LOGIC_EQUAL + "|" + SCRIPT_OPERATOR_LOGIC_NOTEQUAL
        //        + "|" + SCRIPT_OPERATOR_LOGIC_LARGER_REGEX + "|" + SCRIPT_OPERATOR_LOGIC_LARGEREQUAL_REGEX
        //        + "|" + SCRIPT_OPERATOR_LOGIC_SMALLER_REGEX + "|" + SCRIPT_OPERATOR_LOGIC_SMALLEREQUAL_REGEX + ")$");
        //}

        //public static bool IsLogicOperator(string token)
        //{
        //    return Regex.IsMatch(token, "^(" + SCRIPT_OPERATOR_LOGIC_EQUAL + "|" + SCRIPT_OPERATOR_LOGIC_NOTEQUAL
        //        + "|" + SCRIPT_OPERATOR_LOGIC_LARGER_REGEX + "|" + SCRIPT_OPERATOR_LOGIC_LARGEREQUAL_REGEX
        //        + "|" + SCRIPT_OPERATOR_LOGIC_SMALLER_REGEX + "|" + SCRIPT_OPERATOR_LOGIC_SMALLEREQUAL_REGEX + ")$");
        //}

        //public static bool IsGrouping(string token)
        //{
        //    return Regex.IsMatch(token, "^("
        //        + SCRIPT_OPERATOR_GROUPLEFT_REGEX + "|" + SCRIPT_OPERATOR_GROUPRIGHT_REGEX
        //        + ")$");
        //}

        public static bool IsFunction(string token)
        {
            return Regex.IsMatch(token, SCRIPT_NAMES_ALLOWEDNAMES_REGEX + "\\(.*?\\)");
        }

        //public static int OperatorHiearchy(string token)
        //{
        //    // TODO: Insert the correct constants
        //    // Same numbers used as in wikipedia: http://en.wikipedia.org/wiki/Order_of_operations#Programming_languages
        //    // All indexes are however increased by one (1), to allow room for the power operator
        //    if (Regex.IsMatch(token, "(" + SCRIPT_OPERATOR_GROUPLEFT_REGEX + "|" + SCRIPT_OPERATOR_GROUPRIGHT_REGEX + ")"))
        //        return 1;
        //    if (Regex.IsMatch(token, "(!)"))
        //        return 2;
        //    if (Regex.IsMatch(token, SCRIPT_OPERATOR_POWER_REGEX))
        //        return 3;
        //    if (Regex.IsMatch(token, "(" + SCRIPT_OPERATOR_MULTIPLY_REGEX + "|" + SCRIPT_OPERATOR_DIVIDE_REGEX + "|" + SCRIPT_OPERATOR_MODULO_REGEX + ")"))
        //        return 4;
        //    if (Regex.IsMatch(token, "(" + SCRIPT_OPERATOR_ADD_REGEX + "|" + SCRIPT_OPERATOR_SUBTRACT_REGEX +")"))
        //        return 5;
        //    if (Regex.IsMatch(token, "(" + SCRIPT_OPERATOR_LOGIC_SMALLER_REGEX + "|" + SCRIPT_OPERATOR_LOGIC_SMALLEREQUAL_REGEX
        //                        + "|" + SCRIPT_OPERATOR_LOGIC_LARGER_REGEX + "|" + SCRIPT_OPERATOR_LOGIC_LARGEREQUAL_REGEX + ")"))
        //        return 7;
        //    if (Regex.IsMatch(token, SCRIPT_OPERATOR_LOGIC_EQUAL_REGEX + "|" + SCRIPT_OPERATOR_LOGIC_NOTEQUAL_REGEX))
        //        return 8;
        //    if (Regex.IsMatch(token, "(.......)"))
        //        return 9;
        //    if (Regex.IsMatch(token, "&&"))
        //        return 12;
        //    if (Regex.IsMatch(token, "||"))
        //        return 13;

        //    return 14;
        //}
    }
}
