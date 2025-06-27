using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static System.Windows.Forms.Design.AxImporter;

namespace _3dgraphics
{
    internal class Box
    {
        public Vector3 pos { get; private set; }
        public Vector3 size { get; private set; }
        public Vector3 rotation { get; private set; }

        private Vector3[] corners;
        private Line[] edges;

        public Box(Vector3 pos, Vector3 size, Vector3 rotation)
        {       
            corners = new Vector3[8];
            edges = new Line[12];
            Update(pos, size, rotation, true);
        }

        public Box(Box box)
        {
            corners = new Vector3[8];
            edges = new Line[12];
            Update(box.pos, box.size, box.rotation, true);
        }

        public void Update(Vector3 pos, Vector3 size, Vector3 rotation, bool set = false)
        {
            if (!set)
            {
                this.pos += pos;
                this.size += size;
                this.rotation += rotation;
            }
            else
            {
                this.pos = pos;
                this.size = size;
                this.rotation = rotation;
            }

            if (Math.Abs(this.rotation.X) >= Math.PI * 2)
                this.rotation = new Vector3(0, this.rotation.Y, this.rotation.Z);
            if (Math.Abs(this.rotation.Y) >= Math.PI * 2)
                this.rotation = new Vector3(this.rotation.X, 0, this.rotation.Z);
            if (Math.Abs(this.rotation.Z) >= Math.PI * 2)
                this.rotation = new Vector3(this.rotation.X, this.rotation.Y, 0);

            Matrix rotateX = Matrix.CreateRotationX(this.rotation.X);
            Matrix rotateY = Matrix.CreateRotationY(this.rotation.Y);
            Matrix rotateZ = Matrix.CreateRotationZ(this.rotation.Z);
            Matrix scale = Matrix.CreateScale(this.size.X / 2, this.size.Y / 2, this.size.Z / 2);
            Matrix translate = Matrix.CreateTranslation(this.pos.X, this.pos.Y, this.pos.Z);
            Matrix transform = rotateX * rotateY * rotateZ * scale * translate;

            // FRONT
            corners[0] = Vector3.Transform(new Vector3(-1, -1, 1), transform);
            corners[1] = Vector3.Transform(new Vector3(1, -1, 1), transform);
            corners[2] = Vector3.Transform(new Vector3(1, 1, 1), transform);
            corners[3] = Vector3.Transform(new Vector3(-1, 1, 1), transform);
            // BACK
            corners[4] = Vector3.Transform(new Vector3(-1, -1, -1), transform);
            corners[5] = Vector3.Transform(new Vector3(1, -1, -1), transform);
            corners[6] = Vector3.Transform(new Vector3(1, 1, -1), transform);
            corners[7] = Vector3.Transform(new Vector3(-1, 1, -1), transform);

            // FRONT
            edges[0] = new Line(corners[0], corners[1], Color.MediumPurple);
            edges[1] = new Line(corners[1], corners[2], Color.MediumPurple);
            edges[2] = new Line(corners[2], corners[3], Color.MediumPurple);
            edges[3] = new Line(corners[3], corners[0], Color.MediumPurple);
            // BACK
            edges[4] = new Line(corners[4], corners[5], Color.LightGreen);
            edges[5] = new Line(corners[5], corners[6], Color.LightGreen);
            edges[6] = new Line(corners[6], corners[7], Color.LightGreen);
            edges[7] = new Line(corners[7], corners[4], Color.LightGreen);
            // CONNECTING
            edges[8] = new Line(corners[0], corners[4], Color.Red);
            edges[9] = new Line(corners[1], corners[5], Color.Red);
            edges[10] = new Line(corners[2], corners[6], Color.Red);
            edges[11] = new Line(corners[3], corners[7], Color.Red);
        }

        public void Draw(SpriteBatch sb, Viewport vp)
        {               
            foreach (var e in edges)
                e.Draw(sb, vp);
        }
    }
}
