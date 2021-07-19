using Microsoft.Xna.Framework;
using PhobosEngine.Serialization;
using PhobosEngine.Math;

namespace PhobosEngine.Physics
{
    public class CircleCollider : Collider
    {
        // Radius of the circle, centered on the Collider offset.
        public float Radius {get; private set;}

        protected override void RecalculateBounds()
        {
            Bounds = new RectangleF(WorldPos - new Vector2(Radius, Radius), new Vector2(Radius, Radius));
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