using System;
using NUnit.Framework;

using Microsoft.Xna.Framework;

namespace PhobosEngine.Tests.Core
{
    [TestFixture]
    public class TransformTests
    {
        private Transform transform;

        [SetUp]
        public void Setup()
        {
            transform = new Transform();
        }
    
        [Test]
        public void TransformConstructed_HasDefaultSettings()
        {
            Assert.AreEqual(transform.position, Vector2.Zero);
            Assert.AreEqual(transform.rotation, 0);
            Assert.AreEqual(transform.scale, Vector2.One);
        }

        [Test]
        public void AddToTransform_ProperDestination()
        {
            transform.position = Vector2.Zero;
            transform.position += new Vector2(1, 0);
            Assert.AreEqual(transform.position, new Vector2(1, 0));
        }

        [Test]
        public void DefaultTransform_Up_Right_HaveCorrectValues()
        {
            transform.rotation = 0;
            Assert.LessOrEqual(MathF.Abs(transform.Right.X - 1), float.Epsilon);
            Assert.LessOrEqual(MathF.Abs(transform.Up.Y - 1), float.Epsilon);
        }

        [Test]
        public void RotatedTransform_Up_Right_HaveCorrectValues()
        {
            transform.rotation = 90;
            Assert.LessOrEqual(MathF.Abs(transform.Right.Y - 1), float.Epsilon);
            Assert.LessOrEqual(MathF.Abs(transform.Up.X + 1), float.Epsilon);
        }

        [Test]
        public void Transform_PointTowards_CorrectRotation()
        {
            transform.rotation = 0;

            transform.PointTowards(Vector2.UnitX);
            Assert.LessOrEqual(MathF.Abs(transform.rotation), float.Epsilon);

            transform.PointTowards(Vector2.UnitY);
            Assert.LessOrEqual(MathF.Abs(transform.rotation - 90), float.Epsilon);

            transform.PointTowards(new Vector2(1, 1));
            Assert.LessOrEqual(MathF.Abs(transform.rotation - 45), float.Epsilon);

            transform.PointTowards(new Vector2(-1, 1));
            Assert.LessOrEqual(MathF.Abs(transform.rotation - 135), float.Epsilon);
        }
    }
}