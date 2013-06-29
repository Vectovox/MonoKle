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
    public static class GraphicsManager
    {
        private static GraphicsDeviceManager graphicsDeviceManager;
        
        /// <summary>
        /// Gets the current screen size.
        /// </summary>
        public static Vector2Int32 ScreenSize { get; private set; }
        
        /// <summary>
        /// Gets the current screen center.
        /// </summary>
        public static Vector2Int32 ScreenCenter { get; private set; }

        internal static void SetGraphicsDeviceManager(GraphicsDeviceManager graphicsDeviceManager)
        {
            GraphicsManager.graphicsDeviceManager = graphicsDeviceManager;
            GraphicsManager.graphicsDeviceManager.PreparingDeviceSettings += PreparingDeviceSettings;
            // TODO: PerparingDeviceSettings event does not fire in the current MonoGame version.
        }

        public static GraphicsDevice GetGraphicsDevice()
        {
            return graphicsDeviceManager.GraphicsDevice;
        }

        public static void SetScreenSize(Vector2Int32 size)
        {
            graphicsDeviceManager.PreferredBackBufferWidth = size.X;
            graphicsDeviceManager.PreferredBackBufferHeight = size.Y;
            ScreenSize = size;          // TODO: Remove when PreparingDeviceSettings is received
            ScreenCenter = size / 2;    // TODO: Remove when PreparingDeviceSettings is received
            graphicsDeviceManager.ApplyChanges();
            // TODO: ApplyChanges() does not work in the current MonoGame version.
        }

        public static void SetFullscreenEnabled(bool enabled)
        {
            if (enabled && graphicsDeviceManager.IsFullScreen == false ||
                enabled == false && graphicsDeviceManager.IsFullScreen)
            {
                // TODO: This does not work in the current MonoGame version.
                graphicsDeviceManager.ToggleFullScreen();
            }
        }

        private static void PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            Vector2Int32 value = new Vector2Int32(
                    e.GraphicsDeviceInformation.PresentationParameters.BackBufferWidth,
                    e.GraphicsDeviceInformation.PresentationParameters.BackBufferHeight
                );
            ScreenSize = value;
            ScreenCenter = value / 2;
        }
    }
}
