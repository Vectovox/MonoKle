namespace MonoKle.Graphics
{
    using Attributes;
    using Core.Geometry;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using MonoKle.Graphics.Event;

    /// <summary>
    /// Manages graphics.
    /// </summary>
    public class GraphicsManager
    {
        private GraphicsDeviceManager graphicsDeviceManager;

        private MPoint2 resolution;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsManager"/> class.
        /// </summary>
        /// <param name="graphicsDeviceManager">The graphics device manager.</param>
        public GraphicsManager(GraphicsDeviceManager graphicsDeviceManager)
        {
            this.graphicsDeviceManager = graphicsDeviceManager;
            this.graphicsDeviceManager.PreparingDeviceSettings += PreparingDeviceSettings;
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
        public GraphicsDevice GraphicsDevice { get { return graphicsDeviceManager.GraphicsDevice; } }

        /// <summary>
        /// Gets or sets the resolution.
        /// </summary>
        /// <value>
        /// The resolution.
        /// </value>
        public MPoint2 Resolution
        {
            get { return this.resolution; }
            set { this.SetResolution(value); }
        }

        /// <summary>
        /// Gets the center point of the current resolution.
        /// </summary>
        /// <value>
        /// The resolution center.
        /// </value>
        public MPoint2 ResolutionCenter
        {
            get; private set;
        }

        /// <summary>
        /// Gets or sets the height of the resolution.
        /// </summary>
        /// <value>
        /// The height of the resolution.
        /// </value>
        [PropertyVariableAttribute("g_res_y")]
        public int ResolutionHeight
        {
            get { return this.Resolution.Y; }
            set { this.Resolution = new MPoint2(this.Resolution.X, value); }
        }

        /// <summary>
        /// Gets or sets the width of the resolution.
        /// </summary>
        /// <value>
        /// The width of the resolution.
        /// </value>
        [PropertyVariableAttribute("g_res_x")]
        public int ResolutionWidth
        {
            get { return this.Resolution.X; }
            set { this.Resolution = new MPoint2(value, this.Resolution.Y); }
        }

        private void OnResolutionChanged(MPoint2 newResolution)
        {
            var v = this.ResolutionChanged;
            if (v != null)
            {
                v(this, new ResolutionChangedEventArgs(newResolution));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether full screen is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is fullscreen; otherwise, <c>false</c>.
        /// </value>
        [PropertyVariableAttribute("g_fullscreen")]
        public bool IsFullscreen
        {
            get { return graphicsDeviceManager.IsFullScreen; }
            set { this.SetFullscreenEnabled(value); }
        }

        private void SetFullscreenEnabled(bool enabled)
        {
            if (enabled && this.IsFullscreen == false ||
                enabled == false && this.IsFullscreen)
            {
                this.ToggleFullscren();
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
        
        // TODO: PreparingDeviceSettings does only fire the first time applychanges is called (or maybe only before game started).
        private void PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            this.SetResolution(new MPoint2(e.GraphicsDeviceInformation.PresentationParameters.BackBufferWidth,
                e.GraphicsDeviceInformation.PresentationParameters.BackBufferHeight));
            //Vector2DInteger value = new Vector2DInteger(
            //        e.GraphicsDeviceInformation.PresentationParameters.BackBufferWidth,
            //        e.GraphicsDeviceInformation.PresentationParameters.BackBufferHeight
            //    );
            //ScreenSize = value;
            //ScreenCenter = value / 2;
        }

        private void SetResolution(MPoint2 resolution)
        {
            this.graphicsDeviceManager.PreferredBackBufferWidth = resolution.X;
            this.graphicsDeviceManager.PreferredBackBufferHeight = resolution.Y;
            this.resolution = resolution;          // TODO: Remove when PreparingDeviceSettings is received
            this.ResolutionCenter = resolution / 2;    // TODO: Remove when PreparingDeviceSettings is received
            this.graphicsDeviceManager.ApplyChanges();
            this.OnResolutionChanged(resolution);
        }
    }
}