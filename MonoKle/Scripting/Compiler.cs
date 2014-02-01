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
    using MonoKle.Utilities;

    internal class Compiler
    {
        private LinkedList<byte> byteCode;
        private LinkedList<Type> arguments = new LinkedList<Type>();
        private bool sucess;
        private Header header;

        // LABELS are used at compile time. Compiler converts labels into memory addresses.
        // Flow:
        // 1. Pass through script and convert labels into memory addresses
        // 2. Second pass: when encountering an operation utilizing a label, the operation fetches the address it represents and uses that as bytecode
        // When jumping to label,

        // TODO: This code is kind of hard to read. Consider refactoring.
        // TODO: Add so that scripts with return type != void must have return.
        // TODO: Must be able to call void functions without assigning to variable (cuz that is not possible since they are void)
        // TODO: Add flow control (if/else etc.)
        // TODO: Add common math functions
        // TODO: Allow reflection interfacing(?)
        // TODO: 

        private Dictionary<string, Variable> variableByName;
        private Dictionary<string, Header> headerByName;
        private int lineCounter;

        public ByteScript Compile(Source source, Dictionary<string, Header> existingHeaders)
        {
            this.sucess = true;
            this.header = source.Header;
            this.lineCounter = 0;
            this.variableByName = new Dictionary<string, Variable>(byte.MaxValue);
            this.byteCode = new LinkedList<byte>();
            this.headerByName = existingHeaders;

            StringReader reader = new StringReader(source.Text);

            foreach (Argument argument in source.Header.arguments)
            {
                this.AddVariable(argument.name, argument.type);
            }

            this.ProcessLines(reader);
            reader.Close();

            return this.sucess ? new ByteScript(source.Header, this.byteCode.ToArray(), (byte)variableByName.Count) : null;
        }

        private void ProcessLines(StringReader reader)
        {
            while (this.sucess && reader.Peek() != -1)
            {
                // Read lines, but skip empty and commented ones.
                this.lineCounter++;
                string line = reader.ReadLine().Trim();
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

                    // Check for flow control
                    if (false)
                    {
                        // TODO: Insert IF / ELSEIF / ELSE / ENDIF checks
                    }
                    else
                    {
                        this.CompileLine(operation, argument);
                    }
                }
            }
        }

        private void SetVariable(string argument)
        {
            if (Regex.IsMatch(argument, "^[a-zA-Z0-9]+\\s*:.+$"))
            {
                int firstDivider = argument.IndexOf(':');
                string name = argument.Substring(0, firstDivider).Trim();
                string expression = argument.Substring(firstDivider + 1, argument.Length - firstDivider - 1).Trim();
                if (this.variableByName.ContainsKey(name))
                {
                    Type expressionType = null;
                    byte[] expCode;
                    if (this.ExpressionToByteCode(expression, out expressionType, out expCode))
                    {
                        Variable var = this.variableByName[name];
                        if (ScriptBase.IsMathTypeCompatible(var.type, expressionType))
                        {
                            this.byteCode.AddLast(ScriptBase.OP_SETVAR);
                            this.byteCode.AddLast(var.number);
                            foreach (byte b in expCode)
                            {
                                this.byteCode.AddLast(b);
                            }
                        }
                        else
                        {
                            this.ReportError("Variable type mismatch.");
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

        private void SetNewVariable(string type, string argument)
        {
            if (this.variableByName.Count < ScriptBase.SCRIPT_MAX_VARIABLES)
            {
                Type variableType = Type.GetType(ScriptBase.TypeAlias(type));
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
                            if (ScriptBase.IsMathTypeCompatible(variableType, expressionType))
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
                            else
                            {
                                this.ReportError("Variable type mismatch.");
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
                this.ReportError("Maximum amount of variables (" + ScriptBase.SCRIPT_MAX_VARIABLES + ") reached.");
            }
        }

        public string ReverseString(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        private bool CheckExpressionValidity(Token[] tokens)
        {
            int nLeft = 0;
            int nRight = 0;
            for (int i = 0; i < tokens.Length; i++)
            {
                // Grouping
                if (tokens[i].isGroupingLeft)
                {
                    nLeft++;
                }
                else if (tokens[i].isGroupingRight)
                {
                    nRight++;
                }
                if (nRight > nLeft)
                {
                    this.ReportError("Invalid expression. Too many ending parentheses.");
                    return false;
                }
                // Operator not next to other operators
                if (tokens[i].IsOperator() && tokens[i].IsGrouping() == false)
                {
                    if ((i == 0 || i >= tokens.Length - 1)
                        || (tokens[i - 1].IsOperator() && tokens[i - 1].IsGrouping() == false)
                        || (tokens[i + 1].IsOperator() && tokens[i + 1].IsGrouping() == false))
                    {
                        this.ReportError("Invalid expression. Binary operator missing an operand?");
                        return false;
                    }
                }
                // Operands next to operators
                if (tokens[i].isOperand)
                {
                    if ((i > 0 && tokens[i - 1].IsOperatorGrouping() == false)
                        || (i < tokens.Length - 1 && tokens[i + 1].IsOperatorGrouping() == false))
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

        //private bool CheckExpressionValidity(string[] tokens)
        //{
        //    int nLeft = 0;
        //    int nRight = 0;
        //    for (int i = 0; i < tokens.Length; i++)
        //    {
        //        // Grouping
        //        if (tokens[i].Equals(ScriptBase.SCRIPT_OPERATOR_GROUPLEFT))
        //        {
        //            nLeft++;
        //        }
        //        else if (tokens[i].Equals(ScriptBase.SCRIPT_OPERATOR_GROUPRIGHT))
        //        {
        //            nRight++;
        //        }
        //        if (nRight > nLeft)
        //        {
        //            this.ReportError("Invalid expression. Too many ending parentheses.");
        //            return false;
        //        }
        //        // Operator not next to other operators
        //        if (ScriptBase.IsOperator(tokens[i]) && ScriptBase.IsGrouping(tokens[i]) == false)
        //        {
        //            if ((i == 0 || i >= tokens.Length - 1)
        //                || (ScriptBase.IsOperator(tokens[i - 1]) && ScriptBase.IsGrouping(tokens[i - 1]) == false)
        //                || (ScriptBase.IsOperator(tokens[i + 1]) && ScriptBase.IsGrouping(tokens[i + 1]) == false))
        //            {
        //                this.ReportError("Invalid expression. Binary operator missing an operand?");
        //                return false;
        //            }
        //        }
        //        // Operands next to operators
        //        if (ScriptBase.IsOperand(tokens[i]))
        //        {
        //            if ((i > 0 && ScriptBase.IsOperator(tokens[i - 1]) == false)
        //                || (i < tokens.Length - 1 && ScriptBase.IsOperator(tokens[i + 1]) == false))
        //            {
        //                this.ReportError("Invalid expression. Operand missing an operator?");
        //                return false;
        //            }
        //        }
        //    }
        //    if (nRight != nLeft)
        //    {
        //        this.ReportError("Invalid expression. Too few ending parentheses.");
        //        return false;
        //    }

        //    return true;
        //}

        private bool ExpressionToByteCode(string expression, out Type type, out byte[] byteCode)
        {
            type = null;
            byteCode = null;

            //string[] infixTokens = this.TokenizeExpression(expression);
            Token[] infixTokens = this.TokenizeExpression(expression);
            if (infixTokens == null)
            {
                return false;
            }

            //if (this.CheckExpressionValidity(infixTokens) == false)
            //{
            //    this.ReportError("Invalid expression, type mismatch.");
            //    return false;
            //}

            if (this.CheckExpressionValidity(infixTokens) == false)
            {
                this.ReportError("Invalid expression, type mismatch.");
                return false;
            }

            //string[] prefixTokens = this.InfixToPrefix(infixTokens);

            Token[] prefixTokens = this.InfixToPrefix(infixTokens);

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
            // Variable operations (initialize/set)
            if (ScriptBase.IsSupportedVariableType(operation))
            {
                this.SetNewVariable(operation, argument);
            }
            else if (operation.Equals(ScriptBase.SCRIPT_OPERATION_SET_VARIABLE))
            {
                this.SetVariable(argument);
            }
            else
            {
                // Other operations
                Type argumentType = null;
                byte[] expressionCode = null;

                if (argument != null)
                {
                    if (this.ExpressionToByteCode(argument, out argumentType, out expressionCode) == false)
                    {
                        return;
                    }
                }

                if (operation.Equals(ScriptBase.SCRIPT_OPERATION_RETURN))
                {
                    if (argument == null)
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
                        if (ScriptBase.IsReturnTypeCompatible(argumentType, this.header.returnType))
                        {
                            this.byteCode.AddLast(ScriptBase.OP_RETURN_VALUE);
                            this.byteCode.AddLast(ScriptBase.TypeToByte(argumentType));
                            foreach (byte b in expressionCode)
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
                else if (operation.Equals(ScriptBase.SCRIPT_OPERATION_PRINT))
                {
                    if (argument == null)
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
        }

        private byte[] CompileExpression(Token[] tokens)
        {
            LinkedList<byte> bytes = new LinkedList<byte>();
            foreach (Token token in tokens)
            {
                if (token.isOperand)
                {
                    byte[] valueBytes = null;
                    if (ScriptBase.TokenIsInt(token.text))
                    {
                        bytes.AddLast(ScriptBase.OP_CONST_INT);
                        valueBytes = Utilities.ByteConverter.ToBytes(Int32.Parse(token.text));
                    }
                    else if (ScriptBase.TokenIsFloat(token.text))
                    {
                        bytes.AddLast(ScriptBase.OP_CONST_FLOAT);
                        valueBytes = Utilities.ByteConverter.ToBytes(Single.Parse(token.text, System.Globalization.CultureInfo.InvariantCulture));
                    }
                    else if (ScriptBase.TokenIsBool(token.text))
                    {
                        bytes.AddLast(ScriptBase.OP_CONST_BOOL);
                        valueBytes = Utilities.ByteConverter.ToBytes(Boolean.Parse(token.text));
                    }
                    else if (ScriptBase.TokenIsString(token.text))
                    {
                        bytes.AddLast(ScriptBase.OP_CONST_STRING);
                        valueBytes = Utilities.ByteConverter.ToBytes(token.text);
                    }
                    else if (this.variableByName.ContainsKey(token.text))
                    {
                        bytes.AddLast(ScriptBase.OP_GETVAR);
                        valueBytes = new byte[] { this.variableByName[token.text].number };
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
                else if(token.isFunction)
                {
                    bytes.AddLast(ScriptBase.OP_CALLFUNCTION);
                    foreach (byte b in ByteConverter.ToBytes(token.functionName))
                    {
                        bytes.AddLast(b);
                    }
                    bytes.AddLast((byte)token.functionArguments.Length);
                    foreach(Token[] e in token.functionArguments)
                    {
                        foreach (byte b in this.CompileExpression(e))
                        {
                            bytes.AddLast(b);
                        }
                    }
                }
                else
                {
                    if (ScriptBase.TokenHasOPCode(token.text))
                    {
                        byte code = ScriptBase.TokenToOPCode(token.text);
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

        //private byte[] CompileExpression(string[] tokens)
        //{
        //    LinkedList<byte> bytes = new LinkedList<byte>();
        //    foreach (string s in tokens)
        //    {
        //        if (ScriptBase.IsOperand(s))
        //        {
        //            byte[] valueBytes = null;
        //            if (ScriptBase.TokenIsInt(s))
        //            {
        //                bytes.AddLast(ScriptBase.OP_CONST_INT);
        //                valueBytes = Utilities.ByteConverter.ToBytes(Int32.Parse(s));
        //            }
        //            else if (ScriptBase.TokenIsFloat(s))
        //            {
        //                bytes.AddLast(ScriptBase.OP_CONST_FLOAT);
        //                valueBytes = Utilities.ByteConverter.ToBytes(Single.Parse(s, System.Globalization.CultureInfo.InvariantCulture));
        //            }
        //            else if (ScriptBase.TokenIsBool(s))
        //            {
        //                bytes.AddLast(ScriptBase.OP_CONST_BOOL);
        //                valueBytes = Utilities.ByteConverter.ToBytes(Boolean.Parse(s));
        //            }
        //            else if (ScriptBase.TokenIsString(s))
        //            {
        //                bytes.AddLast(ScriptBase.OP_CONST_STRING);
        //                valueBytes = Utilities.ByteConverter.ToBytes(s);
        //            }
        //            else if (this.variableByName.ContainsKey(s))
        //            {
        //                bytes.AddLast(ScriptBase.OP_GETVAR);
        //                valueBytes = new byte[] { this.variableByName[s].number };
        //            }
        //            else
        //            {

        //            }

        //            if (valueBytes != null)
        //            {
        //                for (int i = 0; i < valueBytes.Length; i++)
        //                {
        //                    bytes.AddLast(valueBytes[i]);
        //                }
        //            }
        //            else
        //            {
        //                this.ReportError("Expression invalid. Unrecognized type of operand.");
        //            }
        //        }
        //        else
        //        {
        //            if (ScriptBase.TokenHasOPCode(s))
        //            {
        //                byte code = ScriptBase.TokenToOPCode(s);
        //                bytes.AddLast(code);
        //            }
        //            else
        //            {
        //                this.ReportError("Expression invalid. Unsupported operator encountered.");
        //            }
        //        }
        //    }
        //    return bytes.ToArray();
        //}

        //private Type GetDominantType(string[] tokens, ref int index)
        //{
        //    Type type = null;
        //    string token = tokens[index];
        //    if (ScriptBase.IsLogicOperator(token))
        //    {
        //        index++;
        //        Type leftType = GetDominantType(tokens, ref index);
        //        Type rightType = GetDominantType(tokens, ref index);

        //        if (leftType != null && rightType != null)
        //        {
        //            // See if comparable
        //            if (ScriptBase.IsMathTypeCompatible(leftType, rightType))
        //            {
        //                type = typeof(bool);
        //            }
        //        }
        //    }
        //    else if (ScriptBase.IsOperator(token))
        //    {
        //        index++;
        //        Type leftType = GetDominantType(tokens, ref index);
        //        Type rightType = GetDominantType(tokens, ref index);

        //        if (leftType != null && rightType != null)
        //        {
        //            // See if comparable
        //            if (ScriptBase.IsMathTypeCompatible(leftType, rightType))
        //            {
        //                if (leftType == typeof(float) || rightType == typeof(float))
        //                {
        //                    type = typeof(float);
        //                }
        //                else
        //                {
        //                    type = leftType;    // Just chosen arbitrarily
        //                }
        //            }
        //            else if (leftType == typeof(string) || rightType == typeof(string))
        //            {
        //                // Gives support for adding other values to strings.
        //                type = typeof(string);
        //            }
        //        }
        //    }
        //    else if(ScriptBase.IsFunction(token))
        //    {
        //        string name = Regex.Match(token, ScriptBase.SCRIPT_NAMES_ALLOWEDNAMES_REGEX + "(?=\\(.*?\\))").Value;
        //        string argumentString = Regex.Match(token, "(?<=\\().*?(?=\\))").Value;

        //        if(this.headerByName.ContainsKey(name))
        //        {
        //            type = this.headerByName[name].returnType;
        //        }
        //        else
        //        {
        //            this.ReportError("Function (" + name + ") could not be found.");
        //        }
        //    }
        //    else
        //    {
        //        index++;
        //        Type t = ScriptBase.GetTokenType(token);
        //        if (t == null)
        //        {
        //            if (this.variableByName.ContainsKey(token))
        //            {
        //                Variable v = variableByName[token];
        //                t = v.type;
        //            }
        //            else
        //            {
        //                this.ReportError("Variable (" + token + ") could not be found.");
        //            }
        //        }
        //        type = t;
        //    }

        //    return type;
        //}

        private Type GetDominantType(Token[] tokens, ref int index)
        {
            Type type = null;
            Token token = tokens[index++];
            if (token.isLogicOperator)
            {
                Type leftType = GetDominantType(tokens, ref index);
                Type rightType = GetDominantType(tokens, ref index);

                if (leftType != null && rightType != null)
                {
                    // See if comparable
                    if (ScriptBase.IsMathTypeCompatible(leftType, rightType))
                    {
                        type = typeof(bool);
                    }
                }
            }
            else if (token.isArithmeticOperator)
            {
                Type leftType = GetDominantType(tokens, ref index);
                Type rightType = GetDominantType(tokens, ref index);

                if (leftType != null && rightType != null)
                {
                    // See if comparable
                    if (ScriptBase.IsMathTypeCompatible(leftType, rightType))
                    {
                        if (leftType == typeof(float) || rightType == typeof(float))
                        {
                            type = typeof(float);
                        }
                        else
                        {
                            type = leftType;    // Just chosen arbitrarily
                        }
                    }
                    else if (leftType == typeof(string) || rightType == typeof(string))
                    {
                        // Gives support for adding other values to strings.
                        type = typeof(string);
                    }
                }
            }
            else if (token.isFunction || token.isOperand)
            {
                type = token.operandType;
            }
            else
            {
                throw new InvalidOperationException("Should not get here: Token not recognized.");
            }
            return type;
        }

        private Token[] InfixToPrefix(Token[] tokens)
        {
            Stack<Token> prefix = new Stack<Token>();
            Stack<Token> operatorStack = new Stack<Token>();

            for (int i = tokens.Length - 1; i >= 0; i--)
            {
                if (tokens[i].isOperand)
                {
                    prefix.Push(tokens[i]);
                }
                else
                {
                    if (tokens[i].isGroupingLeft)
                    {
                        while (operatorStack.Count > 0)
                        {
                            Token pop = operatorStack.Pop();
                            if (pop.isGroupingRight)
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
                        int hiearchyStack = operatorStack.Count > 0 ? operatorStack.Peek().hiearchyValue : 0;
                        int hiearchyToken = operatorStack.Count > 0 ? tokens[i].hiearchyValue : 0;
                        while (operatorStack.Count > 0 && hiearchyStack < hiearchyToken && operatorStack.Peek().isGroupingRight == false)
                        {
                            Token pop = operatorStack.Pop();
                            prefix.Push(pop);
                        }
                        operatorStack.Push(tokens[i]);
                    }
                }
            }

            while (operatorStack.Count > 0)
            {
                prefix.Push(operatorStack.Pop());
            }

            return prefix.ToArray();
        }

        //private string[] InfixToPrefix(string[] tokens)
        //{
        //    Stack<string> prefix = new Stack<string>();
        //    Stack<string> operatorStack = new Stack<string>();

        //    for (int i = tokens.Length - 1; i >= 0; i--)
        //    {
        //        if (tokens[i].Length > 0 && tokens[i] != " ")
        //        {
        //            if (ScriptBase.IsOperand(tokens[i]))
        //            {
        //                prefix.Push(tokens[i]);
        //            }
        //            else
        //            {
        //                if (tokens[i] == ScriptBase.SCRIPT_OPERATOR_GROUPLEFT)
        //                {
        //                    while (operatorStack.Count > 0)// && ScriptConstants.OperatorHiearchy(operatorStack.Peek()) >= ScriptConstants.OperatorHiearchy(tokens[i]))
        //                    {
        //                        string pop = operatorStack.Pop();
        //                        if (pop == ScriptBase.SCRIPT_OPERATOR_GROUPRIGHT)
        //                        {
        //                            break;
        //                        }
        //                        else
        //                        {
        //                            prefix.Push(pop);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    int hiearchyStack = operatorStack.Count > 0 ? ScriptBase.OperatorHiearchy(operatorStack.Peek()) : 0;
        //                    int hiearchyToken = operatorStack.Count > 0 ? ScriptBase.OperatorHiearchy(tokens[i]) : 0;
        //                    while (operatorStack.Count > 0 && hiearchyStack < hiearchyToken && operatorStack.Peek() != ScriptBase.SCRIPT_OPERATOR_GROUPRIGHT)
        //                    {
        //                        string pop = operatorStack.Pop();
        //                        prefix.Push(pop);
        //                    }
        //                    operatorStack.Push(tokens[i]);
        //                }
        //            }
        //        }
        //    }

        //    while (operatorStack.Count > 0)
        //    {
        //        prefix.Push(operatorStack.Pop());
        //    }

        //    return prefix.ToArray();
        //}

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

        /// <summary>
        /// Tokenises the argument string. All tokens are checked for validness.
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        private Token[] TokenizeExpression(string argument)
        {
            string splitter = "(" + ScriptBase.SCRIPT_STRING_TOKEN + "[^" + ScriptBase.SCRIPT_STRING_TOKEN + "]*" + ScriptBase.SCRIPT_STRING_TOKEN
                //+ "|" + ScriptBase.SCRIPT_NAMES_ALLOWEDNAMES_REGEX + "\\(.*\\)(?=>(.*\\())"
                + "|" + ScriptBase.SCRIPT_NAMES_ALLOWEDNAMES_REGEX + @"\((?>\((?<D>)|\)(?<-D>)|.?)*(?(D)(?!))\)"
                + "|" + ScriptBase.SCRIPT_OPERATOR_GROUPLEFT_REGEX + "|" + ScriptBase.SCRIPT_OPERATOR_GROUPRIGHT_REGEX
                + "|" + ScriptBase.SCRIPT_OPERATOR_ADD_REGEX + "|" + ScriptBase.SCRIPT_OPERATOR_SUBTRACT_REGEX
                + "|" + ScriptBase.SCRIPT_OPERATOR_MULTIPLY_REGEX + "|" + ScriptBase.SCRIPT_OPERATOR_DIVIDE_REGEX
                + "|" + ScriptBase.SCRIPT_OPERATOR_POWER_REGEX + "|" + ScriptBase.SCRIPT_OPERATOR_MODULO_REGEX
                + "|" + ScriptBase.SCRIPT_OPERATOR_LOGIC_SMALLER_REGEX + "|" + ScriptBase.SCRIPT_OPERATOR_LOGIC_SMALLEREQUAL_REGEX
                + "|" + ScriptBase.SCRIPT_OPERATOR_LOGIC_LARGER_REGEX + "|" + ScriptBase.SCRIPT_OPERATOR_LOGIC_LARGEREQUAL_REGEX
                + "|" + ScriptBase.SCRIPT_OPERATOR_LOGIC_EQUAL_REGEX + "|" + ScriptBase.SCRIPT_OPERATOR_LOGIC_NOTEQUAL_REGEX + ")"
                + "|" + @"\d+($|\.\d+)" + "|" + ScriptBase.SCRIPT_NAMES_ALLOWEDNAMES_REGEX;

            MatchCollection matches = Regex.Matches(argument, splitter);
            //string[] tokenStrings = Regex.Split(argument, splitter);
            LinkedList<Token> tokenList = new LinkedList<Token>();

            foreach(Match m in matches)
            {
                string s = m.Value;
                if (s.Length > 0 && s.Equals(" ") == false)
                {
                    Token token = new Token();
                    token.text = s;
                    switch(s)
                    {
                        case ScriptBase.SCRIPT_OPERATOR_GROUPLEFT:
                            token.isGroupingLeft = true;
                            token.hiearchyValue = 1;
                            break;
                        case ScriptBase.SCRIPT_OPERATOR_GROUPRIGHT:
                            token.isGroupingRight = true;
                            token.hiearchyValue = 1;
                            break;
                        case ScriptBase.SCRIPT_OPERATOR_POWER:
                            token.isArithmeticOperator = true;
                            token.hiearchyValue = 3;
                            break;
                        case ScriptBase.SCRIPT_OPERATOR_MULTIPLY:
                        case ScriptBase.SCRIPT_OPERATOR_DIVIDE:
                        case ScriptBase.SCRIPT_OPERATOR_MODULO:
                            token.isArithmeticOperator = true;
                            token.hiearchyValue = 4;
                            break;
                        case ScriptBase.SCRIPT_OPERATOR_ADD:
                        case ScriptBase.SCRIPT_OPERATOR_SUBTRACT:
                            token.isArithmeticOperator = true;
                            token.hiearchyValue = 5;
                            break;
                        case ScriptBase.SCRIPT_OPERATOR_LOGIC_SMALLER:
                        case ScriptBase.SCRIPT_OPERATOR_LOGIC_SMALLEREQUAL:
                        case ScriptBase.SCRIPT_OPERATOR_LOGIC_LARGER:
                        case ScriptBase.SCRIPT_OPERATOR_LOGIC_LARGEREQUAL:
                            token.isLogicOperator = true;
                            token.hiearchyValue = 7;
                            break;
                        case ScriptBase.SCRIPT_OPERATOR_LOGIC_EQUAL:
                        case ScriptBase.SCRIPT_OPERATOR_LOGIC_NOTEQUAL:
                            token.isLogicOperator = true;
                            token.hiearchyValue = 8;
                            break;
                        default:
                            Match functionMatch = Regex.Match(s, ScriptBase.SCRIPT_NAMES_ALLOWEDNAMES_REGEX + "\\(.*?\\)");
                            if(functionMatch.Success)
                            {
                                token.isFunction = true;
                                string name = Regex.Match(token.text, ScriptBase.SCRIPT_NAMES_ALLOWEDNAMES_REGEX + "(?=\\(.*?\\))").Value;
                                string argumentString = Regex.Match(token.text, @"(?<=(.*?\()).*(?=(\)$))").Value;

                                if (this.headerByName.ContainsKey(name))
                                {
                                    token.operandType = this.headerByName[name].returnType;
                                    token.functionName = name;
                                    string[] argumentStrings = argumentString.Split(new string[]{ScriptBase.SCRIPT_ARGUMENT_SEPARATOR}, StringSplitOptions.RemoveEmptyEntries);
                                    token.functionArguments = new Token[argumentStrings.Length][];
                                    for (int i = 0; i < argumentStrings.Length; i++)
                                    {
                                        token.functionArguments[i] = TokenizeExpression(argumentStrings[i].Trim());
                                        if (token.functionArguments[i]  == null)
                                        {
                                            return null;
                                        }
                                    }
                                }
                                else
                                {
                                    this.ReportError("Function (" + name + ") could not be found.");
                                    return null;
                                }
                            }
                            else
                            {
                                token.isOperand = true;
                                token.operandType = ScriptBase.GetTokenType(s);
                                if (token.operandType == null)
                                {
                                    if (this.variableByName.ContainsKey(s))
                                    {
                                        token.operandType = this.variableByName[s].type;
                                    }
                                    else
                                    {
                                        this.ReportError("Variable (" + token.text + ") could not be found.");
                                        return null;
                                    }
                                }
                            }
                            break;
                    }

                    tokenList.AddLast(token);
                }
            }

            return tokenList.ToArray();
        }


        //    if (Regex.IsMatch(token, "(!)"))
        //        return 2;
        //    if (Regex.IsMatch(token, "(.......)"))
        //        return 9;
        //    if (Regex.IsMatch(token, "&&"))
        //        return 12;
        //    if (Regex.IsMatch(token, "||"))
        //        return 13;

        //    return 14;
        //}




        //private string[] TokenizeExpression(string argument)
        //{
        //    string splitter = "(" + ScriptBase.SCRIPT_STRING_TOKEN + "[^" + ScriptBase.SCRIPT_STRING_TOKEN + "]*" + ScriptBase.SCRIPT_STRING_TOKEN
        //        + "|" + ScriptBase.SCRIPT_NAMES_ALLOWEDNAMES_REGEX + "\\(.*?\\)"
        //        + "|" + ScriptBase.SCRIPT_OPERATOR_GROUPLEFT_REGEX + "|" + ScriptBase.SCRIPT_OPERATOR_GROUPRIGHT_REGEX
        //        + "|" + ScriptBase.SCRIPT_OPERATOR_ADD_REGEX + "|" + ScriptBase.SCRIPT_OPERATOR_SUBTRACT_REGEX
        //        + "|" + ScriptBase.SCRIPT_OPERATOR_MULTIPLY_REGEX + "|" + ScriptBase.SCRIPT_OPERATOR_DIVIDE_REGEX
        //        + "|" + ScriptBase.SCRIPT_OPERATOR_POWER_REGEX + "|" + ScriptBase.SCRIPT_OPERATOR_MODULO_REGEX
        //        + "|" + ScriptBase.SCRIPT_OPERATOR_LOGIC_SMALLER_REGEX + "|" + ScriptBase.SCRIPT_OPERATOR_LOGIC_SMALLEREQUAL_REGEX
        //        + "|" + ScriptBase.SCRIPT_OPERATOR_LOGIC_LARGER_REGEX + "|" + ScriptBase.SCRIPT_OPERATOR_LOGIC_LARGEREQUAL_REGEX
        //        + "|" + ScriptBase.SCRIPT_OPERATOR_LOGIC_EQUAL_REGEX + "|" + ScriptBase.SCRIPT_OPERATOR_LOGIC_NOTEQUAL_REGEX + "|\\s)";

        //    string[] tokens = Regex.Split(argument, splitter).Where(t => t.Length > 0 && t != " ").ToArray();
        //    return tokens;
        //}
    }
}