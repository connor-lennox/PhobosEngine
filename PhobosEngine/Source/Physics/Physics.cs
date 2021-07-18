namespace PhobosEngine.Physics
{
    public static class Physics
    {
        private static SpatialHash spatialHash;

        public static void RegisterCollider(Collider collider)
        {
            spatialHash.Register(collider);
        }

        public static void UpdateCollider(Collider collider)
        {
            spatialHash.Remove(collider);
            spatialHash.Register(collider);
        }
    }
}