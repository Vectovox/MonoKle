namespace MonoKle.Scripting
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// An executable script.
    /// </summary>
    public class Script : IScriptCompilable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Script"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="source">The source.</param>
        public Script(string name, IScriptSource source)
        {
            this.Source = source;
            this.Name = name;
            this.Errors = new List<ScriptCompilationError>();
            this.CompilationDate = DateTime.MinValue;
        }

        /// <summary>
        /// Gets a value indicating whether this instance can execute.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can execute; otherwise, <c>false</c>.
        /// </value>
        public bool CanExecute { get { return InternalScript != null; } }

        /// <summary>
        /// Gets or sets the compilation date in UTC.
        /// </summary>
        /// <value>
        /// The compilation date in UTC.
        /// </value>
        public DateTime CompilationDate { get; set; }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public List<ScriptCompilationError> Errors { get; private set; }

        /// <summary>
        /// Gets or sets the internal script implementation.
        /// </summary>
        /// <value>
        /// The internal script.
        /// </value>
        public ScriptImplementation InternalScript { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is outdated.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is outdated; otherwise, <c>false</c>.
        /// </value>
        public bool IsOutdated => this.Source.Date > this.CompilationDate;

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the type of the return value.
        /// </summary>
        /// <value>
        /// The type of the return value.
        /// </value>
        public Type ReturnType => InternalScript?.ReturnType ?? typeof(void);

        /// <summary>
        /// Gets a value indicating whether the script instance returns a value.
        /// </summary>
        /// <value>
        ///   <c>true</c> if it returns a value; otherwise, <c>false</c>.
        /// </value>
        public bool ReturnsValue => InternalScript != null ? InternalScript.ReturnsValue : false;

        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public IScriptSource Source { get; private set; }

        /// <summary>
        /// Executes with the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public ScriptExecution Execute(params object[] parameters)
        {
            if (this.CanExecute)
            {
                return this.InternalScript.Execute(parameters);
            }

            return new ScriptExecution(null, false, "Script can not execute.");
        }
    }
}