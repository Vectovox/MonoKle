using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MonoKleScript.Compiler;
using MonoKleScript.Script;
using MonoKleScript.IO;
using MonoKleScript.Compiler.Error;

namespace GrammarTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ScriptFileReader reader = new ScriptFileReader();
            ICollection<ScriptSource> sources = reader.GetScriptSources("./", false);

            ScriptCompiler c = new ScriptCompiler();
            c.CompilationError += c_CompilationError;
            CompilationEnvironment environment = new CompilationEnvironment(c);

            environment.LoadSources(sources);
            var v = environment.Compile();

            Console.WriteLine("> " + v.Count + " scripts successfully compiled. " + (sources.Count - v.Count) + " failed.");

            //MonoKleScript.Debug.LanguageDebugger.PrintLexerTokens(source);
            //MonoKleScript.Debug.LanguageDebugger.PrintParserTree(source);
            //MonoKleScript.Debug.LanguageDebugger.PrintParserTokens(source);

            Console.Read();
        }

        static void c_CompilationError(object sender, CompilationErrorEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
