namespace MonoKle.Attributes {
    using System;

    /// <summary>
    /// Attribute class for declaring properties that should be bound through property variables.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PropertyVariableAttribute : Attribute {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyVariableAttribute"/> class.
        /// </summary>
        /// <param name="identifier">The identifier of the variable.</param>
        public PropertyVariableAttribute(string identifier) {
            Identifier = identifier;
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Identifier { get; private set; }
    }
}