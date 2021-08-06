using Microsoft.Xna.Framework;
using PhobosEngine.Serialization;
using PhobosEngine.Math;

using PhobosEngine.Collisions;

namespace PhobosEngine
{
    public class CircleCollider : Collider
    {
        // Radius of the circle, centered on the Collider offset.
        private float radius;
        public float Radius {
            get => radius; 
            set {
                radius = value;
                UpdateCollider();
            }
        }

        private float effectiveRadius;
        public float EffectiveRadius {
            get => effectiveRadius;
        }

        public CircleCollider() : base()
        {
            radius = 0.5f;
        }

        protected override void RecalculateBounds()
        {
            effectiveRadius = Radius * Transform.Scale.X;
            Bounds = new RectangleF(WorldPos - new Vector2(EffectiveRadius, EffectiveRadius), new Vector2(EffectiveRadius*2, EffectiveRadius*2));
        }

        public override bool LineIntersects(Vector2 start, Vector2 end, out RaycastHit hit)
        {
            return CollisionResolvers.LineToCircle(start, end, this, out hit);
        }

        public override void Serialize(ISerializationWriter writer)
        {
            base.Serialize(writer);
            writer.Write(Radius);
        }

        public override void Deserialize(ISerializationReader reader)
        {
            base.Deserialize(reader);
            Radius = reader.ReadFloat();
        }
    }
}