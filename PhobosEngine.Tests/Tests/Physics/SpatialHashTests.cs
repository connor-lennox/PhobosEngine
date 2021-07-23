using System;
using System.IO;
using NUnit.Framework;

using Microsoft.Xna.Framework;

using PhobosEngine;
using PhobosEngine.Math;

namespace PhobosEngine.Tests
{
    [TestFixture]
    public class SpatialHashTests
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

            circleEntity = new GameEntity();
            circleCollider = circleEntity.AddComponent<CircleCollider>();
        }

        [Test]
        public void Broadphase_NearObjects_HitsObjects()
        {
            boxCollider.Register();
            circleCollider.Register();

            Assert.AreEqual(2, Physics.BroadphaseAABB(new RectangleF(0, 0, 10, 10)).Count);
        }

        [Test]
        public void Broadphase_AwayFromObjects_HitsNone()
        {
            boxCollider.Register();
            circleCollider.Register();

            Assert.AreEqual(0, Physics.BroadphaseAABB(new RectangleF(14, -7, 2, 2)).Count);
        }

    }
}