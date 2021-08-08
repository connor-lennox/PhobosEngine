using System;
using Microsoft.Xna.Framework;

namespace PhobosEngine.Math
{
    public static class PBMath
    {
        public static float Rad2Deg(float rad)
        {
            return rad * 180f / MathF.PI;
        }

        public static float Deg2Rad(float deg)
        {
            return deg * MathF.PI / 180f;
        }

        public static float Approach(float start, float end, float shift)
        {
            if(start < end)
            {
                return MathF.Min(start + shift, end);
            } else {
                return MathF.Max(start - shift, end);
            }
        }

        public static Vector2 Transform(Vector2 position, Matrix2D matrix)
        {
            Vector2 result;
            Transform(ref position, ref matrix, out result);
            return result;
        }

        public static Vector2 Perpendicular(ref Vector2 v1, ref Vector2 v2)
        {
            // Flip X and Y and negate new X
            return new Vector2(-1f * (v2.Y - v1.Y), v2.X - v1.X);
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