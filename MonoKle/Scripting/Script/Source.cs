namespace MonoKle.Scripting.Script
{
    internal class Source
    {
        public Header Header { get; private set; }
        public string Text { get; private set; }

        public Source(string text, Header header)
        {
            this.Text = text;
            this.Header = header;
        }
    }
}
