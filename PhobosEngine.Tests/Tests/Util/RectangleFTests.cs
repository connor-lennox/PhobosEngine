using System;
using System.IO;
using NUnit.Framework;

using Microsoft.Xna.Framework;

using PhobosEngine;
using PhobosEngine.Math;

namespace PhobosEngine.Tests.Math
{
    [TestFixture]
    public class RectangleFTests
    {
        [Test]
        public void Rectangle_EdgesCorrect()
        {
            RectangleF rect = new RectangleF(0, 0, 10, 10);

            Assert.AreEqual(0, rect.Top);
            Assert.AreEqual(0, rect.Left);
            Assert.AreEqual(10, rect.Bottom);
            Assert.AreEqual(10, rect.Right);
        }

        [Test]
        public void TouchingRects_Intersects_ReturnsTrue()
        {
            RectangleF rect1 = new RectangleF(0, 0, 10, 10);
            RectangleF rect2 = new RectangleF(5, 5, 10, 10);

            Assert.IsTrue(rect1.Intersects(rect2));
        }

        [Test]
        public void NotTouchingRects_Intersects_ReturnsFalse()
        {
            RectangleF rect1 = new RectangleF(0, 0, 2, 2);
            RectangleF rect2 = new RectangleF(5, 0, 3, 3);

            Assert.IsFalse(rect1.Intersects(rect2));
        }

    }
}