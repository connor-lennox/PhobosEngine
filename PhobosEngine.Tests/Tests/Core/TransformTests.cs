using System;
using NUnit.Framework;

using Microsoft.Xna.Framework;

namespace PhobosEngine.Tests.Core
{
    [TestFixture]
    public class TransformTests
    {
        private GameEntity entity;
        private Transform transform;

        public Transform CreateChildTransform()
        {
            GameEntity childEntity = new GameEntity();
            Transform childTransform = childEntity.Transform;
            childTransform.Parent = transform;
            return childTransform;
        }

        [SetUp]
        public void Setup()
        {
            entity = new GameEntity();
            transform = entity.Transform;
        }
    
        [Test]
        public void TransformConstructed_HasDefaultSettings()
        {
            Assert.AreEqual(transform.Position, Vector2.Zero);
            Assert.AreEqual(transform.Rotation, 0);
            Assert.AreEqual(transform.Scale, Vector2.One);
        }

        [Test]
        public void AddToTransform_ProperDestination()
        {
            transform.Position = Vector2.Zero;
            transform.Position += new Vector2(1, 0);
            Assert.AreEqual(transform.Position, new Vector2(1, 0));
        }

        [Test]
        public void DefaultTransform_Up_Right_HaveCorrectValues()
        {
            transform.Rotation = 0;
            Assert.LessOrEqual(MathF.Abs(transform.Right.X - 1), float.Epsilon);
            Assert.LessOrEqual(MathF.Abs(transform.Up.Y - 1), float.Epsilon);
        }

        [Test]
        public void RotatedTransform_Up_Right_HaveCorrectValues()
        {
            transform.Rotation = MathF.PI / 2;
            Assert.LessOrEqual(MathF.Abs(transform.Right.Y - 1), float.Epsilon);
            Assert.LessOrEqual(MathF.Abs(transform.Up.X + 1), float.Epsilon);
        }

        [Test]
        public void Transform_PointTowards_CorrectRotation()
        {
            transform.Rotation = 0;

            transform.PointTowards(Vector2.UnitX);
            Assert.LessOrEqual(MathF.Abs(transform.Rotation), float.Epsilon);

            transform.PointTowards(Vector2.UnitY);
            Assert.LessOrEqual(MathF.Abs(transform.Rotation - (MathF.PI / 2)), float.Epsilon);

            transform.PointTowards(new Vector2(1, 1));
            Assert.LessOrEqual(MathF.Abs(transform.Rotation - (MathF.PI / 4)), float.Epsilon);

            transform.PointTowards(new Vector2(-1, 1));
            Assert.LessOrEqual(MathF.Abs(transform.Rotation - (3 * MathF.PI / 4)), float.Epsilon);
        }

        [Test]
        public void ChildTransform_SetLocalPosition_CorrectWorldPosition()
        {
            transform.Position = new Vector2(0, 0);

            Transform childTransform = CreateChildTransform();
            childTransform.LocalPosition = new Vector2(1, 1);

            Assert.AreEqual(childTransform.Position, new Vector2(1, 1));
        }

        [Test]
        public void ParentTransformMoves_ChildFollows()
        {
            transform.Position = new Vector2(0, 0);

            GameEntity childEntity = new GameEntity();
            Transform childTransform = CreateChildTransform();
            
            Assert.AreEqual(childTransform.Position, Vector2.Zero);

            transform.Position = new Vector2(1, 1);

            Assert.AreEqual(childTransform.Position, new Vector2(1, 1));
            Assert.AreEqual(childTransform.LocalPosition, Vector2.Zero);
        }

        [Test]
        public void ParentAndChildMove_PositionCorrect()
        {
            Transform childTransform = CreateChildTransform();

            transform.Position = new Vector2(2, 2);
            childTransform.LocalPosition = new Vector2(1, 1);

            Assert.AreEqual(childTransform.Position, new Vector2(3, 3));
        }

        [Test]
        public void ChildTransform_PositionSet_CorrectLocalPosition()
        {
            Transform childTransform = CreateChildTransform();

            transform.Position = new Vector2(1, 1);
            childTransform.Position = Vector2.Zero;
            
            Assert.AreEqual(childTransform.LocalPosition, -Vector2.One);
        }

        [Test]
        public void RootTransform_RotationTests()
        {
            transform.Rotation = 1;
            Assert.AreEqual(transform.LocalRotation, 1);

            transform.RotationDegrees = 180;
            Assert.AreEqual(transform.LocalRotation, MathF.PI);

            transform.LocalRotation = MathF.PI / 2;
            Assert.AreEqual(transform.RotationDegrees, 90);
        }

        [Test]
        public void RootTransform_ScaleTests()
        {
            transform.Scale = 2 * Vector2.One;
            Assert.AreEqual(transform.LocalScale, 2 * Vector2.One);

            transform.LocalScale = Vector2.One;
            Assert.AreEqual(transform.Scale, Vector2.One);
        }

        [Test]
        public void ChildTransform_CorrectRotation()
        {
            Transform childTransform = CreateChildTransform();

            transform.Rotation = 1;
            Assert.AreEqual(childTransform.Rotation, 1);
            Assert.AreEqual(childTransform.LocalRotation, 0);

            childTransform.Rotation = 2;
            Assert.AreEqual(childTransform.Rotation, 2);
            Assert.AreEqual(childTransform.LocalRotation, 1);
        }

        [Test]
        public void ChildTransform_CorrectScale()
        {
            Transform childTransform = CreateChildTransform();

            transform.Scale = 2 * Vector2.One;
            Assert.AreEqual(childTransform.Scale, 2 * Vector2.One);
            Assert.AreEqual(childTransform.LocalScale, Vector2.One);

            childTransform.Scale = Vector2.One;
            Assert.AreEqual(childTransform.Scale, Vector2.One);
            Assert.AreEqual(childTransform.LocalScale, new Vector2(0.5f, 0.5f));
        }
    }
}