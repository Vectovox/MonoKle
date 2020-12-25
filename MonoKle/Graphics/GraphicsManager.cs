using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoKle.Configuration;

namespace MonoKle.Graphics
{
    /// <summary>
    /// Facade for graphics management.
    /// </summary>
    public class GraphicsManager
    {
        private readonly GraphicsDeviceManager graphicsDeviceManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsManager"/> class.
        /// </summary>
        /// <param name="graphicsDeviceManager">The graphics device manager.</param>
        public GraphicsManager(GraphicsDeviceManager graphicsDeviceManager)
        {
            this.graphicsDeviceManager = graphicsDeviceManager;
        }

        /// <summary>
        /// Occurs when resolution is changed.
        /// </summary>
        public event ResolutionChangedEventHandler ResolutionChanged;

        /// <summary>
        /// Gets the graphics device.
        /// </summary>
        /// <value>
        /// The graphics device.
        /// </value>
        public GraphicsDevice GraphicsDevice => graphicsDeviceManager.GraphicsDevice;

        /// <summary>
        /// Gets or sets the resolution.
        /// </summary>
        /// <value>
        /// The resolution.
        /// </value>
        [CVar("g_res")]
        public MPoint2 Resolution
        {
            get { return new MPoint2(graphicsDeviceManager.PreferredBackBufferWidth, graphicsDeviceManager.PreferredBackBufferHeight); }
            set { SetResolution(value); }
        }

        /// <summary>
        /// Gets or sets the height of the resolution.
        /// </summary>
        /// <value>
        /// The height of the resolution.
        /// </value>
        [CVar("g_res_y")]
        public int ResolutionHeight
        {
            get { return Resolution.Y; }
            set { Resolution = new MPoint2(Resolution.X, value); }
        }

        /// <summary>
        /// Gets or sets the width of the resolution.
        /// </summary>
        /// <value>
        /// The width of the resolution.
        /// </value>
        [CVar("g_res_x")]
        public int ResolutionWidth
        {
            get { return Resolution.X; }
            set { Resolution = new MPoint2(value, Resolution.Y); }
        }

        private void OnResolutionChanged(MPoint2 newResolution) => ResolutionChanged?.Invoke(this, new ResolutionChangedEventArgs(newResolution));

        /// <summary>
        /// Gets or sets a value indicating whether full screen is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is fullscreen; otherwise, <c>false</c>.
        /// </value>
        [CVar("g_fullscreen")]
        public bool IsFullscreen
        {
            get { return graphicsDeviceManager.IsFullScreen; }
            set { SetFullscreenEnabled(value); }
        }

        private void SetFullscreenEnabled(bool enabled)
        {
            if (enabled && IsFullscreen == false ||
                enabled == false && IsFullscreen)
            {
                ToggleFullscren();
            }
        }

        /// <summary>
        /// Toggles fullscren.
        /// </summary>
        public void ToggleFullscren()
        {
            graphicsDeviceManager.ToggleFullScreen();
            graphicsDeviceManager.ApplyChanges();
        }

        private void SetResolution(MPoint2 resolution)
        {
            graphicsDeviceManager.PreferredBackBufferWidth = resolution.X;
            graphicsDeviceManager.PreferredBackBufferHeight = resolution.Y;
            graphicsDeviceManager.ApplyChanges();
            OnResolutionChanged(resolution);
        }

        public void Update(MPoint2 windowResolution)
        {
            if (windowResolution != Resolution)
            {
                OnResolutionChanged(Resolution);
                SetResolution(windowResolution);
            }
        }
    }
}