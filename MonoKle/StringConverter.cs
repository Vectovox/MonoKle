using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MonoKle
{
    // TODO: What is this?
    public class StringConverter
    {
        public object ToAny(string s)
        {

            if (int.TryParse(s, out var intVal))
            {
                return intVal;
            }
            if (float.TryParse(s, NumberStyles.Float | NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo, out var floatVal))
            {
                return floatVal;
            }
            if (bool.TryParse(s, out var boolVal))
            {
                return boolVal;
            }
            if (MVector2.TryParse(s, out MVector2 mvec2Val))
            {
                return mvec2Val;
            }
            if (MPoint2.TryParse(s, out MPoint2 mpoint2Val))
            {
                return mpoint2Val;
            }

            Match stringMatch = Regex.Match(s, "^\".*\"$");
            if (stringMatch.Success)
            {
                return s.Substring(1, s.Length - 2);
            }

            return null;
        }
    }
}
