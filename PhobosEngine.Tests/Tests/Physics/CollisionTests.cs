using System;
using System.IO;
using NUnit.Framework;

using Microsoft.Xna.Framework;

using PhobosEngine;
using PhobosEngine.Math;

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

            Assert.IsTrue(CircleCollisions.CircleToCircle(c1, c2, out CollisionResult result));
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

            Assert.IsFalse(CircleCollisions.CircleToCircle(c1, c2, out CollisionResult result));
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

            Assert.IsTrue(CircleCollisions.CircleToPoly(c1, p2, out CollisionResult result));
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

            Assert.IsFalse(CircleCollisions.CircleToPoly(c1, p2, out CollisionResult result));
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

            Assert.IsTrue(CircleCollisions.CircleToPoly(c1, p2, out CollisionResult result));
        }
    }
}