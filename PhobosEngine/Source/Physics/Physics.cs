using System.Collections.Generic;

using PhobosEngine.Collisions;
using PhobosEngine.Math;

namespace PhobosEngine
{
    public static class Physics
    {
        private static SpatialHash spatialHash = new SpatialHash();

        public static void RegisterCollider(Collider collider)
        {
            spatialHash.Register(collider);
        }

        public static void RemoveCollider(Collider collider)
        {
            spatialHash.Remove(collider);
        }

        public static void UpdateCollider(Collider collider)
        {
            spatialHash.Remove(collider);
            spatialHash.Register(collider);
        }

        public static HashSet<Collider> BroadphaseAABB(RectangleF bounds)
        {
            return spatialHash.BroadphaseAABB(ref bounds);
        }

        public static void RemoveAll()
        {
            spatialHash.Clear();
        }
    }
}