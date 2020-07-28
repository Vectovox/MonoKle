using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoKle.Graphics;
using System;
using System.IO;

namespace MonoKle.Asset
{
    /// <summary>
    /// Loads and maintains texture assets.
    /// </summary>
    public class TextureStorage : AbstractAssetStorage<Texture2D>
    {
        private readonly GraphicsDevice graphicsDevice;

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

        protected override bool FileSupported(string extension) =>
            extension.Equals(".png", StringComparison.InvariantCultureIgnoreCase)
            || extension.Equals(".jpg", StringComparison.InvariantCultureIgnoreCase)
            || extension.Equals(".gif", StringComparison.InvariantCultureIgnoreCase);

        protected override Texture2D DoLoadStream(Stream stream) => Texture2D.FromStream(graphicsDevice, stream);
    }
}