namespace MonoKle.Script.Common.Script
{
    using System;

    /// <summary>
    /// A variable in a script.
    /// </summary>
    public struct ScriptVariable
    {
        /// <summary>
        /// Name of the variable.
        /// </summary>
        public string name;

        /// <summary>
        /// Type of the variable.
        /// </summary>
        public Type type;

        /// <summary>
        /// Creates a new instance of <see cref="ScriptVariable"/>.
        /// </summary>
        /// <param name="name">Name of the variable.</param>
        /// <param name="type">Type of the variable.</param>
        public ScriptVariable(string name, Type type)
        {
            this.name = name;
            this.type = type;
        }
    }
}
