namespace MonoKle.IO
{
    /// <summary>
    /// The constants used in reading and writing structs.
    /// </summary>
    internal class StructIOConstants
    {
        public const string REGEX_ENTRY = ENTRY_START + ".*?" + ENTRY_END;
        public const string ENTRY_END = "end";
        public const string ENTRY_START = "struct";
        public const string FIELD_START = "<";
        public const string FIELD_END = ">";
        public const string FIELD_NAMEVALUE_SEPARATOR = "=";
        public const string REGEX_FILEREPLACE = "(#.*?\n)|(\r)|(\n)";
        public const string REGEX_PARAMETER = FIELD_START + "\\s*.+?\\s*" + FIELD_NAMEVALUE_SEPARATOR +"\\s*.+?\\s*" + FIELD_END;
        public const string REGEX_PARAMETER_NAME = "(.*?" + FIELD_START + "\\s*)|(\\s*" + FIELD_NAMEVALUE_SEPARATOR + ".*?" + FIELD_END + ")";
        public const string REGEX_PARAMETER_VALUE = "(" + FIELD_START + ".*?" + FIELD_NAMEVALUE_SEPARATOR + "\\s+)|(\\s*" + FIELD_END + ")";
        public const string REGEX_VALUE_NUMBER = "\\d+";
    }
}
