using System;

namespace MonoKle.Console
{
    /// <summary>
    /// Attribute tagging console command argument.
    /// </summary>
    public class ConsoleArgumentAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the argument.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the description of the argument.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets whether the argument is required.
        /// </summary>
        public bool IsRequired { get; set; }

        public ConsoleArgumentAttribute(string name) => Name = name;
    }
}
