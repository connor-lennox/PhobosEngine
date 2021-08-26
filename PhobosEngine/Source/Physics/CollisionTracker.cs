using System.Collections.Generic;

namespace PhobosEngine
{
    public class CollisionTracker : Component
    {
        public event CollisionEventHandler OnCollisionEnter;
        public event CollisionEventHandler OnCollisionExit;

        private Dictionary<Collider, CollisionResult> prevCollisions = new Dictionary<Collider, CollisionResult>();
        private Dictionary<Collider, CollisionResult> newCollisions = new Dictionary<Collider, CollisionResult>();


        private Collider[] targetColliders;

        public override void Init()
        {
            targetColliders = GetComponents<Collider>();
        }

        public override void OnParentTransformModified()
        {
            ResolveCollisions();
        }

        public void ResolveCollisions()
        {
            // Prep structures
            newCollisions.Clear();

            // Broad + Narrow phase detection
            // Iterate through all colliders on this object (only at the root level)
            foreach(Collider source in targetColliders)
            {
                if(source.Registered)
                {
                    HashSet<Collider> nearbyColliders = Physics.BroadphaseAABBExcludeSelf(source.Bounds, source);
                    foreach(Collider other in nearbyColliders)
                    {
                        if(source.CollidesWith(other, out CollisionResult result))
                        {
                            newCollisions.Add(other, result);
                        }
                    }
                }
            }

            // Create events for entered collisions
            foreach(var collision in newCollisions)
            {
                if(!prevCollisions.ContainsKey(collision.Key))
                {
                    OnCollisionEnter?.Invoke(collision.Value);
                }
            }

            // Create events for exited collisions
            foreach(var oldCollision in prevCollisions)
            {
                if(!newCollisions.ContainsKey(oldCollision.Key))
                {
                    OnCollisionExit?.Invoke(oldCollision.Value);
                }
            }


            prevCollisions.Clear();
            foreach(Collider collider in newCollisions.Keys)
            {
                prevCollisions.Add(collider, newCollisions[collider]);
            }
        }
    }
}