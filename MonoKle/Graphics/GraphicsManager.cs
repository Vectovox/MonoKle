using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoKle.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoKle.Graphics
{
    /// <summary>
    /// Manages graphics.
    /// </summary>
    public class GraphicsManager
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        
        /// <summary>
        /// Gets the current screen size.
        /// </summary>
        public Vector2DInteger ScreenSize { get; private set; }
        
        /// <summary>
        /// Gets the current screen center.
        /// </summary>
        public Vector2DInteger ScreenCenter { get; private set; }

        public GraphicsManager(GraphicsDeviceManager graphicsDeviceManager)
        {
            this.graphicsDeviceManager = graphicsDeviceManager;
            this.graphicsDeviceManager.PreparingDeviceSettings += PreparingDeviceSettings;
            // TODO: PerparingDeviceSettings event does not fire in the current MonoGame version.
        }

        public GraphicsDevice GetGraphicsDevice()
        {
            return graphicsDeviceManager.GraphicsDevice;
        }

        public void SetScreenSize(Vector2DInteger size)
        {
            graphicsDeviceManager.PreferredBackBufferWidth = size.X;
            graphicsDeviceManager.PreferredBackBufferHeight = size.Y;
            ScreenSize = size;          // TODO: Remove when PreparingDeviceSettings is received
            ScreenCenter = size / 2;    // TODO: Remove when PreparingDeviceSettings is received
            graphicsDeviceManager.ApplyChanges();
            // TODO: ApplyChanges() does not work in the current MonoGame version.
        }

        public void SetFullscreenEnabled(bool enabled)
        {
            if (enabled && graphicsDeviceManager.IsFullScreen == false ||
                enabled == false && graphicsDeviceManager.IsFullScreen)
            {
                // TODO: This does not work in the current MonoGame version.
                graphicsDeviceManager.ToggleFullScreen();
            }
        }

        private void PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            Vector2DInteger value = new Vector2DInteger(
                    e.GraphicsDeviceInformation.PresentationParameters.BackBufferWidth,
                    e.GraphicsDeviceInformation.PresentationParameters.BackBufferHeight
                );
            ScreenSize = value;
            ScreenCenter = value / 2;
        }
    }
}
