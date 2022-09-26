using Microsoft.Xna.Framework.Audio;
using System;

namespace MonoKle.Asset
{
    /// <summary>
    /// MonoKle sound effect instance for playing sounds and manipulating its playback characteristics.
    /// </summary>
    public class MSoundEffectInstance
    {
        private readonly SoundEffectInstance _instance;
        private static readonly Random _random = new();

        /// <summary>
        /// Gets the underlying sound effect.
        /// </summary>
        public SoundEffect Effect { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="MSoundEffectInstance"/>.
        /// </summary>
        /// <param name="effect">The sound effect it is based on.</param>
        public MSoundEffectInstance(SoundEffect effect)
        {
            Effect = effect;
            _instance = effect.CreateInstance();
        }

        /// <summary>
        /// Starts playback, resuming it if it was paused previously.
        /// </summary>
        public void Play()
        {
            _instance.Pitch = Pitch + ((float)_random.NextDouble() - 0.5f) * 2 * PitchVariation;
            _instance.Play();
        }

        /// <summary>
        /// Pauses playback.
        /// </summary>
        public void Pause() => _instance.Pause();

        /// <summary>
        /// Gets or sets the left-right pan of the sound, from [-1.0, 1.0] with 0.0 being centered.
        /// </summary>
        public float Pan
        {
            get => _instance.Pan;
            set => _instance.Pan = value;
        }

        /// <summary>
        /// Gets or sets the pitch adjustment in the range of [-1.0, 1.0], where 0 is no change.
        /// </summary>
        public float Pitch { get; set; }

        /// <summary>
        /// The random variation in pitch (up and down) upon playing.
        /// </summary>
        /// <remarks>
        /// Since it is both up and down, a pitch of 0.1 and variation of 0.2 will take values [-0.1, 0.3].
        /// </remarks>
        public float PitchVariation { get; set; }

        /// <summary>
        /// Gets the current playback state.
        /// </summary>
        public SoundState State => _instance.State;

        /// <summary>
        /// Stops playback immediately.
        /// </summary>
        public void Stop() => _instance.Stop();

        /// <summary>
        /// Stops immediately or as authored.
        /// </summary>
        /// <param name="immediate">True if immediate; false if authored.</param>
        public void Stop(bool immediate) => _instance.Stop(immediate);

        /// <summary>
        /// Gets or sets the volume, with 0.0 is silence and 1.0 is full volume.
        /// </summary>
        public float Volume
        {
            get => _instance.Volume;
            set => _instance.Volume = value;
        }

        /// <summary>
        /// Gets or sets whether the sound loops upon playing it.
        /// </summary>
        public bool IsLooped
        {
            get => _instance.IsLooped;
            set => _instance.IsLooped = value;
        }

        /// <summary>
        /// Fluent method that sets pan to the given value.
        /// </summary>
        /// <param name="pan">The pan value.</param>
        /// <returns>This instance of <see cref="MSoundEffectInstance"/>.</returns>
        public MSoundEffectInstance WithPan(float pan)
        {
            Pan = pan;
            return this;
        }

        /// <summary>
        /// Fluent method that sets pitch to the given value.
        /// </summary>
        /// <param name="pitch">The pitch value.</param>
        /// <returns>This instance of <see cref="MSoundEffectInstance"/>.</returns>
        public MSoundEffectInstance WithPitch(float pitch)
        {
            Pitch = pitch;
            return this;
        }

        /// <summary>
        /// Fluent method that sets pitch variation to the given value.
        /// </summary>
        /// <param name="variation">The pitch variation value.</param>
        /// <returns>This instance of <see cref="MSoundEffectInstance"/>.</returns>
        public MSoundEffectInstance WithPitchVariation(float variation)
        {
            PitchVariation = variation;
            return this;
        }

        /// <summary>
        /// Fluent method that sets volume to the given value.
        /// </summary>
        /// <param name="volume">The volume value.</param>
        /// <returns>This instance of <see cref="MSoundEffectInstance"/>.</returns>
        public MSoundEffectInstance WithVolume(float volume)
        {
            Volume = volume;
            return this;
        }

        /// <summary>
        /// Fluent method that sets looping to the given value.
        /// </summary>
        /// <param name="isLooped">True if looped; otherwise false.</param>
        /// <returns>This instance of <see cref="MSoundEffectInstance"/>.</returns>
        public MSoundEffectInstance WithLooped(bool isLooped)
        {
            IsLooped = isLooped;
            return this;
        }
    }
}