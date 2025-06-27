using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace _3dgraphics
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static Texture2D _blankTexture;
        public static SpriteFont _spriteFont;

        private Box box, defaultBox;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 800;
            _graphics.ApplyChanges();

            Camera.Initialize(new Vector3(0, 0, -50), (float)Math.PI / 4);

            defaultBox = new Box(
                new Vector3(0, 0, 0),
                new Vector3(10, 10, 10),
                new Vector3(0.48f, 0.24f, 1.6f)
            );
            box = new Box(defaultBox);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _blankTexture = Content.Load<Texture2D>("blank");
            _spriteFont = Content.Load<SpriteFont>("font");
        }

        protected override void Update(GameTime gameTime)
        {
            KeyMouseReader.Update();

            if (KeyMouseReader.KeyPressed(Keys.Escape))
                Exit();

            if (KeyMouseReader.KeyPressed(Keys.R))
                box = new Box(defaultBox);

            if (KeyMouseReader.keyState.IsKeyDown(Keys.D))
                box.Update(Vector3.Zero, Vector3.Zero, new Vector3((float)Math.PI / 64, 0, 0));
            if (KeyMouseReader.keyState.IsKeyDown(Keys.A))
                box.Update(Vector3.Zero, Vector3.Zero, new Vector3(-(float)Math.PI / 64, 0, 0));
            if (KeyMouseReader.keyState.IsKeyDown(Keys.E))
                box.Update(Vector3.Zero, Vector3.Zero, new Vector3(0, (float)Math.PI / 64, 0));
            if (KeyMouseReader.keyState.IsKeyDown(Keys.Q))
                box.Update(Vector3.Zero, Vector3.Zero, new Vector3(0, -(float)Math.PI / 64, 0));
            if (KeyMouseReader.keyState.IsKeyDown(Keys.W))
                box.Update(Vector3.Zero, Vector3.Zero, new Vector3(0, 0, (float)Math.PI / 64));
            if (KeyMouseReader.keyState.IsKeyDown(Keys.S))
                box.Update(Vector3.Zero, Vector3.Zero, new Vector3(0, 0, -(float)Math.PI / 64));

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);

            _spriteBatch.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
            _spriteBatch.Begin();
            
            box.Draw(_spriteBatch, _graphics.GraphicsDevice.Viewport);

            _spriteBatch.DrawString(_spriteFont, 
                $"Pos: x={box.pos.X}, y={box.pos.Y}, z={box.pos.Z}\n" +
                $"Size: x={box.size.X}, y={box.size.Y}, z={box.size.Z}\n" +
                $"Rotation: x={box.rotation.X}, y={box.rotation.Y}, z={box.rotation.Z}",
            new Vector2(10, 10), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}