using MonoKle.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MonoKle.Scripting
{
    /// <summary>
    /// Class keeping track of scripts.
    /// </summary>
    public class ScriptEnvironment : AbstractFileFinder
    {
        private ScriptCompiler compiler = new ScriptCompiler();
        private Dictionary<string, IScriptCompilable> scriptById = new Dictionary<string, IScriptCompilable>();

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count => scriptById.Count;

        /// <summary>
        /// The referenced assemblies to use when compiling.
        /// </summary>
        public IList<Assembly> ReferencedAssemblies => compiler.ReferencedAssemblies;

        /// <summary>
        /// Returns a list with all known script names.
        /// </summary>
        public List<string> ScriptNames => new List<string>(scriptById.Keys);

        /// <summary>
        /// Gets the <see cref="Script"/> with the specified identifier.
        /// </summary>
        /// <value>
        /// The <see cref="Script"/>.
        /// </value>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public IScript this[string id] => scriptById[id];

        /// <summary>
        /// Adds the specified script, returning a bool indicating if the script was added or not.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <returns>True if added; otherwise false.</returns>
        public bool Add(IScriptCompilable script)
        {
            if (!scriptById.ContainsKey(script.Name))
            {
                scriptById.Add(script.Name, script);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Compiles the specified script. Returning true if the script existed.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <returns>True if script existed.</returns>
        public bool Compile(string script)
        {
            if (scriptById.ContainsKey(script))
            {
                compiler.Compile(scriptById[script]);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Compiles all maintained scripts.
        /// </summary>
        /// <returns>Amount of scripts compiled.</returns>
        public int CompileAll()
        {
            compiler.Compile(scriptById.Values);
            return scriptById.Values.Count;
        }

        /// <summary>
        /// Compiles the outdated scripts, returning the amount compiled.
        /// </summary>
        /// <returns>Amount of scripts compiled.</returns>
        public int CompileOutdated()
        {
            var outdated = scriptById.Values.Where(v => v.IsOutdated).ToList();
            compiler.Compile(outdated);
            return outdated.Count;
        }

        /// <summary>
        /// Determines whether this instance contains a script with the specified name.
        /// </summary>
        /// <param name="scriptName">Name of the script.</param>
        /// <returns>True if such a script exists; otherwise false.</returns>
        public bool Contains(string scriptName) => scriptById.ContainsKey(scriptName);

        /// <summary>
        /// Clears this instance of all scripts.
        /// </summary>
        public void Clear() => scriptById.Clear();

        /// <summary>
        /// Removes the specified script.
        /// </summary>
        /// <param name="name">The script identifying name.</param>
        public bool Remove(string name) => scriptById.Remove(name);

        /// <summary>
        /// Checks if the file is valid.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        protected override bool CheckFile(MFileInfo file) => !Contains(file.OriginalPath) && file.Extension.Equals(".ms", StringComparison.InvariantCultureIgnoreCase);

        /// <summary>
        /// Operates on file.
        /// </summary>
        /// <param name="file">The file.</param>
        protected override bool OperateOnFile(MFileInfo file) => Add(new Script(file.NameWithoutExtension, new FileScriptSource(file)));
    }
}