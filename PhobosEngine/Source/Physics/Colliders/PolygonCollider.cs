using System;
using Microsoft.Xna.Framework;
using PhobosEngine.Math;

namespace PhobosEngine
{
    public class PolygonCollider : Collider
    {
        protected Vector2[] points = new Vector2[0];
        public Vector2[] Points {
            get => points;
            set {
                points = value;
                UpdateCollider();
            }
        }

        private Vector2[] effectivePoints;
        public Vector2[] EffectivePoints { get=>effectivePoints; }

        protected Vector2[] edgeNormals;
        public Vector2[] EdgeNormals {
            get {
                if(edgeNormalsDirty)
                {
                    CalculateEdgeNormals();
                    edgeNormalsDirty = false;
                }
                return edgeNormals;
            }
        }

        private bool edgeNormalsDirty = true;

        public PolygonCollider() : base()
        {
            
        }

        private void CalculateEffectivePoints()
        {
            edgeNormalsDirty = true;
            effectivePoints = new Vector2[points.Length];
            for(int i = 0; i < points.Length; i++)
            {
                effectivePoints[i] = PBMath.Transform(points[i], Transform.WorldTransform);
            }
        }

        protected override void RecalculateBounds()
        {
            CalculateEffectivePoints();
            if(points.Length < 3)
            {
                Bounds = RectangleF.Empty;
            } else {
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

        protected virtual void CalculateEdgeNormals()
        {
            int numEdges = EffectivePoints.Length;
            if(edgeNormals == null || edgeNormals.Length != numEdges)
            {
                edgeNormals = new Vector2[numEdges];
            }

            for(int i = 0, j = EffectivePoints.Length-1; i < EffectivePoints.Length; j = i++)
            {
                Vector2 perp = PBMath.Perpendicular(ref EffectivePoints[i], ref EffectivePoints[j]);
                edgeNormals[i] = Vector2.Normalize(perp);
            }
        }
    }
}