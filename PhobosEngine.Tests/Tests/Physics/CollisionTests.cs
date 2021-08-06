using System;
using System.IO;
using NUnit.Framework;

using Microsoft.Xna.Framework;

using PhobosEngine.Collisions;

namespace PhobosEngine.Tests
{
    [TestFixture]
    public class CollisionTesting
    {
        [Test]
        public void CloseCircles_Collision_ReturnsTrue()
        {
            GameEntity e1 = new GameEntity();
            CircleCollider c1 = e1.AddComponent<CircleCollider>();

            GameEntity e2 = new GameEntity();
            CircleCollider c2 = e2.AddComponent<CircleCollider>();

            c1.Radius = 1f;
            c2.Radius = 1f;
            
            e1.Transform.Position = new Vector2(0, 0);
            e2.Transform.Position = new Vector2(0.5f, 0.5f);

            Assert.IsTrue(CollisionResolvers.CircleToCircle(c1, c2, out CollisionResult result));
        }

        [Test]
        public void FarCircles_Collision_ReturnsFalse()
        {
            GameEntity e1 = new GameEntity();
            CircleCollider c1 = e1.AddComponent<CircleCollider>();

            GameEntity e2 = new GameEntity();
            CircleCollider c2 = e2.AddComponent<CircleCollider>();

            c1.Radius = 1f;
            c2.Radius = 1f;
            
            e1.Transform.Position = new Vector2(0, 0);
            e2.Transform.Position = new Vector2(2.5f, 0.5f);

            Assert.IsFalse(CollisionResolvers.CircleToCircle(c1, c2, out CollisionResult result));
        }

        [Test]
        public void CloseCirclePoly_Collision_ReturnsTrue()
        {
            GameEntity e1 = new GameEntity();
            CircleCollider c1 = e1.AddComponent<CircleCollider>();

            GameEntity e2 = new GameEntity();
            PolygonCollider p2 = e2.AddComponent<PolygonCollider>();

            c1.Transform.Position = new Vector2(-0.3f, -0.3f);
            c1.Radius = 1f;
            p2.Points = new Vector2[] {new Vector2(0, 0), new Vector2(1, 1), new Vector2(0, 1)};

            Assert.IsTrue(CollisionResolvers.CircleToPoly(c1, p2, out CollisionResult result));
        }

        [Test]
        public void FarCirclePoly_Collision_ReturnsFalse()
        {
            GameEntity e1 = new GameEntity();
            CircleCollider c1 = e1.AddComponent<CircleCollider>();

            GameEntity e2 = new GameEntity();
            PolygonCollider p2 = e2.AddComponent<PolygonCollider>();

            c1.Transform.Position = new Vector2(-2.3f, -0.3f);
            c1.Radius = 1f;
            p2.Points = new Vector2[] {new Vector2(0, 0), new Vector2(1, 1), new Vector2(0, 1)};

            Assert.IsFalse(CollisionResolvers.CircleToPoly(c1, p2, out CollisionResult result));
        }

        [Test]
        public void CircleInPoly_Collision_ReturnsTrue()
        {
            GameEntity e1 = new GameEntity();
            CircleCollider c1 = e1.AddComponent<CircleCollider>();

            GameEntity e2 = new GameEntity();
            PolygonCollider p2 = e2.AddComponent<PolygonCollider>();

            c1.Transform.Position = new Vector2(2, 2);
            c1.Radius = 1f;
            p2.Points = new Vector2[] {new Vector2(0, 0), new Vector2(5, 5), new Vector2(0, 5)};

            Assert.IsTrue(CollisionResolvers.CircleToPoly(c1, p2, out CollisionResult result));
        }

        [Test]
        public void NearPolys_Collision_ReturnsTrue()
        {
            GameEntity e1 = new GameEntity();
            PolygonCollider p1 = e1.AddComponent<PolygonCollider>();

            GameEntity e2 = new GameEntity();
            PolygonCollider p2 = e2.AddComponent<PolygonCollider>();

            p1.Points = new Vector2[] {new Vector2(0, 0), new Vector2(2, 0), new Vector2(2, 2), new Vector2(0, 2)};
            p2.Points = new Vector2[] {new Vector2(1, 1), new Vector2(3, 1), new Vector2(1, 3)};

            Assert.IsTrue(CollisionResolvers.PolyToPoly(p1, p2, out CollisionResult result));
        }

        [Test]
        public void FarPolys_Collision_ReturnsFalse()
        {
            GameEntity e1 = new GameEntity();
            PolygonCollider p1 = e1.AddComponent<PolygonCollider>();

            GameEntity e2 = new GameEntity();
            PolygonCollider p2 = e2.AddComponent<PolygonCollider>();

            p1.Points = new Vector2[] {new Vector2(0, 0), new Vector2(2, 0), new Vector2(2, 2), new Vector2(0, 2)};
            p2.Points = new Vector2[] {new Vector2(5, 5), new Vector2(3, 5), new Vector2(5, 3)};

            Assert.IsFalse(CollisionResolvers.PolyToPoly(p1, p2, out CollisionResult result));
        }

