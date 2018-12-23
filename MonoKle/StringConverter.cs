using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MonoKle {
    // TODO: What is this?
    public class StringConverter {
        public object ToAny(string s) {

            if (int.TryParse(s, out var intVal)) {
                return intVal;
            }
            if (float.TryParse(s, NumberStyles.Float | NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo, out var floatVal)) {
                return floatVal;
            }
            if (bool.TryParse(s, out var boolVal)) {
                return boolVal;
            }
            if (MVector2.TryParse(s, out MVector2 mvec2Val)) {
                return mvec2Val;
            }
            if (MPoint2.TryParse(s, out MPoint2 mpoint2Val)) {
                return mpoint2Val;
            }

            Match stringMatch = Regex.Match(s, "^\".*\"$");
            if (stringMatch.Success) {
                return s.Substring(1, s.Length - 2);
            }

            return null;
        }

        public object ToType(string s, Type type) {
            if (type == typeof(MVector2)) {
                return ToMVector2(s);
            } else if (type == typeof(MPoint2)) {
                return ToMPoint2(s);
            }
            return null;
        }

        public MVector2 ToMVector2(string s) => MVector2.Parse(s);

        public MPoint2 ToMPoint2(string s) => MPoint2.Parse(s);
    }
}
