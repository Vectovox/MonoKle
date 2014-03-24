namespace MonoKleScript.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;

    using MonoKleScript.Grammar;
    using MonoKleScript.Script;

    /// <summary>
    /// Compiler of MonoKleScript scripts.
    /// </summary>
    public class ScriptCompiler
    {
        private bool semanticsError;
        private bool syntaxError;

        /// <summary>
        /// Compilation error, fired for both syntax and semantics errors.
        /// </summary>
        public event CompilationErrorEventHandler CompilationError;

        /// <summary>
        /// Compiles the provided script source into a bytecode script. Sets syntax and semantics error flags.
        /// </summary>
        /// <param name="source">Source of script.</param>
        /// <param name="knownScripts">Headers for other scripts to know about.</param>
        /// <returns>Compiled script if compilation was successful, otherwise null.</returns>
        public ByteScript Compile(ScriptSource source, ICollection<ScriptHeader> knownScripts)
        {
            // Reset error flags
            this.syntaxError = false;
            this.semanticsError = false;

            // Set up lexer and parser
            AntlrInputStream stream = new AntlrInputStream(source.Text);
            MonoKleScriptLexer lexer = new MonoKleScriptLexer(stream);
            CommonTokenStream tokenStream = new CommonTokenStream(lexer);
            MonoKleScriptParser parser = new MonoKleScriptParser(tokenStream);

            // Remove console output and add our own listener for syntax errors.
            parser.RemoveErrorListeners();
            parser.AddErrorListener(new SyntaxErrorListener(this));

            // Parse and set start context for walkers
            MonoKleScriptParser.ScriptContext context = parser.script();

            if (this.syntaxError == false)
            {
                // Set up walker and listeners
                ParseTreeWalker walker = new ParseTreeWalker();
                SemanticsListener semanticsListener = new SemanticsListener(source.Header.arguments);
                semanticsListener.SemanticsError += semanticsListener_SemanticsError;

                // Check semantics
                walker.Walk(semanticsListener, context);

                if (this.semanticsError == false)
                {
                    // TODO: Either compile with a compilation listener or fetch compiled data from semantics listener.
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the semantics error flag set by last compilation.
        /// </summary>
        /// <returns>True if there was a semantics error, else false.</returns>
        public bool GetSemanticsErrorFlag()
        {
            return this.semanticsError;
        }

        /// <summary>
        /// Returns the syntax error flag set by last compilation.
        /// </summary>
        /// <returns>True if there was a syntax error, else false.</returns>
        public bool GetSyntaxErrorFlag()
        {
            return this.syntaxError;
        }

        private void OnCompilationError(string message)
        {
            var l = CompilationError;
            l(this, new CompilationErrorEventArgs(message));
        }

        private void semanticsListener_SemanticsError(object sender, SemanticErrorEventArgs e)
        {
            this.semanticsError = true;
            this.OnCompilationError(e.Message);
        }

        /// <summary>
        /// Inner-class listening for syntax errors. Used instead of the compiler as listener in order to make the compiler class free from ANTLR from
        /// an external point of view.
        /// </summary>
        private class SyntaxErrorListener : IAntlrErrorListener<IToken>
        {
            private ScriptCompiler compiler;

            public SyntaxErrorListener(ScriptCompiler compiler)
            {
                this.compiler = compiler;
            }

            public void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Syntax error on line [");
                message.Append(line);
                message.Append(",");
                message.Append(charPositionInLine);
                message.Append("]: ");
                message.Append(msg);
                this.compiler.syntaxError = true;
                this.compiler.OnCompilationError(message.ToString());
            }
        }
    }
}