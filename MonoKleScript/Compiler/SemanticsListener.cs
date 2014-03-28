namespace MonoKleScript.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Antlr4.Runtime.Tree;

    using MonoKleScript.Grammar;
    using MonoKleScript.Script;
    using MonoKleScript.Compiler.Error;

    /// <summary>
    /// Listener that checks script semantics.
    /// </summary>
    internal class SemanticsListener : MonoKleScriptBaseListener
    {
        // TODO: Check that a return always is made.
        // TODO: Add logic operators and exp operator.
        private Dictionary<IParseTree, Type> typeByToken = new Dictionary<IParseTree, Type>();
        private Dictionary<string, Type> typeByVariable = new Dictionary<string, Type>();
        private Dictionary<string, ScriptHeader> functionByName = new Dictionary<string, ScriptHeader>();
        private Type returnType;

        public SemanticsListener(ScriptHeader header, ICollection<ScriptHeader> knownScripts)
        {
            // Add variables and functions
            foreach (ScriptVariable v in header.arguments)
            {
                this.typeByVariable.Add(v.name, v.type);
            }
            foreach(ScriptHeader h in knownScripts)
            {
                this.functionByName.Add(h.name, h);
            }
            // Add support for recursive calls
            if (this.functionByName.ContainsKey(header.name) == false)
            {
                this.functionByName.Add(header.name, header);
            }
            // Set return type
            this.returnType = header.returnType;
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

        private Stack<ICollection<string>> variableNameStack = new Stack<ICollection<string>>();

        public override void ExitInitialization(MonoKleScriptParser.InitializationContext context)
        {
            Type type = CompilerHelper.StringTypeToType(context.TYPE().ToString());
            string name = context.IDENTIFIER().ToString();

            if (this.typeByVariable.ContainsKey(name) == false)
            {
                this.typeByVariable.Add(name, type);
                this.variableNameStack.Peek().Add(name);
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

        public override void EnterBlock(MonoKleScriptParser.BlockContext context)
        {
            this.variableNameStack.Push(new LinkedList<string>());
        }

        public override void ExitBlock(MonoKleScriptParser.BlockContext context)
        {
            foreach(string s in this.variableNameStack.Pop())
            {
                this.typeByVariable.Remove(s);
            }
        }

        public override void ExitFunction(MonoKleScriptParser.FunctionContext context)
        {
            string functionName = context.IDENTIFIER().ToString();

            if(this.functionByName.ContainsKey(functionName))
            {
                ScriptHeader function = this.functionByName[functionName];
                this.typeByToken.Add(context, function.returnType);
                
                // Record function parameters
                Stack<Type> parameters = new Stack<Type>();
                MonoKleScriptParser.ParametersContext pc = context.parameters();
                while (pc != null)
                {
                    parameters.Push(this.typeByToken[pc.expression()]);
                    pc = pc.parameters();
                }

                // Validate function parameters
                if(parameters.Count == function.arguments.Length)
                {
                    for(int i = function.arguments.Length - 1; i > 0; i--)
                    {
                        if(function.arguments[i].type != parameters.Pop())
                        {
                            this.OnSemanticsError("Argument [" + i + "] in function [" + functionName + "] is of wrong type");
                        }
                    }
                }
                else
                {
                    this.OnSemanticsError("Function [" + functionName + "] does not take [" + parameters.Count + "] arguments");
                }
            }
            else
            {
                this.OnSemanticsError("Function [" + functionName + "] not found");
                this.typeByToken.Add(context, typeof(void));
            }
        }

        public override void ExitValueFunction(MonoKleScriptParser.ValueFunctionContext context)
        {
            this.typeByToken.Add(context, this.typeByToken[context.function()]);
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
            else
            {
                this.typeByToken.Add(context, typeof(void));
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