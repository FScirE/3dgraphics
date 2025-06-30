using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        public void Draw(SpriteBatch sb, Viewport vp)
        {
            Vector3 a2D = Camera.Project(a, vp);
            Vector3 b2D = Camera.Project(b, vp);

            Vector3 diff = b2D - a2D;
            Vector3 mid = (a2D + b2D) / 2;
            float length = (float)Math.Sqrt(diff.X * diff.X + diff.Y * diff.Y);
            float angle = (float)Math.Atan2(diff.Y, diff.X);

            float zOrder = (a2D.Z + b2D.Z) / 2;

            sb.Draw(Game1._pixelTexture, new Vector2(mid.X, mid.Y), null, color, angle, new Vector2(0.5f, 0.5f), new Vector2(length, 2), SpriteEffects.None, zOrder);
        }
    }
}
