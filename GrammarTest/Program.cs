using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MonoKleScript.Compiler;
using MonoKleScript.Script;
using MonoKleScript.IO;
using MonoKleScript.Compiler.Event;
using MonoKleScript.VM;
using MonoKleScript.Debug;

namespace GrammarTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ScriptFileReader reader = new ScriptFileReader();
            ICollection<ScriptSource> sources = reader.GetScriptSources("./", false);

            //foreach (ScriptSource s in sources)
            //{
            //    LanguageDebugger.PrintParserTree(s.Text);
            //}

            //Console.Read();
            //return;

            ScriptCompiler c = new ScriptCompiler();
            c.CompilationError += c_CompilationError;
            CompilationEnvironment environment = new CompilationEnvironment(c);

            environment.LoadSources(sources);
            var byteScripts = environment.Compile();

            VirtualMachine vm = new VirtualMachine();
            vm.RuntimeError += vm_RuntimeError;
            vm.Print += vm_Print;
            vm.LoadScripts(byteScripts);

            while(true)
            {
                string input = Console.ReadLine();
                if (input.StartsWith("q"))
                    break;
                string[] splitInput = input.Split(new char[]{' '});
                string[] arguments = new string[splitInput.Length - 1];
                for(int i = 1; i < splitInput.Length; i++)
                {
                    arguments[i - 1] = splitInput[i];
                }
                Result res = vm.ExecuteScript(splitInput[0], arguments);
                Console.WriteLine("Result> " + res.ToString());
            }
        }

        static void vm_Print(object sender, MonoKleScript.VM.Event.PrintEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        static void vm_RuntimeError(object sender, MonoKleScript.VM.Event.RuntimeErrorEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        static void c_CompilationError(object sender, CompilationErrorEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
