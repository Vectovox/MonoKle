namespace MonoKleScript.Compiler
{
    using System.Collections.Generic;

    using MonoKleScript.Common.Script;

    /// <summary>
    /// Compilation environment used to compile multiple scripts, which may reference each other, to bytecode in a single go.
    /// </summary>
    public class CompilationEnvironment : ICompilationEnvironment
    {
        private IScriptCompiler compiler;
        private ICollection<ScriptHeader> loadedHeaders = new LinkedList<ScriptHeader>();
        private ICollection<ScriptSource> loadedSources = new LinkedList<ScriptSource>();

        /// <summary>
        /// Creates new instance of <see cref="CompilationEnvironment"/>.
        /// </summary>
        /// <param name="compiler">The compiler to use.</param>
        public CompilationEnvironment(IScriptCompiler compiler)
        {
            this.compiler = compiler;
        }

        /// <summary>
        /// Compiles the loaded sources and returns the compiled scripts.
        /// </summary>
        /// <returns>Collection of bytescripts.</returns>
        public ICollection<ByteScript> Compile()
        {
            ICollection<ByteScript> compiledScripts = new LinkedList<ByteScript>();
            foreach (ScriptSource s in loadedSources)
            {
                ByteScript script = this.compiler.Compile(s, this.loadedHeaders);
                if (script != null)
                {
                    compiledScripts.Add(script);
                }
            }
            this.loadedHeaders.Clear();
            this.loadedSources.Clear();
            return compiledScripts;
        }

        /// <summary>
        /// Loads the given script source for compilation.
        /// </summary>
        /// <param name="sources">The script to load.</param>
        public void LoadSource(ScriptSource source)
        {
            this.LoadSources(new List<ScriptSource>() { source });
        }

        /// <summary>
        /// Loads the given script sources for compilation.
        /// </summary>
        /// <param name="sources">The scripts to load.</param>
        public void LoadSources(IEnumerable<ScriptSource> sources)
        {
            foreach (ScriptSource s in sources)
            {
                this.loadedSources.Add(s);
                this.loadedHeaders.Add(s.Header);
            }
        }
    }
}