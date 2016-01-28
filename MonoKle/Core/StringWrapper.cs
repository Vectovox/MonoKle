namespace MonoKle.Core
{
    using MonoKle.Asset.Font;

    /// <summary>
    /// Class that wraps strings to fit with a specified size criteria.
    /// </summary>
    public class StringWrapper
    {
        /// <summary>
        /// Wraps a text to fit in a given width, placing linebreaks where applicable. Assuming a scale of 1.0f.
        /// </summary>
        /// <param name="text">The text to wrap</param>
        /// <param name="font">The font to use for measurements</param>
        /// <param name="maximumWidth">The maximum width allowed for the wrapped text</param>
        /// <returns>A string containing the wrapped text</returns>
        /// 
        public string WrapWidth(string text, Font font, float maximumWidth)
        {
            return this.WrapWidth(text, font, maximumWidth, 1f);
        }

        /// <summary>
        /// Wraps a text to fit in a given width, placing linebreaks where applicable.
        /// </summary>
        /// <param name="text">The text to wrap</param>
        /// <param name="font">The font to use for measurements</param>
        /// <param name="maximumWidth">The maximum width allowed for the wrapped text</param>
        /// <param name="scale">Scale of font to adjust for.</param>
        /// <returns>A string containing the wrapped text</returns>
        public string WrapWidth(string text, Font font, float maximumWidth, float scale)
        {
            float width = font.MeasureString(text, scale).X;
            if (width > maximumWidth)
            {
                int startPtr = 0;
                for (int i = 1; i < text.Length - startPtr; i++)
                {
                    if (text[startPtr + i].Equals('\n'))
                    {
                        startPtr += i;
                        i = 0;
                    }
                    else
                    {
                        float length = font.MeasureString(text.Substring(startPtr, i), scale).X;
                        if (length > maximumWidth)
                        {
                            int index = text.LastIndexOfAny(new char[] { ' ', '.', ',' }, startPtr + i - 1, i);
                            if (index != -1)
                            {
                                text = text.Remove(index, 1).Insert(index, "\n");
                            }
                            startPtr = index + 1;
                            i = 0;
                        }
                    }
                }
            }

            return text;
        }

    }
}