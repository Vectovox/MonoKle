namespace MonoKle.Graphics.Event
{
    using Core.Geometry;
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
        public ScreenSizeChangedEventArgs(MPoint2 newScreenSize)
        {
            this.NewScreenSize = newScreenSize;
        }

        /// <summary>
        /// The new screen size.
        /// </summary>
        public MPoint2 NewScreenSize
        {
            get;
            private set;
        }
    }
}