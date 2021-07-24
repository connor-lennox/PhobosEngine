using System;
using Microsoft.Xna.Framework;

namespace PhobosEngine
{
    public static class ShapeUtil
    {
        public static Vector2 ClosestPointOnLine(Vector2 p1, Vector2 p2, Vector2 target)
        {
            // Find orthogonal projection of target point onto line between p1 and p2
            Vector2 dif = p2 - p1;
            Vector2 w = target - p1;
            float interp = Vector2.Dot(w, dif) / Vector2.Dot(dif, dif);
            // Clamp between 0 and 1
            interp = MathF.Max(MathF.Min(interp, 1), 0);
            // Project back into original space
            return p1 + dif * interp;
        }

        public static Vector2 ClosestPointOnPoly(PolygonCollider collider, Vector2 target, out float squareDistance)
        {
            Vector2 closestPoint = Vector2.Zero;
            float minSqDist = float.MaxValue;
            for(int i = 0, j = collider.Points.Length-1; i < collider.Points.Length; j = i++)
            {
                Vector2 p1 = collider.Points[i];
                Vector2 p2 = collider.Points[j];

                Vector2 tempClosest = ClosestPointOnLine(p1, p2, target);
                float sqDist = Vector2.DistanceSquared(tempClosest, target);
                if(sqDist < minSqDist)
                {
                    minSqDist = sqDist;
                    closestPoint = tempClosest;
                }
            }

            squareDistance = minSqDist;
            return closestPoint;
        }

        public static bool PointInPoly(PolygonCollider polygon, Vector2 point)
        {
            bool inside = false;
            for(int i = 0, j = polygon.EffectivePoints.Length-1; i < polygon.EffectivePoints.Length; j = i++)
            {
                if (((polygon.EffectivePoints[i].Y > point.Y) != (polygon.EffectivePoints[j].Y > point.Y)) &&
				    (point.X < (polygon.EffectivePoints[j].X - polygon.EffectivePoints[i].X) * (point.Y - polygon.EffectivePoints[i].Y) / 
                    (polygon.EffectivePoints[j].Y - polygon.EffectivePoints[i].Y) + polygon.EffectivePoints[i].X))
				{
					inside = !inside;
				}
            }
            return inside;
        }
    }
}