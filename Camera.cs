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
            projection = Matrix.CreatePerspectiveFieldOfView(fov, (float)Game1._screenWidth / Game1._screenHeight, 0.1f, 100);
            world = Matrix.Identity;
        }

        public static Vector3 Project(Vector3 vector, Viewport viewport)
        {
            return viewport.Project(vector, projection, view, world);
        }
    }
}
