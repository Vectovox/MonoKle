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
        
        /// <summary>
        /// Compiles the provided script source into a bytecode script.
        /// </summary>
        /// <param name="source">Source of script.</param>
        /// <param name="knownScripts">Headers for other scripts to know about.</param>
        /// <returns>Result of compilation.</returns>
        ICompilationResult Compile(ScriptSource source, ICollection<ScriptHeader> knownScripts);

        /// <summary>
        /// Checks whether the syntax of a script is valid.
        /// </summary>
        /// <param name="source">Soure to check syntax on.</param>
        /// <returns>Result of syntax check.</returns>
        ISyntaxResult CheckSyntax(ScriptSource source);

        /// <summary>
        /// Checks if the provided script source is compilable.
        /// </summary>
        /// <param name="source">Source to check if compilable.</param>
        /// <param name="knownScripts">Headers for other scripts to know about.</param>
        /// <returns>Result of compilable check.</returns>
        ICompilableResult CheckCompilable(ScriptSource source, ICollection<ScriptHeader> knownScripts);
    }
}
