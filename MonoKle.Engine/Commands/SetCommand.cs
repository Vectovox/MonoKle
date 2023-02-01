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
            if (MGame.Variables.System.Contains(Variable) && !MGame.Variables.System.CanSet(Variable))
            {
                console.Log.AddError("Can not set variable since it is read-only");
                return;
            }

            // Value has not been explicitly provided
            if (Value == default)
            {
                if (MGame.Variables.System.TryGetVariable(Variable, out var cvar) && cvar.Type.IsValueType)
                {
                    // Automatically set default for existing value types to allow for shorthand:
                    // set myVariable -> set myVariable true
                    // set myVariable -> set myVariable 0
                    var val = cvar.Type == typeof(bool)
                        ? true
                        : Activator.CreateInstance(cvar.Type);
                    MGame.Variables.Populator.LoadItem(Variable, val.ToString());
                }
                else
                {
                    console.Log.AddError("Variable value not provided and type can not be inferred");
                }
                return;
            }

            // Load the provided value
            if (!MGame.Variables.Populator.LoadItem(Variable, Value))
            {
                console.Log.AddError("Variable assignment failed");
            }
        }

        public ICollection<string> GetPositionalSuggestions(IGameConsole console) => MGame.Variables.System.Identifiers;
    }
}
