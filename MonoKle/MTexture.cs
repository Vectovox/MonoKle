using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoKle
{
    /// <summary>
    /// Texture asset with underlying data and methods of animation.
    /// </summary>
    public class MTexture
    {
        /// <summary>
        /// The raw data that the texture exists in.
        /// </summary>
        public readonly Texture2D TextureData;

        /// <summary>
        /// The source atlas rectangle to get the texture from the data in <see cref="TextureData"/>.
        /// </summary>
        public readonly MRectangleInt AtlasRectangle;

        /// <summary>
        /// The amount of animation frames.
        /// </summary>
        public readonly int FrameCount;

        /// <summary>
        /// The frame rate, in frames per second, of the animation.
        /// </summary>
        public readonly int FrameRate;

        /// <summary>
        /// The margin between different frames in the atlas. E.g. used for bleed zones in sprite maps or animations.
        /// </summary>
        public readonly int AtlasMargin;

        public MTexture(Texture2D texture) : this(texture, new MRectangleInt(0, 0, texture.Width, texture.Height)) { }

        public MTexture(Texture2D texture, int frameCount, int frameRate)
            : this(texture, new MRectangleInt(0, 0, texture.Width, texture.Height), frameCount, frameRate) { }

        public MTexture(Texture2D texture, MRectangleInt atlasRectangle) : this(texture, atlasRectangle, 1, 1) { }

        public MTexture(Texture2D texture, MRectangleInt atlasRectangle, int frameCount, int frameRate)
            : this(texture, atlasRectangle, frameCount, frameRate, 0) { }

        public MTexture(Texture2D texture, MRectangleInt atlasRectangle, int frameCount, int frameRate, int atlasMargin)
        {
            if (frameCount < 1)
            {
                throw new ArgumentException($"Invalid frame count '{frameCount}'. Must be greater than 0.");
            }
            if (frameRate < 1)
            {
                throw new ArgumentException($"Invalid frame rate '{frameRate}'. Must be greater than 0.");
            }
            if (atlasMargin < 0)
            {
                throw new ArgumentException($"Invalid atlas margin '{atlasMargin}'. Must be greater or equal to 0.");
            }

            TextureData = texture;
            AtlasRectangle = atlasRectangle;
            AtlasMargin = atlasMargin;
            FrameCount = frameCount;
            FrameRate = frameRate;
        }

        /// <summary>
        /// Gets the duration of the animation.
        /// </summary>
        public TimeSpan AnimationDuration => TimeSpan.FromSeconds(FrameCount / (float)FrameRate);

        /// <summary>
        /// Gets the width of a single frame once animated.
        /// </summary>
        public int FrameWidth => (AtlasRectangle.Width - AtlasMargin * FrameCount * 2) / FrameCount;

        /// <summary>
        /// Returns new instance of <see cref="MTexture"/> that represents the frame at the provided frame number.
        /// Loops the animation if the provided frame number is greater than <see cref="FrameCount"/> - 1.
        /// </summary>
        /// <param name="frameNumber">The zero indexed frame to animate to.</param>
        public MTexture Animate(int frameNumber) =>
            new MTexture(TextureData, AtlasRectangle
                .RedimensionWidth(FrameWidth)
                .TranslateX(FrameWidth * (frameNumber % FrameCount) + AtlasMargin * (frameNumber % FrameCount + 1) + AtlasMargin * (frameNumber % FrameCount)));

        /// <summary>
        /// Returns new instance of <see cref="MTexture"/> that shows the frame after the provided elapsed time. Loops
        /// the animation when elapsed time is longer than the length of the animation.
        /// </summary>
        /// <param name="elapsedTime">The elapsed time to animate to.</param>
        public MTexture Animate(TimeSpan elapsedTime) => Animate((int)(FrameRate * elapsedTime.TotalSeconds));

        /// <summary>
        /// Returns a new instance of <see cref="MTexture"/> with the atlas rectangle set to the provided value.
        /// </summary>
        /// <param name="atlas">The atlas rectangle to use.</param>
        public MTexture WithAtlas(MRectangleInt atlas) => new MTexture(TextureData, atlas, FrameCount, FrameRate, AtlasMargin);

        /// <summary>
        /// Returns a new instance of <see cref="MTexture"/> with the atlas margin set to the provided value.
        /// </summary>
        /// <param name="margin">The margin to use.</param>
        public MTexture WithMargin(int margin) => new MTexture(TextureData, AtlasRectangle, FrameCount, FrameRate, margin);
    }
}