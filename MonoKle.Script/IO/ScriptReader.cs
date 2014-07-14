namespace MonoKle.Script.IO
{
    using MonoKle.Script.Common.Internal;
    using MonoKle.Script.Common.Script;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Class for reading scripts from sources.
    /// </summary>
    public class ScriptReader : IScriptReader
    {
        /// <summary>
        /// File extension to look for when reading scripts from files.
        /// </summary>
        public string FileExtension { get; set; }

        /// <summary>
        /// Default file extension used.
        /// </summary>
        public const string DEFAULT_SCRIPT_EXTENSION = Constants.FILE_EXTENSION;

        /// <summary>
        /// Creates a new instance of <see cref="ScriptReader"/>.
        /// </summary>
        public ScriptReader()
        {
            this.FileExtension = ScriptReader.DEFAULT_SCRIPT_EXTENSION;
        }

        /// <summary>
        /// Reads all script sources found in the given string.
        /// </summary>
        /// <param name="scriptString">The string to read scripts from.</param>
        /// <returns>Script reading result.</returns>
        public IScriptReaderResult ReadScriptSources(string scriptString)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(scriptString);
                    writer.Flush();
                    stream.Position = 0;
                    return new ScriptReaderResult(this.ReadStream(stream), this.ConsumeErrors());
                }
            }
        }

        /// <summary>
        /// Gets all script sources found in the given stream. The given stream is not closed.
        /// </summary>
        /// <param name="stream">The stream to get scripts from. It is not closed by the operation.</param>
        /// <returns>Script reading result.</returns>
        public IScriptReaderResult ReadScriptSources(Stream stream)
        {
            return new ScriptReaderResult(this.ReadStream(stream), this.ConsumeErrors());
        }

        /// <summary>
        /// Gets all script sources found searching in the given path.
        /// </summary>
        /// <param name="path">Path to search in. May point out a directory or a file.</param>
        /// <param name="recurse">If true, search will include sub-directories.</param>
        /// <returns>Script reading result.</returns>
        public IScriptReaderResult ReadScriptSources(string path, bool recurse)
        {
            ICollection<ScriptSource> scripts = new LinkedList<ScriptSource>();

            if(File.Exists(path))
            {
                foreach(ScriptSource s in this.ReadStream(new FileStream(path, FileMode.Open)))
                {
                    scripts.Add(s);
                }
            }
            else
            {
                string[] files = Directory.GetFiles(path, "*." + this.FileExtension, recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                foreach(string file in files)
                {
                    foreach(ScriptSource s in this.ReadStream(new FileStream(file, FileMode.Open)))
                    {
                        scripts.Add(s);
                    }
                }
            }

            return new ScriptReaderResult(scripts, this.ConsumeErrors());
        }

        private ICollection<ScriptSource> ReadStream(Stream stream)
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
                    if (line.StartsWith(ScriptSyntaxConstants.SCRIPT_COMMENT) == false)
                    {
                        // If beginning / end / middle of script
                        if (Regex.IsMatch(line, ScriptSyntaxConstants.REGEX_START_MATCH))
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
                        else if (Regex.IsMatch(line, ScriptSyntaxConstants.REGEX_END_MATCH))
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
            this.errorList.Add(message);
        }

        private ICollection<string> ConsumeErrors()
        {
            ICollection<string> ret = new List<string>(errorList);
            this.errorList.Clear();
            return ret;
        }

        private List<string> errorList = new List<string>();

        private bool TryMakeHeader(string line, out ScriptHeader header)
        {
            header = null;

            // Check for if the provided line is a valid header
            if (Regex.IsMatch(line, ScriptSyntaxConstants.SCRIPT_HEADER_SPECIFICATION_REGEX))
            {
                Type returnType = CommonHelpers.StringTypeToType(Regex.Match(line, ScriptSyntaxConstants.SCRIPT_HEADER_TYPE_MATCH_REGEX).Value);
                if (returnType != null)
                {
                    string name = Regex.Match(line, ScriptSyntaxConstants.SCRIPT_HEADER_NAME_MATCH_REGEX).Value;
                    string argumentString = Regex.Match(line, ScriptSyntaxConstants.SCRIPT_HEADER_ARGUMENTS_MATCH_REGEX).Value;
                    Match channelMatch = Regex.Match(line, ScriptSyntaxConstants.SCRIPT_HEADER_CHANNEL_MATCH_REGEX);
                    string channel = channelMatch.Success ? channelMatch.Value : null;
                    LinkedList<ScriptVariable> arguments = new LinkedList<ScriptVariable>();

                    if (argumentString.Length <= ScriptSettingsConstants.SCRIPT_MAX_ARGUMENTS)
                    {
                        if (argumentString.Length > 0)
                        {
                            foreach (string s in Regex.Split(argumentString, ScriptSyntaxConstants.SCRIPT_ARGUMENT_SEPARATOR))
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