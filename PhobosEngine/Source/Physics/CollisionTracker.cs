using System.Collections.Generic;

namespace PhobosEngine
{
    public class CollisionTracker : Component
    {
        public event CollisionEventHandler OnCollisionEnter;
        public event CollisionEventHandler OnCollisionExit;

        private Dictionary<Collider, CollisionResult> prevCollisions = new Dictionary<Collider, CollisionResult>();
        private Dictionary<Collider, CollisionResult> newCollisions = new Dictionary<Collider, CollisionResult>();

        private HashSet<Collider> enteredColliders = new HashSet<Collider>();
        private HashSet<Collider> exitedColliders = new HashSet<Collider>();

        private Collider targetCollider;

        public override void Init()
        {
            targetCollider = GetComponent<Collider>();
        }

        public override void OnParentTransformModified()
        {
            // TODO: is it possible that this could be called before the collider updates?
            //       enforce this function called in order of components added?
            if(targetCollider.Registered)
            {
                ResolveCollisions();
            }
        }

        public void ResolveCollisions()
        {
            // Prep structures
            enteredColliders.Clear();
            exitedColliders.Clear();
            newCollisions.Clear();

            // Broad + Narrow phase detection
            HashSet<Collider> nearbyColliders = Physics.BroadphaseAABBExcludeSelf(targetCollider.Bounds, targetCollider);
            foreach(Collider other in nearbyColliders)
            {
                if(targetCollider.CollidesWith(other, out CollisionResult result))
                {
                    newCollisions.Add(other, result);
                }
            }

            // Create events for entered collisions
            foreach(var collision in newCollisions)
            {
                if(!prevCollisions.ContainsKey(collision.Key))
                {
                    OnCollisionEnter.Invoke(collision.Value);
                    enteredColliders.Add(collision.Key);
                }
            }

            // Create events for exited collisions
            foreach(var oldCollision in prevCollisions)
            {
                if(!newCollisions.ContainsKey(oldCollision.Key))
                {
                    OnCollisionExit.Invoke(oldCollision.Value);
                    exitedColliders.Add(oldCollision.Key);
                }
            }

            // Store the new collisions for use next time
            foreach(Collider collider in enteredColliders)
            {
                prevCollisions.Add(collider, newCollisions[collider]);
            }

            // Remove exited collisions
            foreach(Collider collider in exitedColliders)
            {
                prevCollisions.Remove(collider);
            }
        }
    }
}