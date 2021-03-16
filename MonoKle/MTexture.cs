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

        public MTexture(Texture2D texture) : this(texture, new MRectangleInt(0, 0, texture.Width, texture.Height)) { }

        public MTexture(Texture2D texture, int frameCount, int frameRate)
            : this(texture, new MRectangleInt(0, 0, texture.Width, texture.Height), frameCount, frameRate) { }

        public MTexture(Texture2D texture, MRectangleInt atlasRectangle) : this(texture, atlasRectangle, 1, 1) { }

        public MTexture(Texture2D texture, MRectangleInt atlasRectangle, int frameCount, int frameRate)
        {
            if (frameCount < 1)
            {
                throw new ArgumentException($"Invalid framecount '{frameCount}'. Must be greater than 0.");
            }
            TextureData = texture;
            AtlasRectangle = atlasRectangle;
            FrameCount = frameCount;
            FrameRate = frameRate;
        }

        private int FrameWidth => AtlasRectangle.Width / FrameCount;

        /// <summary>
        /// Returns new instance of <see cref="MTexture"/> that represents the frame at the provided frame number.
        /// Loops the animation if the provided frame number is greater than <see cref="FrameCount"/> - 1.
        /// </summary>
        /// <param name="frameNumber">The zero indexed frame to animate to.</param>
        public MTexture Animate(int frameNumber) =>
            new MTexture(TextureData, AtlasRectangle
                .RedimensionWidth(FrameWidth)
                .TranslateX(FrameWidth * (frameNumber % FrameCount)));

        /// <summary>
        /// Returns new instance of <see cref="MTexture"/> that shows the frame after the provided elapsed time. Loops
        /// the animation when elapsed time is longer than the length of the animation.
        /// </summary>
        /// <param name="frame">The elapsed time to animate to.</param>
        public MTexture Animate(TimeSpan elapsedTime) => Animate((int)(FrameRate * elapsedTime.TotalSeconds));

        /// <summary>
        /// Returns a new instance of <see cref="MTexture"/> with the atlas rectangle set to the provided value.
        /// </summary>
        /// <param name="atlas">The atlas rectangle to use.</param>
        public MTexture WithAtlas(MRectangleInt atlas) => new MTexture(TextureData, atlas, FrameCount, FrameRate);
    }
}