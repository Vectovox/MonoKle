namespace MonoKle.Variable
{
    using IO;
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Class for loading variables.
    /// </summary>
    public class VariablePopulator : AbstractFileLoader
    {
        /// <summary>
        /// The token for commented lines.
        /// </summary>
        public const string CommentedLineToken = "//";

        /// <summary>
        /// The variable value divisor.
        /// </summary>
        public const char VariableValueDivisor = '=';

        private VariableSystem system;

        /// <summary>
        /// Initializes a new instance of the <see cref="VariablePopulator"/> class.
        /// </summary>
        /// <param name="system">The system to populate.</param>
        public VariablePopulator(VariableSystem system)
        {
            this.system = system;
        }

        /// <summary>
        /// Loads a variable from the given strings.
        /// </summary>
        /// <param name="identifier">The variable identifier.</param>
        /// <param name="value">The value.</param>
        /// <returns>True if successful; otherwise false.</returns>
        public bool LoadItem(string identifier, string value)
        {
            return this.InterpretLine(identifier + VariablePopulator.VariableValueDivisor + value);
        }

        /// <summary>
        /// Loads variables from the given text.
        /// </summary>
        /// <param name="text">The text.</param>
        public void LoadText(string text)
        {
            this.InterpretText(text);
        }

        protected override bool OperateOnFile(Stream fileStream, string filePath)
        {
            this.OperateOnStream(fileStream);
            return true;
        }

        private bool InterpretLine(string line)
        {
            line = line.Trim();
            if (line.StartsWith(VariablePopulator.CommentedLineToken))
            {
                return true;
            }

            string[] parts = line.Split(VariablePopulator.VariableValueDivisor);
            StringConverter sc = new StringConverter();

            if (parts.Length == 2)
            {
                object value = null;
                string valueText = parts[1].Trim();

                Match stringMatch = Regex.Match(valueText, "^\".*\"$");
                Match intMatch = Regex.Match(valueText, "^-?\\d+$");
                Match floatMatch = Regex.Match(valueText, "^-?(\\d*[.])?\\d+$");
                Match boolMatch = Regex.Match(valueText, "^true|false$", RegexOptions.IgnoreCase);

                if (stringMatch.Success)
                {
                    value = valueText.Substring(1, valueText.Length - 2);
                }
                else if (intMatch.Success)
                {
                    value = Convert.ToInt32(valueText, NumberFormatInfo.InvariantInfo);
                }
                else if (floatMatch.Success)
                {
                    value = Convert.ToSingle(valueText, NumberFormatInfo.InvariantInfo);
                }
                else if (boolMatch.Success)
                {
                    value = Convert.ToBoolean(valueText);
                }
                else
                {
                    return false;
                }

                this.system.SetValue(parts[0].Trim(), value);
                return true;
            }

            return false;
        }

        private void InterpretText(string text)
        {
            StringReader sr = new StringReader(text);
            while (sr.Peek() != -1)
            {
                this.InterpretLine(sr.ReadLine());
            }
        }

        private void OperateOnStream(Stream stream)
        {
            StreamReader sr = new StreamReader(stream);
            while (sr.EndOfStream == false)
            {
                this.InterpretLine(sr.ReadLine());
            }
        }
    }
}