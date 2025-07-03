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
        public const float _perspectiveScale = _screenHeight / 36;

        private static Line2D[] crosshair, axles;
        private static Line2D line;

        private static Box box, defaultBox;
        private static float rotationSpeed;
        private static bool globalRotation;
       
        private static Utility.RenderModes renderMode;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }             

        private static void SetAxles()
        {
            float halfHeight = _screenHeight / 2;
            float halfWidth = _screenWidth / 2;

            if (renderMode == Utility.RenderModes.Perspective)
            {
                axles[0] = new Line2D(Vector2.Zero, Vector2.Zero, 0, Color.White);
                axles[1] = new Line2D(Vector2.Zero, Vector2.Zero, 0, Color.White);
                axles[2] = new Line2D(Vector2.Zero, Vector2.Zero, 0, Color.White);
            }
            else if (renderMode == Utility.RenderModes.Cavalier)
            {
                // X
                axles[0] = new Line2D(new Vector2(0, halfHeight), new Vector2(_screenWidth, halfHeight), 1, Color.SlateGray);
                // Y
                axles[1] = new Line2D(new Vector2(halfWidth, 0), new Vector2(halfWidth, _screenHeight), 1, Color.SlateGray);
                // Z
                axles[2] = new Line2D(new Vector2(halfWidth + halfHeight, 0), new Vector2(halfWidth - halfHeight, _screenHeight), 1, Color.SlateGray);
            }
            else
            {
                // X
                axles[0] = new Line2D(new Vector2(0, halfHeight), new Vector2(_screenWidth, halfHeight), 1, Color.SlateGray);
                // Y
                axles[1] = new Line2D(new Vector2(halfWidth, 0), new Vector2(halfWidth, _screenHeight), 1, Color.SlateGray);
                // Z
                axles[2] = new Line2D(new Vector2(halfWidth, halfHeight), new Vector2(halfWidth, halfHeight), 1, Color.SlateGray);
            }
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
                Matrix.Identity,
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

            renderMode = Utility.RenderModes.Perspective;

            axles = new Line2D[3];
            SetAxles();

            rotationSpeed = (float)Math.PI / 64;
            globalRotation = true;

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

            if (KeyMouseReader.KeyPressed(Keys.Enter))
                box = new Box(defaultBox);
            if (KeyMouseReader.KeyPressed(Keys.R))
                globalRotation = !globalRotation;
            if (KeyMouseReader.KeyPressed(Keys.I))
            {
                renderMode = renderMode == (Utility.RenderModes)Utility.RenderModesAmt ? (Utility.RenderModes)1 : ++renderMode;
                SetAxles();
            }

            if (KeyMouseReader.keyState.IsKeyDown(Keys.D))
                Utility.CallVoidMethod(globalRotation ? box.RotateHorizontal : box.RotateY, rotationSpeed);
            if (KeyMouseReader.keyState.IsKeyDown(Keys.A))
                Utility.CallVoidMethod(globalRotation ? box.RotateHorizontal : box.RotateY, -rotationSpeed);
            if (KeyMouseReader.keyState.IsKeyDown(Keys.W))
                Utility.CallVoidMethod(globalRotation ? box.RotateVertical : box.RotateX, rotationSpeed);
            if (KeyMouseReader.keyState.IsKeyDown(Keys.S))
                Utility.CallVoidMethod(globalRotation ? box.RotateVertical : box.RotateX, -rotationSpeed);
            if (KeyMouseReader.keyState.IsKeyDown(Keys.E))
                Utility.CallVoidMethod(globalRotation ? box.RotatePlane : box.RotateZ, rotationSpeed);
            if (KeyMouseReader.keyState.IsKeyDown(Keys.Q))
                Utility.CallVoidMethod(globalRotation ? box.RotatePlane : box.RotateZ, -rotationSpeed);

            Vector2 startPos = KeyMouseReader.oldMouseState.Position.ToVector2();
            Vector2 mouseDelta = (KeyMouseReader.mouseState.Position - KeyMouseReader.oldMouseState.Position).ToVector2();

            if (KeyMouseReader.mouseState.LeftButton == ButtonState.Pressed)
            {
                float rotHY = mouseDelta.X * (float)Math.PI / _screenWidth;
                float rotVX = -mouseDelta.Y * (float)Math.PI / _screenHeight;

                Utility.CallVoidMethod(globalRotation ? box.RotateHorizontal : box.RotateY, rotHY);
                Utility.CallVoidMethod(globalRotation ? box.RotateVertical : box.RotateX, rotVX);               
            }
            if (KeyMouseReader.mouseState.RightButton == ButtonState.Pressed)
            {
                Vector2 v1 = startPos - new Vector2(_screenWidth / 2, _screenHeight / 2);
                Vector2 v2 = v1 + mouseDelta;
                float dot = v1.X * v2.X + v1.Y * v2.Y;
                float det = v1.X * v2.Y - v1.Y * v2.X;
                float rotPZ = (float)Math.Atan2(det, dot);

                Utility.CallVoidMethod(globalRotation ? box.RotatePlane : box.RotateZ, rotPZ);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);

            // GRID
            _spriteBatch.Begin();

            foreach (var l in axles)
                l.Draw(_spriteBatch);

            _spriteBatch.End();

            // BOX
            _spriteBatch.Begin(SpriteSortMode.BackToFront);
            
            box.Draw(_spriteBatch, _graphics.GraphicsDevice.Viewport, renderMode);

            _spriteBatch.End();

            // UI
            _spriteBatch.Begin();

            _spriteBatch.DrawString(_spriteFont,
                $"Pos: {box.pos.ToString()}\n" +
                $"Size: {box.size.ToString()}\n" +
                $"Rotation (Local): {Utility.GetMatrixString(box.rotationLocal)}\n" +
                $"Rotation (Global): {Utility.GetMatrixString(box.rotationGlobal)}\n" +
                $"\n" +
                $"Render Mode: {renderMode.ToString()}\n" +
                $"Rotation Mode: {(globalRotation ? "Global": "Local")}",
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