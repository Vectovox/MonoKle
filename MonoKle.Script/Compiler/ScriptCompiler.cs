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
        /// Compiles the provided script source into a bytecode script.
        /// </summary>
        /// <param name="source">Source of script.</param>
        /// <param name="knownScripts">Headers for other scripts to know about.</param>
        /// <returns>Result of compilation.</returns>
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

            return new CompilationResult(source.Header.Name, syntaxError, semanticsError, byteScript, errorMessageCollection);
        }

        /// <summary>
        /// Checks whether the syntax of a script is valid.
        /// </summary>
        /// <param name="source">Soure to check syntax on.</param>
        /// <returns>True if valid, else false.</returns>
        public ISyntaxResult CheckSyntax(ScriptSource source)
        {
            // Set up lexer and parser
            AntlrInputStream stream = new AntlrInputStream(source.Text);
            MonoKleScriptLexer lexer = new MonoKleScriptLexer(stream);
            CommonTokenStream tokenStream = new CommonTokenStream(lexer);
            MonoKleScriptParser parser = new MonoKleScriptParser(tokenStream);
            SyntaxErrorListener syntaxErrorListener = new SyntaxErrorListener();

            // Remove console output and add our own listener for parser errors.
            parser.RemoveErrorListeners();
            lexer.RemoveErrorListeners();
            parser.AddErrorListener(syntaxErrorListener);
            lexer.AddErrorListener(new LexerErrorListener());

            // Parse and set start context for walkers
            MonoKleScriptParser.ScriptContext context = parser.script();

            return new SyntaxResult(source.Header.Name, syntaxErrorListener.GetErrorMessages());
        }

        /// <summary>
        /// Checks if the provided script source is compilable, checking both syntax as well as semantics.
        /// </summary>
        /// <param name="source">Source to check if compilable.</param>
        /// <param name="knownScripts">Headers for other scripts to know about.</param>
        /// <returns>True if compilable, else false.</returns>
        public ICompilableResult CheckCompilable(ScriptSource source, ICollection<ScriptHeader> knownScripts)
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
            }

            return new CompilableResult(source.Header.Name, syntaxError, semanticsError, errorMessageCollection);
        }
    }
}