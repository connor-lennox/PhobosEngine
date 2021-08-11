using System.Text.Json;
using Microsoft.Xna.Framework;
using PhobosEngine.Serialization;
using PhobosEngine.Math;

namespace PhobosEngine
{
    public abstract class Collider : Component
    {

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

        public bool Registered {get; protected set;} = false;

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

        protected void UpdateCollider()
        {
            if(this.Entity != null) {
                RecalculateBounds();
                if(Registered)
                {
                    Physics.UpdateCollider(this);
                }
            }
        }

        public void Register()
        {
            Physics.RegisterCollider(this);
            Registered = true;
        }

        public void Deregister()
        {
            Physics.RemoveCollider(this);
            Registered = false;
        }

        public abstract bool LineIntersects(Vector2 start, Vector2 end, out RaycastHit hit);
        public abstract bool CollidesWith(Collider other, out CollisionResult result);

        public override void Serialize(Utf8JsonWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVector2("offset", Offset);
        }

        public override void Deserialize(JsonElement json)
        {
            base.Deserialize(json);
            Offset = json.GetProperty("offset").GetVector2();
        }
    }
}