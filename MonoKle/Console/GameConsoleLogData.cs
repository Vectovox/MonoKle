using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Text;

namespace MonoKle.Console
{
    public class GameConsoleLogData
    {
        private readonly LinkedList<TextEntry> _textEntries = new();

        public IEnumerable<TextEntry> TextEntries => _textEntries;

        public int TabLength { get; set; } = 4;

        public uint Capacity { get; set; } = byte.MaxValue;

        public Color DefaultTextColour { get; set; } = Color.WhiteSmoke;

        public Color DisabledTextColour { get; set; } = Color.Gray;

        public Color WarningTextColour { get; set; } = Color.Yellow;

        public Color ErrorTextColour { get; set; } = Color.Red;

        public int Count => _textEntries.Count;

        public void WriteError(string message) => WriteLine(message, ErrorTextColour);

        public void WriteWarning(string message) => WriteLine(message, WarningTextColour);

        public void WriteLine(string text) => WriteLine(text, DefaultTextColour);

        public void WriteLine(string text, Color color)
        {
            var entryBuilder = new StringBuilder();

            // Convert string to entries, based on newlines
            foreach (var row in text.Split('\n'))
            {
                entryBuilder.Clear();
                var column = 0;

                // Add characters to entry
                foreach (char character in row)
                {
                    // Convert tab characters to spaces
                    if (character == '\t')
                    {
                        var spacesToAdd = TabLength - (column % TabLength);
                        entryBuilder.Append(' ', spacesToAdd);
                        column += spacesToAdd;
                    }
                    else
                    {
                        entryBuilder.Append(character);
                        column++;
                    }
                }

                _textEntries.AddFirst(new TextEntry(entryBuilder.ToString(), color));

                while (_textEntries.Count > Capacity)
                {
                    _textEntries.RemoveLast();
                }
            }
        }

        public void Clear() => _textEntries.Clear();

        public record class TextEntry (string Text, Color Color);
    }
}
