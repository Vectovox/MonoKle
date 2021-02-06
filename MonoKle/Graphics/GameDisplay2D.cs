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
        private DynamicCamera2D _camera = new DynamicCamera2D(MPoint2.One);

        public GraphicsDevice GraphicsDevice { get; }

        public MPoint2 WorldTargetResolution { get; }
        public MPoint2 UiTargetResolution { get; }

        /// <summary>
        /// Gets or sets the camera for the world rendering.
        /// </summary>
        /// <remarks>
        /// Setting the camera will automatically update its size to conform to the rendering resolution.
        /// </remarks>
        public DynamicCamera2D Camera
        {
            get => _camera;
            set
            {
                _camera = value;
                SetCameraSize();
            }
        }

        /// <summary>
        /// Gets the resolution of the display.
        /// </summary>
        public MPoint2 DisplayResolution { get; private set; }

        /// <summary>
        /// Gets the rendering area for the game.
        /// </summary>
        public RenderingArea2D WorldRenderingArea { get; private set; }

        /// <summary>
        /// Gets the rendering area for the UI.
        /// </summary>
        public RenderingArea2D UiRenderingArea { get; private set; }

        /// <summary>
        /// Gets the render target for the world.
        /// </summary>
        public RenderTarget2D WorldRenderTarget { get; private set; }

        /// <summary>
        /// Gets the render target for the UI.
        /// </summary>
        public RenderTarget2D UiRenderTarget { get; private set; }

        public bool MipMap { get; set; }
        public SurfaceFormat SurfaceFormat { get; set; }
        public DepthFormat DepthFormat { get; set; }

        /// <summary>
        /// Creates and initializes a new <see cref="GameDisplay2D"/>,
        /// having the same resolution for the world as the UI.
        /// </summary>
        /// <param name="graphicsManager">Graphics manager to use.</param>
        /// <param name="targetResolution">The target resolution of the rendering.</param>
        public GameDisplay2D(GraphicsManager graphicsManager, MPoint2 targetResolution) :
            this(graphicsManager, targetResolution, targetResolution)
        { }

        /// <summary>
        /// Creates and initializes a new <see cref="GameDisplay2D"/>.
        /// </summary>
        /// <param name="graphicsManager">Graphics manager to use.</param>
        /// <param name="targetWorldResolution">The target resolution of the world rendering.</param>
        /// <param name="targetUiResolution">The target resolution of the UI rendering.</param>
        public GameDisplay2D(GraphicsManager graphicsManager, MPoint2 targetWorldResolution, MPoint2 targetUiResolution)
        {
            // Graphics manager stuff
            _graphicsManager = graphicsManager;
            _graphicsManager.ResolutionChanged += ResolutionChanged;
            GraphicsDevice = _graphicsManager.GraphicsDevice;
            DisplayResolution = graphicsManager.Resolution;

            // Set the targets
            WorldTargetResolution = targetWorldResolution;
            UiTargetResolution = targetUiResolution;

            // Update
            Apply();
        }

        /// <summary>
        /// Call to apply any changes made to the <see cref="GameDisplay2D"/>.
        /// </summary>
        public void Apply()
        {
            // World setup
            WorldRenderingArea = new RenderingArea2D(WorldTargetResolution, DisplayResolution);
            WorldRenderTarget?.Dispose();    // Make sure to dispose or we get a memory leak
            WorldRenderTarget = new RenderTarget2D(GraphicsDevice,
                WorldRenderingArea.Render.Width,
                WorldRenderingArea.Render.Height,
                MipMap,
                SurfaceFormat,
                DepthFormat);

            // UI setup
            UiRenderingArea = new RenderingArea2D(UiTargetResolution, DisplayResolution);
            UiRenderTarget?.Dispose();
            UiRenderTarget = new RenderTarget2D(GraphicsDevice,
                UiRenderingArea.Render.Width,
                UiRenderingArea.Render.Height,
                MipMap,
                SurfaceFormat,
                DepthFormat);

            SetCameraSize();
        }

        private void SetCameraSize() => Camera.Size = WorldRenderingArea.Render.BottomRight;

        /// <summary>
        /// Call to dispose.
        /// </summary>
        public void Dispose()
        {
            WorldRenderTarget.Dispose();
            UiRenderTarget.Dispose();
            _graphicsManager.ResolutionChanged -= ResolutionChanged;
        }

        private void ResolutionChanged(object sender, ResolutionChangedEventArgs e)
        {
            DisplayResolution = e.NewScreenSize;
            Apply();
        }

        /// <summary>
        /// Transforms the given display coordinate (screen pixel) to world coordinate space.
        /// </summary>
        /// <remarks>
        /// E.g. for converting display click to game actions.
        /// </remarks>
        /// <param name="displayCoordinate">The display coordinate.</param>
        public MVector2 DisplayToWorld(MPoint2 displayCoordinate) =>
            Camera.TransformInv(WorldRenderingArea.TransformDisplayToRender(displayCoordinate.ToMVector2()));

        /// <summary>
        /// Transforms the given display delta vector to world coordinate space.
        /// </summary>
        /// <remarks>
        /// E.g. for converting display dragging to game dragging.
        /// </remarks>
        /// <param name="displayCoordinate">The display coordinate.</param>
        public MVector2 DisplayToWorldDelta(MPoint2 displayDelta) =>
            DisplayToWorld(displayDelta) - DisplayToWorld(MPoint2.Zero);

        /// <summary>
        /// Transforms the given world coordinate to display space (screen pixel).
        /// </summary>
        /// <param name="worldCoordinate">The world coordinate to transform.</param>
        public MPoint2 WorldToDisplay(MVector2 worldCoordinate) =>
            WorldRenderingArea.TransformRenderToDisplay(Camera.Transform(worldCoordinate)).ToMPoint2();

        /// <summary>
        /// Transforms the given display coordinate (screen pixel) to UI rendering coordinate space.
        /// </summary>
        /// <remarks>
        /// E.g. for converting display click to UI actions.
        /// </remarks>
        /// <param name="displayCoordinate">The display coordinate.</param>
        public MPoint2 DisplayToUi(MPoint2 displayCoordinate) =>
            UiRenderingArea.TransformDisplayToRender(displayCoordinate.ToMVector2()).ToMPoint2();

        /// <summary>
        /// Transforms the given world coordinate to UI space.
        /// </summary>
        /// <remarks>
        /// E.g. a HUD drawing UI markers over world entities.
        /// </remarks>
        /// <param name="worldCoordinate">The world coordinate to transform.</param>
        public MPoint2 WorldToUi(MVector2 worldCoordinate) => DisplayToUi(WorldToDisplay(worldCoordinate));
    }
}
