namespace MonoKle.IO
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// A class for writing textually defined structs to stream.
    /// </summary>
    /// <typeparam name="T">Type of struct</typeparam>
    public class StreamStructWriter<T>
        where T : struct
    {
        private StreamWriter writer;

        /// <summary>
        /// Initiates a new instance of <see cref="StreamStructWriter{T}"/>. Assumes control of the stream.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        public StreamStructWriter(Stream stream)
        {
            this.writer = new StreamWriter(stream);
        }

        /// <summary>
        /// Closes the <see cref="StreamStructWriter{T}"/> and its underlying stream.
        /// </summary>
        public void Close()
        {
            this.writer.Close();
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
        /// Writes a shell of struct type to stream. That is, a complete struct entry without any data.
        /// </summary>
        public void WriteShell()
        {
            this.writer.Write(this.FormatData(new T(), true));
        }

        private string FormatData(T source, bool shellOnly)
        {
            StringBuilder dataBuilder = new StringBuilder();
            dataBuilder.AppendLine(StructIOConstants.ENTRY_START);
            StreamStructWriter<Microsoft.Xna.Framework.Color> s;
            Type type = source.GetType();

            FieldInfo[] fields = type.GetFields();
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach(FieldInfo f in fields)
            {
                dataBuilder.Append(StructIOConstants.FIELD_START);
                dataBuilder.Append(f.Name);
                dataBuilder.Append(StructIOConstants.FIELD_NAMEVALUE_SEPARATOR);
                if (shellOnly == false)
                {
                    dataBuilder.Append(f.GetValue(source));
                }
                dataBuilder.AppendLine(StructIOConstants.FIELD_END);
            }

            foreach (PropertyInfo p in properties)
            {
                dataBuilder.Append(StructIOConstants.FIELD_START);
                dataBuilder.Append(p.Name);
                dataBuilder.Append(StructIOConstants.FIELD_NAMEVALUE_SEPARATOR);
                if (shellOnly == false)
                {
                    dataBuilder.Append(p.GetValue(source, null));
                }
                dataBuilder.AppendLine(StructIOConstants.FIELD_END);
            }

            dataBuilder.AppendLine(StructIOConstants.ENTRY_END);
            return dataBuilder.ToString();
        }

        /// <summary>
        /// Writes the provided structs to stream.
        /// </summary>
        /// <param name="sources">The structs to write.</param>
        public void WriteStructs(IEnumerable<T> sources)
        {
            foreach (T source in sources)
            {
                WriteStruct(source);
            }
        }
    }
}
