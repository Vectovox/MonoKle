namespace MonoKle.Console
{
    using Microsoft.Xna.Framework;

    internal class Line
    {
        public Line(string text, Color color)
        {
            this.Text = text;
            this.Color = color;
        }

        public Color Color { get; private set; }
        public string Text { get; private set; }
    }
}