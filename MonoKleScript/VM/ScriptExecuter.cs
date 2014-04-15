namespace MonoKleScript.VM
{
    using MonoKle.Utilities;
    using MonoKleScript.Script;
    using MonoKleScript.VM.Event;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    // TODO: Remove error reporting and trust compiler! :)

    internal class ScriptExecuter
    {
        private int pc;
        private byte[] code;
        private bool error;
        private string scriptName;
        private Type returnType;
        private Dictionary<byte, object> variableByID = new Dictionary<byte, object>();
        private Dictionary<string, ByteScript> scriptByName;

        public event RuntimeErrorEventHandler RuntimeError;

        public event PrintEventHandler Print;

        private void OnPrint(string message)
        {
            var l = Print;
            if(l != null)
            {
                l(this, new PrintEventArgs(message));
            }
        }

        private void OnRuntimeError(string message)
        {
            var l = RuntimeError;
            if (l != null)
            {
                l(this, new RuntimeErrorEventArgs(message));
            }
        }

        public Result RunScript(ByteScript script, object[] arguments, Dictionary<string, ByteScript> scriptByName)
        {
            this.Reset(script, arguments, scriptByName);

            while(this.error == false && this.pc < code.Length)
            {
                // Read code
                byte opCode = code[pc++];

                switch(opCode)
                {
                    case Constants.OP_RETURN_VOID:
                        return this.CreateResult(null);
                    case Constants.OP_RETURN_VALUE:
                        {
                            byte firstOperation = code[pc++];
                            return this.CreateResult(this.CalculateExpression(firstOperation));
                        }
                    case Constants.OP_PRINT:
                        {
                            byte firstOperation = code[pc++];
                            this.OnPrint(this.CalculateExpression(firstOperation).ToString());
                        } break;
                    case Constants.OP_INIVAR:
                        {
                            byte id = code[pc++];
                            byte firstOperation = code[pc++];
                            this.variableByID.Add(id, this.CalculateExpression(firstOperation));
                        } break;
                    case Constants.OP_SETVAR:
                        {
                            byte id = code[pc++];
                            byte firstOperation = code[pc++];
                            this.variableByID[id] = this.CalculateExpression(firstOperation);
                        } break;
                    case Constants.OP_REMVAR:
                        {
                            this.variableByID.Remove(code[pc++]);
                        } break;
                    case Constants.OP_CALLFUNCTION:
                        {
                            this.CallFunction();
                        } break;
                    case Constants.OP_IF:
                        {
                            int jmpIfFail = ByteConverter.ToInt32(code, pc);
                            pc += sizeof(int);
                            byte firstOperation = code[pc++];
                            bool expression = (bool)this.CalculateExpression(firstOperation);
                            if(expression == false)
                            {
                                this.pc = jmpIfFail;
                            }
                        } break;
                    case Constants.OP_JUMP:
                        {
                            this.pc = ByteConverter.ToInt32(code, pc);
                        } break;
                    default:
                        this.ReportError("Encountered unrecognized operation.");
                        break;
                }
            }

            return this.CreateResult(null);
        }

        private Result CreateResult(object value)
        {
            if(this.error)
            {
                return Result.Fail;
            }
            else
            {
                return new Result(true, this.returnType, value);
            }
        }

        private object CalculateExpression(byte initialOperation)
        {
            switch(initialOperation)
            {
                case Constants.OP_OR:
                    {
                        byte lhOP = this.code[pc++];
                        object lhRet = CalculateExpression(lhOP);

                        byte rhOP = this.code[pc++];
                        object rhRet = CalculateExpression(rhOP);

                        return ((bool)lhRet) || ((bool)rhRet);
                    }
                case Constants.OP_AND:
                    {
                        byte lhOP = this.code[pc++];
                        object lhRet = CalculateExpression(lhOP);

                        byte rhOP = this.code[pc++];
                        object rhRet = CalculateExpression(rhOP);

                        return ((bool)lhRet) && ((bool)rhRet);
                    }
                case Constants.OP_NOT:
                    {
                        byte lhOP = this.code[pc++];
                        object lhRet = CalculateExpression(lhOP);
                        return !(bool)lhRet;
                    }
                case Constants.OP_SMALLER:
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
                case Constants.OP_SMALLEREQUAL:
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
                case Constants.OP_LARGER:
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
                case Constants.OP_LARGEREQUAL:
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
                case Constants.OP_EQUAL:
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
                case Constants.OP_NOTEQUAL:
                    {
                        byte lhOP = this.code[pc++];
                        object lhRet = CalculateExpression(lhOP);

                        byte rhOP = this.code[pc++];
                        object rhRet = CalculateExpression(rhOP);

                        return lhRet.Equals(rhRet) == false;
                    }
                case Constants.OP_ADD:
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
                case Constants.OP_SUBTRACT:
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
                case Constants.OP_DIVIDE:
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
                case Constants.OP_MULTIPLY:
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
                case Constants.OP_NEGATE:
                    {
                        byte lhOP = this.code[pc++];
                        object lhRet = CalculateExpression(lhOP);
                        return this.SubtractObject(0, lhRet);
                    }
                case Constants.OP_POWER:
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
                case Constants.OP_MODULO:
                    {
                        byte lhOP = this.code[pc++];
                        object lhRet = CalculateExpression(lhOP);

                        byte rhOP = this.code[pc++];
                        object rhRet = CalculateExpression(rhOP);

                        return this.ModuloObject(lhRet, rhRet);
                    }
                case Constants.OP_CONST_BOOL:
                    pc += sizeof(bool);
                    return ByteConverter.ToBoolean(this.code, pc - sizeof(bool));
                case Constants.OP_CONST_INT:
                    pc += sizeof(int);
                    return ByteConverter.ToInt32(this.code, pc - sizeof(int));
                case Constants.OP_CONST_FLOAT:
                    pc += sizeof(float);
                    return ByteConverter.ToFloat32(this.code, pc - sizeof(float));
                case Constants.OP_CONST_STRING:
                    {
                        int bytesRead = 0;
                        string s = ByteConverter.ToString(this.code, pc, out bytesRead);
                        pc += bytesRead;
                        return s.Remove(0, 1).Remove(s.Length - 2, 1);  // Removes the string indication characters
                    }
                case Constants.OP_GETVAR:
                    return this.variableByID[this.code[pc++]];
                case Constants.OP_CALLFUNCTION:
                    return this.CallFunction().returnValue;
                default:
                    {
                        this.ReportError("Encountered unrecognized expression operation.");
                    }break;
            }

            return null;
        }

        private Result CallFunction()
        {
            int br = 0;
            string name = ByteConverter.ToString(this.code, pc, out br);
            pc += br;
            byte nArgs = this.code[pc++];

            object[] args = new object[nArgs];
            for (int i = 0; i < args.Length; i++)
            {
                args[i] = this.CalculateExpression(this.code[pc++]);
            }

            ScriptExecuter e = new ScriptExecuter();
            e.Print += delegate (object o, PrintEventArgs pe){ this.OnPrint(pe.Message); };
            e.RuntimeError += delegate (object o, RuntimeErrorEventArgs re) { this.OnRuntimeError(re.Message); };
            Result result = e.RunScript(scriptByName[name], args, this.scriptByName);
            e.RemoveEventSubscribers();
            return result;
        }

        internal void RemoveEventSubscribers()
        {
            this.RuntimeError = null;
            this.Print = null;
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

        private object ModuloObject(object a, object b)
        {
            if (a.GetType() == typeof(int))
            {
                if (b.GetType() == typeof(int))
                {
                    return (int)a % (int)b;
                }
                else if (b.GetType() == typeof(float))
                {
                    return (int)a % (float)b;
                }
            }
            else if (a.GetType() == typeof(float))
            {
                if (b.GetType() == typeof(int))
                {
                    return (float)a % (int)b;
                }
                else if (b.GetType() == typeof(float))
                {
                    return (float)a % (float)b;
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
            this.OnRuntimeError("[" + this.scriptName + "] Runtime error: " + message);
            this.error = true;
        }

        private void Reset(ByteScript script, object[] arguments, Dictionary<string, ByteScript> scriptByName)
        {
            this.code = script.ByteCode;
            this.pc = 0;
            this.error = false;
            this.scriptName = script.Header.name;
            this.returnType = script.Header.returnType;
            this.variableByID.Clear();
            this.scriptByName = scriptByName;

            if (arguments.Length == script.Header.arguments.Length)
            {
                for (byte i = 0; i < arguments.Length; i++)
                {
                    if (arguments[i] != null)
                    {
                        if (script.Header.arguments[i].type == arguments[i].GetType())
                        {
                            this.variableByID.Add(i, arguments[i]);
                        }
                        else
                        {
                            this.ReportError("Input argument " + i + " not of correct type. Was " + arguments[i].GetType() + ", expected " + script.Header.arguments[i].type);
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
                this.ReportError("Amount of arguments was not correct. Was " + arguments.Length + ", expected " + script.Header.arguments.Length);
            }
        }
    }

}
