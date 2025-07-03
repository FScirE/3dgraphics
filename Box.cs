using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Serialization;

namespace _3dgraphics
{
    internal class Box
    {
        public Vector3 pos { get; private set; }
        public Vector3 size { get; private set; }
        public Matrix rotationGlobal { get; private set; }
        public Matrix rotationLocal { get; private set; }

        private Vector3[] corners;
        private Line[] edges;

        public Box(Vector3 pos, Vector3 size, Matrix rotationGlobal, Matrix rotationLocal)
        {       
            corners = new Vector3[8];
            edges = new Line[12];
            Update(pos, size, rotationGlobal, rotationLocal);
        }

        public Box(Box box)
        {
            corners = new Vector3[8];
            edges = new Line[12];
            Update(box.pos, box.size, box.rotationGlobal, box.rotationLocal);
        }

        private void CreateEdges()
        {
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

        public void Update(Vector3 pos, Vector3 size, Matrix rotationGlobal, Matrix rotationLocal)
        {
            this.pos = pos;
            this.size = size;
            this.rotationGlobal = rotationGlobal;
            this.rotationLocal = rotationLocal;

            Matrix rotation = this.rotationLocal * this.rotationGlobal;
            Matrix scale = Matrix.CreateScale(this.size.X / 2, this.size.Y / 2, this.size.Z / 2);
            Matrix translate = Matrix.CreateTranslation(this.pos.X, this.pos.Y, this.pos.Z);
            Matrix transform = rotation * scale * translate;

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

            CreateEdges();
        }

        public void RotateHorizontal(float amt)
        {
            rotationGlobal *= Matrix.CreateRotationY(amt);
            Update(pos, size, rotationGlobal, rotationLocal);
        }
        public void RotateVertical(float amt)
        {
            rotationGlobal *= Matrix.CreateRotationX(amt);
            Update(pos, size, rotationGlobal, rotationLocal);
        }
        public void RotatePlane(float amt)
        {
            rotationGlobal *= Matrix.CreateRotationZ(amt);
            Update(pos, size, rotationGlobal, rotationLocal);
        }

        public void RotateX(float amt)
        {
            rotationLocal = Matrix.CreateRotationX(amt) * rotationLocal;
            Update(pos, size, rotationGlobal, rotationLocal);
        }
        public void RotateY(float amt)
        {
            rotationLocal = Matrix.CreateRotationY(amt) * rotationLocal;
            Update(pos, size, rotationGlobal, rotationLocal);
        }
        public void RotateZ(float amt)
        {
            rotationLocal = Matrix.CreateRotationZ(amt) * rotationLocal;
            Update(pos, size, rotationGlobal, rotationLocal);
        }

        public void Draw(SpriteBatch sb, Viewport vp, Utility.RenderModes rm)
        {               
            foreach (var e in edges)
                e.Draw(sb, vp, rm);
        }
    }
}
