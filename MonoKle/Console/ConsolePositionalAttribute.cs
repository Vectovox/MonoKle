using System;

namespace MonoKle.Console
{
    /// <summary>
    /// Attribute tagging console command arguments.
    /// </summary>
    public class ConsolePositionalAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the description of the argument.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the zero-based position of the argument.
        /// </summary>
        public int Position { get; }

        /// <summary>
        /// Gets whether the argument is required.
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Creates and initializes a new instance of <see cref="ConsolePositionalAttribute"/>.
        /// </summary>
        /// <param name="position">The zero-based position of the argument.</param>
        public ConsolePositionalAttribute(int position) => Position = position;
    }
}
