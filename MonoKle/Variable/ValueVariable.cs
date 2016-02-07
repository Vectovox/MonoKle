using System;

namespace MonoKle.Variable
{
    /// <summary>
    /// Class for variable containing its own value.
    /// </summary>
    public class ValueVariable : IVariable
    {
        private object value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueVariable"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public ValueVariable(object value)
        {
            this.value = value;
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
        public object GetValue() => this.value;

        /// <summary>
        /// Sets the variable to the provided value if possible.
        /// </summary>
        /// <param name="value">The value to set.</param>
        /// <returns>
        /// True if value could be set; otherwise false.
        /// </returns>
        public bool SetValue(object value)
        {
            this.value = value;
            return true;
        }
    }
}