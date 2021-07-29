using System;
using System.IO;
using NUnit.Framework;

using Microsoft.Xna.Framework;

using PhobosEngine;
using PhobosEngine.Math;

namespace PhobosEngine.Tests
{
    [TestFixture]
    public class RaycastTests
    {
        private GameEntity boxEntity;
        private BoxCollider boxCollider;

        private GameEntity circleEntity;
        private CircleCollider circleCollider;

        [SetUp]
        public void SetUp()
        {
            Physics.RemoveAll();

            boxEntity = new GameEntity();
            boxCollider = boxEntity.AddComponent<BoxCollider>();
            boxCollider.Size = new Vector2(2, 2);

            circleEntity = new GameEntity();
            circleCollider = circleEntity.AddComponent<CircleCollider>();
            circleCollider.Radius = 1;
        }

        [Test]
        public void BoxCollider_RaycastTowards_Hits()
        {
            boxEntity.Transform.Position = new Vector2(5, 0);
            boxCollider.Register();

            Assert.IsTrue(Physics.Raycast(Vector2.Zero, Vector2.UnitX, 10, out RaycastHit hit));
            Assert.AreEqual(boxCollider, hit.collider);
            Assert.AreEqual(4, hit.distance);
            Assert.AreEqual(-Vector2.UnitX, hit.normal);
            Assert.AreEqual(new Vector2(4, 0), hit.point);
        }

        [Test]
        public void CircleCollider_RaycastTowards_Hits()
        {
            circleEntity.Transform.Position = new Vector2(-5, 0);
            circleCollider.Register();

            Assert.IsTrue(Physics.Raycast(Vector2.Zero, -Vector2.UnitX, 10, out RaycastHit hit));
            Assert.AreEqual(circleCollider, hit.collider);
            Assert.AreEqual(4, hit.distance);
            Assert.AreEqual(Vector2.UnitX, hit.normal);
            Assert.AreEqual(new Vector2(-4, 0), hit.point);
        }

        [Test]
        public void Collider_RaycastAway_NoHit()
        {
            boxEntity.Transform.Position = new Vector2(5, 0);
            boxCollider.Register();

            Assert.IsFalse(Physics.Raycast(Vector2.Zero, Vector2.UnitY, 10, out RaycastHit hit));
        }

        [Test]
        public void Collider_RaycastShort_NoHit()
        {
            boxEntity.Transform.Position = new Vector2(5, 0);
            boxCollider.Register();

            Assert.IsFalse(Physics.Raycast(Vector2.Zero, Vector2.UnitX, 2, out RaycastHit hit));
        }

        [Test]
        public void TwoColliders_RaycastAll_HitsBoth()
        {
            boxEntity.Transform.Position = new Vector2(2, 0);
            circleEntity.Transform.Position = new Vector2(5, 0);

            boxCollider.Register();
            circleCollider.Register();

            Assert.AreEqual(2, Physics.RaycastAll(Vector2.Zero, Vector2.UnitX, 10, new RaycastHit[2]));
        }
    }
}