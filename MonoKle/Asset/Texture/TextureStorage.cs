namespace MonoKle.Asset.Texture
{
    using Microsoft.Xna.Framework.Graphics;
    using IO;
    using System;
    using System.IO;

    /// <summary>
    /// Loads and maintains texture assets.
    /// </summary>
    public class TextureStorage : AbstractAssetStorage<Texture2D>
    {
        private GraphicsDevice graphicsDevice;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureStorage"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        /// <param name="whiteTexture">The white texture.</param>
        /// <param name="defaultTexture">The default texture.</param>
        public TextureStorage(GraphicsDevice graphicsDevice, Texture2D defaultTexture, Texture2D whiteTexture)
        {
            this.graphicsDevice = graphicsDevice;
            this.DefaultValue = defaultTexture;
            this.White = whiteTexture;
        }

        /// <summary>
        /// Gets a white texture.
        /// </summary>
        /// <value>
        /// The white texture.
        /// </value>
        public Texture2D White { get; private set; }

        protected override bool CheckFile(MFileInfo file)
        {
            return file.Extension.Equals(".png", StringComparison.InvariantCultureIgnoreCase)
                || file.Extension.Equals(".jpg", StringComparison.InvariantCultureIgnoreCase)
                || file.Extension.Equals(".gif", StringComparison.InvariantCultureIgnoreCase);
        }

        protected override Texture2D DoLoadStream(Stream stream)
        {
            return Texture2D.FromStream(this.graphicsDevice, stream);
        }
    }
}