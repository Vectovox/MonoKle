namespace MonoKle.Graphics
{
    /// <summary>
    /// Enumeration for how the game should be rendered.
    /// </summary>
    public enum GraphicsMode
    {
        /// <summary>
        /// The game runs in a window, using backbuffer resolution to determine window size.
        /// </summary>
        Windowed,
        /// <summary>
        /// The game runs in hard fullscreen, rendering in the backbuffer resolution.
        /// </summary>
        Fullscreen,
        /// <summary>
        /// The game runs in soft fullscreen, rendering in a borderless window at the native display resolution.
        /// </summary>
        Borderless,
    }
}