namespace MonoKle.Assets.Effect
{
    using Microsoft.Xna.Framework.Graphics;
    using System.IO;

    /// <summary>
    /// Class storing and loading effect files.
    /// </summary>
    public class EffectStorage : AbstractAssetStorage<Effect>
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

        protected override Effect DoLoadStream(Stream stream)
        {
            BinaryReader br = new BinaryReader(stream);
            byte[] byteCode = br.ReadBytes((int)stream.Length);
            Effect effect = new Effect(this.graphicsDevice, byteCode);
            return effect;
        }
    }
}