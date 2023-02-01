using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework.Graphics;

namespace MonoKle.Asset
{
    /// <summary>
    /// Class storing and loading effect files.
    /// </summary>
    public class EffectStorage : BasicAssetStorage<Effect>
    {
        private readonly GraphicsDevice _graphicsDevice;

        /// <summary>
        /// Initializes a new instance of <see cref="EffectStorage"/>.
        /// </summary>
        /// <param name="graphicsDevice">Graphics device.</param>
        /// <param name="logger">The logger to use.</param>
        public EffectStorage(GraphicsDevice graphicsDevice, ILogger<EffectStorage> logger) : base(logger)
            => _graphicsDevice = graphicsDevice;

        protected override bool ExtensionSupported(string extension) => extension.Equals(".mfx");

        protected override bool Load(Stream stream, string identifier, out Effect? result)
        {
            int val = stream.ReadByte();
            List<byte> bytes = new();
            while (val != -1)
            {
                bytes.Add((byte)val);
                val = stream.ReadByte();
            }

            try
            {
                result = new Effect(_graphicsDevice, bytes.ToArray());
                result.Name = identifier;
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("Error reading effect: {0}", e.Message);
                result = null;
                return false;
            }
        }
    }
}
