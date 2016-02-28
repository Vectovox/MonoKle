namespace MonoKle.Console
{
    using System.Collections.Generic;

    /// <summary>
    /// Handler for providing input suggestions to arguments at the specified index.
    /// </summary>
    public delegate ICollection<string> ConsoleCommandSuggestionHandler(int index);
}
