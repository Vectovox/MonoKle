namespace MonoKle.Script.Common.Script
{
    using System;

    /// <summary>
    /// Header for a script.
    /// </summary>
    public class ScriptHeader
    {
        /// <summary>
        /// Type of the return value.
        /// </summary>
        public Type ReturnType { get; private set; }

        /// <summary>
        /// The channel of the script.
        /// </summary>
        public string Channel { get; private set; }

        /// <summary>
        /// The name of the script.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The arguments that the script takes.
        /// </summary>
        public ScriptVariable[] Arguments { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="ScriptHeader"/>.
        /// </summary>
        /// <param name="name">Name of the script.</param>
        /// <param name="returnType">Return type of the script.</param>
        /// <param name="channel">Channel of the script.</param>
        /// <param name="arguments">The arguments of the script.</param>
        public ScriptHeader(string name, Type returnType, string channel, ScriptVariable[] arguments)
        {
            this.ReturnType = returnType;
            this.Channel = channel;
            this.Name = name;
            this.Arguments = arguments;
        }
    }
}
