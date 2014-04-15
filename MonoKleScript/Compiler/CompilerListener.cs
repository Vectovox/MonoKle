﻿namespace MonoKleScript.Compiler
{
    using MonoKle.Utilities;
    using MonoKleScript.Grammar;
    using MonoKleScript.Script;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Antlr4.Runtime.Misc;

    internal class CompilerListener : MonoKleScriptBaseListener
    {
        private byte nextVariableID = 0;
        private List<byte> byteCode = new List<byte>();
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


        private Stack<int> whileAddress = new Stack<int>();

        public override void EnterWhile([NotNull]MonoKleScriptParser.WhileContext context)
        {
            this.whileAddress.Push(this.byteCode.Count);
            this.byteCode.Add(Constants.OP_IF);
            this.byteCode.Add(0);
            this.byteCode.Add(0);
            this.byteCode.Add(0);
            this.byteCode.Add(0);
        }

        public override void ExitWhile([NotNull]MonoKleScriptParser.WhileContext context)
        {
            int whileAddress = this.whileAddress.Pop();
            int jmpAddressLocation = whileAddress + 1;

            // Add jump in the end of the loop
            this.byteCode.Add(Constants.OP_JUMP);
            byte[] whileBytes = ByteConverter.ToBytes(whileAddress);
            this.byteCode.Add(whileBytes[0]);
            this.byteCode.Add(whileBytes[1]);
            this.byteCode.Add(whileBytes[2]);
            this.byteCode.Add(whileBytes[3]);

            // Set the address to jump to when condition is false
            byte[] jmpBytes = ByteConverter.ToBytes(this.byteCode.Count);
            this.byteCode[jmpAddressLocation + 0] = jmpBytes[0];
            this.byteCode[jmpAddressLocation + 1] = jmpBytes[1];
            this.byteCode[jmpAddressLocation + 2] = jmpBytes[2];
            this.byteCode[jmpAddressLocation + 3] = jmpBytes[3];
        }

        private Stack<int> ifJmpOnFailStack = new Stack<int>();
        private Stack<ICollection<int>> ifJmpOnCompleteCollectionStack = new Stack<ICollection<int>>();

        public override void EnterIf([NotNull]MonoKleScriptParser.IfContext context)
        {
            this.byteCode.Add(Constants.OP_IF);
            // Set where jump value space is alloated
            this.ifJmpOnFailStack.Push(this.byteCode.Count);
            // Allocate
            this.byteCode.Add(0);
            this.byteCode.Add(0);
            this.byteCode.Add(0);
            this.byteCode.Add(0);
        }

        public override void ExitIf([NotNull]MonoKleScriptParser.IfContext context)
        {
            // Tell to jump upon a true condition
            this.byteCode.Add(Constants.OP_JUMP);
            // Store where jump value space is allocated for jumps on a true condition (i.e. endif)
            this.ifJmpOnCompleteCollectionStack.Peek().Add(this.byteCode.Count);
            // Allocate
            this.byteCode.Add(0);
            this.byteCode.Add(0);
            this.byteCode.Add(0);
            this.byteCode.Add(0);

            // Get address where jump address on fail is stored
            int jmpAddress = this.ifJmpOnFailStack.Pop();
            // Store current pc as jump address for if condition was false
            byte[] bytes = ByteConverter.ToBytes(this.byteCode.Count);
            this.byteCode[jmpAddress + 0] = bytes[0];
            this.byteCode[jmpAddress + 1] = bytes[1];
            this.byteCode[jmpAddress + 2] = bytes[2];
            this.byteCode[jmpAddress + 3] = bytes[3];
        }

        public override void EnterConditional([NotNull]MonoKleScriptParser.ConditionalContext context)
        {
            this.ifJmpOnCompleteCollectionStack.Push(new LinkedList<int>());
        }

        public override void ExitConditional([NotNull]MonoKleScriptParser.ConditionalContext context)
        {
            // Get all stored if/elseif addresses for storing jump address for true condition
            ICollection<int> col = this.ifJmpOnCompleteCollectionStack.Pop();
            // Get current pc
            int pc = this.byteCode.Count;
            // For all if/elseif stored, set current pc as jump point
            byte[] bytes = ByteConverter.ToBytes(pc);
            foreach(int i in col)
            {
                this.byteCode[i + 0] = bytes[0];
                this.byteCode[i + 1] = bytes[1];
                this.byteCode[i + 2] = bytes[2];
                this.byteCode[i + 3] = bytes[3];
            }
        }

        public override void EnterExpNegate([NotNull]MonoKleScriptParser.ExpNegateContext context)
        {
            this.byteCode.Add(Constants.OP_NEGATE);
        }

        public override void EnterExpModulo([NotNull]MonoKleScriptParser.ExpModuloContext context)
        {
            this.byteCode.Add(Constants.OP_MODULO);
        }

        public override void EnterExpPower([NotNull]MonoKleScriptParser.ExpPowerContext context)
        {
            this.byteCode.Add(Constants.OP_POWER);
        }

        public override void EnterExpNotEquals([NotNull]MonoKleScriptParser.ExpNotEqualsContext context)
        {
            this.byteCode.Add(Constants.OP_NOTEQUAL);
        }

        public override void EnterExpEquals([NotNull]MonoKleScriptParser.ExpEqualsContext context)
        {
            this.byteCode.Add(Constants.OP_EQUAL);
        }

        public override void EnterExpSmallerEquals([NotNull]MonoKleScriptParser.ExpSmallerEqualsContext context)
        {
            this.byteCode.Add(Constants.OP_SMALLEREQUAL);
        }

        public override void EnterExpSmaller([NotNull]MonoKleScriptParser.ExpSmallerContext context)
        {
            this.byteCode.Add(Constants.OP_SMALLER);
        }

        public override void EnterExpGreaterEquals([NotNull]MonoKleScriptParser.ExpGreaterEqualsContext context)
        {
            this.byteCode.Add(Constants.OP_LARGEREQUAL);
        }

        public override void EnterExpGreater(MonoKleScriptParser.ExpGreaterContext context)
        {
            this.byteCode.Add(Constants.OP_LARGER);
        }

        public override void EnterExpAnd([NotNull]MonoKleScriptParser.ExpAndContext context)
        {
            this.byteCode.Add(Constants.OP_AND);
        }

        public override void EnterExpOr([NotNull]MonoKleScriptParser.ExpOrContext context)
        {
            this.byteCode.Add(Constants.OP_OR);
        }

        public override void EnterExpDivide(MonoKleScriptParser.ExpDivideContext context)
        {
            this.byteCode.Add(Constants.OP_DIVIDE);
        }

        public override void EnterExpMinus(MonoKleScriptParser.ExpMinusContext context)
        {
            this.byteCode.Add(Constants.OP_SUBTRACT);
        }

        public override void EnterExpMultiply(MonoKleScriptParser.ExpMultiplyContext context)
        {
            this.byteCode.Add(Constants.OP_MULTIPLY);
        }

        public override void EnterExpNot(MonoKleScriptParser.ExpNotContext context)
        {
            this.byteCode.Add(Constants.OP_NOT);
        }

        public override void EnterInitialization(MonoKleScriptParser.InitializationContext context)
        {
            string name = context.IDENTIFIER().ToString();
            this.byteCode.Add(Constants.OP_INIVAR);
            this.byteCode.Add(this.nextVariableID);
            this.variableIDByName.Add(name, this.nextVariableID);
            this.variableNameStack.Peek().Add(name);
            this.nextVariableID++;
        }

        public override void EnterAssignment(MonoKleScriptParser.AssignmentContext context)
        {
            this.byteCode.Add(Constants.OP_SETVAR);
            this.byteCode.Add(this.variableIDByName[context.IDENTIFIER().ToString()]);
        }

        public override void EnterKeyReturn(MonoKleScriptParser.KeyReturnContext context)
        {
            if (context.expression() == null)
            {
                this.byteCode.Add(Constants.OP_RETURN_VOID);
            }
            else
            {
                this.byteCode.Add(Constants.OP_RETURN_VALUE);
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
                this.byteCode.Add(Constants.OP_REMVAR);
                this.byteCode.Add(this.variableIDByName[s]);
                this.variableIDByName.Remove(s);
                this.nextVariableID--;
            }
        }

        public override void EnterExpPlus(MonoKleScriptParser.ExpPlusContext context)
        {
            this.byteCode.Add(Constants.OP_ADD);
        }

        public override void EnterValueInt(MonoKleScriptParser.ValueIntContext context)
        {
            this.byteCode.Add(Constants.OP_CONST_INT);
            byte[] bytes = ByteConverter.ToBytes(Int32.Parse(context.INT().ToString()));
            foreach(byte b in bytes)
            {
                this.byteCode.Add(b);
            }
        }

        public override void EnterValueBool(MonoKleScriptParser.ValueBoolContext context)
        {
            this.byteCode.Add(Constants.OP_CONST_BOOL);
            byte[] bytes = ByteConverter.ToBytes(Boolean.Parse(context.BOOL().ToString()));
            foreach (byte b in bytes)
            {
                this.byteCode.Add(b);
            }
        }

        public override void EnterValueFloat(MonoKleScriptParser.ValueFloatContext context)
        {
            this.byteCode.Add(Constants.OP_CONST_FLOAT);
            byte[] bytes = ByteConverter.ToBytes(Single.Parse(context.FLOAT().ToString()));
            foreach (byte b in bytes)
            {
                this.byteCode.Add(b);
            }
        }

        public override void EnterValueString(MonoKleScriptParser.ValueStringContext context)
        {
            this.byteCode.Add(Constants.OP_CONST_STRING);
            byte[] bytes = ByteConverter.ToBytes(context.STRING().ToString());
            foreach (byte b in bytes)
            {
                this.byteCode.Add(b);
            }
        }

        public override void EnterFunction(MonoKleScriptParser.FunctionContext context)
        {
            string function = context.IDENTIFIER().ToString();
            this.byteCode.Add(Constants.OP_CALLFUNCTION);
            byte[] bytes = ByteConverter.ToBytes(function);
            foreach (byte b in bytes)
            {
                this.byteCode.Add(b);
            }
            this.byteCode.Add((byte)this.functionByName[function].arguments.Length);
        }

        public override void EnterKeyPrint(MonoKleScriptParser.KeyPrintContext context)
        {
            this.byteCode.Add(Constants.OP_PRINT);
        }

        public override void EnterValueVariable(MonoKleScriptParser.ValueVariableContext context)
        {
            this.byteCode.Add(Constants.OP_GETVAR);
            this.byteCode.Add(this.variableIDByName[context.IDENTIFIER().ToString()]);
        }
    }
}
