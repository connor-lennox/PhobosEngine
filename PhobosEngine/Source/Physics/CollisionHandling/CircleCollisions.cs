using System;
using Microsoft.Xna.Framework;

namespace PhobosEngine
{
    public static class CircleCollisions
    {
        public static bool CircleToCircle(CircleCollider c1, CircleCollider c2, out CollisionResult result)
        {
            result = new CollisionResult();

            float distSq = Vector2.DistanceSquared(c1.WorldPos, c2.WorldPos);
            float radiiSum = (c1.EffectiveRadius + c2.EffectiveRadius);
            bool collided = distSq < radiiSum * radiiSum;

            if(collided)
            {
                result.normal = Vector2.Normalize(c1.WorldPos - c2.WorldPos);
                result.depth = radiiSum - MathF.Sqrt(distSq);
                result.point = c2.WorldPos + result.normal * c2.EffectiveRadius;

                return true;
            }

            return false;
        }
    }
}