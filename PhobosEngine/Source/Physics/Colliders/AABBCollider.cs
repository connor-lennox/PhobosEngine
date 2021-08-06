using Microsoft.Xna.Framework;

using PhobosEngine.Math;
using PhobosEngine.Serialization;
using PhobosEngine.Collisions;

namespace PhobosEngine
{
    public class AABBCollider : Collider
    {
        private Vector2 size;
        public Vector2 Size {
            get => size;
            set {
                size = value;
                UpdateCollider();
            }
        }

        private Vector2 effectiveSize;
        public Vector2 EffectiveSize {
            get => effectiveSize;
            private set => effectiveSize = value;
        }

        private Vector2[] points;
        public Vector2[] Points { get=>points; }

        protected override void RecalculateBounds()
        {
            effectiveSize = new Vector2(size.X * Transform.Scale.X, size.Y * Transform.Scale.Y);
            Bounds = new RectangleF(WorldPos - (effectiveSize/2), effectiveSize);
            points = new Vector2[] {new Vector2(Bounds.Left, Bounds.Top), new Vector2(Bounds.Right, Bounds.Top), 
                                    new Vector2(Bounds.Right, Bounds.Bottom), new Vector2(Bounds.Left, Bounds.Bottom)};
        }

        public override bool LineIntersects(Vector2 start, Vector2 end, out RaycastHit hit)
        {
            return CollisionResolvers.LineToAABB(start, end, this, out hit);
        }

        public override void Serialize(ISerializationWriter writer)
        {
            base.Serialize(writer);
            writer.Write(size);
        }

        public override void Deserialize(ISerializationReader reader)
        {
            base.Deserialize(reader);
            Size = reader.ReadVector2();
        }
    }
}