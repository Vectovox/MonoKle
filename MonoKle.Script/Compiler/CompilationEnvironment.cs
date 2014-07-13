namespace MonoKle.Script.Compiler
{
    using MonoKle.Script.Common.Script;
    using System.Collections.Generic;

    /// <summary>
    /// Compilation environment used to compile multiple scripts, which may reference each other, to bytecode in a single go.
    /// </summary>
    public class CompilationEnvironment : ICompilationEnvironment
    {
        private IScriptCompiler compiler;
        private HashSet<ScriptHeader> loadedHeaders = new HashSet<ScriptHeader>();
        private HashSet<ScriptSource> loadedSources = new HashSet<ScriptSource>();

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
        public ICollection<ICompilationResult> Compile()
        {
            HashSet<ScriptSource> failedScripts = new HashSet<ScriptSource>();

            for(bool failed = true; failed; )
            {
                failed = false;
                foreach(ScriptSource s in loadedSources)
                {
                    if(this.compiler.CheckCompilable(s, this.loadedHeaders).Success == false)
                    {
                        failedScripts.Add(s);
                        failed = true;
                    }
                }

                foreach(ScriptSource s in failedScripts)
                {
                    this.loadedSources.Remove(s);
                    this.loadedHeaders.Remove(s.Header);
                }
            }

            ICollection<ICompilationResult> results = new LinkedList<ICompilationResult>();
            foreach(ScriptSource s in failedScripts)
            {
                results.Add(this.compiler.Compile(s, this.loadedHeaders));
            }
            foreach(ScriptSource s in this.loadedSources)
            {
                results.Add(this.compiler.Compile(s, this.loadedHeaders));
            }

            this.loadedHeaders.Clear();
            this.loadedSources.Clear();
            return results;
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
        /// <returns>The amount of loaded sources.</returns>
        public int LoadSources(IEnumerable<ScriptSource> sources)
        {
            int nLoaded = 0;
            foreach(ScriptSource s in sources)
            {
                this.loadedSources.Add(s);
                this.loadedHeaders.Add(s.Header);
                nLoaded++;
            }
            return nLoaded;
        }
    }
}