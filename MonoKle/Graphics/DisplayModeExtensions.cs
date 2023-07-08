using Microsoft.Xna.Framework.Graphics;

namespace MonoKle.Graphics
{
    public static class DisplayModeExtensions
    {
        public static MPoint2 Dimensions(this DisplayMode mode) => new(mode.Width, mode.Height);
    }
}
