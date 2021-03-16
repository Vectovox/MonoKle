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

        public Type Type => typeof(object);

        public bool CanSet() => true;

        public object GetValue() => _value;

        public bool SetValue(object value)
        {
            _value = value;
            return true;
        }
    }
}