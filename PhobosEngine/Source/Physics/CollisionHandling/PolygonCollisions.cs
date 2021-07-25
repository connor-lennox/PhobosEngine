using System;
using Microsoft.Xna.Framework;

namespace PhobosEngine.Collisions
{
    public class PolygonCollisions
    {
        public static bool PolyToPoly(PolygonCollider p1, PolygonCollider p2, out CollisionResult result)
        {
            // Separating axis theorem: if an axis can be used to separate two convex polygons, they are not colliding.
            result = new CollisionResult();
            // Assume they are intersecting and prove otherwise
            bool intersecting = true;

            Vector2[] p1Normals = p1.EdgeNormals;
            Vector2[] p2Normals = p2.EdgeNormals;

            Vector2 curAxis;
            float minInterval = float.PositiveInfinity;

            for(int edge = 0; edge < p1Normals.Length + p2Normals.Length; edge++)
            {
                curAxis = (edge < p1Normals.Length) ? p1Normals[edge] : p2Normals[edge - p1Normals.Length];

                float minA = 0;
                float maxA = 0;
                float minB = 0; 
                float maxB = 0;
                PolygonProjectionInterval(curAxis, p1, ref minA, ref maxA);
                PolygonProjectionInterval(curAxis, p2, ref minB, ref maxB);

                float intervalDist = IntervalDistance(minA, maxA, minB, maxB);
                if(intervalDist > 0)
                {
                    intersecting = false;
                }

                if(!intersecting)
                {
                    return false;
                }

                intervalDist = MathF.Abs(intervalDist);
                if(intervalDist < minInterval)
                {
                    minInterval = intervalDist;
                    result.normal = curAxis;
                    result.depth = minInterval;
                }
            }

            return true;
        }

        private static float IntervalDistance(float minA, float maxA, float minB, float maxB)
        {
            return (minA < minB) ? (minB-maxA) : (minA-maxB);
        }

        private static void PolygonProjectionInterval(Vector2 axis, PolygonCollider poly, ref float min, ref float max)
        {
            float dot;
            Vector2.Dot(ref poly.EffectivePoints[0], ref axis, out dot);
            min = max = dot;

            for(int i = 1; i < poly.EffectivePoints.Length; i++)
            {
                Vector2.Dot(ref poly.EffectivePoints[i], ref axis, out dot);
                if(dot < min)
                {
                    min = dot;
                } else if(dot > max)
                {
                    max = dot;
                }
            }
        }
    }
}