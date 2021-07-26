using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using MonoKle.Configuration;
using System;

namespace MonoKle
{
    public class Mixer
    {
        private float _masterVolume;
        private float _songVolume;
        private float _songFade;
        private float _effectVolume;

        public Mixer()
        {
            MasterVolume = 1f;
            SongVolume = 1f;
            SongFade = 1f;
            EffectVolume = 1f;
        }

        /// <summary>
        /// Gets the overall master volume. Applied to both songs and sound effects. Valid values are [0,1].
        /// </summary>
        [CVar("mixer_master")]
        public float MasterVolume
        {
            get => _masterVolume;
            set
            {
                AssertValue(value);
                _masterVolume = value;
                UpdateSongVolume();
                UpdateEffectVolume();
            }
        }

        /// <summary>
        /// Gets or sets the song master volume. Valid values are [0,1].
        /// </summary>
        [CVar("mixer_song")]
        public float SongVolume
        {
            get => _songVolume;
            set
            {
                AssertValue(value);
                _songVolume = value;
                UpdateSongVolume();
            }
        }

        /// <summary>
        /// Gets or sets a secondary song volume for fading in/out music without changing the master setting. Valid values are [0,1].
        /// </summary>
        [CVar("mixer_song_fade")]
        public float SongFade
        {
            get => _songFade;
            set
            {
                AssertValue(value);
                _songFade = value;
                UpdateSongVolume();
            }
        }

        /// <summary>
        /// Gets or sets the sound effect master volume. Valid values are [0,1].
        /// </summary>
        [CVar("mixer_effect")]
        public float EffectVolume
        {
            get => _effectVolume;
            set
            {
                AssertValue(value);
                _effectVolume = value;
                UpdateEffectVolume();
            }
        }

        private void AssertValue(float value)
        {
            if (value < 0 || value > 1)
            {
                throw new ArgumentException("Volume must be provided in the interval [0,1]");
            }
        }

        private void UpdateSongVolume() => MediaPlayer.Volume = _masterVolume * _songVolume * _songFade;

        private void UpdateEffectVolume() => SoundEffect.MasterVolume = _masterVolume * _effectVolume;
    }
}
