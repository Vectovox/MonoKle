namespace MonoKle.Script.Compiler
{
    using MonoKle.Script.Common.Script;
    using System.Collections.Generic;

    /// <summary>
    /// Interface defining a script compiler.
    /// </summary>
    public interface IScriptCompiler
    {
        // TODO: Make this interface asynchronous. Imagine checking and compiling 1000 scripts at the same time! :)
        // TODO: When checking syntax and compilability, return a result, giving both a boolean and all error messages (no code though since it does not compile).
        // The above todo will help when making prepasses of what to compile but still want error messages for those that do not compile.

        /// <summary>
        /// Compiles the provided script source into a bytecode script. Sets syntax and semantics error flags.
        /// </summary>
        /// <param name="source">Source of script.</param>
        /// <param name="knownScripts">Headers for other scripts to know about.</param>
        /// <returns>Result of compilation.</returns>
        ICompilationResult Compile(ScriptSource source, ICollection<ScriptHeader> knownScripts);

        /// <summary>
        /// Checks whether the syntax of a script is valid.
        /// </summary>
        /// <param name="source">Soure to check syntax on.</param>
        /// <returns>True if valid, else false.</returns>
        bool IsSyntaxValid(ScriptSource source);

        /// <summary>
        /// Checks if the provided script source is compilable.
        /// </summary>
        /// <param name="source">Source to check if compilable.</param>
        /// <param name="knownScripts">Headers for other scripts to know about.</param>
        /// <returns>True if compilable, else false.</returns>
        bool IsCompilable(ScriptSource source, ICollection<ScriptHeader> knownScripts);
    }
}
