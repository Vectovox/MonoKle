namespace MonoKle.Script.Compiler.Listeners
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Antlr4.Runtime.Tree;

    using MonoKle.Script.Grammar;
    using MonoKle.Script.Common.Script;
    using MonoKle.Script.Common.Internal;
    using Antlr4.Runtime.Misc;

    /// <summary>
    /// Listener that checks script semantics.
    /// </summary>
    internal class SemanticsListener : MonoKleScriptBaseListener
    {
        // TODO: TODOS ARE IN ORDER OF PRIORITY, FROM HIGHEST TO LOWEST
        // TODO: FIX: Objects can return null values which introduces null into system.
        // TODO: Add object field/property/method chaining
        // TODO: Check that a return always is made.

        private LinkedList<string> errorList = new LinkedList<string>();
        private Dictionary<IParseTree, Type> typeByToken = new Dictionary<IParseTree, Type>();
        private Dictionary<string, Type> typeByVariable = new Dictionary<string, Type>();
        private Dictionary<string, ScriptHeader> functionByName = new Dictionary<string, ScriptHeader>();
        private Type returnType;

        public bool WasSuccessful() { return this.errorList.Count == 0; }
        public ICollection<string> GetErrorMessages() { return this.errorList; }

        public SemanticsListener(ScriptHeader header, ICollection<ScriptHeader> knownScripts)
        {
            // Add variables and functions
            foreach (ScriptVariable v in header.Arguments)
            {
                this.typeByVariable.Add(v.name, v.type);
            }
            foreach(ScriptHeader h in knownScripts)
            {
                this.functionByName.Add(h.Name, h);
            }
            // Set return type
            this.returnType = header.ReturnType;
        }

        public override void ExitNewObject([NotNull]MonoKleScriptParser.NewObjectContext context)
        {
            this.typeByToken.Add(context, typeof(object));
        }

        public override void ExitAssignment_writeobject([NotNull]MonoKleScriptParser.Assignment_writeobjectContext context)
        {
            string objectName = context.IDENTIFIER(0).ToString();
            if(this.CheckVariableExists(objectName))
            {
                if(this.typeByVariable[objectName] != typeof(object))
                {
                    this.AddError("Read variable [" + objectName + "] is not an object");
                }
            }
        }

        public override void ExitInitialization_readobject([NotNull]MonoKleScriptParser.Initialization_readobjectContext context)
        {
            string name = context.IDENTIFIER().ToString();

            if( this.typeByVariable.Count < ScriptSettingsConstants.SCRIPT_MAX_VARIABLES )
            {
                if( this.typeByVariable.ContainsKey(name) == false )
                {
                    Type type = CommonHelpers.StringTypeToType(context.TYPE().ToString());
                    this.typeByVariable.Add(name, type);
                    this.variableNameStack.Peek().Add(name);
                }
                else
                {
                    this.AddError("Variable [" + name + "] is already declared in scope");
                }
            }
            else
            {
                this.AddError("Variable [" + name + "] is not initialized. Max number of variables [" + ScriptSettingsConstants.SCRIPT_MAX_VARIABLES + "] reached.");
            }
        }

        public override void ExitOV([NotNull]MonoKleScriptParser.OVContext context)
        {
            string objectName = context.IDENTIFIER(0).ToString();
            if(this.CheckVariableExists(objectName))
            {
                if(this.typeByVariable[objectName] != typeof(object))
                {
                    this.AddError("Read variable [" + objectName + "] is not an object");
                }
            }
        }

        public override void ExitObjectfunction([NotNull]MonoKleScriptParser.ObjectfunctionContext context)
        {
            string objectName = context.IDENTIFIER(0).ToString();
            if(this.CheckVariableExists(objectName))
            {
                if(this.typeByVariable[objectName] != typeof(object))
                {
                    this.AddError("Read variable [" + objectName + "] is not an object");
                }
            }
        }

        public override void ExitAssignment_readobject([NotNull]MonoKleScriptParser.Assignment_readobjectContext context)
        {
            this.CheckVariableExists(context.IDENTIFIER().ToString());
        }

        public override void ExitWhile([NotNull]MonoKleScriptParser.WhileContext context)
        {
            Type type = this.typeByToken[context.expression()];
            this.CheckCorrectType(type, typeof(bool));
        }

        public override void ExitIf([NotNull]MonoKleScriptParser.IfContext context)
        {
            Type type = this.typeByToken[context.expression()];
            this.CheckCorrectType(type, typeof(bool));
        }

        public override void ExitExpNegate([NotNull]MonoKleScriptParser.ExpNegateContext context)
        {
            Type type = this.typeByToken[context.expression()];
            if(type != typeof(int) && type != typeof(float))
            {
                this.AddError("Impossible to negate the provided type");
            }
            this.typeByToken.Add(context, type);
        }

        public override void ExitExpModulo([NotNull]MonoKleScriptParser.ExpModuloContext context)
        {
            Type lhs = this.typeByToken[context.expression(0)];
            Type rhs = this.typeByToken[context.expression(1)];

            Type type = GetArithmeticType(lhs, rhs);

            if (type == typeof(void) || type == typeof(string) )
            {
                this.AddError("Types provided to modulo operator are not valid");
            }

            this.typeByToken.Add(context, type);
        }

        public override void ExitExpPower([NotNull]MonoKleScriptParser.ExpPowerContext context)
        {
            Type lhs = this.typeByToken[context.expression(0)];
            Type rhs = this.typeByToken[context.expression(1)];

            Type type = GetArithmeticType(lhs, rhs);

            if (type == typeof(void) || type == typeof(string))
            {
                this.AddError("Types provided to power operator are not valid");
            }

            this.typeByToken.Add(context, type);
        }

        public override void ExitAssignment(MonoKleScriptParser.AssignmentContext context)
        {
            string variable = context.IDENTIFIER().ToString();

            if (this.CheckVariableExists(variable))
            {
                this.CheckCorrectType(this.typeByToken[context.expression()], this.typeByVariable[variable]);
            }
        }

        public override void ExitExpGrouping(MonoKleScriptParser.ExpGroupingContext context)
        {
            this.typeByToken.Add(context, this.typeByToken[context.expression()]);
        }

        public override void ExitExpValue(MonoKleScriptParser.ExpValueContext context)
        {
            this.typeByToken.Add(context, this.typeByToken[context.value()]);
        }

        public override void ExitExpNewObject([NotNull]MonoKleScriptParser.ExpNewObjectContext context)
        {
            this.typeByToken.Add(context, this.typeByToken[context.newObject()]);
        }

        public override void ExitExpOr([NotNull]MonoKleScriptParser.ExpOrContext context)
        {
            this.CheckCorrectType(this.typeByToken[context.expression(0)], typeof(bool));
            this.CheckCorrectType(this.typeByToken[context.expression(1)], typeof(bool));
            this.typeByToken.Add(context, typeof(bool));
        }

        public override void ExitExpAnd([NotNull]MonoKleScriptParser.ExpAndContext context)
        {
            this.CheckCorrectType(this.typeByToken[context.expression(0)], typeof(bool));
            this.CheckCorrectType(this.typeByToken[context.expression(1)], typeof(bool));
            this.typeByToken.Add(context, typeof(bool));
        }

        private Stack<ICollection<string>> variableNameStack = new Stack<ICollection<string>>();

        public override void ExitInitialization(MonoKleScriptParser.InitializationContext context)
        {
            string name = context.IDENTIFIER().ToString();

            if (this.typeByVariable.Count < ScriptSettingsConstants.SCRIPT_MAX_VARIABLES)
            {
                if (this.typeByVariable.ContainsKey(name) == false)
                {
                    Type type = CommonHelpers.StringTypeToType(context.TYPE().ToString());
                    if( this.CheckCorrectType(this.typeByToken[context.expression()], type))
                    {
                        this.typeByVariable.Add(name, type);
                        this.variableNameStack.Peek().Add(name);
                    }
                }
                else
                {
                    this.AddError("Variable [" + name + "] is already declared in scope");
                }
            }
            else
            {
                this.AddError("Variable [" + name + "] is not initialized. Max number of variables [" + ScriptSettingsConstants.SCRIPT_MAX_VARIABLES + "] reached.");
            }
        }

        public override void ExitExpPlus(MonoKleScriptParser.ExpPlusContext context)
        {
            var a = context.expression(0);
            var b = context.expression(1);

            Type type = this.GetArithmeticType(this.typeByToken[context.expression(0)], this.typeByToken[context.expression(1)]);
            if(type == typeof(void))
            {
                this.AddError("Type provided to addition operator is not valid");
            }
            this.typeByToken.Add(context, type);
        }

        private Type GetArithmeticType(Type lhs, Type rhs)
        {
            if (CommonHelpers.IsTypeArithmetic(lhs) && CommonHelpers.IsTypeArithmetic(rhs))
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
                this.AddError("Type provided to division operator is not valid");
            }
            this.typeByToken.Add(context, type);
        }

        public override void ExitExpEquals([NotNull]MonoKleScriptParser.ExpEqualsContext context)
        {
            Type lhs = this.typeByToken[context.expression(0)];
            Type rhs = this.typeByToken[context.expression(1)];

            if(this.IsTypesComparable(lhs, rhs) == false)
            {
                this.AddError("Types can not be compared for equality");
            }

            this.typeByToken.Add(context, typeof(bool));
        }

        public override void ExitExpNotEquals([NotNull]MonoKleScriptParser.ExpNotEqualsContext context)
        {
            Type lhs = this.typeByToken[context.expression(0)];
            Type rhs = this.typeByToken[context.expression(1)];

            if (this.IsTypesComparable(lhs, rhs) == false)
            {
                this.AddError("Types can not be compared for equality");
            }

            this.typeByToken.Add(context, typeof(bool));
        }

        public override void ExitExpGreater(MonoKleScriptParser.ExpGreaterContext context)
        {
            Type lhs = this.typeByToken[context.expression(0)];
            Type rhs = this.typeByToken[context.expression(1)];

            this.CheckTypeHasGreatness(lhs);
            this.CheckTypeHasGreatness(rhs);
            
            this.typeByToken.Add(context, typeof(bool));
        }

        public override void ExitExpGreaterEquals([NotNull]MonoKleScriptParser.ExpGreaterEqualsContext context)
        {
            Type lhs = this.typeByToken[context.expression(0)];
            Type rhs = this.typeByToken[context.expression(1)];

            this.CheckTypeHasGreatness(lhs);
            this.CheckTypeHasGreatness(rhs);

            this.typeByToken.Add(context, typeof(bool));
        }

        public override void ExitExpSmaller([NotNull]MonoKleScriptParser.ExpSmallerContext context)
        {
            Type lhs = this.typeByToken[context.expression(0)];
            Type rhs = this.typeByToken[context.expression(1)];

            this.CheckTypeHasGreatness(lhs);
            this.CheckTypeHasGreatness(rhs);

            this.typeByToken.Add(context, typeof(bool));
        }

        public override void ExitExpSmallerEquals([NotNull]MonoKleScriptParser.ExpSmallerEqualsContext context)
        {
            Type lhs = this.typeByToken[context.expression(0)];
            Type rhs = this.typeByToken[context.expression(1)];

            this.CheckTypeHasGreatness(lhs);
            this.CheckTypeHasGreatness(rhs);

            this.typeByToken.Add(context, typeof(bool));
        }

        public override void ExitExpNot(MonoKleScriptParser.ExpNotContext context)
        {
            Type type = this.typeByToken[context.expression()];
            this.CheckCorrectType(type, typeof(bool));
            this.typeByToken.Add(context, typeof(bool));
        }

        public override void ExitExpMinus(MonoKleScriptParser.ExpMinusContext context)
        {
            Type type = this.GetArithmeticType(this.typeByToken[context.expression(0)], this.typeByToken[context.expression(1)]);
            if (type == typeof(void) || type == typeof(string))
            {
                this.AddError("Type provided to subtraction operator is not valid");
            }
            this.typeByToken.Add(context, type);
        }

        public override void ExitExpMultiply(MonoKleScriptParser.ExpMultiplyContext context)
        {
            Type type = this.GetArithmeticType(this.typeByToken[context.expression(0)], this.typeByToken[context.expression(1)]);
            if (type == typeof(void) || type == typeof(string))
            {
                this.AddError("Type provided to multiplication operator is not valid");
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
                this.typeByToken.Add(context, function.ReturnType);
                
                // Record function parameters
                Stack<Type> parameters = new Stack<Type>();
                MonoKleScriptParser.ParametersContext pc = context.parameters();
                while (pc != null)
                {
                    parameters.Push(this.typeByToken[pc.expression()]);
                    pc = pc.parameters();
                }

                // Validate function parameters
                if(parameters.Count == function.Arguments.Length)
                {
                    for(int i = function.Arguments.Length - 1; i > 0; i--)
                    {
                        if(function.Arguments[i].type != parameters.Pop())
                        {
                            this.AddError("Argument [" + i + "] in function [" + functionName + "] is of wrong type");
                        }
                    }
                }
                else
                {
                    this.AddError("Function [" + functionName + "] does not take [" + parameters.Count + "] arguments");
                }
            }
            else
            {
                this.AddError("Function [" + functionName + "] was not found");
                this.typeByToken.Add(context, typeof(void));
            }
        }

        public override void ExitValueFunction(MonoKleScriptParser.ValueFunctionContext context)
        {
            this.typeByToken.Add(context, this.typeByToken[context.function()]);
        }

        public override void ExitKeyReturn(MonoKleScriptParser.KeyReturnContext context)
        {
            if (this.returnType == typeof(void))
            {
                if(context.expression() != null)
                {
                    this.AddError("Return value can not be provided in void scripts");
                }
            }
            else
            {
                if (context.expression() == null)
                {
                    this.AddError("Return value must be provided for non-void scripts");
                }
                else
                {
                    this.CheckCorrectType(this.typeByToken[context.expression()], this.returnType);
                }
            }
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
            if (target.IsAssignableFrom(type) == false)
            {
                this.AddError("Type [" + type + "] incompatible with [" + target + "]");
                return false;
            }

            return true;
        }

        private bool CheckTypeHasGreatness(Type type)
        {
            if (type != typeof(int) && type != typeof(float))
            {
                this.AddError("Type [" + type + "] can not be compared for greatness");
                return false;
            }
            return true;
        }

        private bool IsTypesComparable(Type a, Type b)
        {
            if(a == typeof(int) || a == typeof(float))
            {
                return b == typeof(int) || b == typeof(float);
            }

            return a == b;
        }

        private bool CheckVariableExists(string variable)
        {
            if (this.typeByVariable.ContainsKey(variable) == false)
            {
                this.AddError("Variable [" + variable + "] is not declared in scope");
                return false;
            }

            return true;
        }

        private void AddError(string message)
        {
            this.errorList.AddLast(message);
        }
    }
}