using System;
using NUnit.Framework;

using Microsoft.Xna.Framework;

namespace PhobosEngine.Tests
{
    [TestFixture]
    public class CollisionTrackingTests
    {
        private GameEntity entity1;
        private GameEntity entity2;

        private CircleCollider collider1;
        private CircleCollider collider2;

        private bool activeCollision;
        private CollisionResult activeResult = new CollisionResult();

        private void EnterCollision(CollisionResult result) {
            activeCollision = true;
            activeResult = result;
        }

        private void ExitCollision(CollisionResult result)
        {
            activeCollision = false;
        }

        [SetUp]
        public void SetUp()
        {
            // Clear physics from previous test
            Physics.RemoveAll();

            // Setup: two entities, spread apart, colliders on both, tracker on e1
            entity1 = new GameEntity();
            entity1.Transform.Position = new Vector2(5, 0);
            collider1 = entity1.AddComponent<CircleCollider>();
            collider1.Radius = 1;
            CollisionTracker tracker = entity1.AddComponent<CollisionTracker>();
            tracker.OnCollisionEnter += EnterCollision;
            tracker.OnCollisionExit += ExitCollision;

            entity2 = new GameEntity();
            collider2 = entity2.AddComponent<CircleCollider>();
            collider2.Radius = 1;

            // Add new colliders to Physics
            collider1.Register();
            collider2.Register();

            // Initialize active collision to false
            activeCollision = false;
        }

        [Test]
        public void ObjectsMove_FarApart_NoCollision()
        {
            entity1.Transform.Position += new Vector2(0, 1);
            Assert.IsFalse(activeCollision);
        }

        [Test]
        public void ObjectsMove_IntoRange_CollisionOccurs()
        {
            entity1.Transform.Position += new Vector2(-3.1f, 0);
            Assert.IsTrue(activeCollision);

            Assert.AreEqual(collider2, activeResult.other);
            Assert.LessOrEqual(float.Epsilon, MathF.Abs(activeResult.depth - .1f));
            Assert.AreEqual(Vector2.UnitX, activeResult.normal);
            Assert.AreEqual(new Vector2(1, 0), activeResult.point);
        }
    }
}