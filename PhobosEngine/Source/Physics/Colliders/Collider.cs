using Microsoft.Xna.Framework;
using PhobosEngine.Serialization;
using PhobosEngine.Math;

namespace PhobosEngine.Physics
{
    public abstract class Collider : Component
    {
        public event CollisionEventHandler OnCollisionEnter;
        public event CollisionEventHandler OnCollisionExit;

        public RectangleF Bounds {get; protected set;}

        public Vector2 Offset {get; protected set;}
        public Vector2 WorldPos => Entity.Transform.Position + Offset;

        protected abstract void RecalculateBounds();
        
        public override void OnParentTransformModified()
        {
            RecalculateBounds();
        }

        public override void Serialize(ISerializationWriter writer)
        {
            base.Serialize(writer);
            writer.Write(Offset);
        }

        public override void Deserialize(ISerializationReader reader)
        {
            base.Deserialize(reader);
            Offset = reader.ReadVector2();
        }
    }
}