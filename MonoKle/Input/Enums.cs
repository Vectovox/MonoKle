namespace MonoKle.Input
{
    /// <summary>
    /// Enumeration for specifying how input collections should be addressed.
    /// </summary>
    public enum CollectionQueryBehavior
    {
        /// <summary>
        /// Any of the specified inputs.
        /// </summary>
        Any,

        /// <summary>
        /// All of the specified inputs.
        /// </summary>
        All
    }

    /// <summary>
    /// Enumeration for mouse buttons.
    /// </summary>
    public enum MouseButton
    {
        /// <summary>
        /// The left mouse button.
        /// </summary>
        Left,

        /// <summary>
        /// The middle mouse button.
        /// </summary>
        Middle,

        /// <summary>
        /// The right mouse button.
        /// </summary>
        Right,

        /// <summary>
        /// The first extra mouse button.
        /// </summary>
        XButton1,

        /// <summary>
        /// The second extra mouse button.
        /// </summary>
        XButton2
    }

    /// <summary>
    /// Enumeration for mouse scroll direction.
    /// </summary>
    public enum MouseScrollDirection
    {
        /// <summary>
        /// Up direction.
        /// </summary>
        Up,

        /// <summary>
        /// Down direction.
        /// </summary>
        Down,

        /// <summary>
        /// No scrolling.
        /// </summary>
        None
    }
}