using Microsoft.Xna.Framework;
using PhobosEngine.Serialization;
using PhobosEngine.Math;

namespace PhobosEngine
{
    public abstract class Collider : Component
    {
        public event CollisionEventHandler OnCollisionEnter;
        public event CollisionEventHandler OnCollisionExit;

        public RectangleF Bounds {get; protected set;}

        private Vector2 offset;
        public Vector2 Offset {
            get => offset;
            set {
                offset = value;
                UpdateCollider();
            }
        }

        public Vector2 WorldPos => Entity.Transform.Position + Offset;

        protected bool registered = false;

        public Collider()
        {
            Offset = Vector2.Zero;
        }

        public override void Init()
        {
            UpdateCollider();
        }

        protected abstract void RecalculateBounds();
        
        public override void OnParentTransformModified()
        {
            UpdateCollider();
        }

        private void UpdateCollider()
        {
            if(this.Entity != null) {
                RecalculateBounds();
                if(registered)
                {
                    Physics.UpdateCollider(this);
                }
            }
        }

        public void Register()
        {
            Physics.RegisterCollider(this);
            registered = true;
        }

        public void Deregister()
        {
            Physics.RemoveCollider(this);
            registered = false;
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