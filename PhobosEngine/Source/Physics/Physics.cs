using System.Collections.Generic;
using Microsoft.Xna.Framework;

using PhobosEngine.Collisions;
using PhobosEngine.Math;

namespace PhobosEngine
{
    public static class Physics
    {
        private static SpatialHash spatialHash = new SpatialHash();

        private static RaycastHit[] tempHit = new RaycastHit[1];

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

        public static HashSet<Collider> BroadphaseAABBExcludeSelf(RectangleF bounds, Collider self)
        {
            HashSet<Collider> result = spatialHash.BroadphaseAABB(ref bounds);
            result.Remove(self);
            return result;
        }

        public static bool Raycast(Vector2 origin, Vector2 direction, float maxLength, out RaycastHit hit)
        {
            if(spatialHash.Linecast(origin, origin + Vector2.Normalize(direction) * maxLength, tempHit) > 0) 
            {
                hit = tempHit[0];
                return true;
            }
            hit = new RaycastHit();
            return false;
        }

        public static int RaycastAll(Vector2 origin, Vector2 direction, float maxLength, RaycastHit[] hits)
        {
            return spatialHash.Linecast(origin, origin + Vector2.Normalize(direction) * maxLength, hits);
        }

        public static void RemoveAll()
        {
            spatialHash.Clear();
        }
    }
}