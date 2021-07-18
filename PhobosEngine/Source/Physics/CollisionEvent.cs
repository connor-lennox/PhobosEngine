namespace PhobosEngine.Physics
{
    public struct CollisionEvent
    {
        Collider other;
    }

    public delegate void CollisionEventHandler(CollisionEvent collision);
}