namespace MonoKle.Script.Compiler
{
    using MonoKle.Script.Compiler.Event;
    using MonoKle.Script.Common.Script;
    using System.Collections.Generic;
    
    /// <summary>
    /// Interface defining a script compiler.
    /// </summary>
    public interface IScriptCompiler
    {
        /// <summary>
        /// Compilation error, fired for both syntax and semantics errors.
        /// </summary>
        event CompilationErrorEventHandler CompilationError;

        /// <summary>
        /// Compiles the provided script source into a bytecode script. Sets syntax and semantics error flags.
        /// </summary>
        /// <param name="source">Source of script.</param>
        /// <param name="knownScripts">Headers for other scripts to know about.</param>
        /// <returns>Compiled script if compilation was successful, otherwise null.</returns>
        ByteScript Compile(ScriptSource source, ICollection<ScriptHeader> knownScripts);

        /// <summary>
        /// Returns the semantics error flag set by last compilation.
        /// </summary>
        /// <returns>True if there was a semantics error, else false.</returns>
        bool GetSemanticsErrorFlag();

        /// <summary>
        /// Returns the syntax error flag set by last compilation.
        /// </summary>
        /// <returns>True if there was a syntax error, else false.</returns>
        bool GetSyntaxErrorFlag();

        /// <summary>
        /// Returns the compilation error flag set by last compilation.
        /// </summary>
        /// <returns>True if there was a compilation error, else false.</returns>
        bool GetCompilationErrorFlag();
    }
}
