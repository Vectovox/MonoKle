namespace MonoKle.Graphics
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using MonoKle.Core;
    using MonoKle.Graphics.Event;

    /// <summary>
    /// Manages graphics.
    /// </summary>
    public class GraphicsManager
    {
        private GraphicsDeviceManager graphicsDeviceManager;

        public GraphicsManager(GraphicsDeviceManager graphicsDeviceManager)
        {
            this.graphicsDeviceManager = graphicsDeviceManager;
            this.graphicsDeviceManager.PreparingDeviceSettings += PreparingDeviceSettings;
        }

        public event ScreenSizeChangedEventHandler ScreenSizeChanged;

        /// <summary>
        /// Gets the current screen center.
        /// </summary>
        public IntVector2 ScreenCenter
        {
            get; private set;
        }

        /// <summary>
        /// Gets the current screen size.
        /// </summary>
        public IntVector2 ScreenSize
        {
            get; private set;
        }

        public GraphicsDevice GetGraphicsDevice()
        {
            return graphicsDeviceManager.GraphicsDevice;
        }

        public void SetScreenSize(IntVector2 size)
        {
            this.graphicsDeviceManager.PreferredBackBufferWidth = size.X;
            this.graphicsDeviceManager.PreferredBackBufferHeight = size.Y;
            this.ScreenSize = size;          // TODO: Remove when PreparingDeviceSettings is received
            this.ScreenCenter = size / 2;    // TODO: Remove when PreparingDeviceSettings is received
            this.graphicsDeviceManager.ApplyChanges();
            this.OnScreenSizeChanged(size);
        }

        private void OnScreenSizeChanged(IntVector2 newScreenSize)
        {
            var v = this.ScreenSizeChanged;
            if(v != null)
            {
                v(this, new ScreenSizeChangedEventArgs(newScreenSize));
            }
        }

        // TODO: Fullscreen does not work in current Mono?
        //public void SetFullscreenEnabled(bool enabled)
        //{
        //    if (enabled && graphicsDeviceManager.IsFullScreen == false ||
        //        enabled == false && graphicsDeviceManager.IsFullScreen)
        //    {
        //        // TODO: This does not work in the current MonoGame version.
        //        graphicsDeviceManager.ToggleFullScreen();
        //    }
        //}
        //public void ToggleFullscren()
        //{
        //    graphicsDeviceManager.ToggleFullScreen();
        //    graphicsDeviceManager.ApplyChanges();
        //}
        // TODO: PreparingDeviceSettings does only fire the first time applychanges is called (or maybe only before game started).
        private void PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            this.SetScreenSize(new IntVector2(e.GraphicsDeviceInformation.PresentationParameters.BackBufferWidth, e.GraphicsDeviceInformation.PresentationParameters.BackBufferHeight));
            //Vector2DInteger value = new Vector2DInteger(
            //        e.GraphicsDeviceInformation.PresentationParameters.BackBufferWidth,
            //        e.GraphicsDeviceInformation.PresentationParameters.BackBufferHeight
            //    );
            //ScreenSize = value;
            //ScreenCenter = value / 2;
        }
    }
}