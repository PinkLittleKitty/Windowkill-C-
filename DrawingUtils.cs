using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Windowkill
{
    public static class DrawingUtils
    {
        private static Texture2D pixelTexture;

        public static void Initialize(GraphicsDevice graphicsDevice)
        {
            // Create a 1x1 white texture
            pixelTexture = new Texture2D(graphicsDevice, 1, 1);
            pixelTexture.SetData(new[] { Color.White });
        }

        public static void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color)
        {
            Vector2 edge = end - start;
            float angle = (float)Math.Atan2(edge.Y, edge.X);

            spriteBatch.Draw(pixelTexture, new Rectangle((int)start.X, (int)start.Y, (int)edge.Length(), 1), null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }

        public static void DrawCircle(SpriteBatch spriteBatch, Vector2 center, float radius, Color color)
        {
            const int numSegments = 100;
            float angleIncrement = MathHelper.TwoPi / numSegments;

            Vector2 prevPoint = center + new Vector2(radius, 0);

            for (int i = 1; i <= numSegments; i++)
            {
                float angle = angleIncrement * i;
                Vector2 nextPoint = center + new Vector2(radius * (float)Math.Cos(angle), radius * (float)Math.Sin(angle));

                DrawLine(spriteBatch, prevPoint, nextPoint, color);

                prevPoint = nextPoint;
            }
        }

        public static void DrawFilledCircle(SpriteBatch spriteBatch, Vector2 center, float radius, Color color)
        {
            int diameter = (int)(radius * 2);
            int radiusSquared = (int)(radius * radius);

            for (int x = -diameter / 2; x < diameter / 2; x++)
            {
                int height = (int)Math.Sqrt(radiusSquared - x * x);

                for (int y = -height / 2; y < height / 2; y++)
                {
                    spriteBatch.Draw(pixelTexture, new Vector2(center.X + x, center.Y + y), color);
                }
            }
        }
    }
}
