using Microsoft.Xna.Framework;

namespace MonoKle
{
    public static class ColorExtensions
    {
        /// <summary>
        /// Returns the average float representation of the R,G,B,A components.
        /// </summary>
        /// <param name="color">The color.</param>
        public static float Average(this Color color) => (color.R + color.G + color.B + color.A) / 4f / 255f;

        /// <summary>
        /// Returns the average byte representation of the R,G,B,A components.
        /// </summary>
        /// <param name="color">The color.</param>
        public static byte AverageByte(this Color color) => (byte)((color.R + color.G + color.B + color.A) / 4);

        /// <summary>
        /// Returns the average float representation of the R,G,B components.
        /// </summary>
        /// <param name="color">The color.</param>
        public static float AverageColor(this Color color) => (color.R + color.G + color.B) / 3f / 255f;

        /// <summary>
        /// Returns the average byte representation of the R,G,B components.
        /// </summary>
        /// <param name="color">The color.</param>
        public static byte AverageColorByte(this Color color) => (byte)((color.R + color.G + color.B) / 3);
    }
}
