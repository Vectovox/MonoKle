namespace MonoKle.Graphics
{
    /// <summary>
    /// Class containing the information required to render in 2D at any resolution and be able to scale it
    /// to any display resolution.
    /// </summary>
    public class RenderingArea2D
    {
        /// <summary>
        /// Gets the rectangle representing the area in which the game wants to render.
        /// </summary>
        public MRectangle RenderRectangleDesired { get; }

        /// <summary>
        /// Gets the rendering rectangle the game needs to render in order to avoid letterboxing. This
        /// encompasses <see cref="RenderRectangleDesired"/> plus additional area around.
        /// </summary>
        public MRectangle RenderRectangleAdjusted { get; }

        /// <summary>
        /// Gets the rectangle representing the screen area available to render in.
        /// </summary>
        public MRectangle ScreenRectangle { get; }

        /// <summary>
        /// Gets the rectangle representing the logic area of the screen, i.e. the area that represents
        /// the original <see cref="RenderRectangleDesired"/> scaled to fit the <see cref="ScreenRectangle"/>.
        /// </summary>
        /// <remarks>
        /// Intended usage is to separate required rendering space from additional area that could be used
        /// for UI elements.
        /// </remarks>
        public MRectangle ScreenLogicRectangle { get; }

        public RenderingArea2D(MPoint2 desiredRenderResolution, MPoint2 displayResolution)
        {
            ScreenRectangle = new MRectangle(displayResolution.ToMVector2());
            RenderRectangleDesired =
                new MRectangle(desiredRenderResolution.ToMVector2());

            ScreenLogicRectangle =
                RenderRectangleDesired
                .ScaleToFit(ScreenRectangle);

            RenderRectangleAdjusted = new MRectangle(
                ScreenRectangle
                .ScaleToFill(RenderRectangleDesired)
                .Dimensions
            );
        }
    }
}
