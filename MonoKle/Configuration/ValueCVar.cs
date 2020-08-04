using System;

namespace MonoKle.Configuration
{
    /// <summary>
    /// Class for a simple CVar variable hodling a value.
    /// </summary>
    public class ValueCVar : ICVar
    {
        private object _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueCVar"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public ValueCVar(object value)
        {
            _value = value;
        }

        /// <summary>
        /// Gets the type of the variable.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public Type Type => typeof(object);

        /// <summary>
        /// Determines whether this instance can set.
        /// </summary>
        /// <returns></returns>
        public bool CanSet() => true;

        /// <summary>
        /// Gets the variable value.
        /// </summary>
        /// <returns></returns>
        public object GetValue() => _value;

        /// <summary>
        /// Sets the variable to the provided value if possible.
        /// </summary>
        /// <param name="value">The value to set.</param>
        /// <returns>
        /// True if value could be set; otherwise false.
        /// </returns>
        public bool SetValue(object value)
        {
            _value = value;
            return true;
        }
    }
}