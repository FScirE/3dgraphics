using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3dgraphics
{
    internal static class Camera
    {
        private static Matrix view, projection, world;

        public static void Initialize(Vector3 pos, float fov)
        {
            view = Matrix.CreateLookAt(pos, Vector3.Zero, Vector3.Up);
            projection = Matrix.CreatePerspectiveFieldOfView(fov, 1, 0.1f, 100);
            world = Matrix.Identity;
        }

        public static Vector2 Project(Vector3 vector, Viewport viewport)
        {
            Vector3 projected = viewport.Project(vector, projection, view, world);
            return new Vector2(projected.X, projected.Y);
        }
    }
}
