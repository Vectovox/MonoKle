namespace MonoKle.Script
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;

    using MonoKle.IO;

    internal class ScriptCompiler
    {
        private const int HEADER_MAX_PARTS = 4;
        private const int HEADER_MIN_PARTS = 3;
        private const string REGEX_CHANNEL = "(?<=.*:\\s*)([A-Za-z0-9_]+)";
        private const string REGEX_HEADER_CHECK = "(^\\s*)(script\\s+)([A-Za-z0-9_]+\\s+)([A-Za-z0-9_]+\\s*)($|(>\\s*)([A-Za-z0-9_]+))(\\s*$)";
        private const string REGEX_NAME = "(?<=(script\\s+))([A-Za-z0-9_]+)(?=($|\\s))";

        private LinkedList<byte> byteCode;
        private string channel;
        private string name;

        //private const string OPCODE_RESOURCE_NAME = "opcodes";
        //private Dictionary<string, Operation> operationByName = new Dictionary<string, Operation>();
        private Dictionary<byte, Operation> operationByCode = new Dictionary<byte, Operation>();

        // LABELS are used at compile time. Compiler converts labels into memory addresses.
        // Flow:
        // 1. Pass through script and convert labels into memory addresses
        // 2. Second pass: when encountering an operation utilizing a label, the operation fetches the address it represents and uses that as bytecode
        // When jumping to label,
        private StringReader reader;
        private Type returnType;
        private bool sucess;

        //private List<FieldInfo> GetOperationFields(Type type)
        //{
        //    FieldInfo[] fieldInfos = type.GetFields(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy);
        //    return fieldInfos.Where(f => f.Name.StartsWith("OP")).ToList();
        //}
        /// <summary>
        /// Initializes the compiler.
        /// </summary>
        //public void Initialize()
        //{
        //List<FieldInfo> constants = this.GetOperationFields(typeof(ScriptConstants));
        //foreach(FieldInfo c in constants)
        //{
        //    Operation o = (Operation)c.GetValue(null);
        //    this.operationByCode.Add(o.byteCode, o);
        //}
        //byte[] file = (byte[])Resources.CompilerResources.ResourceManager.GetObject(OPCODE_RESOURCE_NAME);
        //MemoryStream stream = new MemoryStream(file);
        //StreamStructReader<Operation> dataReader = new StreamStructReader<Operation>(stream);
        //if (dataReader.CanGetStruct())
        //{
        //    IEnumerable<Operation> codes = dataReader.GetNextStructs();
        //    foreach (Operation code in codes)
        //    {
        //        if (operationByName.ContainsKey(code.stringToken) || operationByCode.ContainsKey(code.byteCode))
        //        {
        //            MonoKleGame.Logger.AddLog("Operation opcode conflicts with existing opcode: " + code.ToString(), Logging.LogLevel.Error);
        //        }
        //        else
        //        {
        //            operationByName.Add(code.stringToken, code);
        //            operationByCode.Add(code.byteCode, code);
        //        }
        //    }
        //}
        //else
        //{
        //    MonoKleGame.Logger.AddLog("No opcodes loaded. Script compiler will not work.", Logging.LogLevel.Error);
        //}
        //dataReader.Close();
        //}
        // TODO: This code is kind of hard to read. Consider refactoring.
        // TODO: Seriously. Refactor this, so many clauses down below.

        // TODO: Check to see that there is an endscript.
        private Dictionary<string, Variable> variableByName;
        private int lineCounter;

        public Script Compile(string source)
        {
            this.Reset();
            this.reader = new StringReader(source);

            this.ProcessHeader();
            this.ProcessLines();
            this.reader.Close();

            return this.sucess ? new Script(this.name, this.returnType, this.channel, this.byteCode.ToArray(), (byte)variableByName.Count) : null;
        }

        private void ProcessLines()
        {
            while (this.sucess && this.reader.Peek() != -1)
            {
                // Read lines, but skip empty and commented ones.
                this.lineCounter++;
                string line = this.reader.ReadLine().Trim();
                if (line.Length > 0 && line.StartsWith(ScriptConstants.SCRIPT_COMMENT) == false)
                {
                    string argument = null;
                    string operation = line;

                    // Assign argument if it exists
                    int firstSpace = line.IndexOf(' ');
                    if (firstSpace != -1)
                    {
                        operation = line.Substring(0, firstSpace);   // Since we trimmed spaces and checked length this is safe
                        argument = line.Substring(firstSpace + 1, line.Length - firstSpace - 1);
                    }

                    // Check for specials first
                    if (Regex.IsMatch(operation, ScriptConstants.SCRIPT_END))
                    {
                        if (this.reader.Peek() != -1)
                        {
                            this.ReportError("Unexpected end of script reached.");
                        }
                    }
                    else
                    {
                        this.CompileLine(operation, argument);
                    }
                }
            }
        }

        public string ReverseString(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        private bool CheckExpressionValidity(string[] tokens)
        {
            int nLeft = 0;
            int nRight = 0;
            for (int i = 0; i < tokens.Length; i++)
            {
                // Grouping
                if (tokens[i].Equals(ScriptConstants.SCRIPT_OPERATOR_GROUPLEFT))
                {
                    nLeft++;
                }
                else if (tokens[i].Equals(ScriptConstants.SCRIPT_OPERATOR_GROUPRIGHT))
                {
                    nRight++;
                }
                if (nRight > nLeft)
                {
                    this.ReportError("Invalid expression. Too many ending parentheses.");
                    return false;
                }
                // Operator not next to other operators
                if(ScriptConstants.IsOperator(tokens[i]) && ScriptConstants.IsGrouping(tokens[i]) == false)
                {
                    if((i == 0 || i >= tokens.Length - 1)
                        || (ScriptConstants.IsOperator(tokens[i - 1]) && ScriptConstants.IsGrouping(tokens[i - 1]) == false)
                        || (ScriptConstants.IsOperator(tokens[i + 1]) && ScriptConstants.IsGrouping(tokens[i + 1]) == false))
                    {
                        this.ReportError("Invalid expression. Binary operator missing an operand?");                    
                        return false;
                    }
                }
                // Operands next to operators
                if(ScriptConstants.IsOperand(tokens[i]))
                {
                    if((i > 0 && ScriptConstants.IsOperator(tokens[i - 1]) == false)
                        || (i < tokens.Length - 1 && ScriptConstants.IsOperator(tokens[i + 1]) == false))
                    {
                        this.ReportError("Invalid expression. Operand missing an operator?");
                        return false;
                    }
                }
            }
            if (nRight != nLeft)
            {
                this.ReportError("Invalid expression. Too few ending parentheses.");
                return false;
            }

            return true;
        }

        private void CompileLine(string operation, string argument)
        {
            string[] infixArgument = null;
            string[] prefixArgument = null;
            Type argumentType = null;
            byte[] argumentByteCode = null;

            if(argument != null)
            {
                infixArgument = this.TokenizeArgument(argument);
                
                if (this.CheckExpressionValidity(infixArgument) == false)
                {
                    return;
                }
                
                prefixArgument = this.InfixToPrefix(infixArgument);
                int index = 0;
                argumentType = this.GetDominantType(prefixArgument, ref index);
                
                if(argumentType == null)
                {
                    this.ReportError("Invalid expression, type mismatch.");
                    return;
                }

                argumentByteCode = this.CompileExpression(prefixArgument);
            }

            // From here on expression is correct!
            if (operation.Equals(ScriptConstants.OP_RETURN_VOIDVALUE_TOKEN))
            {
                if(argument == null)
                {
                    if (returnType == typeof(void))
                    {
                        this.byteCode.AddLast(ScriptConstants.OP_RETURN_VOID);
                    }
                    else
                    {
                        this.ReportError("No return value specified. Expected: " + this.returnType.Name);
                    }
                }
                else
                {
                    this.byteCode.AddLast(ScriptConstants.OP_RETURN_VALUE);
                    if (ScriptConstants.IsReturnTypeCompatible(argumentType, this.returnType))
                    {
                        this.byteCode.AddLast(ScriptConstants.TypeToByte(argumentType));
                        foreach(byte b in argumentByteCode)
                        {
                            this.byteCode.AddLast(b);
                        }
                    }
                    else
                    {
                        this.ReportError("Script does not return " + argumentType.Name + ". It expects: " + this.returnType.Name);
                    }
                }
            }
        }

        private byte[] CompileExpression(string[] tokens)
        {
            LinkedList<byte> bytes = new LinkedList<byte>();
            foreach (string s in tokens)
            {
                if(ScriptConstants.IsOperand(s))
                {
                    byte[] valueBytes = null;
                    if(ScriptConstants.TokenIsInt(s))
                    {
                        bytes.AddLast(ScriptConstants.OP_CONST_INT);
                        valueBytes = Utilities.ByteConverter.ToBytes(Int32.Parse(s));
                    } else if(ScriptConstants.TokenIsFloat(s))
                    {
                        bytes.AddLast(ScriptConstants.OP_CONST_FLOAT);
                        valueBytes = Utilities.ByteConverter.ToBytes(Single.Parse(s));
                    }
                    else if (ScriptConstants.TokenIsBool(s))
                    {
                        bytes.AddLast(ScriptConstants.OP_CONST_BOOL);
                        valueBytes = Utilities.ByteConverter.ToBytes(Boolean.Parse(s));
                    }
                    else
                    {
                        this.ReportError("Expression invalid. Unrecognized type of operand.");
                    }

                    if (valueBytes != null)
                    {
                        for (int i = 0; i < valueBytes.Length; i++)
                        {
                            bytes.AddLast(valueBytes[i]);
                        }
                    }
                }
                else
                {
                    if (ScriptConstants.TokenHasOPCode(s))
                    {
                        byte code = ScriptConstants.TokenToOPCode(s);
                        bytes.AddLast(code);
                    }
                    else
                    {
                        this.ReportError("Expression invalid. Unsupported operator encountered.");
                    }
                }
            }
            return bytes.ToArray();
        }

        private Type GetDominantType(string[] tokens, ref int index)
        {
            // SOME KIND OF TREE? YES. TREES ARE AWESOME
            //
            //        =  
            //    >      false
            //  1   5.0
            // 
            // DAMN IM GOOD :3
            // = > 5 6 false

            Type type = null;
            string token = tokens[index];
            if (ScriptConstants.IsLogicOperator(token))
            {
                index++;
                Type leftType = GetDominantType(tokens, ref index);
                Type rightType = GetDominantType(tokens, ref index);

                if(leftType != null && rightType != null)
                {
                    // See if comparable
                    if(ScriptConstants.IsMathTypeCompatible(leftType, rightType))
                    {
                        type = typeof(bool);
                    }
                }
            }
            else if (ScriptConstants.IsOperator(token))
            {
                index++;
                Type leftType = GetDominantType(tokens, ref index);
                Type rightType = GetDominantType(tokens, ref index);

                if (leftType != null && rightType != null)
                {
                    // See if comparable
                    if (ScriptConstants.IsMathTypeCompatible(leftType, rightType))
                    {
                        if(leftType == typeof(float) || rightType == typeof(float))
                        {
                            type = typeof(float);
                        }
                        else
                        {
                            type = leftType;    // Just chosen arbitrarily
                        }
                    }
                }
            }
            else
            {
                index++;
                Type t = ScriptConstants.GetTokenType(token);
                if(t == null)
                {
                    if (variableByName.ContainsKey(token))
                    {
                        Variable v = variableByName[token];
                        t = v.type;
                    }
                }
                type = t;
            }


            return type;




            //Type type = null;
            //foreach(string t in tokens)
            //{
            //    // 5 + 5 > 10
            //    // TODO: Evaluate on all sides of logical operators individually
            //    if (ScriptConstants.IsLogicOperator(t))
            //    {
            //        type = typeof(bool);
            //    }
            //    else if (ScriptConstants.IsOperand(t))
            //    {
            //        if (ScriptConstants.TokenIsInt(t))
            //        {
            //            if (type == null)
            //            {
            //                type = typeof(int);
            //            }
            //            else if (type.Equals(typeof(bool)) || type.Equals(typeof(string)) || type.Equals(typeof(object)))
            //            {
            //                return null;
            //            }
            //        }
            //        else if (ScriptConstants.TokenIsFloat(t))
            //        {
            //            if (type == null || type == typeof(int))
            //            {
            //                type = typeof(float);
            //            }
            //            else if (type.Equals(typeof(bool)) || type.Equals(typeof(string)) || type.Equals(typeof(object)))
            //            {
            //                return null;
            //            }
            //        }
            //        else if (ScriptConstants.TokenIsBool(t))
            //        {
            //            if (type != null && type != typeof(bool))
            //            {
            //                return null;
            //            }
            //            else
            //            {
            //                type = typeof(bool);
            //            }
            //        }
            //        else if (Regex.IsMatch(t, "^\"[a-zA-Z0-9]+\"$"))
            //        {
            //            if (type.Equals(typeof(object)))
            //            {
            //                return null;
            //            }
            //            else
            //            {
            //                type = typeof(string);
            //            }
            //        }
            //        else
            //        {
            //            if (variableByName.ContainsKey(t))
            //            {
            //                Variable v = variableByName[t];
            //                if (type == null)
            //                {
            //                    type = v.type;
            //                }
            //                else
            //                {
            //                    return null;
            //                }
            //            }
            //            else
            //            {
            //                return null;
            //            }
            //        }
            //    }
            //}
            //return type;
        }

        private string[] InfixToPrefix(string[] tokens)
        {
            Stack<string> prefix = new Stack<string>();
            Stack<string> operatorStack = new Stack<string>();

            for (int i = tokens.Length - 1; i >= 0; i--)
            {
                if (tokens[i].Length > 0 && tokens[i] != " ")
                {
                    if (ScriptConstants.IsOperand(tokens[i]))
                    {
                        prefix.Push(tokens[i]);
                    }
                    else
                    {
                        if (tokens[i] == ScriptConstants.SCRIPT_OPERATOR_GROUPLEFT)
                        {
                            while (operatorStack.Count > 0)// && ScriptConstants.OperatorHiearchy(operatorStack.Peek()) >= ScriptConstants.OperatorHiearchy(tokens[i]))
                            {
                                string pop = operatorStack.Pop();
                                if (pop == ScriptConstants.SCRIPT_OPERATOR_GROUPRIGHT)
                                {
                                    break;
                                }
                                else
                                {
                                    prefix.Push(pop);
                                }
                            }
                        }
                        else
                        {
                            int hiearchyStack = operatorStack.Count > 0 ? ScriptConstants.OperatorHiearchy(operatorStack.Peek()) : 0;
                            int hiearchyToken = operatorStack.Count > 0 ? ScriptConstants.OperatorHiearchy(tokens[i]) : 0;
                            while (operatorStack.Count > 0 && hiearchyStack < hiearchyToken && operatorStack.Peek() != ScriptConstants.SCRIPT_OPERATOR_GROUPRIGHT)
                            {
                                string pop = operatorStack.Pop();
                                prefix.Push(pop);
                            }
                            operatorStack.Push(tokens[i]);
                        }
                    }
                }
            }

            while(operatorStack.Count > 0)
            {
                prefix.Push(operatorStack.Pop());
            }

            return prefix.ToArray();
        }

        private void ProcessHeader()
        {
            if(this.reader.Peek() != -1)
            {
                string header = this.reader.ReadLine();
                if(Regex.IsMatch(header, ScriptCompiler.REGEX_HEADER_CHECK))
                {
                    string[] parts = header.Split(new char[] { ' ', '>' }, StringSplitOptions.RemoveEmptyEntries);
                    this.returnType = Type.GetType(this.TypeAlias(parts[1]));
                    this.name = parts[2];
                    if (parts.Length >= 4)
                    {
                        this.channel = parts[3];
                    }
                }
                else
                {
                    this.ReportError("Header not correctly specified.");
                }
            }
            else
            {
                this.ReportError("No header present.");
            }
        }

        private void ReportError(string message)
        {
            this.sucess = false;
            StringBuilder sb = new StringBuilder();
            sb.Append("Compilation error on line ");
            sb.Append(this.lineCounter);
            if(this.name != null)
            {
                sb.Append(" in ");
                sb.Append(this.name);
            }
            sb.Append(": ");
            sb.Append(message);
            MonoKleGame.Logger.AddLog(sb.ToString(), Logging.LogLevel.Error);
        }

        private void Reset()
        {
            this.reader = null;
            this.sucess = true;
            this.name = null;
            this.returnType = null;
            this.channel = null;
            this.lineCounter = 0;
            this.variableByName = new Dictionary<string, Variable>(byte.MaxValue);
            this.byteCode = new LinkedList<byte>();
        }

        private byte[] StringToBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private string[] TokenizeArgument(string argument)
        {
            string splitter = "(" + ScriptConstants.SCRIPT_OPERATOR_GROUPLEFT_REGEX + "|" + ScriptConstants.SCRIPT_OPERATOR_GROUPRIGHT_REGEX
                + "|" + ScriptConstants.SCRIPT_OPERATOR_ADD_REGEX + "|" + ScriptConstants.SCRIPT_OPERATOR_SUBTRACT_REGEX
                + "|" + ScriptConstants.SCRIPT_OPERATOR_MULTIPLY_REGEX + "|" + ScriptConstants.SCRIPT_OPERATOR_DIVIDE_REGEX
                + "|" + ScriptConstants.SCRIPT_OPERATOR_POWER_REGEX + "|" + ScriptConstants.SCRIPT_OPERATOR_MODULO_REGEX
                + "|" + ScriptConstants.SCRIPT_OPERATOR_LOGIC_SMALLER_REGEX + "|" + ScriptConstants.SCRIPT_OPERATOR_LOGIC_SMALLEREQUAL_REGEX
                + "|" + ScriptConstants.SCRIPT_OPERATOR_LOGIC_LARGER_REGEX + "|" + ScriptConstants.SCRIPT_OPERATOR_LOGIC_LARGEREQUAL_REGEX
                + "|" + ScriptConstants.SCRIPT_OPERATOR_LOGIC_EQUAL_REGEX + "|" + ScriptConstants.SCRIPT_OPERATOR_LOGIC_NOTEQUAL_REGEX + "|\\s)";

            string[] tokens = Regex.Split(argument, splitter).Where(t => t.Length > 0 && t != " ").ToArray();
            return tokens;
        }

        private string TypeAlias(string type)
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
            return "";
        }
    }
}