using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace MonoKle.Asset
{
    /// <summary>
    /// Class storing and loading effect files.
    /// </summary>
    public class EffectStorage : AbstractAssetStorage<Effect>
    {
        private readonly GraphicsDevice graphicsDevice;

        /// <summary>
        /// Initializes a new instance of <see cref="EffectStorage"/>.
        /// </summary>
        /// <param name="graphicsDevice">Graphics device.</param>
        public EffectStorage(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }

        protected override Effect DoLoadStream(Stream stream)
        {
            var br = new BinaryReader(stream);
            byte[] byteCode = br.ReadBytes((int)stream.Length);
            var effect = new Effect(graphicsDevice, byteCode);
            return effect;
        }

        protected override bool FileSupported(string extension) => extension.Equals(".fx");
    }
}