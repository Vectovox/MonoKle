namespace MonoKle.Script.Compiler
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;
    using MonoKle.Script.Common.Script;
    using MonoKle.Script.Compiler.Listeners;
    using MonoKle.Script.Grammar;
    using System.Collections.Generic;
    using System;


    /// <summary>
    /// Compiler of MonoKleScript scripts.
    /// </summary>
    public class ScriptCompiler : IScriptCompiler
    {
        /// <summary>
        /// Compiles the provided script source into a bytecode script. Sets syntax and semantics error flags.
        /// </summary>
        /// <param name="source">Source of script.</param>
        /// <param name="knownScripts">Headers for other scripts to know about.</param>
        /// <returns>Compiled script if compilation was successful, otherwise null.</returns>
        public ICompilationResult Compile(ScriptSource source, ICollection<ScriptHeader> knownScripts)
        {
            // Error data
            bool syntaxError = false;
            bool semanticsError = false;
            ICollection<string> errorMessageCollection = new LinkedList<string>();

            // Set up lexer and parser
            AntlrInputStream stream = new AntlrInputStream(source.Text);
            MonoKleScriptLexer lexer = new MonoKleScriptLexer(stream);
            CommonTokenStream tokenStream = new CommonTokenStream(lexer);
            MonoKleScriptParser parser = new MonoKleScriptParser(tokenStream);

            // Remove console output and add our own listener for parser errors.
            SyntaxErrorListener syntaxListener = new SyntaxErrorListener();
            parser.RemoveErrorListeners();
            lexer.RemoveErrorListeners();
            parser.AddErrorListener(syntaxListener);
            lexer.AddErrorListener(new LexerErrorListener());

            // Parse and set start context for walkers
            MonoKleScriptParser.ScriptContext context = parser.script();
            syntaxError = syntaxListener.WasSuccessful() == false;

            ByteScript byteScript = null;
            if(syntaxError)
            {
                foreach(string m in syntaxListener.GetErrorMessages())
                {
                    errorMessageCollection.Add(m);
                }
            }
            else
            {
                // Set up walker and listeners
                ParseTreeWalker walker = new ParseTreeWalker();
                SemanticsListener semanticsListener = new SemanticsListener(source.Header, knownScripts);
                
                // Check semantics
                walker.Walk(semanticsListener, context);
                semanticsError = semanticsListener.WasSuccessful() == false;
                if(semanticsError)
                {
                    foreach(string m in semanticsListener.GetErrorMessages())
                    {
                        errorMessageCollection.Add(m);
                    }
                }
                else
                {
                    CompilerListener compilerListener = new CompilerListener(source.Header, knownScripts);
                    walker.Walk(compilerListener, context);
                    byteScript = new ByteScript(source.Header, compilerListener.GetByteCode());
                }
            }

            return new CompilationResult(source.Header.Name, semanticsError == false && syntaxError == false, syntaxError, semanticsError,
                byteScript, errorMessageCollection);
        }

        /// <summary>
        /// Checks whether the syntax of a script is valid.
        /// </summary>
        /// <param name="source">Soure to check syntax on.</param>
        /// <returns>True if valid, else false.</returns>
        public bool IsSyntaxValid(ScriptSource source)
        {
            // Set up lexer and parser
            AntlrInputStream stream = new AntlrInputStream(source.Text);
            MonoKleScriptLexer lexer = new MonoKleScriptLexer(stream);
            CommonTokenStream tokenStream = new CommonTokenStream(lexer);
            MonoKleScriptParser parser = new MonoKleScriptParser(tokenStream);

            // Remove console output and add our own listener for parser errors.
            parser.RemoveErrorListeners();
            lexer.RemoveErrorListeners();
            parser.AddErrorListener(new SyntaxErrorListener());
            lexer.AddErrorListener(new LexerErrorListener());

            // Parse and set start context for walkers
            MonoKleScriptParser.ScriptContext context = parser.script();

            return parser.NumberOfSyntaxErrors == 0;
        }

        /// <summary>
        /// Checks if the provided script source is compilable.
        /// </summary>
        /// <param name="source">Source to check if compilable.</param>
        /// <param name="knownScripts">Headers for other scripts to know about.</param>
        /// <returns>True if compilable, else false.</returns>
        public bool IsCompilable(ScriptSource source, ICollection<ScriptHeader> knownScripts)
        {
            // Set up lexer and parser
            AntlrInputStream stream = new AntlrInputStream(source.Text);
            MonoKleScriptLexer lexer = new MonoKleScriptLexer(stream);
            CommonTokenStream tokenStream = new CommonTokenStream(lexer);
            MonoKleScriptParser parser = new MonoKleScriptParser(tokenStream);

            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(new LexerErrorListener());

            // Remove console output and add our own listener for parser errors.
            parser.RemoveErrorListeners();
            lexer.RemoveErrorListeners();
            parser.AddErrorListener(new SyntaxErrorListener());
            lexer.AddErrorListener(new LexerErrorListener());

            // Parse and set start context for walkers
            MonoKleScriptParser.ScriptContext context = parser.script();

            if(parser.NumberOfSyntaxErrors == 0)
            {
                // Set up walker and listeners
                ParseTreeWalker walker = new ParseTreeWalker();
                SemanticsListener semanticsListener = new SemanticsListener(source.Header, knownScripts);
                
                // Check semantics
                walker.Walk(semanticsListener, context);
                return semanticsListener.WasSuccessful();
            }

            return false;
        }
    }
}