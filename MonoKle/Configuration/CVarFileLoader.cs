using MonoKle.IO;
using System.IO;

namespace MonoKle.Configuration
{
    /// <summary>
    /// Class for loading variables from file.
    /// </summary>
    public class CVarFileLoader : AbstractFileReader
    {
        /// <summary>
        /// The token for commented lines.
        /// </summary>
        public const string CommentedLineToken = "//";

        /// <summary>
        /// The variable value divisor.
        /// </summary>
        public const char VariableValueDivisor = '=';

        private CVarSystem system;

        /// <summary>
        /// Initializes a new instance of the <see cref="CVarFileLoader"/> class.
        /// </summary>
        /// <param name="system">The system to populate.</param>
        public CVarFileLoader(CVarSystem system)
        {
            this.system = system;
        }

        /// <summary>
        /// Loads a variable from the given strings.
        /// </summary>
        /// <param name="identifier">The variable identifier.</param>
        /// <param name="value">The value.</param>
        /// <returns>True if successful; otherwise false.</returns>
        public bool LoadItem(string identifier, string value) => InterpretLine(identifier + CVarFileLoader.VariableValueDivisor + value);

        /// <summary>
        /// Loads variables from the given text.
        /// </summary>
        /// <param name="text">The text.</param>
        public void LoadText(string text) => InterpretText(text);

        /// <summary>
        /// Template pattern method that is called on each file that is loaded. The stream is disposed after the call.
        /// </summary>
        /// <param name="fileStream">A filestream to a read file.</param>
        /// <param name="file">The read file.</param>
        /// <returns></returns>
        protected override bool ReadFile(Stream fileStream, MFileInfo file)
        {
            OperateOnStream(fileStream);
            return true;
        }

        private bool InterpretLine(string line)
        {
            line = line.Trim();
            if (line.StartsWith(CVarFileLoader.CommentedLineToken))
            {
                return true;
            }

            string[] parts = line.Split(CVarFileLoader.VariableValueDivisor);

            if (parts.Length == 2)
            {
                string variableText = parts[0].Trim();
                string valueText = parts[1].Trim();

                var sc = new StringConverter();
                object value = sc.ToAny(valueText);

                return system.SetValue(variableText, value);
            }

            return false;
        }

        private void InterpretText(string text)
        {
            var sr = new StringReader(text);
            while (sr.Peek() != -1)
            {
                InterpretLine(sr.ReadLine());
            }
        }

        private void OperateOnStream(Stream stream)
        {
            var sr = new StreamReader(stream);
            while (sr.EndOfStream == false)
            {
                InterpretLine(sr.ReadLine());
            }
        }
    }
}