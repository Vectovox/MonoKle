namespace MonoKle.Script.VM
{
    using MonoKle.Utilities.Conversion;
    using MonoKle.Script.Common.Internal;
    using MonoKle.Script.Common.Script;
    using MonoKle.Script.VM.Event;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

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
            if(l != null)
            {
                l(this, new RuntimeErrorEventArgs(message));
            }
        }

        public ExecutionResult RunScript(ByteScript script, object[] arguments, Dictionary<string, ByteScript> scriptByName)
        {
            this.Reset(script, arguments, scriptByName);

            while(this.error == false && this.pc < code.Length)
            {
                // Read code
                byte opCode = code[pc++];

                switch(opCode)
                {
                    case ByteCodeValues.OP_RETURN_VOID:
                        return this.CreateResult(null);
                    case ByteCodeValues.OP_RETURN_VALUE:
                        {
                            byte firstOperation = code[pc++];
                            return this.CreateResult(this.CalculateExpression(firstOperation));
                        }
                    case ByteCodeValues.OP_PRINT:
                        {
                            byte firstOperation = code[pc++];
                            this.OnPrint(this.CalculateExpression(firstOperation).ToString());
                        }
                        break;
                    case ByteCodeValues.OP_INIVAR:
                        {
                            byte id = code[pc++];
                            byte firstOperation = code[pc++];
                            this.variableByID.Add(id, this.CalculateExpression(firstOperation));
                        }
                        break;
                    case ByteCodeValues.OP_INIVAR_READOBJECT:
                        {
                            byte newVariableId = code[pc++];
                            byte type = code[pc++];
                            Type t = CommonHelpers.ConstantTypeToType(type);
                            object readObject = this.ReadObject();
                            if(readObject != null)
                            {
                                if(t.IsAssignableFrom(readObject.GetType()))
                                {
                                    this.variableByID.Add(newVariableId, readObject);
                                }
                                else
                                {
                                    this.ReportError("Variable can not be assigned the provided type");
                                }
                            }
                            break;
                        }
                    case ByteCodeValues.OP_SETVAR_READOBJECT:
                        {
                            byte variableToSetId = code[pc++];
                            object readObject = this.ReadObject();
                            if(this.variableByID[variableToSetId].GetType().IsAssignableFrom(readObject.GetType()))
                            {
                                this.variableByID[variableToSetId] = readObject;
                            }
                            else
                            {
                                this.ReportError("Variable can not be assigned the provided type");
                            }
                        }
                        break;
                    case ByteCodeValues.OP_SETVAR:
                        {
                            byte id = code[pc++];
                            byte firstOperation = code[pc++];
                            this.variableByID[id] = this.CalculateExpression(firstOperation);
                        }
                        break;
                    case ByteCodeValues.OP_REMVAR:
                        {
                            this.variableByID.Remove(code[pc++]);
                        }
                        break;
                    case ByteCodeValues.OP_CALLFUNCTION:
                        {
                            this.CallFunction();
                            break;
                        }
                    case ByteCodeValues.OP_WRITEOBJECT_FIELDPROPERTY:
                        {
                            object readVariable = this.variableByID[code[pc++]];
                            int bytesRead = 0;
                            string fieldName = ByteConverter.ToString(code, pc, out bytesRead);
                            this.pc += bytesRead;
                            object value = this.CalculateExpression(this.code[pc++]);
                            FieldInfo field = readVariable.GetType().GetField(fieldName);
                            PropertyInfo property = readVariable.GetType().GetProperty(fieldName);
                            if(field != null)
                            {
                                if(field.FieldType.IsAssignableFrom(value.GetType()))
                                {
                                    field.SetValue(readVariable, value);
                                }
                                else
                                {
                                    this.ReportError("Field [" + fieldName + "] can not be assigned the provided type");
                                }
                            }
                            else if(property != null)
                            {
                                if(property.PropertyType.IsAssignableFrom(value.GetType()))
                                {
                                    property.SetValue(readVariable, value, null);
                                }
                                else
                                {
                                    this.ReportError("Property [" + fieldName + "] can not be assigned the provided type");
                                }
                            }
                            else
                            {
                                this.ReportError("No such field/property [" + fieldName + "] for object");
                            }
                            break;
                        }
                    case ByteCodeValues.OP_READOBJECT_FUNCTION:
                        {
                            object readVariable = this.variableByID[code[pc++]];
                            int bytesRead = 0;
                            string methodName = ByteConverter.ToString(code, pc, out bytesRead);
                            this.pc += bytesRead;
                            this.CallObjectMethod(readVariable, methodName, ReadArguments());
                            break;
                        }
                    case ByteCodeValues.OP_IF:
                        {
                            int jmpIfFail = ByteConverter.ToInt32(code, pc);
                            pc += sizeof(int);
                            byte firstOperation = code[pc++];
                            bool expression = (bool)this.CalculateExpression(firstOperation);
                            if(expression == false)
                            {
                                this.pc = jmpIfFail;
                            }
                        }
                        break;
                    case ByteCodeValues.OP_JUMP:
                        {
                            this.pc = ByteConverter.ToInt32(code, pc);
                        }
                        break;
                    default:
                        this.ReportError("Encountered unrecognized operation.");
                        break;
                }
            }

            return this.CreateResult(null);
        }

        private object ReadObject()
        {
            byte readType = code[pc++];
            switch(readType)
            {
                case ByteCodeValues.OP_READOBJECT_FIELDPROPERTY:
                    {
                        object readVariable = this.variableByID[code[pc++]];
                        int bytesRead = 0;
                        string fieldName = ByteConverter.ToString(code, pc, out bytesRead);
                        this.pc += bytesRead;
                        FieldInfo field = readVariable.GetType().GetField(fieldName);
                        PropertyInfo property = readVariable.GetType().GetProperty(fieldName);
                        if(field != null)
                        {
                            return field.GetValue(readVariable);
                        }
                        else if(property != null)
                        {
                            return property.GetValue(readVariable, null);
                        }
                        this.ReportError("No such field/property [" + fieldName + "] for object");
                        return null;
                    }
                case ByteCodeValues.OP_READOBJECT_FUNCTION:
                    {
                        object readVariable = this.variableByID[code[pc++]];
                        int bytesRead = 0;
                        string methodName = ByteConverter.ToString(code, pc, out bytesRead);
                        this.pc += bytesRead;
                        return this.CallObjectMethod(readVariable, methodName, ReadArguments());
                    }
                default:
                    {
                        this.ReportError("Illegal READOBJECT type");
                        return null;
                    }
            }
        }

        private object[] ReadArguments()
        {
            object[] args = new object[this.code[pc++]];
            for(int i = 0; i < args.Length; i++)
            {
                args[i] = this.CalculateExpression(this.code[pc++]);
            }
            return args;
        }

        private object CallObjectMethod(object callee, string methodName, object[] arguments)
        {
            MethodInfo method = callee.GetType().GetMethod(methodName);
            if(method != null)
            {
                ParameterInfo[] parameters = method.GetParameters();
                if(arguments.Length == parameters.Length)
                {
                    for(int i = 0; i < arguments.Length; i++)
                    {
                        if(parameters[i].ParameterType.IsAssignableFrom(arguments[i].GetType()) == false)
                        {
                            this.ReportError("Invalid parameters types provided to object method [" + methodName + "]");
                        }
                    }
                    if(this.error == false)
                    {
                        return method.Invoke(callee, arguments);
                    }
                }
                else
                {
                    this.ReportError("Invalid amount of parameters for object method [" + methodName + "]");
                }
            }
            else
            {
                this.ReportError("No such method [" + methodName + "] for object");
            }
            return null;
        }

        private ExecutionResult CreateResult(object value)
        {
            if(this.error)
            {
                return ExecutionResult.CreateFail(this.scriptName);
            }
            else
            {
                return new ExecutionResult(true, this.returnType, value, this.scriptName);
            }
        }

        private object CalculateExpression(byte initialOperation)
        {
            switch(initialOperation)
            {
                case ByteCodeValues.OP_OR:
                    {
                        byte lhOP = this.code[pc++];
                        object lhRet = CalculateExpression(lhOP);

                        byte rhOP = this.code[pc++];
                        object rhRet = CalculateExpression(rhOP);

                        return ((bool)lhRet) || ((bool)rhRet);
                    }
                case ByteCodeValues.OP_AND:
                    {
                        byte lhOP = this.code[pc++];
                        object lhRet = CalculateExpression(lhOP);

                        byte rhOP = this.code[pc++];
                        object rhRet = CalculateExpression(rhOP);

                        return ((bool)lhRet) && ((bool)rhRet);
                    }
                case ByteCodeValues.OP_NOT:
                    {
                        byte lhOP = this.code[pc++];
                        object lhRet = CalculateExpression(lhOP);
                        return !(bool)lhRet;
                    }
                case ByteCodeValues.OP_SMALLER:
                    {
                        byte lhOP = this.code[pc++];
                        object lhRet = CalculateExpression(lhOP);

                        byte rhOP = this.code[pc++];
                        object rhRet = CalculateExpression(rhOP);

                        return SmallerObject(lhRet, rhRet);
                    }
                case ByteCodeValues.OP_SMALLEREQUAL:
                    {
                        byte lhOP = this.code[pc++];
                        object lhRet = CalculateExpression(lhOP);

                        byte rhOP = this.code[pc++];
                        object rhRet = CalculateExpression(rhOP);

                        return SmallerEqualObject(lhRet, rhRet);
                    }
                case ByteCodeValues.OP_LARGER:
                    {
                        byte lhOP = this.code[pc++];
                        object lhRet = CalculateExpression(lhOP);

                        byte rhOP = this.code[pc++];
                        object rhRet = CalculateExpression(rhOP);

                        return GreaterObject(lhRet, rhRet);
                    }
                case ByteCodeValues.OP_LARGEREQUAL:
                    {
                        byte lhOP = this.code[pc++];
                        object lhRet = CalculateExpression(lhOP);

                        byte rhOP = this.code[pc++];
                        object rhRet = CalculateExpression(rhOP);

                        return GreaterEqualObject(lhRet, rhRet);
                    }
                case ByteCodeValues.OP_EQUAL:
                    {
                        byte lhOP = this.code[pc++];
                        object lhRet = CalculateExpression(lhOP);

                        byte rhOP = this.code[pc++];
                        object rhRet = CalculateExpression(rhOP);

                        return lhRet.Equals(rhRet);
                    }
                case ByteCodeValues.OP_NOTEQUAL:
                    {
                        byte lhOP = this.code[pc++];
                        object lhRet = CalculateExpression(lhOP);

                        byte rhOP = this.code[pc++];
                        object rhRet = CalculateExpression(rhOP);

                        return lhRet.Equals(rhRet) == false;
                    }
                case ByteCodeValues.OP_ADD:
                    {
                        byte lhOP = this.code[pc++];
                        object lhRet = CalculateExpression(lhOP);

                        byte rhOP = this.code[pc++];
                        object rhRet = CalculateExpression(rhOP);

                        return AddObject(lhRet, rhRet);
                    }
                case ByteCodeValues.OP_SUBTRACT:
                    {
                        byte lhOP = this.code[pc++];
                        object lhRet = CalculateExpression(lhOP);

                        byte rhOP = this.code[pc++];
                        object rhRet = CalculateExpression(rhOP);

                        return SubtractObject(lhRet, rhRet);
                    }
                case ByteCodeValues.OP_DIVIDE:
                    {
                        byte lhOP = this.code[pc++];
                        object lhRet = CalculateExpression(lhOP);

                        byte rhOP = this.code[pc++];
                        object rhRet = CalculateExpression(rhOP);

                        return DivideObject(lhRet, rhRet);
                    }
                case ByteCodeValues.OP_MULTIPLY:
                    {
                        byte lhOP = this.code[pc++];
                        object lhRet = CalculateExpression(lhOP);

                        byte rhOP = this.code[pc++];
                        object rhRet = CalculateExpression(rhOP);

                        return MultiplyObject(lhRet, rhRet);
                    }
                case ByteCodeValues.OP_NEGATE:
                    {
                        byte lhOP = this.code[pc++];
                        object lhRet = CalculateExpression(lhOP);
                        return this.SubtractObject(0, lhRet);
                    }
                case ByteCodeValues.OP_POWER:
                    {
                        byte lhOP = this.code[pc++];
                        object lhRet = CalculateExpression(lhOP);

                        byte rhOP = this.code[pc++];
                        object rhRet = CalculateExpression(rhOP);

                        return PowerObject(lhRet, rhRet);
                    }
                case ByteCodeValues.OP_MODULO:
                    {
                        byte lhOP = this.code[pc++];
                        object lhRet = CalculateExpression(lhOP);

                        byte rhOP = this.code[pc++];
                        object rhRet = CalculateExpression(rhOP);

                        return this.ModuloObject(lhRet, rhRet);
                    }
                case ByteCodeValues.OP_CONST_BOOL:
                    pc += sizeof(bool);
                    return ByteConverter.ToBoolean(this.code, pc - sizeof(bool));
                case ByteCodeValues.OP_CONST_INT:
                    pc += sizeof(int);
                    return ByteConverter.ToInt32(this.code, pc - sizeof(int));
                case ByteCodeValues.OP_CONST_FLOAT:
                    pc += sizeof(float);
                    return ByteConverter.ToFloat32(this.code, pc - sizeof(float));
                case ByteCodeValues.OP_CONST_STRING:
                    {
                        int bytesRead = 0;
                        string s = ByteConverter.ToString(this.code, pc, out bytesRead);
                        pc += bytesRead;
                        return s.Remove(0, 1).Remove(s.Length - 2, 1);  // Removes the string indication characters
                    }
                case ByteCodeValues.OP_GETVAR:
                    return this.variableByID[this.code[pc++]];
                case ByteCodeValues.OP_CALLFUNCTION:
                    return this.CallFunction().ReturnValue;
                default:
                    {
                        this.ReportError("Encountered unrecognized expression operation.");
                    }
                    break;
            }

            return null;
        }

        private ExecutionResult CallFunction()
        {
            int br = 0;
            string name = ByteConverter.ToString(this.code, pc, out br);
            pc += br;
            object[] args = this.ReadArguments();

            ScriptExecuter e = new ScriptExecuter();
            e.Print += delegate (object o, PrintEventArgs pe) { this.OnPrint(pe.Message); };
            e.RuntimeError += delegate (object o, RuntimeErrorEventArgs re) { this.OnRuntimeError(re.Message); };
            ExecutionResult result = e.RunScript(scriptByName[name], args, this.scriptByName);
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
                if(b.GetType() == typeof(int))
                {
                    return (float)a > (int)b;
                }
                else if(b.GetType() == typeof(float))
                {
                    return (float)a > (float)b;
                }
            }
            return null;
        }

        private object GreaterEqualObject(object a, object b)
        {
            if(a.GetType() == typeof(int))
            {
                if(b.GetType() == typeof(int))
                {
                    return (int)a >= (int)b;
                }
                else if(b.GetType() == typeof(float))
                {
                    return (int)a >= (float)b;
                }
            }
            else if(a.GetType() == typeof(float))
            {
                if(b.GetType() == typeof(int))
                {
                    return (float)a >= (int)b;
                }
                else if(b.GetType() == typeof(float))
                {
                    return (float)a >= (float)b;
                }
            }
            return null;
        }

        private object SmallerObject(object a, object b)
        {
            if(a.GetType() == typeof(int))
            {
                if(b.GetType() == typeof(int))
                {
                    return (int)a < (int)b;
                }
                else if(b.GetType() == typeof(float))
                {
                    return (int)a < (float)b;
                }
            }
            else if(a.GetType() == typeof(float))
            {
                if(b.GetType() == typeof(int))
                {
                    return (float)a < (int)b;
                }
                else if(b.GetType() == typeof(float))
                {
                    return (float)a < (float)b;
                }
            }
            return null;
        }

        private object SmallerEqualObject(object a, object b)
        {
            if(a.GetType() == typeof(int))
            {
                if(b.GetType() == typeof(int))
                {
                    return (int)a <= (int)b;
                }
                else if(b.GetType() == typeof(float))
                {
                    return (int)a <= (float)b;
                }
            }
            else if(a.GetType() == typeof(float))
            {
                if(b.GetType() == typeof(int))
                {
                    return (float)a <= (int)b;
                }
                else if(b.GetType() == typeof(float))
                {
                    return (float)a <= (float)b;
                }
            }
            return null;
        }

        private object SubtractObject(object a, object b)
        {
            if(a.GetType() == typeof(int))
            {
                if(b.GetType() == typeof(int))
                {
                    return (int)a - (int)b;
                }
                else if(b.GetType() == typeof(float))
                {
                    return (int)a - (float)b;
                }
            }
            else if(a.GetType() == typeof(float))
            {
                if(b.GetType() == typeof(int))
                {
                    return (float)a - (int)b;
                }
                else if(b.GetType() == typeof(float))
                {
                    return (float)a - (float)b;
                }
            }
            return null;
        }

        private object PowerObject(object a, object b)
        {
            if(a.GetType() == typeof(int))
            {
                if(b.GetType() == typeof(int))
                {
                    return (int)Math.Pow((int)a, (int)b);
                }
                else if(b.GetType() == typeof(float))
                {
                    return (float)Math.Pow((int)a, (float)b);
                }
            }
            else if(a.GetType() == typeof(float))
            {
                if(b.GetType() == typeof(int))
                {
                    return (float)Math.Pow((float)a, (int)b);
                }
                else if(b.GetType() == typeof(float))
                {
                    return (float)Math.Pow((float)a, (float)b);
                }
            }
            return null;
        }

        private object ModuloObject(object a, object b)
        {
            if(a.GetType() == typeof(int))
            {
                if(b.GetType() == typeof(int))
                {
                    return (int)a % (int)b;
                }
                else if(b.GetType() == typeof(float))
                {
                    return (int)a % (float)b;
                }
            }
            else if(a.GetType() == typeof(float))
            {
                if(b.GetType() == typeof(int))
                {
                    return (float)a % (int)b;
                }
                else if(b.GetType() == typeof(float))
                {
                    return (float)a % (float)b;
                }
            }
            return null;
        }

        private object MultiplyObject(object a, object b)
        {
            if(a.GetType() == typeof(int))
            {
                if(b.GetType() == typeof(int))
                {
                    return (int)a * (int)b;
                }
                else if(b.GetType() == typeof(float))
                {
                    return (int)a * (float)b;
                }
            }
            else if(a.GetType() == typeof(float))
            {
                if(b.GetType() == typeof(int))
                {
                    return (float)a * (int)b;
                }
                else if(b.GetType() == typeof(float))
                {
                    return (float)a * (float)b;
                }
            }
            return null;
        }

        private object DivideObject(object a, object b)
        {
            if(a.GetType() == typeof(int))
            {
                if(b.GetType() == typeof(int))
                {
                    return (int)a / (int)b;
                }
                else if(b.GetType() == typeof(float))
                {
                    return (int)a / (float)b;
                }
            }
            else if(a.GetType() == typeof(float))
            {
                if(b.GetType() == typeof(int))
                {
                    return (float)a / (int)b;
                }
                else if(b.GetType() == typeof(float))
                {
                    return (float)a / (float)b;
                }
            }
            return null;
        }

        private object AddObject(object a, object b)
        {
            if(a.GetType() == typeof(int))
            {
                if(b.GetType() == typeof(int))
                {
                    return (int)a + (int)b;
                }
                else if(b.GetType() == typeof(float))
                {
                    return (int)a + (float)b;
                }
            }
            else if(a.GetType() == typeof(float))
            {
                if(b.GetType() == typeof(int))
                {
                    return (float)a + (int)b;
                }
                else if(b.GetType() == typeof(float))
                {
                    return (float)a + (float)b;
                }
            }
            else if(a.GetType() == typeof(string) || b.GetType() == typeof(string))
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
            this.scriptName = script.Header.Name;
            this.returnType = script.Header.ReturnType;
            this.variableByID.Clear();
            this.scriptByName = scriptByName;

            if(arguments.Length == script.Header.Arguments.Length)
            {
                for(byte i = 0; i < arguments.Length; i++)
                {
                    if(arguments[i] != null)
                    {
                        if(script.Header.Arguments[i].type == arguments[i].GetType() || script.Header.Arguments[i].type == typeof(object))
                        {
                            this.variableByID.Add(i, arguments[i]);
                        }
                        else
                        {
                            this.ReportError("Input argument " + i + " not of correct type. Was " + arguments[i].GetType() + ", expected " + script.Header.Arguments[i].type);
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
                this.ReportError("Amount of arguments was not correct. Was " + arguments.Length + ", expected " + script.Header.Arguments.Length);
            }
        }
    }

}
