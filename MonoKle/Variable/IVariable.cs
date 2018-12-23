namespace MonoKle.Variable {
    using System;

    /// <summary>
    /// Interface for an assignable variable.
    /// </summary>
    public interface IVariable {
        /// <summary>
        /// Gets the type of the variable.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        Type Type { get; }

        /// <summary>
        /// Determines whether this instance can be set.
        /// </summary>
        /// <returns>True if it can be set; otherwise false.</returns>
        bool CanSet();

        /// <summary>
        /// Gets the variable value.
        /// </summary>
        /// <returns></returns>
        object GetValue();

        /// <summary>
        /// Sets the variable to the provided value if possible.
        /// </summary>
        /// <param name="value">The value to set.</param>
        /// <returns>True if value could be set; otherwise false.</returns>
        bool SetValue(object value);
    }
}