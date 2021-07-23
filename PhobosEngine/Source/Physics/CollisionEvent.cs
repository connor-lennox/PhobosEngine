using Microsoft.Xna.Framework;

namespace PhobosEngine
{
    public struct CollisionEvent
    {
        Collider other;
        Vector2 normal;
    }

    public delegate void CollisionEventHandler(CollisionEvent collision);
}