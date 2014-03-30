namespace MonoKleScript.Compiler
{
    using MonoKle.Utilities;
    using MonoKleScript.Grammar;
    using MonoKleScript.Script;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal class CompilerListener : MonoKleScriptBaseListener
    {
        private byte nextVariableID = 0;
        private Queue<byte> byteCode = new Queue<byte>();
        private Dictionary<string, byte> variableIDByName = new Dictionary<string, byte>();
        private Stack<ICollection<string>> variableNameStack = new Stack<ICollection<string>>();
        private Dictionary<string, ScriptHeader> functionByName = new Dictionary<string,ScriptHeader>();

        public CompilerListener(ScriptHeader header, ICollection<ScriptHeader> knownScripts)
        {
            // Add variables and functions
            foreach (ScriptVariable v in header.arguments)
            {
                this.variableIDByName.Add(v.name, nextVariableID);
                this.nextVariableID++;
            }
            foreach(ScriptHeader h in knownScripts)
            {
                this.functionByName.Add(h.name, h);
            }
        }

        public byte[] GetByteCode()
        {
            return byteCode.ToArray();
        }

        public override void EnterExpDivide(MonoKleScriptParser.ExpDivideContext context)
        {
            this.byteCode.Enqueue(Constants.OP_DIVIDE);
        }

        public override void EnterExpMinus(MonoKleScriptParser.ExpMinusContext context)
        {
            this.byteCode.Enqueue(Constants.OP_SUBTRACT);
        }

        public override void EnterExpMultiply(MonoKleScriptParser.ExpMultiplyContext context)
        {
            this.byteCode.Enqueue(Constants.OP_MULTIPLY);
        }

        public override void EnterExpGreater(MonoKleScriptParser.ExpGreaterContext context)
        {
            this.byteCode.Enqueue(Constants.OP_LARGER);
        }

        public override void EnterExpNot(MonoKleScriptParser.ExpNotContext context)
        {
            this.byteCode.Enqueue(Constants.OP_NOT);
        }

        public override void EnterInitialization(MonoKleScriptParser.InitializationContext context)
        {
            string name = context.IDENTIFIER().ToString();
            this.byteCode.Enqueue(Constants.OP_INIVAR);
            this.byteCode.Enqueue(this.nextVariableID);
            this.variableIDByName.Add(name, this.nextVariableID);
            this.variableNameStack.Peek().Add(name);
            this.nextVariableID++;
        }

        public override void EnterAssignment(MonoKleScriptParser.AssignmentContext context)
        {
            this.byteCode.Enqueue(Constants.OP_SETVAR);
            this.byteCode.Enqueue(this.variableIDByName[context.ASSIGNMENT().ToString()]);
        }

        public override void EnterKeyReturn(MonoKleScriptParser.KeyReturnContext context)
        {
            if (context.expression() == null)
            {
                this.byteCode.Enqueue(Constants.OP_RETURN_VOID);
            }
            else
            {
                this.byteCode.Enqueue(Constants.OP_RETURN_VALUE);
            }
        }

        public override void EnterBlock(MonoKleScriptParser.BlockContext context)
        {
            this.variableNameStack.Push(new LinkedList<string>());
        }

        public override void ExitBlock(MonoKleScriptParser.BlockContext context)
        {
            foreach(string s in this.variableNameStack.Pop())
            {
                this.byteCode.Enqueue(Constants.OP_REMVAR);
                this.byteCode.Enqueue(this.variableIDByName[s]);
                this.variableIDByName.Remove(s);
                this.nextVariableID--;
            }
        }

        public override void EnterExpPlus(MonoKleScriptParser.ExpPlusContext context)
        {
            this.byteCode.Enqueue(Constants.OP_ADD);
        }

        public override void EnterValueInt(MonoKleScriptParser.ValueIntContext context)
        {
            this.byteCode.Enqueue(Constants.OP_CONST_INT);
            byte[] bytes = ByteConverter.ToBytes(Int32.Parse(context.INT().ToString()));
            foreach(byte b in bytes)
            {
                this.byteCode.Enqueue(b);
            }
        }

        public override void EnterValueBool(MonoKleScriptParser.ValueBoolContext context)
        {
            this.byteCode.Enqueue(Constants.OP_CONST_BOOL);
            byte[] bytes = ByteConverter.ToBytes(Boolean.Parse(context.BOOL().ToString()));
            foreach (byte b in bytes)
            {
                this.byteCode.Enqueue(b);
            }
        }

        public override void EnterValueFloat(MonoKleScriptParser.ValueFloatContext context)
        {
            this.byteCode.Enqueue(Constants.OP_CONST_FLOAT);
            byte[] bytes = ByteConverter.ToBytes(Single.Parse(context.FLOAT().ToString()));
            foreach (byte b in bytes)
            {
                this.byteCode.Enqueue(b);
            }
        }

        public override void EnterValueString(MonoKleScriptParser.ValueStringContext context)
        {
            this.byteCode.Enqueue(Constants.OP_CONST_STRING);
            byte[] bytes = ByteConverter.ToBytes(context.STRING().ToString());
            foreach (byte b in bytes)
            {
                this.byteCode.Enqueue(b);
            }
        }

        public override void EnterFunction(MonoKleScriptParser.FunctionContext context)
        {
            string function = context.IDENTIFIER().ToString();
            this.byteCode.Enqueue(Constants.OP_CALLFUNCTION);
            byte[] bytes = ByteConverter.ToBytes(function);
            foreach (byte b in bytes)
            {
                this.byteCode.Enqueue(b);
            }
            this.byteCode.Enqueue((byte)this.functionByName[function].arguments.Length);
        }

        public override void EnterKeyPrint(MonoKleScriptParser.KeyPrintContext context)
        {
            this.byteCode.Enqueue(Constants.OP_PRINT);
        }

        public override void EnterValueVariable(MonoKleScriptParser.ValueVariableContext context)
        {
            this.byteCode.Enqueue(Constants.OP_GETVAR);
            this.byteCode.Enqueue(this.variableIDByName[context.IDENTIFIER().ToString()]);
        }
    }
}
