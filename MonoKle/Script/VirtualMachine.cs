namespace MonoKle.Script
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal class VirtualMachine
    {


        private int pc;
        private byte[] code;
        private bool error;
        private bool done;
        private string scriptName;
        private Type returnType;

        public Result RunScript(Script script)
        {
            this.Reset(script);

            while(this.error == false && this.pc < code.Length)
            {
                // Read code
                byte opCode = code[pc++];

                switch(opCode)
                {
                    case ScriptConstants.OP_RETURN_VOID:
                        return this.CreateResult(null);
                    case ScriptConstants.OP_RETURN_VALUE:
                        {
                            byte typeByte = code[pc++];
                            Type type = ScriptConstants.ByteToType(typeByte);
                            if(type == script.ReturnType)
                            {
                                byte firstOperation = code[pc++];
                                return this.CreateResult(this.CalculateExpression(firstOperation));
                            }
                            else
                            {
                                this.ReportError("Encountered mismatched return type.");
                            }
                        }break;
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
                case ScriptConstants.OP_SMALLER:
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
                case ScriptConstants.OP_SMALLEREQUAL:
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
                case ScriptConstants.OP_LARGER:
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
                case ScriptConstants.OP_LARGEREQUAL:
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
                case ScriptConstants.OP_EQUAL:
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
                case ScriptConstants.OP_ADD:
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
                case ScriptConstants.OP_SUBTRACT:
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
                case ScriptConstants.OP_DIVIDE:
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
                case ScriptConstants.OP_MULTIPLY:
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
                case ScriptConstants.OP_POWER:
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
                case ScriptConstants.OP_CONST_BOOL:
                    return Utilities.ByteConverter.ToBoolean(this.code, pc++);
                case ScriptConstants.OP_CONST_INT:
                    pc += 4;
                    return Utilities.ByteConverter.ToInt32(this.code, pc - 4);
                case ScriptConstants.OP_CONST_FLOAT:
                    pc += 4;
                    return Utilities.ByteConverter.ToFloat32(this.code, pc - 4);
                case ScriptConstants.OP_CONST_STRING:
                    this.ReportError("Not implemented!");
                    break;
                default:
                    {
                        this.ReportError("Encountered unrecognized operation in expression.");
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
            MonoKleGame.Logger.AddLog("Script runtime error in " + this.scriptName + ": " + message, Logging.LogLevel.Error);
            this.error = true;
        }

        private void Reset(Script script)
        {
            this.code = script.ByteCode;
            this.pc = 0;
            this.error = false;
            this.scriptName = script.Name;
            this.returnType = script.ReturnType;
        }
    }

}
