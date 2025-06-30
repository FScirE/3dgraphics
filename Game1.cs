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
        public static Texture2D _pixelTexture;
        public static SpriteFont _spriteFont;

        public const int _screenWidth = 800;
        public const int _screenHeight = 800;

        private Line2D[] crosshair;
        private Line2D line;

        private Box box, defaultBox;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        private static string GetMatrixString(Matrix matrix)
        {
            string representation = "\n";
            for (int i = 0; i < 16; i++)
            {
                representation += matrix[i].ToString();
                if ((i + 1) % 4 == 0)
                    representation += "\n";
                else 
                    representation += ", ";
            }
            return representation;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = _screenWidth;
            _graphics.PreferredBackBufferHeight = _screenHeight;
            _graphics.ApplyChanges();

            Camera.Initialize(new Vector3(0, 0, -50), (float)Math.PI / 4);

            defaultBox = new Box(
                new Vector3(0, 0, 0),
                new Vector3(10, 10, 10),
                Matrix.Identity
            );
            box = new Box(defaultBox);

            float halfWidth = _screenWidth / 2;
            float halfHeight = _screenHeight / 2;
            crosshair = [
                new Line2D(new Vector2(halfWidth - 5, halfHeight), new Vector2(halfWidth + 5, halfHeight), 2, Color.White),
                new Line2D(new Vector2(halfWidth, halfHeight - 5), new Vector2(halfWidth, halfHeight + 5), 2, Color.White)
            ];

            line = new Line2D(new Vector2(halfWidth, halfHeight), Vector2.Zero, 1, Color.HotPink);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _blankTexture = Content.Load<Texture2D>("blank");
            _pixelTexture = Content.Load<Texture2D>("pixel");
            _spriteFont = Content.Load<SpriteFont>("font");
        }

        protected override void Update(GameTime gameTime)
        {
            KeyMouseReader.Update();

            if (KeyMouseReader.KeyPressed(Keys.Escape))
                Exit();

            line.b = KeyMouseReader.mouseState.Position.ToVector2();

            if (KeyMouseReader.KeyPressed(Keys.R))
                box = new Box(defaultBox);

            if (KeyMouseReader.keyState.IsKeyDown(Keys.D))
                box.RotateHorizontal((float)Math.PI / 64);
            if (KeyMouseReader.keyState.IsKeyDown(Keys.A))
                box.RotateHorizontal(-(float)Math.PI / 64);
            if (KeyMouseReader.keyState.IsKeyDown(Keys.W))
                box.RotateVertical((float)Math.PI / 64);
            if (KeyMouseReader.keyState.IsKeyDown(Keys.S))
                box.RotateVertical(-(float)Math.PI / 64);
            if (KeyMouseReader.keyState.IsKeyDown(Keys.E))
                box.RotatePlane((float)Math.PI / 64);
            if (KeyMouseReader.keyState.IsKeyDown(Keys.Q))
                box.RotatePlane(-(float)Math.PI / 64);

            if (KeyMouseReader.mouseState.LeftButton == ButtonState.Pressed)
            {
                Vector2 startPos = KeyMouseReader.oldMouseState.Position.ToVector2();
                Vector2 mouseDelta = (KeyMouseReader.mouseState.Position - KeyMouseReader.oldMouseState.Position).ToVector2();

                float rotH = mouseDelta.X * (float)Math.PI / _screenWidth;
                float rotV = -mouseDelta.Y * (float)Math.PI / _screenHeight;

                Vector2 v1 = startPos - new Vector2(_screenWidth / 2, _screenHeight / 2);
                Vector2 v2 = v1 + mouseDelta;
                float dot = v1.X * v2.X + v1.Y * v2.Y;
                float det = v1.X * v2.Y - v1.Y * v2.X;
                float rotP = (float)Math.Atan2(det, dot);

                box.RotateHorizontal(rotH);
                box.RotateVertical(rotV);
                box.RotatePlane(rotP);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);

            // BOX
            _spriteBatch.Begin(SpriteSortMode.BackToFront);
            
            box.Draw(_spriteBatch, _graphics.GraphicsDevice.Viewport);

            _spriteBatch.End();

            // UI
            _spriteBatch.Begin();

            _spriteBatch.DrawString(_spriteFont,
                $"Pos: {box.pos.ToString()}\n" +
                $"Size: {box.size.ToString()}\n" +
                $"Rotation: {GetMatrixString(box.rotation)}",
                new Vector2(10, 10), Color.White
            );
            foreach (var l in crosshair)
                l.Draw(_spriteBatch);
            line.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}