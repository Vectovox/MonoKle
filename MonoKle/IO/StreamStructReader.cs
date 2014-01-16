namespace MonoKle.IO
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// A class for reading textually defined structs from stream.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StreamStructReader<T>
        where T : struct
    {
        private bool couldReadStruct;
        private T currentStruct;
        private StreamReader reader;

        /// <summary>
        /// Initiates a new instance of <see cref="StreamStructReader{T}"/>. Assumes control of the stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        public StreamStructReader(Stream stream)
        {
            this.reader = new StreamReader(stream);
            this.ReadNextStruct();
        }

        /// <summary>
        /// Returns if there are available structs to read.
        /// </summary>
        /// <returns>True if there are available structs, otherwise false.</returns>
        public bool CanGetStruct()
        {
            return this.couldReadStruct;
        }

        /// <summary>
        /// Closes the <see cref="StreamStructReader{T}"/> and its underlying stream.
        /// </summary>
        public void Close()
        {
            this.reader.Close();
        }

        /// <summary>
        /// Gets the next struct available from the stream.
        /// </summary>
        /// <returns>The next available struct.</returns>
        public T GetNextStruct()
        {
            if (this.couldReadStruct)
            {
                T ret = this.currentStruct;
                this.ReadNextStruct();
                return ret;
            }
            throw new InvalidOperationException("There are no more structs to get.");
        }

        /// <summary>
        /// Gets all available structs from the stream.
        /// </summary>
        /// <returns>An IEnumerable containing the available structs.</returns>
        public IEnumerable<T> GetNextStructs()
        {
            if (this.couldReadStruct)
            {
                LinkedList<T> structs = new LinkedList<T>();
                while (this.couldReadStruct)
                {
                    structs.AddLast(currentStruct);
                    this.ReadNextStruct();
                }
                return structs;
            }
            throw new InvalidOperationException("There are no more structs to get.");
        }

        private void InsertValue(object on, object value, string fullName)
        {
            Type onType = on.GetType();

            int delimiter = fullName.IndexOf('.');
            if (delimiter != -1)
            {
                if (fullName.Length > delimiter)
                {
                    string memberName = fullName.Substring(0, delimiter);

                    FieldInfo field = onType.GetField(memberName);
                    if (field != null)
                    {
                        object newValue = field.GetValue(on);
                        this.InsertValue(newValue, value, fullName.Substring(delimiter + 1, fullName.Length - delimiter - 1));
                        field.SetValue(on, newValue);
                    }
                    else
                    {
                        PropertyInfo property = on.GetType().GetProperty(memberName);
                        if (property != null)
                        {
                            object newValue = property.GetValue(on, null);
                            this.InsertValue(newValue, value, fullName.Substring(delimiter + 1, fullName.Length - delimiter - 1));
                            property.SetValue(on, newValue, null);
                        }
                    }
                }
            }
            else
            {
                FieldInfo field = on.GetType().GetField(fullName);
                if (field != null)
                {
                    field.SetValue(on, Convert.ChangeType(value, field.FieldType));
                }
                else
                {
                    PropertyInfo property = on.GetType().GetProperty(fullName);
                    if (property != null)
                    {
                        property.SetValue(on, Convert.ChangeType(value, property.PropertyType), null);
                    }
                }
            }
        }

        private T PullStruct(string entry)
        {
            object ret = new T();   // Boxed since it is a struct
            string strippedEntry = Regex.Replace(entry, StructIOConstants.REGEX_FILEREPLACE, "");
            MatchCollection parameters = Regex.Matches(strippedEntry, StructIOConstants.REGEX_PARAMETER, RegexOptions.IgnoreCase);
            foreach (Match parameter in parameters)
            {
                string name = Regex.Replace(parameter.Value, StructIOConstants.REGEX_PARAMETER_NAME, "");
                string value = Regex.Replace(parameter.Value, StructIOConstants.REGEX_PARAMETER_VALUE, "");
                this.InsertValue(ret, value, name);
            }
            return (T)ret;          // Unboxed
        }

        private void ReadNextStruct()
        {
            this.couldReadStruct = false;

            StringBuilder currentStructText = new StringBuilder();
            bool hasBegin = false;

            while (this.reader.EndOfStream == false && this.couldReadStruct == false)
            {
                string line = this.reader.ReadLine().Trim();
                if (line.Length > 0 && line[0] != ';')
                {
                    if (Regex.IsMatch(line, StructIOConstants.ENTRY_START, RegexOptions.IgnoreCase))
                    {
                        currentStructText.Clear();
                        currentStructText.Append(line);
                        hasBegin = true;
                    }
                    else if (hasBegin && Regex.IsMatch(line, StructIOConstants.ENTRY_END, RegexOptions.IgnoreCase))
                    {
                        currentStructText.Append(line);
                        Match match = Regex.Match(currentStructText.ToString(), StructIOConstants.REGEX_ENTRY);
                        currentStructText.Clear();

                        this.currentStruct = PullStruct(match.Value);
                        this.couldReadStruct = true;
                    }
                    else if (hasBegin)
                    {
                        currentStructText.Append(line);
                    }
                }
            }
        }
    }
}