namespace MonoKle.Graphics
{
    using System;

    /// <summary>
    /// Event arguments for changed screen size events.
    /// </summary>
    public class ResolutionChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance of <see cref="ResolutionChangedEventArgs"/>.
        /// </summary>
        /// <param name="newScreenSize">The new screen size.</param>
        public ResolutionChangedEventArgs(MPoint2 newScreenSize)
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