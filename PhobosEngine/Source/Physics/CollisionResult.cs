using Microsoft.Xna.Framework;

namespace PhobosEngine
{
    public struct CollisionResult
    {
        Collider other;
        Vector2 point;
        Vector2 normal;
        float depth;
    }

    public delegate void CollisionEventHandler(CollisionResult collision);
}