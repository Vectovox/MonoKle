namespace MonoKle.Variable
{
    /// <summary>
    /// Interface for an assignable variable.
    /// </summary>
    public interface IVariable
    {
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