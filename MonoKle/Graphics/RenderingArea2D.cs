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
        public MRectangle Desired { get; }

        /// <summary>
        /// Gets the rectangle in which to render the game in, avoid letterboxing by matching screen aspect ratio.
        /// This encompasses <see cref="Desired"/> plus additional area around.
        /// </summary>
        public MRectangle Render { get; }

        /// <summary>
        /// Gets the rectangle for the screen area available to render in (display resolution).
        /// </summary>
        public MRectangle Display { get; }

        /// <summary>
        /// Gets the rectangle for the logic area of the screen, i.e. the area that important gameplay can happen.
        /// This is the original <see cref="Desired"/> rectangle scaled to fit the display from <see cref="Display"/>.
        /// </summary>
        /// <remarks>
        /// Intended usage is to separate required rendering space from additional area that could be used
        /// for UI elements.
        /// </remarks>
        public MRectangle DisplayLogic { get; }

        /// <summary>
        /// Creates and initializes a new instance <see cref="RenderingArea2D"/>.
        /// </summary>
        /// <param name="desiredRenderResolution">The desired render resolution.</param>
        /// <param name="displayResolution">The available display resolution.</param>
        public RenderingArea2D(MPoint2 desiredRenderResolution, MPoint2 displayResolution)
        {
            Display = new MRectangle(displayResolution.ToMVector2());
            Desired = new MRectangle(desiredRenderResolution.ToMVector2());
            DisplayLogic = Desired.ScaleToFit(Display);
            Render = new MRectangle(Display.ScaleToFill(Desired).Dimensions);
        }

        /// <summary>
        /// Transforms the given display coordinate to game rendering space.
        /// </summary>
        /// <param name="displayCoordinate">The display coordinate to transform.</param>
        public MVector2 TransformDisplayToRender(MVector2 displayCoordinate) =>
            new MVector2(displayCoordinate.X / Display.Width * Render.Width, displayCoordinate.Y / Display.Height * Render.Height);

        /// <summary>
        /// Transforms the given game rendering coordinate to display space.
        /// </summary>
        /// <param name="renderCoordinate">The render-space coordinate to transform.</param>
        public MVector2 TransformRenderToDisplay(MVector2 renderCoordinate) =>
            new MVector2(renderCoordinate.X / Render.Width * Display.Width, renderCoordinate.Y / Render.Height * Display.Height);
    }
}
