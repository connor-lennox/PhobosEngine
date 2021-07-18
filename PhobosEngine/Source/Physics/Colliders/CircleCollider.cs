using PhobosEngine.Serialization;

namespace PhobosEngine.Physics
{
    public class CircleCollider : Collider
    {
        // Radius of the circle, centered on the Collider offset.
        public float Radius {get; private set;}

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