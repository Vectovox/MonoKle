using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace MonoKle.Asset
{
    /// <summary>
    /// Class storing and loading effect files.
    /// </summary>
    public class EffectStorage : AbstractAssetStorage<Effect>
    {
        private readonly GraphicsDevice _graphicsDevice;

        /// <summary>
        /// Initializes a new instance of <see cref="EffectStorage"/>.
        /// </summary>
        /// <param name="graphicsDevice">Graphics device.</param>
        public EffectStorage(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
        }

        protected override Effect DoLoadStream(Stream stream)
        {
            int val = stream.ReadByte();
            List<byte> bytes = new List<byte>();
            while (val != -1)
            {
                bytes.Add((byte)val);
                val = stream.ReadByte();
            }
            var effect = new Effect(_graphicsDevice, bytes.ToArray());
            return effect;
        }

        protected override bool FileSupported(string extension) => extension.Equals(".mfx");
    }
}
