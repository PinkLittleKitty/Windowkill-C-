using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Windowkill
{
    public class Bullet
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float radius;

        public Bullet(Vector2 position, Vector2 velocity, float radius)
        {
            Position = position;
            Velocity = velocity;
            this.radius = radius;
        }

        public void Update()
        {
            // Update the position based on velocity
            Position += Velocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the bullet
            DrawingUtils.DrawFilledCircle(spriteBatch, Position, radius, Color.Red);
        }
    }
}
