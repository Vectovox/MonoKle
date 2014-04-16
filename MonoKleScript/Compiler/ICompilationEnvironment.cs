namespace MonoKle.Script.Compiler
{
    using MonoKle.Script.Common.Script;
    using System.Collections.Generic;

    /// <summary>
    /// Interface for a compilation environment, compiling multiple source files at once.
    /// </summary>
    public interface ICompilationEnvironment
    {
        /// <summary>
        /// Compiles the loaded sources and returns the compiled scripts.
        /// </summary>
        /// <returns>Collection of bytescripts.</returns>
        ICollection<ByteScript> Compile();

        /// <summary>
        /// Loads the given script source for compilation.
        /// </summary>
        /// <param name="source">The script to load.</param>
        void LoadSource(ScriptSource source);

        /// <summary>
        /// Loads the given script sources for compilation.
        /// </summary>
        /// <param name="sources">The scripts to load.</param>
        /// <returns>The amount of loaded sources.</returns>
        int LoadSources(IEnumerable<ScriptSource> sources);
    }
}
