using MonoKle.Scripting.Script;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MonoKle.Scripting
{
    internal class CompilerEngine
    {
        private LinkedList<Source> sources = new LinkedList<Source>();
        private Dictionary<string, Header> headerByName = new Dictionary<string, Header>();
        private Compiler scriptCompiler = new Compiler();

        public bool AddSource(Source source)
        {
            return this.AddSources(new Source[] { source }) != 0;
        }

        public int AddSources(IEnumerable<Source> sources)
        {
            int added = 0;
            foreach (Source source in sources)
            {
                if (this.headerByName.ContainsKey(source.Header.name) == false)
                {
                    this.headerByName.Add(source.Header.name, source.Header);
                    this.sources.AddLast(source);
                    added++;
                }
                else
                {
                    this.ReportHeaderError("Existing script with the same name is conflicting with " + source.Header.name + ".");
                }
            }
            return added;
        }

        public int ClearSources()
        {
            int ret = sources.Count;
            sources.Clear();
            return ret;
        }

        public int GetSourcesAmount()
        {
            return sources.Count;
        }

        public IEnumerable<ByteScript> Compile(out int failures)
        {
            LinkedList<ByteScript> ret = new LinkedList<ByteScript>();
            int fails = 0;

            foreach(Source source in sources)
            {
                ByteScript script = scriptCompiler.Compile(source, headerByName);
                if (script == null)
                {
                    fails++;
                }
                else
                {
                    ret.AddLast(script);
                }
            }

            failures = fails;
            return ret;
        }

        private void ReportHeaderError(string message)
        {
            MonoKleGame.Logger.AddLog("Source not added due to header error: " + message, Logging.LogLevel.Error);
        }
    }
}
