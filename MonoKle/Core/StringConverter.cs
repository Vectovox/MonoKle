namespace MonoKle.Core
{
    using Geometry;
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;

    public class StringConverter
    {
        public object ToAny(string s)
        {
            int intVal;
            float floatVal;
            bool boolVal;
            MVector2 mvec2Val;
            MPoint2 mpoint2Val;
            
            if (int.TryParse(s, out intVal))
            {
                return intVal;
            }
            if(float.TryParse(s, NumberStyles.Float | NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo, out floatVal))
            {
                return floatVal;
            }
            if (bool.TryParse(s, out boolVal))
            {
                return boolVal;
            }
            if (MVector2.TryParse(s, out mvec2Val))
            {
                return mvec2Val;
            }
            if (MPoint2.TryParse(s, out mpoint2Val))
            {
                return mpoint2Val;
            }

            Match stringMatch = Regex.Match(s, "^\".*\"$");
            if(stringMatch.Success)
            {
                return s.Substring(1, s.Length - 2);
            }

            return null;
        }

        public object ToType(string s, Type type)
        {
            if(type == typeof(MVector2))
            {
                return this.ToMVector2(s);
            }
            else if(type == typeof(MPoint2))
            {
                return this.ToMPoint2(s);
            }
            return null;
        }

        public MVector2 ToMVector2(string s)
        {
            return MVector2.Parse(s);
        }
        
        public MPoint2 ToMPoint2(string s)
        {
            return MPoint2.Parse(s);
        }
    }
}
