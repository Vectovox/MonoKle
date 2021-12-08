using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using MonoKle.Logging;

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
        public EffectStorage(GraphicsDevice graphicsDevice, Logger logger) : base(logger) => _graphicsDevice = graphicsDevice;

        protected override bool FileSupported(string extension) => extension.Equals(".mfx");

        protected override bool Load(Stream stream, string identifier, out Effect? result)
        {
            int val = stream.ReadByte();
            List<byte> bytes = new List<byte>();
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
                _logger.Log($"Error reading effect: {e.Message}", LogLevel.Error);
                result = null;
                return false;
            }
        }
    }
}
