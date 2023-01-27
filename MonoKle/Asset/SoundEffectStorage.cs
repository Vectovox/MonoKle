using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework.Audio;
using System;
using System.IO;

namespace MonoKle.Asset
{
    /// <summary>
    /// Loads and maintains sound effect assets. Supports wave file in the RIFF bitstream format:
    ///     8-bit unsigned PCM,
    ///     16-bit signed PCM,
    ///     24-bit signed PCM,
    ///     32-bit IEEE float PCMMS-ADPCM,
    ///     4-bit compressed IMA/ADPCM(IMA4),
    ///     4-bit compressed
    /// </summary>
    public class SoundEffectStorage : BasicAssetStorage<SoundEffect, MSoundEffectInstance>
    {
        public SoundEffectStorage(ILogger logger) : base(logger)
        {
        }

        protected override bool ExtensionSupported(string extension) =>
            extension.Equals(".wav", StringComparison.InvariantCultureIgnoreCase);

        protected override MSoundEffectInstance GetInstance(SoundEffect data) => new(data);

        protected override bool Load(Stream stream, string identifier, out SoundEffect? result)
        {
            try
            {
                result = SoundEffect.FromStream(stream);
                result.Name = identifier;
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"Error reading audio: {e.Message}");
                result = null;
                return false;
            }
        }
    }
}