using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoKle.Graphics
{
    /// <summary>
    /// Class that represents a 2D game display, solving the problem of addressing different
    /// display resolutions.
    /// </summary>
    public class GameDisplay2D : IDisposable
    {
        private readonly GraphicsManager _graphicsManager;

        public GraphicsDevice GraphicsDevice { get; }
        public MPoint2 TargetResolution { get; }
        public MPoint2 DisplayResolution { get; private set; }
        public RenderingArea2D RenderingArea { get; private set; }
        public Camera2D Camera { get; }
        public RenderTarget2D RenderTarget { get; private set; }
        public bool MipMap { get; set; }
        public SurfaceFormat SurfaceFormat { get; set; }
        public DepthFormat DepthFormat { get; set; }

        public GameDisplay2D(GraphicsManager graphicsManager, MPoint2 targetResolution)
        {
            _graphicsManager = graphicsManager;
            _graphicsManager.ResolutionChanged += ResolutionChanged;
            GraphicsDevice = _graphicsManager.GraphicsDevice;
            TargetResolution = targetResolution;
            DisplayResolution = graphicsManager.Resolution;
            Camera = new Camera2D(MPoint2.One);
            Apply();
        }

        /// <summary>
        /// Call to apply any changes made to the <see cref="GameDisplay2D"/>.
        /// </summary>
        public void Apply()
        {
            RenderingArea = new RenderingArea2D(TargetResolution, DisplayResolution);
            Camera.Size = RenderingArea.Render.BottomRight.ToMPoint2();
            RenderTarget?.Dispose();    // Make sure to dispose or we get a memory leak
            RenderTarget = new RenderTarget2D(GraphicsDevice,
                (int)RenderingArea.Render.Width,
                (int)RenderingArea.Render.Height,
                MipMap,
                SurfaceFormat,
                DepthFormat);
        }

        /// <summary>
        /// Call to dispose.
        /// </summary>
        public void Dispose()
        {
            RenderTarget.Dispose();
            _graphicsManager.ResolutionChanged -= ResolutionChanged;
        }

        private void ResolutionChanged(object sender, ResolutionChangedEventArgs e)
        {
            DisplayResolution = e.NewScreenSize;
            Apply();
        }

        /// <summary>
        /// Transforms the given display coordinate to game rendering coordinate space.
        /// </summary>
        /// <param name="displayCoordinate">The display coordinate.</param>
        public MVector2 TransformToGameSpace(MVector2 displayCoordinate) =>
            Camera.TransformInv(RenderingArea.TransformDisplayToRender(displayCoordinate));

        /// <summary>
        /// Transforms the given game rendering coordinate to display coordinate space.
        /// </summary>
        /// <param name="gameCoordinate">The game space coordinate.</param>
        public MVector2 TransformToDisplaySpace(MVector2 gameCoordinate) => Camera.Transform(gameCoordinate);
    }
}
