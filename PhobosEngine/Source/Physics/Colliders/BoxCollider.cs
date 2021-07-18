using Microsoft.Xna.Framework;
using PhobosEngine.Serialization;

namespace PhobosEngine.Physics
{
    public class BoxCollider : Collider
    {
        // Size represents the width and height of the collider.
        // Values are relative to the Collider offset, which is the centerpoint of the box.
        public Vector2 Size {get; private set;}

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