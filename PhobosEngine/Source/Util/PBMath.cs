using System;

namespace PhobosEngine
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
    }
}