using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MonoKleScript.Compiler;
using MonoKleScript.Script;

namespace GrammarTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string source = @"int x : 5 int y : 5";
            
            ScriptCompiler c = new ScriptCompiler();
            c.CompilationError += c_CompilationError;
            c.Compile(new ScriptSource(source, new ScriptHeader("name", typeof(void), null, new ScriptVariable[0])), new LinkedList<ScriptHeader>());

            MonoKleScript.Debug.LanguageDebugger.PrintLexerTokens(source);
            MonoKleScript.Debug.LanguageDebugger.PrintParserTree(source);
            MonoKleScript.Debug.LanguageDebugger.PrintParserTokens(source);

            Console.Read();
        }

        static void c_CompilationError(object sender, CompilationErrorEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
