using System;

namespace MonoKle.Console
{
    /// <summary>
    /// Attribute tagging console command arguments.
    /// </summary>
    public class ConsoleFlagAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the argument.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the description of the argument.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        public ConsoleFlagAttribute(string name) => Name = name;
    }
}
