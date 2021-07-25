namespace PhobosEngine.Collisions
{
    public static class AABBCollisions
    {
        public static bool AABBToAABB(AABBCollider b1, AABBCollider b2, out CollisionResult result)
        {
            result = new CollisionResult();

            bool collided = b1.Bounds.Intersects(b2.Bounds);

            // TODO: can some of the result properties be filled in?

            return collided;
        }
    }
}