using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using MonoKle.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoKle.Console
{
    public class GameConsoleLogData
    {
        private readonly LinkedList<Entry> _entries = new();

        /// <summary>
        /// Sync modifications thread-safely with this.
        /// </summary>
        public object SyncRoot { get; } = new();

        public IEnumerable<Entry> Entries => _entries;

        /// <summary>
        /// Copies entries, sycing safely with <see cref="SyncRoot"/>.
        /// </summary>
        public IEnumerable<Entry> CopyEntries()
        {
            lock (SyncRoot)
            {
                return _entries.ToList();
            }
        }

        [CVar("console_tabLength")]
        public int TabLength { get; set; } = 4;

        [CVar("console_capacity")]
        public uint Capacity { get; set; } = byte.MaxValue * 2;

        [CVar("console_logLevel")]
        public LogLevel LogLevel { get; set; } = LogLevel.Information;

        public Color DefaultTextColour { get; set; } = Color.WhiteSmoke;

        public Color DisabledTextColour { get; set; } = Color.Gray;

        public Color WarningTextColour { get; set; } = Color.Yellow;

        public Color ErrorTextColour { get; set; } = Color.Red;

        public int Count => _entries.Count;

        public void AddError(string message) => AddLine(message, ErrorTextColour);

        public void AddWarning(string message) => AddLine(message, WarningTextColour);

        public void AddLine(string text) => AddLine(text, DefaultTextColour);

        public void AddLine(string text, Color color)
        {
            lock (SyncRoot)
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

                    _entries.AddFirst(new Entry(entryBuilder.ToString(), color));

                    while (_entries.Count > Capacity)
                    {
                        _entries.RemoveLast();
                    }
                }
            }
        }

        public void Clear()
        {
            lock (SyncRoot)
            {
                _entries.Clear();
            }
        }

        public record class Entry(string Text, Color Color);
    }
}
