using MonoKle.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoKle.Graphics;
using System;
using System.IO;

namespace MonoKle.Asset.Texture
{
    /// <summary>
    /// Loads and maintains texture assets.
    /// </summary>
    public class TextureStorage : AbstractAssetStorage<Texture2D>
    {
        private GraphicsDevice graphicsDevice;

        /// <summary>
        /// Gets a square white texture.
        /// </summary>
        public Texture2D White { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureStorage"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        public TextureStorage(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            DefaultValue = new Texture2D(graphicsDevice, 16, 16).Fill(Color.White);
            White = new Texture2D(graphicsDevice, 16, 16).Fill(Color.Purple);
        }

        protected override bool CheckFile(MFileInfo file) =>
            file.Extension.Equals(".png", StringComparison.InvariantCultureIgnoreCase)
            || file.Extension.Equals(".jpg", StringComparison.InvariantCultureIgnoreCase)
            || file.Extension.Equals(".gif", StringComparison.InvariantCultureIgnoreCase);

        protected override Texture2D DoLoadStream(Stream stream) => Texture2D.FromStream(graphicsDevice, stream);
    }
}