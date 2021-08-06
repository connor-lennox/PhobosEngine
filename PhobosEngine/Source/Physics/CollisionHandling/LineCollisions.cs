using System;
using Microsoft.Xna.Framework;

namespace PhobosEngine.Collisions
{
    public static partial class CollisionResolvers 
    {
        public static bool LineToLine(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, out Vector2 intersectPoint)
        {
            intersectPoint = Vector2.Zero;

            Vector2 r = a2 - a1;
            Vector2 s = b2 - b1;
            float rDotSPerp = r.X * s.Y - r.Y * s.X;

            // dot product of zero means the lines are parallel
            if(rDotSPerp == 0)
            {
                return false;
            }

            Vector2 c = b1 - a1;

            // Check if intersection point falls outside of the second line
            float t = (c.X * s.Y - c.Y * s.X) / rDotSPerp;
            if (t < 0 || t > 1)
            {
                return false;
            }

            // Check if intersection point is outside of the first line
            float u = (c.X * r.Y - c.Y * r.X) / rDotSPerp;
            if (u < 0 || u > 1)
            {
                return false;
            }

            // Actual intersection point via interpolation on the first line
            intersectPoint = a1 + t * r;
            return true;
        }

        public static bool LineToPoly(Vector2 start, Vector2 end, PolygonCollider polygon, out RaycastHit result)
        {
            result = new RaycastHit();

            Vector2 normal = Vector2.Zero;
            Vector2 point = Vector2.Zero;
            float sqDist = float.PositiveInfinity;

            bool intersected = false;

            Vector2 tempIntersect;

            for(int i = 0, j = polygon.EffectivePoints.Length-1; i < polygon.EffectivePoints.Length; j = i++)
            {
                Vector2 p1 = polygon.EffectivePoints[i];
                Vector2 p2 = polygon.EffectivePoints[j];

                if(LineToLine(start, end, p1, p2, out tempIntersect))
                {
                    intersected = true;

                    float tempSqDist = Vector2.DistanceSquared(start, tempIntersect);
                    if(tempSqDist < sqDist)
                    {
                        Vector2 edge = p2 - p1;
                        normal = new Vector2(edge.Y, -edge.X);
                        point = tempIntersect;
                        sqDist = tempSqDist;
                    }
                }
            }

            if(intersected)
            {
                result.normal = Vector2.Normalize(normal);
                result.distance = MathF.Sqrt(sqDist);
                result.point = point;
                result.collider = polygon;
                return true;
            }

            return false;
        }

        public static bool LineToCircle(Vector2 start, Vector2 end, CircleCollider circle, out RaycastHit result)
        {
            result = new RaycastHit();

            float lineLength = Vector2.Distance(start, end);
            Vector2 d = (end - start) / lineLength;
            Vector2 m = start - circle.WorldPos;
            float b = Vector2.Dot(m, d);
            float c = Vector2.Dot(m, m) - circle.EffectiveRadius * circle.EffectiveRadius;

            // if line is moving away from the circle, and doesn't start inside, no collision
            if(c > 0f && b > 0f)
            {
                return false;
            }

            float discr = b * b - c;

            // Negative discriminant means no collision
            if (discr < 0)
            {
                return false;
            }

            // Calculate interpolated point of intersection
            float t = -b - MathF.Sqrt(discr);

            // Clamp t to be positive
            t = MathF.Max(0, t);

            result.point = start + t * d;
            Vector2.Distance(ref start, ref result.point, out result.distance);
            result.normal = Vector2.Normalize(result.point - circle.WorldPos);
            result.collider = circle;

            return true;
        }

        public static bool LineToAABB(Vector2 start, Vector2 end, AABBCollider collider, out RaycastHit result)
        {
             result = new RaycastHit();

            Vector2 normal = Vector2.Zero;
            Vector2 point = Vector2.Zero;
            float sqDist = float.PositiveInfinity;

            bool intersected = false;

            Vector2 tempIntersect;

            for(int i = 0, j = collider.Points.Length-1; i < collider.Points.Length; j = i++)
            {
                Vector2 p1 = collider.Points[j];
                Vector2 p2 = collider.Points[i];

                if(LineToLine(start, end, p1, p2, out tempIntersect))
                {
                    intersected = true;

                    float tempSqDist = Vector2.DistanceSquared(start, tempIntersect);
                    if(tempSqDist < sqDist)
                    {
                        Vector2 edge = p2 - p1;
                        normal = new Vector2(edge.Y, -edge.X);
                        point = tempIntersect;
                        sqDist = tempSqDist;
                    }
                }
            }

            if(intersected)
            {
                result.normal = Vector2.Normalize(normal);
                result.distance = MathF.Sqrt(sqDist);
                result.point = point;
                result.collider = collider;
                return true;
            }

            return false;
        }
    }
}