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
            object value = MGame.Variables.System.GetValue(Variable);
            if (value != null)
            {
                console.Log.WriteLine(value.ToString());
            }
            else
            {
                console.Log.WriteError("No such variable exist");
            }
        }

        public ICollection<string> GetPositionalSuggestions(IGameConsole console) => MGame.Variables.System.Identifiers;
    }
}