        [Test]
        public void LineToLine_Intersecting_CorrectValues()
        {
            Vector2 a1 = new Vector2(1, 1);
            Vector2 a2 = new Vector2(2, 3);

            Vector2 b1 = new Vector2(0, 2);
            Vector2 b2 = new Vector2(4, 0);

            Assert.IsTrue(CollisionResolvers.LineToLine(a1, a2, b1, b2, out Vector2 intersect));
            Assert.AreEqual(new Vector2(1.2f, 1.4f), intersect);
        }

        [Test]
        public void LineToLine_NotIntersecting_ReturnsFalse()
        {
            Vector2 a1 = new Vector2(1, 1);
            Vector2 a2 = new Vector2(2, 3);

            Vector2 b1 = new Vector2(0, 2);
            Vector2 b2 = new Vector2(1, 1.5f);

            Assert.IsFalse(CollisionResolvers.LineToLine(a1, a2, b1, b1, out Vector2 intersect));
        }

        [Test]
        public void LineToLine_Parallel_ReturnsFalse()
        {
            Vector2 a1 = new Vector2(1, 1);
            Vector2 a2 = new Vector2(2, 3);

            Vector2 b1 = a1 + Vector2.One;
            Vector2 b2 = a2 + Vector2.One;

            Assert.IsFalse(CollisionResolvers.LineToLine(a1, a2, b1, b2, out Vector2 intersect));
        }

        [Test]
        public void LineToCircle_Intersect_CorrectValues()
        {
            Vector2 a1 = new Vector2(0, 0);
            Vector2 a2 = new Vector2(5, 0);

            GameEntity entity = new GameEntity();
            entity.Transform.Position = new Vector2(3, 0);
            CircleCollider circle = entity.AddComponent<CircleCollider>();
            circle.Radius = 1f;

            Assert.IsTrue(CollisionResolvers.LineToCircle(a1, a2, circle, out RaycastHit hit));
            Assert.AreEqual(new Vector2(-1, 0), hit.normal);
            Assert.AreEqual(circle, hit.collider);
            Assert.AreEqual(new Vector2(2, 0), hit.point);
            Assert.AreEqual(2, hit.distance);
        }

        [Test]
        public void LineToCircle_NotIntersecting_ReturnsFalse()
        {
            Vector2 a1 = new Vector2(0, 0);
            Vector2 a2 = new Vector2(5, 0);

            GameEntity entity = new GameEntity();
            entity.Transform.Position = new Vector2(3, 3);
            CircleCollider circle = entity.AddComponent<CircleCollider>();
            circle.Radius = 1f;

            Assert.IsFalse(CollisionResolvers.LineToCircle(a1, a2, circle, out RaycastHit hit));
        }

        [Test]
        public void LineToAABB_Intersecting_CorrectValues()
        {
            Vector2 a1 = new Vector2(0, 0);
            Vector2 a2 = new Vector2(5, 0);

            GameEntity entity = new GameEntity();
            entity.Transform.Position = new Vector2(3, 0);
            AABBCollider collider = entity.AddComponent<AABBCollider>();
            collider.Size = new Vector2(2, 2);

            Assert.IsTrue(CollisionResolvers.LineToAABB(a1, a2, collider, out RaycastHit hit));
            Assert.AreEqual(collider, hit.collider);
            Assert.AreEqual(new Vector2(-1, 0), hit.normal);
            Assert.AreEqual(new Vector2(2, 0), hit.point);
            Assert.AreEqual(2, hit.distance);
        }

        [Test]
        public void LineToAABB_NotIntersecting_ReturnsFalse()
        {
            Vector2 a1 = new Vector2(0, 0);
            Vector2 a2 = new Vector2(5, 0);
            
            GameEntity entity = new GameEntity();
            entity.Transform.Position = new Vector2(3, 3);
            AABBCollider collider = entity.AddComponent<AABBCollider>();
            collider.Size = Vector2.One;

            Assert.IsFalse(CollisionResolvers.LineToAABB(a1, a2, collider, out RaycastHit hit));
        }

        [Test]
        public void LineToPoly_Intersecting_CorrectValues()
        {
            Vector2 a1 = new Vector2(0, 0);
            Vector2 a2 = new Vector2(5, 0);
            GameEntity entity = new GameEntity();
            entity.Transform.Position = new Vector2(3, 0);
            PolygonCollider collider = entity.AddComponent<PolygonCollider>();
            collider.Points = new Vector2[] {new Vector2(-1, -1), new Vector2(-1, 1), new Vector2(1, 0)};

            Assert.IsTrue(CollisionResolvers.LineToPoly(a1, a2, collider, out RaycastHit hit));
            Assert.AreEqual(collider, hit.collider);
            Assert.AreEqual(new Vector2(2, 0), hit.point);
            Assert.AreEqual(new Vector2(-1, 0), hit.normal);
            Assert.AreEqual(2, hit.distance);
        }

        [Test]
        public void LineToPoly_NotIntersecting_ReturnsFalse()
        {
            Vector2 a1 = new Vector2(0, 0);
            Vector2 a2 = new Vector2(5, 0);
            GameEntity entity = new GameEntity();
            entity.Transform.Position = new Vector2(3, 3);
            PolygonCollider collider = entity.AddComponent<PolygonCollider>();
            collider.Points = new Vector2[] {new Vector2(-1, -1), new Vector2(-1, 1), new Vector2(1, 0)};

            Assert.IsFalse(CollisionResolvers.LineToPoly(a1, a2, collider, out RaycastHit hit));
        }
    }
}