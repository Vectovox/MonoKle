namespace MonoKle.Script.IO
{
    using MonoKle.Script.Common.Internal;
    using MonoKle.Script.Common.Script;
    using MonoKle.Script.IO.Event;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public class ScriptFileReader : IScriptFileReader
    {
        /// <summary>
        /// File extension to look for when reading scripts.
        /// </summary>
        public string FileExtension { get; set; }

        /// <summary>
        /// Event fired if there is an error reading a script.
        /// </summary>
        public event ScriptReadingErrorEventHandler ScriptReadingError;

        /// <summary>
        /// Default file extension used.
        /// </summary>
        public const string DEFAULT_SCRIPT_EXTENSION = ".ms";

        /// <summary>
        /// Creates a new instance of <see cref="ScriptFileReader"/>.
        /// </summary>
        public ScriptFileReader()
        {
            this.FileExtension = ScriptFileReader.DEFAULT_SCRIPT_EXTENSION;
        }

        /// <summary>
        /// Gets all script sources found searching in the given path.
        /// </summary>
        /// <param name="path">Path to search in. May point out a directory or a file.</param>
        /// <param name="recurse">If true, search will include sub-directories.</param>
        /// <returns>Collection of script sources.</returns>
        public ICollection<ScriptSource> GetScriptSources(string path, bool recurse)
        {
            ICollection<ScriptSource> scripts = new LinkedList<ScriptSource>();

            if (File.Exists(path) && this.ExtensionValid(path))
            {
                foreach (ScriptSource s in this.ParseFile(new FileStream(path, FileMode.Open)))
                {
                    scripts.Add(s);
                }
            }
            else if (Directory.Exists(path))
            {
                foreach (string file in Directory.GetFiles(path))
                {
                    foreach (ScriptSource s in this.GetScriptSources(file, recurse))
                    {
                        scripts.Add(s);
                    }
                }

                if (recurse)
                {
                    foreach (string directory in Directory.GetDirectories(path))
                    {
                        foreach (ScriptSource s in this.GetScriptSources(directory, recurse))
                        {
                            scripts.Add(s);
                        }
                    }
                }
            }

            return scripts;
        }

        private bool ExtensionValid(string path)
        {
            return path.EndsWith(this.FileExtension, StringComparison.CurrentCultureIgnoreCase);
        }

        private IEnumerable<ScriptSource> ParseFile(Stream stream)
        {
            ICollection<ScriptSource> sources = new LinkedList<ScriptSource>();
            StreamReader reader = new StreamReader(stream);

            ScriptHeader currentHeader = null;
            StringBuilder currentSource = new StringBuilder();
            bool begun = false;

            while (reader.EndOfStream == false)
            {
                string line = reader.ReadLine().Trim();

                // Only read lines with text (or empty lines if within a script)
                if (line.Length > 0 || begun)
                {
                    // Skip commented lines completely
                    if (line.StartsWith(ScriptNamingConstants.SCRIPT_COMMENT) == false)
                    {
                        // If beginning / end / middle of script
                        if (Regex.IsMatch(line, ScriptNamingConstants.REGEX_START_MATCH))
                        {
                            // If already reading a script: give error
                            if (begun)
                            {
                                this.OnError("Encountered header before end of script.");
                            }
                            else
                            {
                                begun = TryMakeHeader(line, out currentHeader);
                            }
                        }
                        else if (Regex.IsMatch(line, ScriptNamingConstants.REGEX_END_MATCH))
                        {
                            if (begun)
                            {
                                // Script complete
                                begun = false;
                                sources.Add(new ScriptSource(currentSource.ToString(), currentHeader));
                                currentSource.Clear();
                            }
                            else
                            {
                                this.OnError("Encountered unexpected end of script.");
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

        private void OnError(string message)
        {
            var l = this.ScriptReadingError;
            if (l != null)
            {
                l(this, new ScriptReadingErrorEventArgs(message));
            }
        }

        private bool TryMakeHeader(string line, out ScriptHeader header)
        {
            header = null;

            // Check for if the provided line is a valid header
            if (Regex.IsMatch(line, ScriptNamingConstants.SCRIPT_HEADER_SPECIFICATION_REGEX))
            {
                Type returnType = CommonHelpers.StringTypeToType(Regex.Match(line, ScriptNamingConstants.SCRIPT_HEADER_TYPE_MATCH_REGEX).Value);
                if (returnType != null)
                {
                    string name = Regex.Match(line, ScriptNamingConstants.SCRIPT_HEADER_NAME_MATCH_REGEX).Value;
                    string argumentString = Regex.Match(line, ScriptNamingConstants.SCRIPT_HEADER_ARGUMENTS_MATCH_REGEX).Value;
                    Match channelMatch = Regex.Match(line, ScriptNamingConstants.SCRIPT_HEADER_CHANNEL_MATCH_REGEX);
                    string channel = channelMatch.Success ? channelMatch.Value : null;
                    LinkedList<ScriptVariable> arguments = new LinkedList<ScriptVariable>();

                    if (argumentString.Length <= ScriptSettingsConstants.SCRIPT_MAX_ARGUMENTS)
                    {
                        if (argumentString.Length > 0)
                        {
                            foreach (string s in Regex.Split(argumentString, ScriptNamingConstants.SCRIPT_ARGUMENT_SEPARATOR))
                            {
                                string[] sArray = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                string argName = sArray[1].Trim();
                                Type argType = CommonHelpers.StringTypeToType(sArray[0].Trim());
                                arguments.AddLast(new ScriptVariable(argName, argType));
                            }
                        }

                        header = new ScriptHeader(name, returnType, channel, arguments.ToArray());
                        return true;   
                    }
                    else
                    {
                        this.OnError("Too many arguments in header.");
                    }
                }
                else
                {
                    this.OnError("Invalid return type specified in header.");
                }
            }
            else
            {
                this.OnError("Header not correctly specified.");
            }

            return false;
        }
    }
}