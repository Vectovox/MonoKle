namespace MonoKle
{
    /// <summary>
    /// Interface for drawable components.
    /// </summary>
    public interface IMDrawable
    {
        /// <summary>
        /// Draws the specified component with the specified seconds since last drawal.
        /// </summary>
        /// <param name="seconds">The amount of seconds since last drawal.</param>
        void Draw(double seconds);
    }
}