namespace MonoKleScript.Compiler
{
    using MonoKleScript.Grammar;
    using MonoKleScript.Script;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class SemanticsListener : MonoKleScriptBaseListener
    {
        /// <summary>
        /// Event that fires when a semantic error is encountered.
        /// </summary>
        public event SemanticErrorEventHandler SemanticsError;

        private void OnSemanticsError(string message)
        {
            var l = SemanticsError;
            l(this, new SemanticErrorEventArgs(message));
        }

        private Dictionary<string, Type> typeByVariable = new Dictionary<string, Type>();

        public SemanticsListener(ICollection<ScriptVariable> inputArguments)
        {
            foreach (ScriptVariable v in inputArguments)
            {
                typeByVariable.Add(v.name, v.type);
            }
        }

        public override void ExitInitialization(MonoKleScriptParser.InitializationContext context)
        {
            Type type = CompilerHelper.StringTypeToType(context.TYPE().ToString());
            string name = context.IDENTIFIER().ToString();

            if (typeByVariable.ContainsKey(name) == false)
            {
                typeByVariable.Add(name, type);

                // TODO: Push variable to stack or something like that as well.
            }
            else
            {
                this.OnSemanticsError("Variable with name " + name + " already exists!");
            }

            // TODO: Check correct type.
        }

        public override void EnterValue(MonoKleScriptParser.ValueContext context)
        {
            var v = context.GetTokens(-1);

            // TODO: Check known functions
        }
    }
}
