namespace MonoKle.Graphics
{
    /// <summary>
    /// Class containing the information required to render in 2D at any resolution and be able to scale it
    /// to any display resolution.
    /// </summary>
    public class RenderingArea2D
    {
        /// <summary>
        /// Gets the rectangle for the area in which the game wants to render (render resolution).
        /// </summary>
        public MRectangleInt Desired { get; }

        /// <summary>
        /// Gets the rectangle in which to render the game in, avoid letterboxing by matching screen aspect ratio.
        /// This encompasses <see cref="Desired"/> plus additional area around.
        /// </summary>
        public MRectangleInt Render { get; }

        /// <summary>
        /// Gets the rectangle for the screen area available to render in (display resolution).
        /// </summary>
        public MRectangleInt Display { get; }

        /// <summary>
        /// Gets the rectangle for the logic area of the screen, i.e. the area that important gameplay can happen.
        /// This is the original <see cref="Desired"/> rectangle scaled to fit the display from <see cref="Display"/>.
        /// </summary>
        /// <remarks>
        /// Intended usage is to separate required rendering space from additional area that could be used
        /// for UI elements.
        /// </remarks>
        public MRectangleInt DisplayLogic { get; }

        /// <summary>
        /// Creates and initializes a new instance <see cref="RenderingArea2D"/>.
        /// </summary>
        /// <param name="desiredRenderResolution">The desired render resolution.</param>
        /// <param name="displayResolution">The available display resolution.</param>
        public RenderingArea2D(MPoint2 desiredRenderResolution, MPoint2 displayResolution)
        {
            Display = new MRectangleInt(displayResolution);
            Desired = new MRectangleInt(desiredRenderResolution);
            DisplayLogic = Desired.ToMRectangle().ScaleToFit(Display.ToMRectangle()).ToMRectangleInt();
            Render = new MRectangleInt(Display.ToMRectangle().ScaleToFill(Desired.ToMRectangle()).Dimensions.ToMPoint2());
        }

        /// <summary>
        /// Transforms the given display coordinate to game rendering space.
        /// </summary>
        /// <param name="displayCoordinate">The display coordinate to transform.</param>
        public MVector2 TransformDisplayToRender(MVector2 displayCoordinate) =>
            new(displayCoordinate.X / Display.Width * Render.Width, displayCoordinate.Y / Display.Height * Render.Height);

        /// <summary>
        /// Transforms the given render coordinate to display space.
        /// </summary>
        /// <param name="renderCoordinate">The render coordinate to transform.</param>
        public MVector2 TransformRenderToDisplay(MVector2 renderCoordinate) =>
            new(renderCoordinate.X / Render.Width * Display.Width, renderCoordinate.Y / Render.Height * Display.Height);
    }
}
