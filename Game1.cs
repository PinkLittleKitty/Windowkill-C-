using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Windowkill
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Vector2 playerPosition;
        private MouseState prevMouseState;
        private List<Bullet> bullets;
        private bool isShooting;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            bullets = new List<Bullet>();
        }

        protected override void Initialize()
        {
            // Set player initial position
            playerPosition = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            DrawingUtils.Initialize(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Move the player
            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.W))
                playerPosition.Y -= 5;
            if (keyboardState.IsKeyDown(Keys.S))
                playerPosition.Y += 5;
            if (keyboardState.IsKeyDown(Keys.A))
                playerPosition.X -= 5;
            if (keyboardState.IsKeyDown(Keys.D))
                playerPosition.X += 5;

            // Ensure player stays within window bounds
            playerPosition.X = MathHelper.Clamp(playerPosition.X, 0, GraphicsDevice.Viewport.Width);
            playerPosition.Y = MathHelper.Clamp(playerPosition.Y, 0, GraphicsDevice.Viewport.Height);

            // Shooting
            if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
            {
                ShootBullet(mouseState.Position.ToVector2());
            }

            // Update bullets
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                Bullet bullet = bullets[i];
                bullet.Update();

                // Check if bullet hits the window
                if (!GraphicsDevice.Viewport.Bounds.Contains(bullet.Position))
                {

                    _graphics.PreferredBackBufferWidth += 10;
                    _graphics.ApplyChanges();

                    bullets.RemoveAt(i);
                }
            }

            prevMouseState = mouseState;

            base.Update(gameTime);
        }




        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // Draw the player
            DrawingUtils.DrawCircle(_spriteBatch, playerPosition, 20, Color.White);

            // Draw bullets
            foreach (Bullet bullet in bullets)
            {
                bullet.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ShootBullet(Vector2 targetPosition)
        {
            Vector2 direction = Vector2.Normalize(targetPosition - playerPosition);

            float bulletRadius = 5;

            bullets.Add(new Bullet(playerPosition, direction * 5, bulletRadius));
        }
    }
}
