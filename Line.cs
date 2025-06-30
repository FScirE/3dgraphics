using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace _3dgraphics
{
    internal class Line
    {
        private Vector3 a, b;
        private Color color;

        public Line(Vector3 a, Vector3 b, Color color)
        {
            this.a = a;
            this.b = b;
            this.color = color;
        }

        public float GetZ()
        {
            return (a.Z + b.Z) / 2;
        }

        public void Draw(SpriteBatch sb, Viewport vp, bool isometric)
        {
            Vector3 mid, diff;
            float length, angle, zOrder;
            if (!isometric)
            {
                Vector3 a2D = Camera.Project(a, vp);
                Vector3 b2D = Camera.Project(b, vp);

                diff = b2D - a2D;
                mid = (a2D + b2D) / 2;

                zOrder = (a2D.Z + b2D.Z) / 2;
            }
            else
            {
                Vector2 aXY = new Vector2(-a.X + a.Z / (float)Math.Sqrt(2), -a.Y - a.Z / (float)Math.Sqrt(2));
                Vector2 bXY = new Vector2(-b.X + b.Z / (float)Math.Sqrt(2), -b.Y - b.Z / (float)Math.Sqrt(2));

                // OFFSET AND SCALE COMPENSATION
                aXY *= Game1.isoScale;
                bXY *= Game1.isoScale;
                aXY += new Vector2(Game1._screenWidth / 2, Game1._screenHeight / 2);
                bXY += new Vector2(Game1._screenWidth / 2, Game1._screenHeight / 2);

                Vector2 diff2D = bXY - aXY;
                Vector2 mid2D = (aXY + bXY) / 2;
                diff = new Vector3(diff2D.X, diff2D.Y, 0);
                mid = new Vector3(mid2D.X, mid2D.Y, 0);

                float halfScreen = Game1._screenWidth < Game1._screenHeight ? Game1._screenWidth / 2 : Game1._screenHeight / 2;
                float minZ = -(halfScreen / Game1.isoScale) * (float)Math.Sqrt(2);
                float maxZ = (halfScreen / Game1.isoScale) * (float)Math.Sqrt(2);

                zOrder = (GetZ() - minZ) / (maxZ - minZ);
            }
            length = (float)Math.Sqrt(diff.X * diff.X + diff.Y * diff.Y);
            angle = (float)Math.Atan2(diff.Y, diff.X);
            sb.Draw(Game1._pixelTexture, new Vector2(mid.X, mid.Y), null, color, angle, new Vector2(0.5f, 0.5f), new Vector2(length, 2), SpriteEffects.None, zOrder);
        }
    }
}
