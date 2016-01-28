namespace MonoKle
{
    /// <summary>
    /// Interface for updateable components.
    /// </summary>
    public interface IMUpdateable
    {
        /// <summary>
        /// Updates the component with the specified seconds since last update.
        /// </summary>
        /// <param name="seconds">The amount of seconds since last update.</param>
        void Update(double seconds);
    }
}