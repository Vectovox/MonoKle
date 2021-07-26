using MonoKle.Asset;

namespace MonoKle.Engine
{
    /// <summary>
    /// Container for MonoKle Engine asset storages.
    /// </summary>
    public class AssetStorageContainer
    {
        /// <summary>
        /// Gets the sound effect storage, loading and providing sound effects.
        /// </summary>
        public SoundEffectStorage SoundEffect { get; set; }

        /// <summary>
        /// Gets the effect storage, loading and providing effects.
        /// </summary>
        public EffectStorage Effect { get; set; }

        /// <summary>
        /// Gets the font storage, loading and providing fonts.
        /// </summary>
        public FontStorage Font { get; set; }

        /// <summary>
        /// Gets the texture storage, loading and providing textures.
        /// </summary>
        public TextureStorage Texture { get; set; }
    }
}
