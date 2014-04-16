namespace MonoKle.Script.Common.Script
{
    using System;

    /// <summary>
    /// Header for a script.
    /// </summary>
    public struct ScriptHeader
    {
        /// <summary>
        /// Type of the return value.
        /// </summary>
        public Type returnType;

        /// <summary>
        /// The channel of the script.
        /// </summary>
        public string channel;

        /// <summary>
        /// The name of the script.
        /// </summary>
        public string name;

        /// <summary>
        /// The arguments that the script takes.
        /// </summary>
        public ScriptVariable[] arguments;

        /// <summary>
        /// Creates a new instance of <see cref="ScriptHeader"/>.
        /// </summary>
        /// <param name="name">Name of the script.</param>
        /// <param name="returnType">Return type of the script.</param>
        /// <param name="channel">Channel of the script.</param>
        /// <param name="arguments">The arguments of the script.</param>
        public ScriptHeader(string name, Type returnType, string channel, ScriptVariable[] arguments)
        {
            this.returnType = returnType;
            this.channel = channel;
            this.name = name;
            this.arguments = arguments;
        }
    }
}
