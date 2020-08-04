using MonoKle.Console;
using System.Collections.Generic;

namespace MonoKle.Engine.Commands
{
    [ConsoleCommand("set", Description = "Assigns the provided variable with the given value.")]
    public class SetCommand : IConsoleCommand
    {
        [ConsolePositional(0, Description = "The variable to set.", IsRequired = true)]
        public string Variable { get; set; }

        [ConsolePositional(1, Description = "The value to assign.", IsRequired = true)]
        public string Value { get; set; }

        public void Call(IGameConsole console)
        {
            if (MGame.Variables.Variables.Contains(Variable) && MGame.Variables.Variables.CanSet(Variable) == false)
            {
                console.WriteError("Can not set variable since it is read-only");
            }
            else if (MGame.Variables.VariablePopulator.LoadItem(Variable, Value) == false)
            {
                console.WriteError("Variable assignment failed");
            }
        }

        public ICollection<string> GetPositionalSuggestions() => MGame.Variables.Variables.Identifiers;
    }
}
