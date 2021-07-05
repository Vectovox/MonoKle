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
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsManager"/> class.
        /// </summary>
        /// <param name="graphicsDeviceManager">The graphics device manager.</param>
        public GraphicsManager(GraphicsDeviceManager graphicsDeviceManager) => GraphicsDeviceManager = graphicsDeviceManager;

        /// <summary>
        /// Occurs when resolution is changed.
        /// </summary>
        public event ResolutionChangedEventHandler? ResolutionChanged;

        /// <summary>
        /// Gets the underlying graphics device.
        /// </summary>
        public GraphicsDevice GraphicsDevice => GraphicsDeviceManager.GraphicsDevice;

        /// <summary>
        /// Gets the underlying <see cref="Microsoft.Xna.Framework.GraphicsDeviceManager"/>.
        /// </summary>
        public GraphicsDeviceManager GraphicsDeviceManager { get; }

        /// <summary>
        /// Gets or sets the backbuffer resolution.
        /// </summary>
        [CVar("graphics_backbuffer_size")]
        public MPoint2 Resolution
        {
            get { return new MPoint2(GraphicsDeviceManager.PreferredBackBufferWidth, GraphicsDeviceManager.PreferredBackBufferHeight); }
            set { SetResolution(value); }
        }

        /// <summary>
        /// Gets or sets the height of the backbuffer resolution.
        /// </summary>
        [CVar("graphics_backbuffer_size_y")]
        public int ResolutionHeight
        {
            get { return Resolution.Y; }
            set { Resolution = new MPoint2(Resolution.X, value); }
        }

        /// <summary>
        /// Gets or sets the width of the backbuffer resolution.
        /// </summary>
        [CVar("graphics_backbuffer_size_x")]
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
        [CVar("graphics_fullscreen")]
        public bool IsFullscreen
        {
            get { return GraphicsDeviceManager.IsFullScreen; }
            set { SetFullscreenEnabled(value); }
        }

        private void SetFullscreenEnabled(bool enabled)
        {
            if (enabled != IsFullscreen)
            {
                ToggleFullscreen();
            }
        }

        /// <summary>
        /// Toggles fullscren.
        /// </summary>
        public void ToggleFullscreen()
        {
            GraphicsDeviceManager.ToggleFullScreen();
            GraphicsDeviceManager.ApplyChanges();
        }

        private void SetResolution(MPoint2 resolution)
        {
            GraphicsDeviceManager.PreferredBackBufferWidth = resolution.X;
            GraphicsDeviceManager.PreferredBackBufferHeight = resolution.Y;
            GraphicsDeviceManager.ApplyChanges();
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