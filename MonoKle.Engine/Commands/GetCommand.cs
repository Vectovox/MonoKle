using MonoKle.Console;
using System.Collections.Generic;

namespace MonoKle.Engine.Commands
{
    [ConsoleCommand("get", Description = "Prints the value of the provided variable.")]
    public class GetCommand : IConsoleCommand
    {
        [ConsolePositional(0, Description = "The variable to print.", IsRequired = true)]
        public string Variable { get; set; }

        public void Call(IGameConsole console)
        {
            object value = MonoKleGame.Variables.Variables.GetValue(Variable);
            if (value != null)
            {
                console.WriteLine(value.ToString());
            }
            else
            {
                console.WriteError("No such variable exist");
            }
        }

        public ICollection<string> GetPositionalSuggestions() => MonoKleGame.Variables.Variables.Identifiers;
    }
}
