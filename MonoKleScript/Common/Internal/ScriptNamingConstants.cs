namespace MonoKle.Script.Common.Internal
{
    internal static class ScriptNamingConstants
    {
        internal const string SCRIPT_START = "script";
        internal const string SCRIPT_END = "endscript";
        internal const string SCRIPT_EXTENSION = ".ms";
        internal const string SCRIPT_COMMENT = ";";
        internal const string SCRIPT_STRING_TOKEN = "\"";

        internal const string SCRIPT_TYPE_ALLOWEDTYPES_REGEX = "(bool|int|float|string|object)";
        internal const string REGEX_START_MATCH = "(^|\\s+)" + SCRIPT_START + "(\\s+|$)";
        internal const string REGEX_END_MATCH = "(^|\\s+)" + SCRIPT_END + "(\\s+|$)";
        internal const string SCRIPT_NAMES_ALLOWEDNAMES_REGEX = "[A-Za-z0-9_]+";
        internal const string SCRIPT_HEADER_TYPE_ALLOWEDTYPES_REGEX = "(void|bool|int|float|string|object)";
        internal const string SCRIPT_HEADER_ARGUMENT_REGEX = SCRIPT_TYPE_ALLOWEDTYPES_REGEX + "\\s+" + SCRIPT_NAMES_ALLOWEDNAMES_REGEX;
        internal const string SCRIPT_HEADER_ARGUMENTS_REGEX = "\\(\\s*(|" + SCRIPT_HEADER_ARGUMENT_REGEX + "|"
            + SCRIPT_HEADER_ARGUMENT_REGEX + "(\\s*" + SCRIPT_ARGUMENT_SEPARATOR + "\\s*" + SCRIPT_HEADER_ARGUMENT_REGEX + ")*)\\s*\\)";
        internal const string SCRIPT_HEADER_SPECIFICATION_REGEX = "^\\s*" + SCRIPT_START + "\\s+" + SCRIPT_HEADER_TYPE_ALLOWEDTYPES_REGEX
            + "\\s+" + SCRIPT_NAMES_ALLOWEDNAMES_REGEX + "\\s*" + SCRIPT_HEADER_ARGUMENTS_REGEX + "\\s*($|" + SCRIPT_CHANNEL_PREFIX + "\\s*" +
            SCRIPT_NAMES_ALLOWEDNAMES_REGEX + "\\s*$)";
        internal const string SCRIPT_HEADER_TYPE_MATCH_REGEX = "(?<=^\\s*" + SCRIPT_START + "\\s+)" + SCRIPT_HEADER_TYPE_ALLOWEDTYPES_REGEX;
        internal const string SCRIPT_HEADER_NAME_MATCH_REGEX = "(?<=^\\s*" + SCRIPT_START + "\\s+" + SCRIPT_HEADER_TYPE_ALLOWEDTYPES_REGEX
            + "\\s+)" + SCRIPT_NAMES_ALLOWEDNAMES_REGEX;
        internal const string SCRIPT_HEADER_ARGUMENTS_MATCH_REGEX = "(?<=\\(\\s*).*(?=\\s*\\))";
        internal const string SCRIPT_HEADER_CHANNEL_MATCH_REGEX = "(?<=" + SCRIPT_CHANNEL_PREFIX + "\\s*)" + SCRIPT_NAMES_ALLOWEDNAMES_REGEX;
        internal const string SCRIPT_ARGUMENT_SEPARATOR = ",";
        internal const string SCRIPT_CHANNEL_PREFIX = ">";
    }
}
