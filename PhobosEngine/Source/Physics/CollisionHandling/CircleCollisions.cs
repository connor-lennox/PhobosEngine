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

        public static bool CircleToPoly(CircleCollider c1, PolygonCollider p2, out CollisionResult result)
        {
            result = new CollisionResult();

            Vector2 closestPoint = ShapeUtil.ClosestPointOnPoly(p2, c1.WorldPos, out float squareDistance);
            if(ShapeUtil.PointInPoly(p2, c1.WorldPos))
            {
                result.normal = Vector2.Normalize(c1.WorldPos - closestPoint);
                result.depth = MathF.Sqrt(squareDistance) - c1.EffectiveRadius;
                result.point = closestPoint;

                return true;
            }

            bool collided = squareDistance < c1.EffectiveRadius;
            if(collided)
            {
                result.normal = Vector2.Normalize(c1.WorldPos - closestPoint);
                result.depth = c1.EffectiveRadius - MathF.Sqrt(squareDistance);
                result.point = closestPoint;

                return true;
            }

            return false;
        }
    }
}