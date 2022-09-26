using MonoKle.Console;
using System;
using System.Collections.Generic;

namespace MonoKle.Engine.Commands
{
    [ConsoleCommand("set", Description = "Assigns the provided variable with the given value.")]
    public class SetCommand : IConsoleCommand
    {
        [ConsolePositional(0, Description = "The variable to set.", IsRequired = true)]
        public string Variable { get; set; }

        [ConsolePositional(1, Description = "The value to assign.", IsRequired = false)]
        public string Value { get; set; }

        public void Call(IGameConsole console)
        {
            // Value is read only
            if (MGame.Variables.Variables.Contains(Variable) && !MGame.Variables.Variables.CanSet(Variable))
            {
                console.WriteError("Can not set variable since it is read-only");
                return;
            }

            // Value has not been explicitly provided
            if (Value == default)
            {
                if (MGame.Variables.Variables.TryGetVariable(Variable, out var cvar) && cvar.Type.IsValueType)
                {
                    // Automatically set default for existing value types to allow for shorthand:
                    // set myVariable -> set myVariable true
                    // set myVariable -> set myVariable 0
                    var val = cvar.Type == typeof(bool)
                        ? true
                        : Activator.CreateInstance(cvar.Type);
                    MGame.Variables.VariablePopulator.LoadItem(Variable, val.ToString());
                }
                else
                {
                    console.WriteError("Variable value not provided and type can not be inferred");
                }
                return;
            }

            // Load the provided value
            if (!MGame.Variables.VariablePopulator.LoadItem(Variable, Value))
            {
                console.WriteError("Variable assignment failed");
            }
        }

        public ICollection<string> GetPositionalSuggestions(IGameConsole console) => MGame.Variables.Variables.Identifiers;
    }
}
