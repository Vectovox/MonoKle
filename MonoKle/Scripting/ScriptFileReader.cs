namespace MonoKle.Scripting
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    using MonoKle.Scripting.Script;

    internal class ScriptFileReader
    {
        private string currentPath = "";

        public IEnumerable<Source> GetScriptSources(string path, bool recurse)
        {
            LinkedList<Source> scripts = new LinkedList<Source>();

            if (File.Exists(path) && this.FileIsValid(path))
            {
                currentPath = path;
                foreach (Source s in this.ParseFile(new FileStream(path, FileMode.Open)))
                {
                    scripts.AddLast(s);
                }
            }
            else if (Directory.Exists(path))
            {
                foreach (string file in Directory.GetFiles(path))
                {
                    foreach (Source s in this.GetScriptSources(file, recurse))
                    {
                        scripts.AddLast(s);
                    }
                }

                if (recurse)
                {
                    foreach (string directory in Directory.GetDirectories(path))
                    {
                        foreach (Source s in this.GetScriptSources(directory, recurse))
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

        private IEnumerable<Source> ParseFile(Stream stream)
        {
            LinkedList<Source> sources = new LinkedList<Source>();
            StreamReader reader = new StreamReader(stream);

            Header currentHeader = new Header();
            StringBuilder currentSource = new StringBuilder();
            bool begun = false;

            while (reader.EndOfStream == false)
            {
                string line = reader.ReadLine().Trim();

                // Only read lines with text (or empty lines if within a script)
                if (line.Length > 0 || begun)
                {
                    // Skip commented lines completely
                    if (line.StartsWith(ScriptBase.SCRIPT_COMMENT) == false)
                    {
                        // If beginning / end / middle of script
                        if (Regex.IsMatch(line, ScriptBase.REGEX_START_MATCH))
                        {
                            // If already reading a script: give error
                            if (begun)
                            {
                                this.ReportError("Encountered header before end of script.");
                            }
                            else
                            {
                                begun = TryMakeHeader(line, out currentHeader);
                            }
                        }
                        else if (Regex.IsMatch(line, ScriptBase.REGEX_END_MATCH))
                        {
                            if (begun)
                            {
                                // Script complete
                                begun = false;
                                sources.AddLast(new Source(currentSource.ToString(), currentHeader));
                                currentSource.Clear();
                            }
                            else
                            {
                                this.ReportError("Encountered unexpected end of script.");
                            }
                        }
                        else if (begun)
                        {
                            // Only append script between header and end-of-script
                            currentSource.AppendLine(line);
                        }
                    }
                }
            }

            reader.Close();
            return sources;
        }

        private void ReportError(string message)
        {
            MonoKleGame.Logger.AddLog("Error in script file: " + message, Logging.LogLevel.Error);
        }

        private bool TryMakeHeader(string line, out Header header)
        {
            // Check for if the provided line is a valid header
            if (Regex.IsMatch(line, ScriptBase.SCRIPT_HEADER_SPECIFICATION_REGEX))
            {
                Type returnType = Type.GetType(ScriptBase.TypeAlias(Regex.Match(line, ScriptBase.SCRIPT_HEADER_TYPE_MATCH_REGEX).Value));
                if (returnType != null)
                {
                    string name = Regex.Match(line, ScriptBase.SCRIPT_HEADER_NAME_MATCH_REGEX).Value;
                    string argumentString = Regex.Match(line, ScriptBase.SCRIPT_HEADER_ARGUMENTS_MATCH_REGEX).Value;
                    Match channelMatch = Regex.Match(line, ScriptBase.SCRIPT_HEADER_CHANNEL_MATCH_REGEX);
                    string channel = channelMatch.Success ? channelMatch.Value : null;
                    LinkedList<Argument> arguments = new LinkedList<Argument>();

                    if (argumentString.Length <= ScriptBase.SCRIPT_MAX_ARGUMENTS)
                    {
                        if (argumentString.Length > 0)
                        {
                            foreach (string s in Regex.Split(argumentString, ScriptBase.SCRIPT_ARGUMENT_SEPARATOR))
                            {
                                string[] sArray = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                string argName = sArray[1].Trim();
                                Type argType = Type.GetType(ScriptBase.TypeAlias(sArray[0].Trim()));
                                arguments.AddLast(new Argument(argName, argType));
                            }
                        }

                        header = new Header(name, returnType, channel, arguments.ToArray());
                        return true;   
                    }
                    else
                    {
                        this.ReportError("Too many arguments in header.");
                    }
                }
                else
                {
                    this.ReportError("Invalid return type specified in header.");
                }
            }
            else
            {
                this.ReportError("Header not correctly specified.");
            }

            header = new Header();
            return false;
        }
    }
}