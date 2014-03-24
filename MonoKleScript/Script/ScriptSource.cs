namespace MonoKleScript.Script
{
    public class ScriptSource
    {
        public ScriptHeader Header { get; private set; }
        public string Text { get; private set; }

        public ScriptSource(string text, ScriptHeader header)
        {
            this.Text = text;
            this.Header = header;
        }
    }
}
