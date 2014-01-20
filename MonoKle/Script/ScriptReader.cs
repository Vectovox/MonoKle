namespace MonoKle.Script
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    internal class ScriptReader
    {
        internal ScriptReader()
        {

        }

        public IEnumerable<string> GetScripts(string path, bool recurse)
        {
            LinkedList<string> scripts = new LinkedList<string>();

            if (File.Exists(path) && this.FileIsValid(path))
            {
                foreach (string s in this.ParseFile(new FileStream(path, FileMode.Open)))
                {
                    scripts.AddLast(s);
                }
            }
            else if (Directory.Exists(path))
            {
                foreach(string file in Directory.GetFiles(path))
                {
                    foreach(string s in this.GetScripts(file, recurse))
                    {
                        scripts.AddLast(s);
                    }
                }

                if (recurse)
                {
                    foreach (string directory in Directory.GetDirectories(path))
                    {
                        foreach (string s in this.GetScripts(directory, recurse))
                        {
                            scripts.AddLast(s);
                        }
                    }
                }
            }

            return scripts;
        }

        private bool FileIsValid(string path)
        {
            return path.EndsWith(ScriptBase.SCRIPT_EXTENSION, StringComparison.CurrentCultureIgnoreCase);
        }

        private IEnumerable<string> ParseFile(Stream stream)
        {
            LinkedList<string> scripts = new LinkedList<string>();
            StreamReader reader = new StreamReader(stream);

            StringBuilder currentText = new StringBuilder();
            int nBegin = 0;
            int nEnd = 0;

            while(reader.EndOfStream == false)
            {
                string line = reader.ReadLine().Trim();
                if (line.Length > 0 || nBegin != nEnd)
                {
                    if (Regex.IsMatch(line, ScriptBase.REGEX_START_MATCH))
                    {
                        nBegin++;
                    }
                    else if (Regex.IsMatch(line, ScriptBase.REGEX_END_MATCH))
                    {
                        nEnd++;
                    }

                    if (nBegin > 0 && nBegin == nEnd)
                    {
                        currentText.Append(line);
                        scripts.AddLast(currentText.ToString());
                        currentText.Clear();
                        nBegin = 0;
                        nEnd = 0;
                    }
                    else
                    {
                        currentText.AppendLine(line);
                    }
                }
            }

            reader.Close();
            return scripts;
        }

    }
}
