namespace MonoKle.Script
{
    using MonoKle.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal class VirtualMachine
    {
        private int pc;
        private byte[] code;
        private bool error;
        private string scriptName;
        private Type returnType;
        private object[] variables;

        public Result RunScript(Script script, object[] arguments)
        {
            this.Reset(script, arguments);

            while(this.error == false && this.pc < code.Length)
            {
                // Read code
                byte opCode = code[pc++];

                switch(opCode)
                {
                    case ScriptBase.OP_RETURN_VOID:
                        return this.CreateResult(null);
                    case ScriptBase.OP_RETURN_VALUE:
                        {
                            byte typeByte = code[pc++];
                            Type type = ScriptBase.ByteToType(typeByte);

                            if(ScriptBase.IsReturnTypeCompatible(type, this.returnType))
                            {
                                byte firstOperation = code[pc++];
                                return this.CreateResult(this.CalculateExpression(firstOperation));
                            }
                            else
                            {
                                this.ReportError("Encountered mismatched return type.");
                            }
                        } break;
                    case ScriptBase.OP_PRINT:
                        {
                            byte firstOperation = code[pc++];
                            MonoKleGame.Console.WriteLine(this.CalculateExpression(firstOperation).ToString());
                        } break;
                    case ScriptBase.OP_INIVAR:
                    case ScriptBase.OP_SETVAR:
                        {
                            byte number = code[pc++];
                            byte firstOperation = code[pc++];
                            this.variables[number] = this.CalculateExpression(firstOperation);
                        } break;
                    default:
                        this.ReportError("Encountered unrecognized operation.");
                        break;
                }
            }

            return Result.Fail;
        }

        private Result CreateResult(object value)
        {
            return new Result(this.error == false, this.returnType, value);
        }

        private object CalculateExpression(byte initialOperation)
        {
            switch(initialOperation)
            {
                case ScriptBase.OP_SMALLER:
                    {
                        byte lhOP = this.code[pc++];
                        object lhRet = CalculateExpression(lhOP);

                        byte rhOP = this.code[pc++];
                        object rhRet = CalculateExpression(rhOP);

                        if (rhRet == null || lhRet == null)
                        {
                            this.ReportError("Logical less-than operation failed.");
                        }
                        else
                        {
                            return SmallerObject(lhRet, rhRet);
                        }
                    } break;
                case ScriptBase.OP_SMALLEREQUAL:
                    {
                        byte lhOP = this.code[pc++];
                        object lhRet = CalculateExpression(lhOP);

                        byte rhOP = this.code[pc++];
                        object rhRet = CalculateExpression(rhOP);

                        if (rhRet == null || lhRet == null)
                        {
                            this.ReportError("Logical less-than-or-equal operation failed.");
                        }
                        else
                        {
                            return SmallerEqualObject(lhRet, rhRet);
                        }
                    } break;
                case ScriptBase.OP_LARGER:
                    {
                        byte lhOP = this.code[pc++];
                        object lhRet = CalculateExpression(lhOP);

                        byte rhOP = this.code[pc++];
                        object rhRet = CalculateExpression(rhOP);

                        if (rhRet == null || lhRet == null)
                        {
                            this.ReportError("Logical greater-than operation failed.");
                        }
                        else
                        {
                            return GreaterObject(lhRet, rhRet);
                        }
                    } break;
                case ScriptBase.OP_LARGEREQUAL:
                    {
                        byte lhOP = this.code[pc++];
                        object lhRet = CalculateExpression(lhOP);

                        byte rhOP = this.code[pc++];
                        object rhRet = CalculateExpression(rhOP);

                        if (rhRet == null || lhRet == null)
                        {
                            this.ReportError("Logical greater-than-or-equal operation failed.");
                        }
                        else
                        {
                            return GreaterEqualObject(lhRet, rhRet);
                        }
                    } break;
                case ScriptBase.OP_EQUAL:
                    {
                        byte lhOP = this.code[pc++];
                        object lhRet = CalculateExpression(lhOP);

                        byte rhOP = this.code[pc++];
                        object rhRet = CalculateExpression(rhOP);

                        if (rhRet == null || lhRet == null)
                        {
                            this.ReportError("Logical equality operation failed.");
                        }
                        else
                        {
                            return lhRet.Equals(rhRet);
                        }
                    }break;
                case ScriptBase.OP_ADD:
                    {
                        byte lhOP = this.code[pc++];
                        object lhRet = CalculateExpression(lhOP);

                        byte rhOP = this.code[pc++];
                        object rhRet = CalculateExpression(rhOP);

                        if (rhRet == null || lhRet == null)
                        {
                            this.ReportError("Addition operation failed.");
                        }
                        else
                        {
                            return AddObject(lhRet, rhRet);
                        }
                    } break;
                case ScriptBase.OP_SUBTRACT:
                    {
                        byte lhOP = this.code[pc++];
                        object lhRet = CalculateExpression(lhOP);

                        byte rhOP = this.code[pc++];
                        object rhRet = CalculateExpression(rhOP);

                        if (rhRet == null || lhRet == null)
                        {
                            this.ReportError("Subtract operation failed.");
                        }
                        else
                        {
                            return SubtractObject(lhRet, rhRet);
                        }
                    } break;
                case ScriptBase.OP_DIVIDE:
                    {
                        byte lhOP = this.code[pc++];
                        object lhRet = CalculateExpression(lhOP);

                        byte rhOP = this.code[pc++];
                        object rhRet = CalculateExpression(rhOP);

                        if (rhRet == null || lhRet == null)
                        {
                            this.ReportError("Division operation failed.");
                        }
                        else
                        {
                            return DivideObject(lhRet, rhRet);
                        }
                    } break;
                case ScriptBase.OP_MULTIPLY:
                    {
                        byte lhOP = this.code[pc++];
                        object lhRet = CalculateExpression(lhOP);

                        byte rhOP = this.code[pc++];
                        object rhRet = CalculateExpression(rhOP);

                        if (rhRet == null || lhRet == null)
                        {
                            this.ReportError("Multiply operation failed.");
                        }
                        else
                        {
                            return MultiplyObject(lhRet, rhRet);
                        }
                    } break;
                case ScriptBase.OP_POWER:
                    {
                        byte lhOP = this.code[pc++];
                        object lhRet = CalculateExpression(lhOP);

                        byte rhOP = this.code[pc++];
                        object rhRet = CalculateExpression(rhOP);

                        if (rhRet == null || lhRet == null)
                        {
                            this.ReportError("Power operation failed.");
                        }
                        else
                        {
                            return PowerObject(lhRet, rhRet);
                        }
                    } break;
                case ScriptBase.OP_CONST_BOOL:
                    pc += sizeof(bool);
                    return ByteConverter.ToBoolean(this.code, pc - sizeof(bool));
                case ScriptBase.OP_CONST_INT:
                    pc += sizeof(int);
                    return ByteConverter.ToInt32(this.code, pc - sizeof(int));
                case ScriptBase.OP_CONST_FLOAT:
                    pc += sizeof(float);
                    return ByteConverter.ToFloat32(this.code, pc - sizeof(float));
                case ScriptBase.OP_CONST_STRING:
                    {
                        int bytesRead = 0;
                        string s = ByteConverter.ToString(this.code, pc, out bytesRead);
                        pc += bytesRead;
                        return s.Remove(0, 1).Remove(s.Length - 2, 1);  // Removes the string indication characters
                    }
                case ScriptBase.OP_GETVAR:
                    return this.variables[this.code[pc++]];
                default:
                    {
                        this.ReportError("Encountered unrecognized expression operation.");
                    }break;
            }

            return null;
        }

        private object GreaterObject(object a, object b)
        {
            if(a.GetType() == typeof(int))
            {
                if(b.GetType() == typeof(int))
                {
                    return (int)a > (int)b;
                }
                else if(b.GetType() == typeof(float))
                {
                    return (int)a > (float)b;
                }
            }
            else if(a.GetType() == typeof(float))
            {
                if (b.GetType() == typeof(int))
                {
                    return (float)a > (int)b;
                }
                else if (b.GetType() == typeof(float))
                {
                    return (float)a > (float)b;
                }
            }
            return null;
        }

        private object GreaterEqualObject(object a, object b)
        {
            if (a.GetType() == typeof(int))
            {
                if (b.GetType() == typeof(int))
                {
                    return (int)a >= (int)b;
                }
                else if (b.GetType() == typeof(float))
                {
                    return (int)a >= (float)b;
                }
            }
            else if (a.GetType() == typeof(float))
            {
                if (b.GetType() == typeof(int))
                {
                    return (float)a >= (int)b;
                }
                else if (b.GetType() == typeof(float))
                {
                    return (float)a >= (float)b;
                }
            }
            return null;
        }

        private object SmallerObject(object a, object b)
        {
            if (a.GetType() == typeof(int))
            {
                if (b.GetType() == typeof(int))
                {
                    return (int)a < (int)b;
                }
                else if (b.GetType() == typeof(float))
                {
                    return (int)a < (float)b;
                }
            }
            else if (a.GetType() == typeof(float))
            {
                if (b.GetType() == typeof(int))
                {
                    return (float)a < (int)b;
                }
                else if (b.GetType() == typeof(float))
                {
                    return (float)a < (float)b;
                }
            }
            return null;
        }

        private object SmallerEqualObject(object a, object b)
        {
            if (a.GetType() == typeof(int))
            {
                if (b.GetType() == typeof(int))
                {
                    return (int)a <= (int)b;
                }
                else if (b.GetType() == typeof(float))
                {
                    return (int)a <= (float)b;
                }
            }
            else if (a.GetType() == typeof(float))
            {
                if (b.GetType() == typeof(int))
                {
                    return (float)a <= (int)b;
                }
                else if (b.GetType() == typeof(float))
                {
                    return (float)a <= (float)b;
                }
            }
            return null;
        }

        private object SubtractObject(object a, object b)
        {
            if (a.GetType() == typeof(int))
            {
                if (b.GetType() == typeof(int))
                {
                    return (int)a - (int)b;
                }
                else if (b.GetType() == typeof(float))
                {
                    return (int)a - (float)b;
                }
            }
            else if (a.GetType() == typeof(float))
            {
                if (b.GetType() == typeof(int))
                {
                    return (float)a - (int)b;
                }
                else if (b.GetType() == typeof(float))
                {
                    return (float)a - (float)b;
                }
            }
            return null;
        }

        private object PowerObject(object a, object b)
        {
            if (a.GetType() == typeof(int))
            {
                if (b.GetType() == typeof(int))
                {
                    return (int)Math.Pow((int)a, (int)b);
                }
                else if (b.GetType() == typeof(float))
                {
                    return (float)Math.Pow((int)a, (float)b);
                }
            }
            else if (a.GetType() == typeof(float))
            {
                if (b.GetType() == typeof(int))
                {
                    return (float)Math.Pow((float)a, (int)b);
                }
                else if (b.GetType() == typeof(float))
                {
                    return (float)Math.Pow((float)a, (float)b);
                }
            }
            return null;
        }

        private object MultiplyObject(object a, object b)
        {
            if (a.GetType() == typeof(int))
            {
                if (b.GetType() == typeof(int))
                {
                    return (int)a * (int)b;
                }
                else if (b.GetType() == typeof(float))
                {
                    return (int)a * (float)b;
                }
            }
            else if (a.GetType() == typeof(float))
            {
                if (b.GetType() == typeof(int))
                {
                    return (float)a * (int)b;
                }
                else if (b.GetType() == typeof(float))
                {
                    return (float)a * (float)b;
                }
            }
            return null;
        }

        private object DivideObject(object a, object b)
        {
            if (a.GetType() == typeof(int))
            {
                if (b.GetType() == typeof(int))
                {
                    return (int)a / (int)b;
                }
                else if (b.GetType() == typeof(float))
                {
                    return (int)a / (float)b;
                }
            }
            else if (a.GetType() == typeof(float))
            {
                if (b.GetType() == typeof(int))
                {
                    return (float)a / (int)b;
                }
                else if (b.GetType() == typeof(float))
                {
                    return (float)a / (float)b;
                }
            }
            return null;
        }

        private object AddObject(object a, object b)
        {
            if (a.GetType() == typeof(int))
            {
                if (b.GetType() == typeof(int))
                {
                    return (int)a + (int)b;
                }
                else if (b.GetType() == typeof(float))
                {
                    return (int)a + (float)b;
                }
            }
            else if (a.GetType() == typeof(float))
            {
                if (b.GetType() == typeof(int))
                {
                    return (float)a + (int)b;
                }
                else if (b.GetType() == typeof(float))
                {
                    return (float)a + (float)b;
                }
            }
            else if (a.GetType() == typeof(string) || b.GetType() == typeof(string))
            {
                return a.ToString() + b.ToString();
            }

            return null;
        }

        private void ReportError(string message)
        {
            MonoKleGame.Logger.AddLog("[" + this.scriptName + "] Script runtime error: " + message, Logging.LogLevel.Error);
            this.error = true;
        }

        private void Reset(Script script, object[] arguments)
        {
            this.code = script.ByteCode;
            this.pc = 0;
            this.error = false;
            this.scriptName = script.Name;
            this.returnType = script.ReturnType;
            this.variables = new object[script.VariableAmount];

            if (arguments.Length == script.Arguments.Length)
            {
                for (int i = 0; i < arguments.Length; i++)
                {
                    if (arguments[i] != null)
                    {
                        if (script.Arguments[i] == arguments[i].GetType())
                        {
                            variables[i] = arguments[i];
                        }
                        else
                        {
                            this.ReportError("Input argument " + i + " not of correct type. Was " + arguments[i].GetType() + ", expected " + script.Arguments[i]);
                        }
                    }
                    else
                    {
                        this.ReportError("Input argument " + i + " was null. Null arguments are not supported!");
                    }
                }
            }
            else
            {
                this.ReportError("Amount of arguments was not correct. Was " + arguments.Length + ", expected " + script.Arguments.Length);
            }
        }
    }

}
