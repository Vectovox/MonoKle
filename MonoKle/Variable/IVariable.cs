namespace MonoKle.Variable
{
    /// <summary>
    /// Interface for an assignable variable.
    /// </summary>
    public interface IVariable
    {
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
