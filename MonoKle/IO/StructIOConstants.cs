namespace MonoKle.IO
{
    /// <summary>
    /// The constants used in reading and writing structs.
    /// </summary>
    internal class StructIOConstants
    {
        public const string COMMENT = ";";
        public const string ENTRY_END = "end";
        public const string ENTRY_START = "struct";
        public const string FIELD_END = ">";
        public const string FIELD_NAMEVALUE_SEPARATOR = "=";
        public const string FIELD_NESTING_SEPARATOR = ".";
        public const string FIELD_START = "<";
        public const string FIELD_VALUE_GROUPING = "\"";
        public const string REGEX_ENTRY = ENTRY_START + ".*?" + ENTRY_END;
        public const string REGEX_ENTRY_END = "(\\s+|^)" + ENTRY_END + "(\\s+|$)";
        public const string REGEX_ENTRY_START = "(\\s+|^)" + ENTRY_START + "(\\s+|$)";
        public const string REGEX_FILEREPLACE = "(#.*?\n)|(\r)|(\n)";
        public const string REGEX_PARAMETER = FIELD_START + "\\s*([a-zA-Z0-9\\.]+)\\s*" + FIELD_NAMEVALUE_SEPARATOR + "\\s*" +
            FIELD_VALUE_GROUPING + "?(.*?)" + FIELD_VALUE_GROUPING + "?\\s*" + FIELD_END;
    }
}