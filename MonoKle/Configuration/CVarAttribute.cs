using System;

namespace MonoKle.Configuration
{
    /// <summary>
    /// Attribute class for declaring properties that should be bound through property variables.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class CVarAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CVarAttribute"/> class with a specific identifier.
        /// </summary>
        /// <param name="identifier">Identifier to use for the variable.</param>
        public CVarAttribute(string identifier)
        {
            Identifier = identifier;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CVarAttribute"/> class with an auto-generated identifier.
        /// </summary>
        public CVarAttribute()
        {
            Identifier = string.Empty;
        }

        /// <summary>
        /// Gets the identifier for the variable.
        /// </summary>
        public string Identifier { get; }
    }

    /// <summary>
    /// Attribute class for declaring CVar prefix naming.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class CVarPrefixAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CVarPrefixAttribute"/>.
        /// </summary>
        /// <param name="prefix">Prefix to use for variable names.</param>
        public CVarPrefixAttribute(string prefix)
        {
            Prefix = prefix;
        }

        /// <summary>
        /// Gets the prefix.
        /// </summary>
        public string Prefix { get; }
    }
}