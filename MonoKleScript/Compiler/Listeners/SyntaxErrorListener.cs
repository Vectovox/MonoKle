namespace MonoKle.Script.Compiler.Listeners
{
    using Antlr4.Runtime;
    using System.Collections.Generic;
    using System.Text;

    internal class SyntaxErrorListener : IAntlrErrorListener<IToken>
    {
        private LinkedList<string> errorList = new LinkedList<string>();
        public bool WasSuccessful() { return this.errorList.Count == 0; }
        public ICollection<string> GetErrorMessages() { return this.errorList; }

        public void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            StringBuilder message = new StringBuilder();
            message.Append("Syntax error on line [");
            message.Append(line);
            message.Append(",");
            message.Append(charPositionInLine);
            message.Append("]: ");
            message.Append(msg);
            this.errorList.AddLast(message.ToString());
        }
    }
}
