namespace MonoKle.Script
{
    using Microsoft.CSharp;
    using MonoKle.IO;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// Class for storing and executing scripts.
    /// </summary>
    public class ScriptEnvironment : AbstractFileLoader
    {
        private const string MainClassName = "MScriptMainClass";
        private static readonly string ScriptPrefix = "using " + Assembly.GetExecutingAssembly().GetName().Name + "; public class " + MainClassName + "{";
        private const string ScriptSuffix = "}";

        private List<string> sourceList = new List<string>();

        private object mainInstance;
        private Dictionary<string, MethodInfo> scriptByID;
        private Dictionary<string, ICollection<MethodInfo>> scriptsByChannel;

        public bool Compiled { get; private set; }

        /// <summary>
        /// Adds a script source string.
        /// </summary>
        /// <param name="source">The source.</param>
        public void AddSourceString(string source)
        {
            this.sourceList.Add(source);
            this.Compiled = false;
        }

        /// <summary>
        /// Clears the added sources.
        /// </summary>
        public void ClearSources()
        {
            this.sourceList.Clear();
        }

        /// <summary>
        /// Calls the script with the provided ID.
        /// </summary>
        /// <param name="scriptID">The script identifier.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Return value of script.</returns>
        public object CallScript(string scriptID, params object[] parameters)
        {
            if (scriptByID != null && scriptByID.ContainsKey(scriptID))
            {
                return scriptByID[scriptID].Invoke(this.mainInstance, parameters);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Calls the channel with the provided id.
        /// </summary>
        /// <param name="channel">The channel to call.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Array of return values from each script on the channel.</returns>
        public object[] CallChannel(string channel, params object[] parameters)
        {
            if(this.scriptsByChannel != null && this.scriptsByChannel.ContainsKey(channel))
            {
                ICollection<MethodInfo> scripts = this.scriptsByChannel[channel];
                object[] ret = new object[scripts.Count];
                int counter = 0;
                foreach (MethodInfo i in scripts)
                {
                    ret[counter++] = i.Invoke(this.mainInstance, parameters);
                }
                return ret;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Compiles the currently added scripts.
        /// </summary>
        /// <returns></returns>
        public CompilationResult CompileScripts()
        {
            string s = this.CreateScriptString();
            CompilerResults r = this.Compile(s);
            CompilationResult result = this.CreateResult(r);
            
            if(result.Success)
            {
                this.mainInstance = this.CreateMainInstance(r.CompiledAssembly);
                this.scriptByID = this.CreateScriptDictionary(this.mainInstance.GetType());
                this.scriptsByChannel = this.CreateChannelDictionary(this.scriptByID.Values);
                this.Compiled = true;
            }

            return result;
        }

        /// <summary>
        /// Gets the compiled script identifiers.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetScriptIDs()
        {
            return this.scriptByID.Keys;
        }

        protected override bool OperateOnFile(Stream fileStream, string filePath)
        {
            StreamReader reader = new StreamReader(fileStream);
            string s = reader.ReadToEnd();
            reader.Close();
            this.AddSourceString(s);
            return true;
        }

        private Dictionary<string, ICollection<MethodInfo>> CreateChannelDictionary(IEnumerable<MethodInfo> m)
        {
            Dictionary<string, LinkedList<MethodInfo>> tempRet = new Dictionary<string, LinkedList<MethodInfo>>();
            foreach(MethodInfo i in m)
            {
                object[] attributes = i.GetCustomAttributes(typeof(ChannelAttribute), false);
                foreach (ChannelAttribute a in attributes)
                {
                    if(tempRet.ContainsKey(a.Channel) == false)
                    {
                        tempRet.Add(a.Channel, new LinkedList<MethodInfo>());
                    }
                    tempRet[a.Channel].AddLast(i);
                }
            }

            Dictionary<string, ICollection<MethodInfo>> ret = new Dictionary<string, ICollection<MethodInfo>>();
            foreach (string s in tempRet.Keys)
            {
                ret.Add(s, tempRet[s]);
            }
            return ret;
        }

        private Dictionary<string, MethodInfo> CreateScriptDictionary(Type t)
        {
            Dictionary<string, MethodInfo> ret = new Dictionary<string, MethodInfo>();
            foreach(MethodInfo i in t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                ret.Add(i.Name, i);
            }
            return ret;
        }

        private object CreateMainInstance(Assembly a)
        {
            Type t = a.GetType(ScriptEnvironment.MainClassName);
            return Activator.CreateInstance(t);
        }

        private string CreateScriptString()
        {
            StringBuilder sb = new StringBuilder(ScriptEnvironment.ScriptPrefix);
            foreach (string s in sourceList)
            {
                sb.Append(s);
            }
            sb.Append(ScriptEnvironment.ScriptSuffix);
            return sb.ToString();
        }

        private CompilationResult CreateResult(CompilerResults result)
        {
            List<string> errorList = new List<string>();
            foreach (CompilerError e in result.Errors)
            {
                errorList.Add(e.ToString());
            }

            return new CompilationResult(errorList);
        }

        private CompilerResults Compile(string s)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = true;
            string test = Assembly.GetExecutingAssembly().Location;
            parameters.ReferencedAssemblies.Add(test);
            return provider.CompileAssemblyFromSource(parameters, s);
        }
    }
}
