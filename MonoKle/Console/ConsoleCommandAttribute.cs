using System;

namespace MonoKle.Console
{
    /// <summary>
    /// Attribute tagging console commands.
    /// </summary>
    public class ConsoleCommandAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the command.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the description of the command.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        public ConsoleCommandAttribute(string name) => Name = name;
    }
}
