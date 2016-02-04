namespace MonoKle.Setting
{
    /// <summary>
    /// Interface for setting instances.
    /// </summary>
    public interface ISettingInstance
    {
        /// <summary>
        /// Gets the setting value.
        /// </summary>
        /// <returns></returns>
        object GetValue();

        /// <summary>
        /// Sets the setting value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        void SetValue(object value);
    }
}
