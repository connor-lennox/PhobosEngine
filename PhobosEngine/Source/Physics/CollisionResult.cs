using Microsoft.Xna.Framework;

namespace PhobosEngine
{
    public struct CollisionResult
    {
        public Collider other;
        public Vector2 point;
        public Vector2 normal;
        public float depth;
    }

    public delegate void CollisionEventHandler(CollisionResult collision);
}