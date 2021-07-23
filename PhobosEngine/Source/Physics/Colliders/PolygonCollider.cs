using System;
using Microsoft.Xna.Framework;
using PhobosEngine.Math;

namespace PhobosEngine
{
    public class PolygonCollider : Collider
    {
        protected Vector2[] points;
        public Vector2[] Points {
            get => points;
            set {
                points = value;
                RecalculateBounds();
            }
        }

        private Vector2[] effectivePoints;

        private void CalculateEffectivePoints()
        {
            effectivePoints = new Vector2[points.Length];
            for(int i = 0; i < points.Length; i++)
            {
                effectivePoints[i] = PBMath.Transform(points[i], Transform.WorldTransform);
            }
        }

        protected override void RecalculateBounds()
        {
            CalculateEffectivePoints();
            if(points.Length < 1)
            {
                Bounds = RectangleF.Empty;
            }

            float minX = effectivePoints[0].X;
            float maxX = effectivePoints[0].X;
            float minY = effectivePoints[0].Y;
            float maxY = effectivePoints[0].Y;

            for(int i = 1; i < effectivePoints.Length; i++)
            {
                float x = effectivePoints[i].X;
                float y = effectivePoints[i].Y;
                minX = MathF.Min(minX, x);
                maxX = MathF.Max(maxX, x);
                minY = MathF.Min(minY, y);
                maxY = MathF.Max(maxY, y);
            }

            Bounds = new RectangleF(minX, minY, (maxX - minX), (maxY - minY));
        }
    }
}