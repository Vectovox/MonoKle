namespace MonoKle.IO
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// A class for writing textually defined structs to stream.
    /// </summary>
    /// <typeparam name="T">Type of struct</typeparam>
    public class StreamStructWriter<T> : IDisposable
        where T : struct
    {
        private StreamWriter writer;

        /// <summary>
        /// Initiates a new instance of <see cref="StreamStructWriter{T}"/>. Assumes control of the stream.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        public StreamStructWriter(Stream stream)
        {
            this.writer = new StreamWriter(stream, Encoding.Unicode);
        }

        /// <summary>
        /// Closes the <see cref="StreamStructWriter{T}"/> and its underlying stream.
        /// </summary>
        public void Close()
        {
            this.writer.Close();
        }

        /// <summary>
        /// Disposes the <see cref="StreamStructWriter{T}"/> and its underlying stream.
        /// </summary>
        public void Dispose()
        {
            this.writer.Dispose();
        }

        /// <summary>
        /// Writes a shell of struct type to stream. That is, a complete struct entry without any data.
        /// </summary>
        public void WriteShell()
        {
            this.writer.Write(this.FormatData(new T(), true));
        }

        /// <summary>
        /// Writes the provided struct to stream.
        /// </summary>
        /// <param name="source">The struct to write.</param>
        public void WriteStruct(T source)
        {
            this.writer.Write(this.FormatData(source, false));
        }

        /// <summary>
        /// Writes the provided structs to stream.
        /// </summary>
        /// <param name="sources">The structs to write.</param>
        public void WriteStructs(IEnumerable<T> sources)
        {
            foreach(T source in sources)
            {
                WriteStruct(source);
            }
        }

        private string FormatData(object source, bool shellOnly)
        {
            StringBuilder dataBuilder = new StringBuilder();
            List<string> values = this.GetValues(source, shellOnly);

            dataBuilder.AppendLine(StructIOConstants.ENTRY_START);
            foreach(string s in values)
            {
                dataBuilder.Append(StructIOConstants.FIELD_START);
                dataBuilder.Append(s);
                dataBuilder.AppendLine(StructIOConstants.FIELD_END);
            }
            dataBuilder.AppendLine(StructIOConstants.ENTRY_END);

            return dataBuilder.ToString();
        }

        private List<string> GetValues(object source, bool shellOnly)
        {
            Type type = source.GetType();
            FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            List<string> ret = new List<string>();
            StringBuilder lineBuilder = new StringBuilder();

            foreach(FieldInfo f in fields)
            {
                if(f.FieldType.IsValueType && f.FieldType.IsEnum == false && f.FieldType.IsPrimitive == false)
                {
                    List<string> values = this.GetValues(f.GetValue(source), shellOnly);
                    foreach(string s in values)
                    {
                        lineBuilder.Clear();
                        lineBuilder.Append(f.Name);
                        lineBuilder.Append(StructIOConstants.FIELD_NESTING_SEPARATOR);
                        lineBuilder.Append(s);
                        ret.Add(lineBuilder.ToString());
                    }
                }
                else
                {
                    lineBuilder.Clear();
                    lineBuilder.Append(f.Name);
                    lineBuilder.Append(StructIOConstants.FIELD_NAMEVALUE_SEPARATOR);
                    lineBuilder.Append(StructIOConstants.FIELD_VALUE_GROUPING);
                    if(shellOnly == false)
                    {
                        lineBuilder.Append(f.GetValue(source));
                    }
                    lineBuilder.Append(StructIOConstants.FIELD_VALUE_GROUPING);
                    ret.Add(lineBuilder.ToString());
                }
            }

            foreach(PropertyInfo f in properties)
            {
                if(f.GetSetMethod(true) != null)
                {
                    if(f.PropertyType.IsValueType && f.PropertyType.IsEnum == false && f.PropertyType.IsPrimitive == false)
                    {
                        List<string> values = this.GetValues(f.GetValue(source, null), shellOnly);
                        foreach(string s in values)
                        {
                            lineBuilder.Clear();
                            lineBuilder.Append(f.Name);
                            lineBuilder.Append(StructIOConstants.FIELD_NESTING_SEPARATOR);
                            lineBuilder.Append(s);
                            ret.Add(lineBuilder.ToString());
                        }
                    }
                    else
                    {
                        lineBuilder.Clear();
                        lineBuilder.Append(f.Name);
                        lineBuilder.Append(StructIOConstants.FIELD_NAMEVALUE_SEPARATOR);
                        lineBuilder.Append(StructIOConstants.FIELD_VALUE_GROUPING);
                        if(shellOnly == false)
                        {
                            lineBuilder.Append(f.GetValue(source, null));
                        }
                        lineBuilder.Append(StructIOConstants.FIELD_VALUE_GROUPING);
                        ret.Add(lineBuilder.ToString());
                    }
                }
            }

            return ret;
        }
    }
}