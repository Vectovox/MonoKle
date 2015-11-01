namespace MonoKle.Graphics.Event
{
    using MonoKle.Core;
    using System;

    /// <summary>
    /// Event arguments for changed screen size events.
    /// </summary>
    public class ScreenSizeChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance of <see cref="ScreenSizeChangedEventArgs"/>.
        /// </summary>
        /// <param name="newScreenSize">The new screen size.</param>
        public ScreenSizeChangedEventArgs(IntVector2 newScreenSize)
        {
            this.NewScreenSize = newScreenSize;
        }

        /// <summary>
        /// The new screen size.
        /// </summary>
        public IntVector2 NewScreenSize
        {
            get;
            private set;
        }
    }
}