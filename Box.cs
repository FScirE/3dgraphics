using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3dgraphics
{
    internal class Box
    {
        public Vector3 pos { get; private set; }
        public Vector3 size { get; private set; }
        public Vector2 rotation { get; private set; }

        private Vector3[] corners;
        private Line[] edges;

        public Box(Vector3 pos, Vector3 size, Vector2 rotation)
        {       
            corners = new Vector3[4];
            edges = new Line[4];
            Update(pos, size, rotation);
        }

        public void Update(Vector3 pos, Vector3 size, Vector2 rotation)
        {
            this.pos = pos;
            this.size = size;
            this.rotation = rotation;

            float r = (float)Math.Sqrt(size.X * size.X + size.Y * size.Y) / 2;
            float phi = rotation.X;
            float theta = 0;

            float theta_offset = (float)Math.Atan(size.Y / size.X);

            corners[0] = new Vector3(
                pos.X + r * (float)Math.Cos(phi - theta_offset), 
                pos.Y + r * (float)Math.Sin(phi - theta_offset), 
                0
            );
            corners[1] = new Vector3(
                pos.X + r * (float)Math.Cos(phi + theta_offset),
                pos.Y + r * (float)Math.Sin(phi + theta_offset),
                0
            );
            corners[2] = new Vector3(
                pos.X + r * (float)Math.Cos(phi + Math.PI - theta_offset),
                pos.Y + r * (float)Math.Sin(phi + Math.PI - theta_offset),
                0
            );
            corners[3] = new Vector3(
                pos.X + r * (float)Math.Cos(phi + Math.PI + theta_offset),
                pos.Y + r * (float)Math.Sin(phi + Math.PI + theta_offset),
                0
            );

            edges[0] = new Line(corners[0], corners[1], Color.White);
            edges[1] = new Line(corners[1], corners[2], Color.White);
            edges[2] = new Line(corners[2], corners[3], Color.White);
            edges[3] = new Line(corners[3], corners[0], Color.White);
        }

        public void Draw(SpriteBatch sb)
        {               
            foreach (var e in edges)
            {
                e.Draw(sb);
            }
        }
    }
}
