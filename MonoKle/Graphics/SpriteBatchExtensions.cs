using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoKle.Graphics
{
    public static class SpriteBatchExtensions
    {
        public static void Draw(this SpriteBatch spriteBatch, MTexture texture, Vector2 position, Color color,
            float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth) =>
            spriteBatch.Draw(texture.Data, position, texture.AtlasRectangle, color, rotation, origin, scale, effects, layerDepth);

        public static void Draw(this SpriteBatch spriteBatch, MTexture texture, Vector2 position, Color color,
            float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth) =>
            spriteBatch.Draw(texture.Data, position, texture.AtlasRectangle, color, rotation, origin, scale, effects, layerDepth);

        public static void Draw(this SpriteBatch spriteBatch, MTexture texture, Rectangle destinationRectangle,
            Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth) =>
            spriteBatch.Draw(texture.Data, destinationRectangle, texture.AtlasRectangle, color, rotation, origin, effects, layerDepth);

        public static void Draw(this SpriteBatch spriteBatch, MTexture texture, Vector2 position, Color color) =>
            spriteBatch.Draw(texture.Data, position, texture.AtlasRectangle, color);

        public static void Draw(this SpriteBatch spriteBatch, MTexture texture, Rectangle destinationRectangle, Color color) =>
            spriteBatch.Draw(texture.Data, destinationRectangle, texture.AtlasRectangle, color);
    }
}
