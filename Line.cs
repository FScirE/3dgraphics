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

        public void Draw(SpriteBatch sb)
        {
            Vector3 diff = b - a;
            float length = (float)Math.Sqrt(diff.X * diff.X + diff.Y * diff.Y);
            float angle = (float)Math.Atan2(diff.Y, diff.X);

            sb.Draw(Game1._blankTexture, new Rectangle((int)a.X, (int)a.Y, (int)length, 2), null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }
    }
}
