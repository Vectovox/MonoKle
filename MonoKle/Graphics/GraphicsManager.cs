using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoKle.Configuration;
using System;

namespace MonoKle.Graphics
{
    /// <summary>
    /// Facade for graphics management.
    /// </summary>
    public class GraphicsManager : IDisposable
    {
        private readonly Game _gameInstance;

        private MPoint2 _resolution;
        private GraphicsMode _graphicsMode = GraphicsMode.Windowed;
        private bool _vSync = true;
        private bool _displayDirty = true;
        private bool _isDisposed;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsManager"/> class.
        /// </summary>
        public GraphicsManager(Game gameInstance)
        {
            GraphicsDeviceManager = new GraphicsDeviceManager(gameInstance);
            _gameInstance = gameInstance;
            GraphicsDeviceManager.DeviceCreated += GraphicsDeviceManager_DeviceCreated;
            gameInstance.Window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            if (_graphicsMode == GraphicsMode.Windowed)
            {
                // Window was manually changed so make a hard resolution assignment
                Resolution = new MPoint2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            }
            else if (_graphicsMode == GraphicsMode.Borderless)
            {
                // Fixes orientation change automatically on mobile since 
                // recalculating borderless assigns backbuffer to adapter values
                _displayDirty = true;
            }
        }

        private void GraphicsDeviceManager_DeviceCreated(object sender, EventArgs e)
        {
            // Sets the initial resolution to use
            Resolution = new MPoint2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
        }

        /// <summary>
        /// Occurs when resolution is changed.
        /// </summary>
        public event ResolutionChangedEventHandler? BackBufferChanged;

        /// <summary>
        /// Gets the underlying graphics device.
        /// </summary>
        public GraphicsDevice GraphicsDevice => GraphicsDeviceManager.GraphicsDevice;

        /// <summary>
        /// Gets the underlying <see cref="Microsoft.Xna.Framework.GraphicsDeviceManager"/>.
        /// </summary>
        public GraphicsDeviceManager GraphicsDeviceManager { get; }

        /// <summary>
        /// Gets the underlying backbuffer resolution.
        /// </summary>
        [CVar("graphics_backbuffer_resolution")]
        public MPoint2 BackBufferResolution => new(GraphicsDeviceManager.PreferredBackBufferWidth, GraphicsDeviceManager.PreferredBackBufferHeight);

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

        /// <summary>
        /// Gets or sets wether VSync is enabled.
        /// </summary>
        [CVar("graphics_vsync")]
        public bool VSyncEnabled
        {
            get => _vSync;
            set { _vSync = value; _displayDirty = true; }
        }

        /// <summary>
        /// Gets or sets wether frames are locked to the frequency set by <see cref="FrameLockTarget"/>.
        /// </summary>
        [CVar("graphics_framelock")]
        public bool FrameLockEnabled
        {
            get => _gameInstance.IsFixedTimeStep;
            set { _gameInstance.IsFixedTimeStep = value; }
        }

        /// <summary>
        /// Gets or sets the target render frequency.
        /// </summary>
        [CVar("graphics_framelock_target")]
        public int FrameLockTarget
        {
            get => (int)(1 / _gameInstance.TargetElapsedTime.TotalSeconds);
            set { _gameInstance.TargetElapsedTime = TimeSpan.FromSeconds(1f / value); }
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
                GraphicsDeviceManager.SynchronizeWithVerticalRetrace = _vSync;
                GraphicsDeviceManager.ApplyChanges();
                _displayDirty = false;
                OnBackbufferChanged(BackBufferResolution);
            }
        }

        private void OnBackbufferChanged(MPoint2 newResolution) => BackBufferChanged?.Invoke(this, new ResolutionChangedEventArgs(newResolution));

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    // Free managed resources
                    GraphicsDeviceManager.Dispose();
                }

                // Set large fields to null
                _isDisposed = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}