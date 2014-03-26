namespace MonoKleScript.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Antlr4.Runtime.Tree;

    using MonoKleScript.Grammar;
    using MonoKleScript.Script;

    /// <summary>
    /// Listener that checks script semantics.
    /// </summary>
    public class SemanticsListener : MonoKleScriptBaseListener
    {
        private Dictionary<IParseTree, Type> typeByToken = new Dictionary<IParseTree, Type>();
        private Dictionary<string, Type> typeByVariable = new Dictionary<string, Type>();
        private Type returnType;

        public SemanticsListener(ScriptHeader header)
        {
            foreach (ScriptVariable v in header.arguments)
            {
                typeByVariable.Add(v.name, v.type);
            }

            returnType = header.returnType;
        }

        /// <summary>
        /// Event that fires when a semantic error is encountered.
        /// </summary>
        public event SemanticErrorEventHandler SemanticsError;

        public override void ExitAssignment(MonoKleScriptParser.AssignmentContext context)
        {
            string variable = context.IDENTIFIER().ToString();

            if (this.CheckVariableExists(variable))
            {
                this.CheckCorrectType(this.typeByToken[context.expression()], this.typeByVariable[variable]);
            }
        }

        public override void ExitExpValue(MonoKleScriptParser.ExpValueContext context)
        {
            this.typeByToken.Add(context, this.typeByToken[context.value()]);
        }

        public override void ExitInitialization(MonoKleScriptParser.InitializationContext context)
        {
            Type type = CompilerHelper.StringTypeToType(context.TYPE().ToString());
            string name = context.IDENTIFIER().ToString();

            if (this.typeByVariable.ContainsKey(name) == false)
            {
                this.typeByVariable.Add(name, type);

                // TODO: Push variable to stack or something like that as well.
            }
            else
            {
                this.OnSemanticsError("Variable with name [" + name + "] already declared in scope");
            }

            this.CheckCorrectType(this.typeByToken[context.expression()], type);
        }

        public override void ExitExpPlus(MonoKleScriptParser.ExpPlusContext context)
        {
            Type type = this.GetArithmeticType(this.typeByToken[context.expression(0)], this.typeByToken[context.expression(1)]);
            if(type == typeof(void))
            {
                this.OnSemanticsError("Type provided to addition operator not valid");
            }
            this.typeByToken.Add(context, type);
        }

        private Type GetArithmeticType(Type lhs, Type rhs)
        {
            if (CompilerHelper.IsTypeArithmetic(lhs) && CompilerHelper.IsTypeArithmetic(rhs))
            {
                if (lhs == typeof(float) || rhs == typeof(float))
                {
                    return typeof(float);
                }
                else
                {
                    return typeof(int);
                }
            }
            else
            {
                if (lhs == typeof(string) || rhs == typeof(string))
                {
                    return typeof(string);
                }
            }
            return typeof(void);
        }

        public override void ExitExpDivide(MonoKleScriptParser.ExpDivideContext context)
        {
            Type type = this.GetArithmeticType(this.typeByToken[context.expression(0)], this.typeByToken[context.expression(1)]);
            if (type == typeof(void) || type == typeof(string))
            {
                this.OnSemanticsError("Type provided to division operator not valid");
            }
            this.typeByToken.Add(context, type);
        }

        public override void ExitExpMinus(MonoKleScriptParser.ExpMinusContext context)
        {
            Type type = this.GetArithmeticType(this.typeByToken[context.expression(0)], this.typeByToken[context.expression(1)]);
            if (type == typeof(void) || type == typeof(string))
            {
                this.OnSemanticsError("Type provided to subtraction operator not valid");
            }
            this.typeByToken.Add(context, type);
        }

        public override void ExitExpMultiply(MonoKleScriptParser.ExpMultiplyContext context)
        {
            Type type = this.GetArithmeticType(this.typeByToken[context.expression(0)], this.typeByToken[context.expression(1)]);
            if (type == typeof(void) || type == typeof(string))
            {
                this.OnSemanticsError("Type provided to multiplication operator not valid");
            }
            this.typeByToken.Add(context, type);
        }

        public override void ExitBlock(MonoKleScriptParser.BlockContext context)
        {
            // TODO: Pop stack
        }

        public override void ExitValueFunction(MonoKleScriptParser.ValueFunctionContext context)
        {
            // TODO: IMPLEMENT
        }

        public override void ExitKeyReturn(MonoKleScriptParser.KeyReturnContext context)
        {
            this.CheckCorrectType(this.typeByToken[context.expression()], this.returnType);
        }

        public override void ExitValueBool(MonoKleScriptParser.ValueBoolContext context)
        {
            this.typeByToken.Add(context, typeof(bool));
        }

        public override void ExitValueFloat(MonoKleScriptParser.ValueFloatContext context)
        {
            this.typeByToken.Add(context, typeof(float));
        }

        public override void ExitValueInt(MonoKleScriptParser.ValueIntContext context)
        {
            this.typeByToken.Add(context, typeof(int));
        }

        public override void ExitValueString(MonoKleScriptParser.ValueStringContext context)
        {
            this.typeByToken.Add(context, typeof(string));
        }

        public override void ExitValueVariable(MonoKleScriptParser.ValueVariableContext context)
        {
            string variable = context.IDENTIFIER().ToString();

            if (this.CheckVariableExists(variable))
            {
                this.typeByToken.Add(context, this.typeByVariable[variable]);
            }
        }

        private bool CheckCorrectType(Type type, Type target)
        {
            if (CompilerHelper.IsTypeCompatibleToTarget(type, target) == false)
            {
                this.OnSemanticsError("Type [" + type + "] incompatible with [" + target + "]");
                return false;
            }

            return true;
        }

        private bool CheckVariableExists(string variable)
        {
            if (this.typeByVariable.ContainsKey(variable) == false)
            {
                this.OnSemanticsError("Variable [" + variable + "] not declared in scope");
                return false;
            }

            return true;
        }

        private void OnSemanticsError(string message)
        {
            var l = SemanticsError;
            l(this, new SemanticErrorEventArgs(message));
        }
    }
}