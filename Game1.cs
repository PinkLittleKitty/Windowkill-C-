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
        private Point originalWindowPos;

        private MouseState prevMouseState;
        private List<Bullet> bullets;

        private readonly int WindowExpandAmount = 15;
        private int shrinkAmountPerFrame = 1;
        private int frameCount = 0;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            bullets = new List<Bullet>();
        }

        protected override void Initialize()
        {
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

                if (bullet.Position.X < 0)
                {
                    _graphics.PreferredBackBufferWidth += WindowExpandAmount;
                    _graphics.ApplyChanges();

                    playerPosition.X += WindowExpandAmount;


                    Window.Position = new Point(Window.Position.X - WindowExpandAmount, Window.Position.Y);

                    bullets.RemoveAt(i);
                    break;
                }
                else if (bullet.Position.X > GraphicsDevice.Viewport.Width)
                {
                    _graphics.PreferredBackBufferWidth += WindowExpandAmount;
                    _graphics.ApplyChanges();

                    bullets.RemoveAt(i);
                    break;
                }

                if (bullet.Position.Y < 0)
                {
                    _graphics.PreferredBackBufferHeight += WindowExpandAmount;
                    _graphics.ApplyChanges();

                    playerPosition.Y += WindowExpandAmount;


                    Window.Position = new Point(Window.Position.X, Window.Position.Y - WindowExpandAmount);

                    bullets.RemoveAt(i);
                    break;
                }
                else if (bullet.Position.Y > GraphicsDevice.Viewport.Height)
                {
                    _graphics.PreferredBackBufferHeight += WindowExpandAmount;
                    _graphics.ApplyChanges();

                    bullets.RemoveAt(i);
                    break;
                }
            }

            prevMouseState = mouseState;

            if (frameCount % 5 == 0)
            {
                ShrinkWindow(shrinkAmountPerFrame);
            }

            frameCount++;

            base.Update(gameTime);
        }

        private void ShrinkWindow(int speed)
        {
            int shrinkAmount = speed;
            if (shrinkAmount > 10)
            {
                shrinkAmount = 10 + (shrinkAmount - 10) / 2;
            }

            // Get distance to each side
            int leftDist = (int)playerPosition.X;
            int rightDist = (int)(GraphicsDevice.Viewport.Width - playerPosition.X);
            int topDist = (int)playerPosition.Y;
            int bottomDist = (int)(GraphicsDevice.Viewport.Height - playerPosition.Y);

            // Calculate new width and height based on distances
            int newWidth = leftDist + rightDist - shrinkAmount;
            int newHeight = topDist + bottomDist - shrinkAmount;

            // Set new size
            _graphics.PreferredBackBufferWidth = newWidth;
            _graphics.PreferredBackBufferHeight = newHeight;
            _graphics.ApplyChanges();

            // Adjust position
            int xOffset = leftDist - shrinkAmount / 2;
            int yOffset = topDist - shrinkAmount / 2;
            Window.Position = new Point(xOffset, yOffset);
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
