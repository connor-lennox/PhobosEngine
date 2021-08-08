using System;
using NUnit.Framework;
using Microsoft.Xna.Framework;

using PhobosEngine.Math;

namespace PhobosEngine.Tests.Math
{
    [TestFixture]
    public class Matrix2DTests
    {

        private void TestMatrix(Matrix2D matrix, float e11, float e12, float e13, float e21, float e22, float e23)
        {
            Assert.AreEqual(e11, matrix.m11);
            Assert.AreEqual(e12, matrix.m12);
            Assert.AreEqual(e13, matrix.m13);
            Assert.AreEqual(e21, matrix.m21);
            Assert.AreEqual(e22, matrix.m22);
            Assert.AreEqual(e23, matrix.m23);
        }

        [Test]
        public void CreateTransformations_FunctionsProperly()
        {
            Matrix2D translation = Matrix2D.CreateTranslation(new Vector2(3, 4));
            TestMatrix(translation, 1, 0, 3, 0, 1, 4);

            Matrix2D rotation = Matrix2D.CreateRotation(1);
            TestMatrix(rotation, MathF.Cos(1), -MathF.Sin(1), 0, MathF.Sin(1), MathF.Cos(1), 0);

            Matrix2D scale = Matrix2D.CreateScale(new Vector2(3, 4));
            TestMatrix(scale, 3, 0, 0, 0, 4, 0);
        }

        [Test]
        public void MultiplyTransformations_CorrectResult()
        {
            Matrix2D scale = Matrix2D.CreateScale(new Vector2(3, 4));
            Matrix2D translation = Matrix2D.CreateTranslation(new Vector2(1, 2));

            Matrix2D result = Matrix2D.Multiply(scale, translation);
            TestMatrix(result, 3, 0, 3, 0, 4, 8);
        }

        [Test]
        public void ApplyTransformation_CorrectResult()
        {
            Matrix2D scale = Matrix2D.CreateScale(new Vector2(3, 4));
            Matrix2D translation = Matrix2D.CreateTranslation(new Vector2(1, 2));

            Matrix2D result = Matrix2D.Multiply(scale, translation);

            Vector2 position = new Vector2(2, 6);
            Vector2 transformedPosition = PBMath.Transform(position, result);

            Assert.AreEqual(9, transformedPosition.X);
            Assert.AreEqual(32, transformedPosition.Y);
        }
    }
}