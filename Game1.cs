using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Windowkill
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D playerTexture;
        private Vector2 playerPosition;
        private MouseState prevMouseState;
        private List<Bullet> bullets;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            bullets = new List<Bullet>();
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            playerTexture = Content.Load<Texture2D>("player");
            playerPosition = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

           // Player Movement
           var KeyboardState = Keyboard.GetState();
           if (KeyboardState.IsKeyDown(Keys.W))
           {
               playerPosition.Y -= 5;
           }
           if (KeyboardState.IsKeyDown(Keys.S))
           {
               playerPosition.Y += 5;
           }
           if (KeyboardState.IsKeyDown(Keys.A))
           {
               playerPosition.X -= 5;
           }
           if (KeyboardState.IsKeyDown(Keys.D))
           {
               playerPosition.X += 5;
           }

           if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
           {
                ShootBullet(mouseState.Position.ToVector2());
           }

           foreach (var bullet in bullets)
           {
               bullet.Update();
           }

           // Remove the bullets that are out of the screen
           bullets.RemoveAll(b => !GraphicsDevice.Viewport.Bounds.Contains(b.Position));

           prevMouseState = mouseState;

           base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            Vector2 playerScale = new Vector2(0.07f);


            _spriteBatch.Draw(playerTexture, playerPosition, null, Color.White, 0f, Vector2.Zero, playerScale, SpriteEffects.None, 0f);

            foreach (var bullet in bullets)
            {
                bullet.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ShootBullet(Vector2 targetPosition)
        {
            Vector2 direction = Vector2.Normalize(targetPosition - playerPosition);
            bullets.Add(new Bullet(playerPosition, direction * 5));
        }
    }
}