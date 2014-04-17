namespace MonoKle.Script.Compiler.Listeners
{
    using Antlr4.Runtime;

    internal class LexerErrorListener : IAntlrErrorListener<int>
    {
        public void SyntaxError(IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            // Does nothing for now.
        }
    }
}
