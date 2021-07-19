using System;
using Microsoft.Xna.Framework;

namespace PhobosEngine.Math
{
    static class PBMath
    {
        public static float Rad2Deg(float rad)
        {
            return rad * 180f / MathF.PI;
        }

        public static float Deg2Rad(float deg)
        {
            return deg * MathF.PI / 180f;
        }

        public static void Transform(ref Vector2 position, ref Matrix2D matrix, out Vector2 result)
        {
            float x = (matrix.m11 * position.X) + (matrix.m12 * position.Y) + matrix.m13;
            float y = (matrix.m21 * position.X) + (matrix.m22 * position.Y) + matrix.m23;
            result.X = x;
            result.Y = y;
        }
    }
}