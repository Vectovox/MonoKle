namespace GrammarTest
{
    using MonoKleScript.Common.Script;
    using MonoKleScript.Compiler;
    using MonoKleScript.Compiler.Event;
    using MonoKleScript.IO;
    using MonoKleScript.VM;
    using System;
    using System.Collections.Generic;

    public struct TestStruct
    {
        public int x;
        public int y;
    }

    public class TestClass
    {

        public TestStruct field;
        public int Property
        { get; set; }

        public int plainIntField = 8;

        private int toSet = 0;

        public void Setter(int value)
        {
            this.toSet = value;
        }

        public int Getter()
        {
            return this.toSet;
        }

        public int Add(int a, int b)
        {
            return a + b;
        }

        public TestClass()
        {
            this.Property = 3;
            this.field.x = 1;
            this.field.y = 2;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ScriptFileReader reader = new ScriptFileReader();
            reader.ScriptReadingError +=Reader_ScriptReadingError;
            ICollection<ScriptSource> sources = reader.GetScriptSources("./", false);

            //foreach( ScriptSource s in sources )
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

            vm.ExecuteScript("RunTests", new object[] { new TestClass() });

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

        private static void Reader_ScriptReadingError(object sender, MonoKleScript.IO.Event.ScriptReadingErrorEventArgs e)
        {
            Console.WriteLine(e.Message);
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
