using Microsoft.Xna.Framework.Audio;
using System;
using System.IO;

namespace MonoKle.Asset
{
    /// <summary>
    /// Loads and maintains audio assets. Supports wave file in the RIFF bitstream format:
    ///     8-bit unsigned PCM,
    ///     16-bit signed PCM,
    ///     24-bit signed PCM,
    ///     32-bit IEEE float PCMMS-ADPCM,
    ///     4-bit compressed IMA/ADPCM(IMA4),
    ///     4-bit compressed
    /// </summary>
    public class AudioStorage : BasicAssetStorage<SoundEffect>
    {
        protected override bool FileSupported(string extension) =>
            extension.Equals(".wav", StringComparison.InvariantCultureIgnoreCase);

        protected override bool Load(Stream stream, out SoundEffect result)
        {
            try
            {
                result = SoundEffect.FromStream(stream);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }
    }
}