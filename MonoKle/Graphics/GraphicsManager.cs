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
        private MPoint2 _resolution;
        private GraphicsMode _graphicsMode;
        private bool _displayDirty;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsManager"/> class.
        /// </summary>
        public GraphicsManager(GraphicsDeviceManager graphicsDeviceManager)
        {
            GraphicsDeviceManager = graphicsDeviceManager;
        }

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
        /// Gets or sets the backbuffer resolution. Used in <see cref="GraphicsMode.Windowed"/> and <see cref="GraphicsMode.Fullscreen"/>.
        /// </summary>
        [CVar("graphics_resolution")]
        public MPoint2 Resolution
        {
            get => _resolution;
            set { _resolution = value; _displayDirty = true; }
        }

        /// <summary>
        /// Gets or sets the height of the backbuffer resolution. Used in <see cref="GraphicsMode.Windowed"/> and <see cref="GraphicsMode.Fullscreen"/>.
        /// </summary>
        [CVar("graphics_resolution_height")]
        public int ResolutionHeight
        {
            get { return Resolution.Y; }
            set { Resolution = new MPoint2(Resolution.X, value); }
        }

        /// <summary>
        /// Gets or sets the width of the backbuffer resolution. Used in <see cref="GraphicsMode.Windowed"/> and <see cref="GraphicsMode.Fullscreen"/>.
        /// </summary>
        [CVar("graphics_resolution_width")]
        public int ResolutionWidth
        {
            get { return Resolution.X; }
            set { Resolution = new MPoint2(value, Resolution.Y); }
        }

        /// <summary>
        /// Gets or sets the graphics mode.
        /// </summary>
        [CVar("graphics_mode")]
        public GraphicsMode GraphicsMode
        {
            get => _graphicsMode;
            set { _graphicsMode = value; _displayDirty = true; }
        }

        public void Update()
        {
            if (_displayDirty)
            {
                if (_graphicsMode == GraphicsMode.Borderless)
                {
                    // Reset fullscreen so we can query the native resolution
                    GraphicsDeviceManager.IsFullScreen = false;
                    GraphicsDeviceManager.ApplyChanges();
                    // Apply mode
                    GraphicsDeviceManager.HardwareModeSwitch = false;
                    GraphicsDeviceManager.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
                    GraphicsDeviceManager.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height;
                    GraphicsDeviceManager.IsFullScreen = true;
                }
                else
                {
                    GraphicsDeviceManager.HardwareModeSwitch = true;
                    GraphicsDeviceManager.PreferredBackBufferWidth = _resolution.X;
                    GraphicsDeviceManager.PreferredBackBufferHeight = _resolution.Y;
                    GraphicsDeviceManager.IsFullScreen = _graphicsMode == GraphicsMode.Fullscreen;
                }
                GraphicsDeviceManager.ApplyChanges();
                _displayDirty = false;
                OnResolutionChanged(new MPoint2(GraphicsDeviceManager.PreferredBackBufferWidth, GraphicsDeviceManager.PreferredBackBufferHeight));
            }
        }

        private void OnResolutionChanged(MPoint2 newResolution) => ResolutionChanged?.Invoke(this, new ResolutionChangedEventArgs(newResolution));
    }
}