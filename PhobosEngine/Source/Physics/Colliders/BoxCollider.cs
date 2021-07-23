using Microsoft.Xna.Framework;
using PhobosEngine.Serialization;

namespace PhobosEngine.Physics
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
                RecalculateBounds();
            }
        }

        public float Width
        {
            get => size.X;
            set {
                size.X = value;
                RecalculateBounds();
            }
        }

        public float Height
        {
            get => size.Y;
            set {
                size.Y = value;
                RecalculateBounds();
            }
        }

        protected override void RecalculateBounds()
        {
            Vector2 topLeft = Offset - (size / 2);
            Vector2 bottomRight = Offset - (size / 2);
            points = new Vector2[] {topLeft, new Vector2(topLeft.X, bottomRight.Y), bottomRight, new Vector2(bottomRight.X, topLeft.Y)};
            base.RecalculateBounds();
        }

        public override void Serialize(ISerializationWriter writer)
        {
            base.Serialize(writer);
            writer.Write(Size);
        }

        public override void Deserialize(ISerializationReader reader)
        {
            base.Deserialize(reader);
            Size = reader.ReadVector2();
        }
    }
}