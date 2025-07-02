using System;
using Microsoft.Xna.Framework;

namespace _3dgraphics
{ 
    public static class Utility
    {
        public enum RenderModes
        {
            Perspective = 1,
            Cavalier = 2,
            Isometric = 3
        }
        public const int RenderModesAmt = 3;

        public static void CallVoidMethod(Action<float> Method, float amt)
        {
            Method(amt);
        }

        public static string GetMatrixString(Matrix matrix)
        {
            string representation = "\n";
            for (int i = 0; i < 16; i++)
            {
                representation += matrix[i].ToString();
                if ((i + 1) % 4 == 0 && i + 1 < 16)
                    representation += "\n";
                else
                    representation += ", ";
            }
            return representation;
        }
    }
}