using System;

namespace MonoKle.Attributes
{
    /// <summary>
    /// Attribute class for declaring properties that should be bound through property variables.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class VariableAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VariableAttribute"/> class.
        /// </summary>
        /// <param name="identifier">The identifier of the variable.</param>
        public VariableAttribute(string identifier)
        {
            Identifier = identifier;
        }

        /// <summary>
        /// Gets the identifier for the variable.
        /// </summary>
        public string Identifier { get; private set; }
    }
}