namespace MonoKle.Scripting
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;

    using MonoKle.IO;
    using MonoKle.Scripting.Script;

    internal class Compiler
    {
        private LinkedList<byte> byteCode;
        private LinkedList<Type> arguments = new LinkedList<Type>();
        private StringReader reader;
        private bool sucess;
        private Header header;

        // LABELS are used at compile time. Compiler converts labels into memory addresses.
        // Flow:
        // 1. Pass through script and convert labels into memory addresses
        // 2. Second pass: when encountering an operation utilizing a label, the operation fetches the address it represents and uses that as bytecode
        // When jumping to label,

        // TODO: This code is kind of hard to read. Consider refactoring.
        // TODO: Seriously. Refactor this, so many clauses down below.

        private Dictionary<string, Variable> variableByName;
        private int lineCounter;

        public ByteScript Compile(Source source, Dictionary<string, Header> existingHeaders)
        {
            this.Reset(source);
            this.reader = new StringReader(source.Text);

            foreach(Argument argument in source.Header.arguments)
            {
                this.AddVariable(argument.name, argument.type);
            }

            this.ProcessLines();
            this.reader.Close();

            return this.sucess ? new ByteScript(source.Header, this.byteCode.ToArray(), (byte)variableByName.Count) : null;
        }

        private void ProcessLines()
        {
            while (this.sucess && this.reader.Peek() != -1)
            {
                // Read lines, but skip empty and commented ones.
                this.lineCounter++;
                string line = this.reader.ReadLine().Trim();
                if (line.Length > 0 && line.StartsWith(ScriptBase.SCRIPT_COMMENT) == false)
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
                    if (Regex.IsMatch(operation, ScriptBase.SCRIPT_END))
                    {
                        if (this.reader.Peek() != -1)
                        {
                            this.ReportError("Unexpected end of script reached.");
                        }
                    }
                    else if(ScriptBase.IsSupportedVariableType(operation))  // TODO: Break this and below out and put in compile line.
                    {
                        this.CompileNewVariable(operation, argument);
                    }
                    else if(operation.Equals("set")) // TODO: Make nicer
                    {
                        if (Regex.IsMatch(argument, "^[a-zA-Z0-9]+\\s*:.+$"))
                        {
                            int firstDivider = argument.IndexOf(':');
                            string name = argument.Substring(0, firstDivider).Trim();
                            string expression = argument.Substring(firstDivider + 1, argument.Length - firstDivider - 1).Trim();
                            if (this.variableByName.ContainsKey(name))
                            {
                                Type expressionType = null;
                                byte[] expressionCode;
                                if (this.ExpressionToByteCode(expression, out expressionType, out expressionCode))
                                {
                                    Variable var = this.variableByName[name];
                                    this.byteCode.AddLast(ScriptBase.OP_SETVAR);
                                    this.byteCode.AddLast(var.number);
                                    foreach (byte b in expressionCode)
                                    {
                                        this.byteCode.AddLast(b);
                                    }
                                }
                            }
                            else
                            {
                                this.ReportError("Variable (" + name + ") has not been declared yet.");
                            }
                        }
                        else
                        {
                            this.ReportError("Variable not declared properly.");
                        }
                    }
                    else
                    {
                        this.CompileLine(operation, argument);
                    }
                }
            }
        }

        private void CompileNewVariable(string type, string argument)
        {
            if (this.variableByName.Count < ScriptBase.SCRIPT_MAX_VARIABLES)
            {
                Type variableType = Type.GetType(ScriptBase.TypeAlias(type));

                if (variableType == typeof(int) || true) // TODO: Below code can be before/after, so not to repeat all these things
                {
                    if (Regex.IsMatch(argument, "^[a-zA-Z0-9]+\\s*:.+$"))
                    {
                        int firstDivider = argument.IndexOf(':');
                        string name = argument.Substring(0, firstDivider).Trim();
                        string expression = argument.Substring(firstDivider + 1, argument.Length - firstDivider - 1).Trim();
                        if (this.variableByName.ContainsKey(name) == false)
                        {
                            Type expressionType = null;
                            byte[] expressionCode;
                            if (this.ExpressionToByteCode(expression, out expressionType, out expressionCode))
                            {
                                byte number = (byte)this.variableByName.Count;
                                this.variableByName.Add(name, new Variable(expressionType, number));
                                this.byteCode.AddLast(ScriptBase.OP_INIVAR);
                                this.byteCode.AddLast(number);
                                foreach (byte b in expressionCode)
                                {
                                    this.byteCode.AddLast(b);
                                }
                            }
                        }
                        else
                        {
                            this.ReportError("Variable name (" + name + ") conflicts with existing variable.");
                        }
                    }
                    else
                    {
                        this.ReportError("Variable not declared properly.");
                    }
                }
                else
                {
                    this.ReportError("Type declaration not implemented yet!");
                }
            } else
            {
                this.ReportError("Maximum amount of variables (" + ScriptBase.SCRIPT_MAX_VARIABLES + ") reached.");
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
                if (tokens[i].Equals(ScriptBase.SCRIPT_OPERATOR_GROUPLEFT))
                {
                    nLeft++;
                }
                else if (tokens[i].Equals(ScriptBase.SCRIPT_OPERATOR_GROUPRIGHT))
                {
                    nRight++;
                }
                if (nRight > nLeft)
                {
                    this.ReportError("Invalid expression. Too many ending parentheses.");
                    return false;
                }
                // Operator not next to other operators
                if(ScriptBase.IsOperator(tokens[i]) && ScriptBase.IsGrouping(tokens[i]) == false)
                {
                    if((i == 0 || i >= tokens.Length - 1)
                        || (ScriptBase.IsOperator(tokens[i - 1]) && ScriptBase.IsGrouping(tokens[i - 1]) == false)
                        || (ScriptBase.IsOperator(tokens[i + 1]) && ScriptBase.IsGrouping(tokens[i + 1]) == false))
                    {
                        this.ReportError("Invalid expression. Binary operator missing an operand?");                    
                        return false;
                    }
                }
                // Operands next to operators
                if(ScriptBase.IsOperand(tokens[i]))
                {
                    if((i > 0 && ScriptBase.IsOperator(tokens[i - 1]) == false)
                        || (i < tokens.Length - 1 && ScriptBase.IsOperator(tokens[i + 1]) == false))
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

        private bool ExpressionToByteCode(string expression, out Type type, out byte[] byteCode)
        {
            string[] infixTokens = this.TokenizeExpression(expression);
            type = null;
            byteCode = null;
            
            if (this.CheckExpressionValidity(infixTokens) == false)
            {
                this.ReportError("Invalid expression, type mismatch.");
                return false;
            }

            string[] prefixTokens = this.InfixToPrefix(infixTokens);

            int index = 0;
            type = this.GetDominantType(prefixTokens, ref index);

            if (type == null)
            {
                this.ReportError("Invalid expression, type mismatch.");
                return false;
            }

            byteCode = this.CompileExpression(prefixTokens);
            return true;
        }

        private void CompileLine(string operation, string argument)
        {   
            Type argumentType = null;
            byte[] expressionCode = null;

            if(argument != null)
            {
                if(this.ExpressionToByteCode(argument, out argumentType, out expressionCode) == false)
                {
                    return;
                }
            }

            // From here on expression is evaluated to be correct
            if (operation.Equals(ScriptBase.OP_RETURN_VOIDVALUE_TOKEN))
            {
                if(argument == null)
                {
                    if (this.header.returnType == typeof(void))
                    {
                        this.byteCode.AddLast(ScriptBase.OP_RETURN_VOID);
                    }
                    else
                    {
                        this.ReportError("No return value specified. Expected: " + this.header.returnType);
                    }
                }
                else
                {
                    this.byteCode.AddLast(ScriptBase.OP_RETURN_VALUE);
                    if (ScriptBase.IsReturnTypeCompatible(argumentType, this.header.returnType))
                    {
                        this.byteCode.AddLast(ScriptBase.TypeToByte(argumentType));
                        foreach(byte b in expressionCode)
                        {
                            this.byteCode.AddLast(b);
                        }
                    }
                    else
                    {
                        this.ReportError("Script does not return " + argumentType.Name + ". It expects: " + this.header.returnType.Name);
                    }
                }
            }
            else if (operation.Equals(ScriptBase.OP_PRINT_TOKEN))
            {
                if(argument == null)
                {
                    this.ReportError("Missing argument.");
                }
                else
                {
                    this.byteCode.AddLast(ScriptBase.OP_PRINT);
                    foreach (byte b in expressionCode)
                    {
                        this.byteCode.AddLast(b);
                    }
                }
            }
            else
            {
                this.ReportError("Unrecognized operation.");
            }
        }

        private byte[] CompileExpression(string[] tokens)
        {
            LinkedList<byte> bytes = new LinkedList<byte>();
            foreach (string s in tokens)
            {
                if(ScriptBase.IsOperand(s))
                {
                    byte[] valueBytes = null;
                    if(ScriptBase.TokenIsInt(s))
                    {
                        bytes.AddLast(ScriptBase.OP_CONST_INT);
                        valueBytes = Utilities.ByteConverter.ToBytes(Int32.Parse(s));
                    } else if(ScriptBase.TokenIsFloat(s))
                    {
                        bytes.AddLast(ScriptBase.OP_CONST_FLOAT);
                        valueBytes = Utilities.ByteConverter.ToBytes(Single.Parse(s, System.Globalization.CultureInfo.InvariantCulture));
                    }
                    else if (ScriptBase.TokenIsBool(s))
                    {
                        bytes.AddLast(ScriptBase.OP_CONST_BOOL);
                        valueBytes = Utilities.ByteConverter.ToBytes(Boolean.Parse(s));
                    }
                    else if (ScriptBase.TokenIsString(s))
                    {
                        bytes.AddLast(ScriptBase.OP_CONST_STRING);
                        valueBytes = Utilities.ByteConverter.ToBytes(s);
                    }
                    else if (this.variableByName.ContainsKey(s))
                    {
                        bytes.AddLast(ScriptBase.OP_GETVAR);
                        valueBytes = new byte[] { this.variableByName[s].number };
                    }

                    if (valueBytes != null)
                    {
                        for (int i = 0; i < valueBytes.Length; i++)
                        {
                            bytes.AddLast(valueBytes[i]);
                        }
                    }
                    else
                    {
                        this.ReportError("Expression invalid. Unrecognized type of operand.");
                    }
                }
                else
                {
                    if (ScriptBase.TokenHasOPCode(s))
                    {
                        byte code = ScriptBase.TokenToOPCode(s);
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
            Type type = null;
            string token = tokens[index];
            if (ScriptBase.IsLogicOperator(token))
            {
                index++;
                Type leftType = GetDominantType(tokens, ref index);
                Type rightType = GetDominantType(tokens, ref index);

                if(leftType != null && rightType != null)
                {
                    // See if comparable
                    if(ScriptBase.IsMathTypeCompatible(leftType, rightType))
                    {
                        type = typeof(bool);
                    }
                }
            }
            else if (ScriptBase.IsOperator(token))
            {
                index++;
                Type leftType = GetDominantType(tokens, ref index);
                Type rightType = GetDominantType(tokens, ref index);

                if (leftType != null && rightType != null)
                {
                    // See if comparable
                    if (ScriptBase.IsMathTypeCompatible(leftType, rightType))
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
                    else if(leftType == typeof(string) || rightType == typeof(string))
                    {
                        // Gives support for adding other values to strings.
                        type = typeof(string);
                    }
                }
            }
            else
            {
                index++;
                Type t = ScriptBase.GetTokenType(token);
                if(t == null)
                {
                    if (variableByName.ContainsKey(token))
                    {
                        Variable v = variableByName[token];
                        t = v.type;
                    }
                    else
                    {
                        this.ReportError("Variable (" + token + ") could not be found.");
                    }
                }
                type = t;
            }

            return type;
        }

        private string[] InfixToPrefix(string[] tokens)
        {
            Stack<string> prefix = new Stack<string>();
            Stack<string> operatorStack = new Stack<string>();

            for (int i = tokens.Length - 1; i >= 0; i--)
            {
                if (tokens[i].Length > 0 && tokens[i] != " ")
                {
                    if (ScriptBase.IsOperand(tokens[i]))
                    {
                        prefix.Push(tokens[i]);
                    }
                    else
                    {
                        if (tokens[i] == ScriptBase.SCRIPT_OPERATOR_GROUPLEFT)
                        {
                            while (operatorStack.Count > 0)// && ScriptConstants.OperatorHiearchy(operatorStack.Peek()) >= ScriptConstants.OperatorHiearchy(tokens[i]))
                            {
                                string pop = operatorStack.Pop();
                                if (pop == ScriptBase.SCRIPT_OPERATOR_GROUPRIGHT)
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
                            int hiearchyStack = operatorStack.Count > 0 ? ScriptBase.OperatorHiearchy(operatorStack.Peek()) : 0;
                            int hiearchyToken = operatorStack.Count > 0 ? ScriptBase.OperatorHiearchy(tokens[i]) : 0;
                            while (operatorStack.Count > 0 && hiearchyStack < hiearchyToken && operatorStack.Peek() != ScriptBase.SCRIPT_OPERATOR_GROUPRIGHT)
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

        private void AddVariable(string name, Type type)
        {
            if (this.variableByName.Count < ScriptBase.SCRIPT_MAX_VARIABLES)
            {
                if (this.variableByName.ContainsKey(name) == false)
                {
                    this.variableByName.Add(name, new Variable(type, (byte)variableByName.Count));
                }
                else
                {
                    this.ReportError("Invalid return type specified in header.");
                }
            }
            else
            {
                this.ReportError("Maximum amount of variables reached.");
            }
        }

        private void ReportError(string message)
        {
            this.sucess = false;
            StringBuilder sb = new StringBuilder();
            sb.Append("Compilation error on line ");
            sb.Append(this.lineCounter);
            sb.Append(" in ");
            sb.Append(this.header.name);
            sb.Append(": ");
            sb.Append(message);
            MonoKleGame.Logger.AddLog(sb.ToString(), Logging.LogLevel.Error);
        }

        private void Reset(Source source)
        {
            this.reader = null;
            this.sucess = true;
            this.header = source.Header;
            this.lineCounter = 0;
            this.variableByName = new Dictionary<string, Variable>(byte.MaxValue);
            this.byteCode = new LinkedList<byte>();
        }

        private string[] TokenizeExpression(string argument)
        {
            string splitter = "(" + ScriptBase.SCRIPT_STRING_TOKEN + "[^" + ScriptBase.SCRIPT_STRING_TOKEN + "]*" + ScriptBase.SCRIPT_STRING_TOKEN
                + "|" + ScriptBase.SCRIPT_NAMES_ALLOWEDNAMES_REGEX + "\\(.*?\\)"
                + "|" + ScriptBase.SCRIPT_OPERATOR_GROUPLEFT_REGEX + "|" + ScriptBase.SCRIPT_OPERATOR_GROUPRIGHT_REGEX
                + "|" + ScriptBase.SCRIPT_OPERATOR_ADD_REGEX + "|" + ScriptBase.SCRIPT_OPERATOR_SUBTRACT_REGEX
                + "|" + ScriptBase.SCRIPT_OPERATOR_MULTIPLY_REGEX + "|" + ScriptBase.SCRIPT_OPERATOR_DIVIDE_REGEX
                + "|" + ScriptBase.SCRIPT_OPERATOR_POWER_REGEX + "|" + ScriptBase.SCRIPT_OPERATOR_MODULO_REGEX
                + "|" + ScriptBase.SCRIPT_OPERATOR_LOGIC_SMALLER_REGEX + "|" + ScriptBase.SCRIPT_OPERATOR_LOGIC_SMALLEREQUAL_REGEX
                + "|" + ScriptBase.SCRIPT_OPERATOR_LOGIC_LARGER_REGEX + "|" + ScriptBase.SCRIPT_OPERATOR_LOGIC_LARGEREQUAL_REGEX
                + "|" + ScriptBase.SCRIPT_OPERATOR_LOGIC_EQUAL_REGEX + "|" + ScriptBase.SCRIPT_OPERATOR_LOGIC_NOTEQUAL_REGEX + "|\\s)";
            
            string[] tokens = Regex.Split(argument, splitter).Where(t => t.Length > 0 && t != " ").ToArray();
            return tokens;
        }
    }
}