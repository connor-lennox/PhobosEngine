using Microsoft.Xna.Framework;
using PhobosEngine.Serialization;

using PhobosEngine.Math;

namespace PhobosEngine
{
    public class BoxCollider : PolygonCollider
    {
        // Size represents the width and height of the collider.
        // Values are relative to the Collider offset, which is the centerpoint of the box.
        private Vector2 size;
        public Vector2 Size {
            get => size;
            set {
                size = value;
                UpdateCollider();
            }
        }

        public float Width
        {
            get => size.X;
            set {
                size.X = value;
                UpdateCollider();
            }
        }

        public float Height
        {
            get => size.Y;
            set {
                size.Y = value;
                UpdateCollider();
            }
        }

        public BoxCollider() : base()
        {
            size = new Vector2(1, 1);
        }

        protected override void RecalculateBounds()
        {
            Vector2 topLeft = Offset - (size / 2);
            Vector2 bottomRight = Offset + (size / 2);
            points = new Vector2[] {topLeft, new Vector2(topLeft.X, bottomRight.Y), bottomRight, new Vector2(bottomRight.X, topLeft.Y)};
            base.RecalculateBounds();
        }

        protected override void CalculateEdgeNormals()
        {
            // Boxes only ever have 2 normals
            if(edgeNormals == null || edgeNormals.Length != 2)
            {
                edgeNormals = new Vector2[2];
            }

            edgeNormals[0] = Vector2.Normalize(PBMath.Perpendicular(ref points[0], ref points[1]));
            edgeNormals[1] = Vector2.Normalize(PBMath.Perpendicular(ref points[1], ref points[2]));
        }

        public override SerializedInfo Serialize()
        {
            SerializedInfo info = base.Serialize();
            info.Write("size", Size);
            return info;
        }

        public override void Deserialize(SerializedInfo info)
        {
            base.Deserialize(info);
            Size = info.ReadVector2("size");
        }
    }
}