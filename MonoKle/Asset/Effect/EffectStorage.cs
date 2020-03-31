using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace MonoKle.Asset.Effect
{
    /// <summary>
    /// Class storing and loading effect files.
    /// </summary>
    public class EffectStorage : AbstractAssetStorage<Microsoft.Xna.Framework.Graphics.Effect>
    {
        private GraphicsDevice graphicsDevice;

        /// <summary>
        /// Initializes a new instance of <see cref="EffectStorage"/>.
        /// </summary>
        /// <param name="graphicsDevice">Graphics device.</param>
        public EffectStorage(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }

        protected override Microsoft.Xna.Framework.Graphics.Effect DoLoadStream(Stream stream)
        {
            var br = new BinaryReader(stream);
            byte[] byteCode = br.ReadBytes((int)stream.Length);
            var effect = new Microsoft.Xna.Framework.Graphics.Effect(graphicsDevice, byteCode);
            return effect;
        }
    }
}