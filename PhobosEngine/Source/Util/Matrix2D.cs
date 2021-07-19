using Microsoft.Xna.Framework;
using System;

namespace PhobosEngine.Math
{
    public struct Matrix2D
    {
        public float m11;
        public float m12;
        public float m13;
        public float m21;
        public float m22;
        public float m23;


        public static Matrix2D Add(Matrix2D mat1, Matrix2D mat2)
        {
            mat1.m11 += mat2.m11;
            mat1.m12 += mat2.m12;
            mat1.m13 += mat2.m13;
            mat1.m21 += mat2.m21;
            mat1.m22 += mat2.m22;
            mat1.m23 += mat2.m23;

            return mat1;
        }

        public static void Add(ref Matrix2D mat1, ref Matrix2D mat2, out Matrix2D result)
        {
            result.m11 = mat1.m11 + mat2.m11;
            result.m12 = mat1.m12 + mat2.m12;
            result.m13 = mat1.m13 + mat2.m13;
            result.m21 = mat1.m21 + mat2.m21;
            result.m22 = mat1.m22 + mat2.m22;
            result.m23 = mat1.m23 + mat2.m23;
        }

        public static Matrix2D Multiply(Matrix2D mat1, Matrix2D mat2)
        {
            float m11 = (mat1.m11*mat2.m11) + (mat1.m12*mat2.m21);
            float m12 = (mat1.m11*mat2.m12) + (mat1.m12*mat2.m22);
            float m13 = (mat1.m11*mat2.m13) + (mat1.m12*mat2.m23) + mat1.m13;
            float m21 = (mat1.m21*mat2.m11) + (mat1.m22*mat2.m21);
            float m22 = (mat1.m21*mat2.m12) + (mat1.m22*mat2.m22);
            float m23 = (mat1.m21*mat2.m13) + (mat1.m22*mat2.m23) + mat1.m23;

            mat1.m11 = m11;
            mat1.m12 = m12;
            mat1.m13 = m13;
            mat1.m21 = m21;
            mat1.m22 = m22;
            mat1.m23 = m23;

            return mat1;
        }

        public static void Multiply(ref Matrix2D mat1, ref Matrix2D mat2, out Matrix2D result)
        {
            float m11 = (mat1.m11*mat2.m11) + (mat1.m12*mat2.m21);
            float m12 = (mat1.m11*mat2.m12) + (mat1.m12*mat2.m22);
            float m13 = (mat1.m11*mat2.m13) + (mat1.m12*mat2.m23) + mat1.m13;
            float m21 = (mat1.m21*mat2.m11) + (mat1.m22*mat2.m21);
            float m22 = (mat1.m21*mat2.m12) + (mat1.m22*mat2.m22);
            float m23 = (mat1.m21*mat2.m13) + (mat1.m22*mat2.m23) + mat1.m23;

            result.m11 = m11;
            result.m12 = m12;
            result.m13 = m13;
            result.m21 = m21;
            result.m22 = m22;
            result.m23 = m23;
        }

        public static Matrix2D Transpose(Matrix2D matrix)
        {
            Matrix2D result;
            Transpose(ref matrix, out result);
            return result;
        }

        public static void Transpose(ref Matrix2D matrix, out Matrix2D result)
        {
            result.m11 = matrix.m11;
            result.m12 = matrix.m21;
            result.m13 = 0;
            result.m21 = matrix.m12;
            result.m22 = matrix.m22;
            result.m23 = 0;
        }

        public float Determinant()
        {
            return (m11 * m22) - (m12 * m21);
        }

        public static Matrix2D Invert(Matrix2D matrix)
        {
            Matrix2D result;
            Invert(ref matrix, out result);
            return result;
        }

        public static void Invert(ref Matrix2D matrix, out Matrix2D result)
        {
            float invDet = 1 / matrix.Determinant();

            result.m11 = matrix.m22 * invDet;
            result.m12 = -matrix.m12 * invDet;
            result.m13 = ((matrix.m12 * matrix.m23) - (matrix.m22 * matrix.m13)) * invDet;
            result.m21 = -matrix.m21 * invDet;
            result.m22 = matrix.m11 * invDet;
            result.m23 = -((matrix.m11 * matrix.m23) - (matrix.m21 * matrix.m13)) * invDet;
        }

        public static Matrix2D CreateTranslation(Vector2 translation)
        {
            Matrix2D result;
            CreateTranslation(translation, out result);
            return result;
        }

        public static void CreateTranslation(Vector2 translation, out Matrix2D result)
        {
            result.m11 = 1;
            result.m12 = 0;
            result.m13 = translation.X;
            result.m21 = 0;
            result.m22 = 1;
            result.m23 = translation.Y;
        }

        public static Matrix2D CreateRotation(float radians)
        {
            Matrix2D result;
            CreateRotation(radians, out result);
            return result;
        }

        public static void CreateRotation(float radians, out Matrix2D result)
        {
            float cos = MathF.Cos(radians);
            float sin = MathF.Sin(radians);

            result.m11 = cos;
            result.m12 = -sin;
            result.m13 = 0;
            result.m21 = sin;
            result.m22 = cos;
            result.m23 = 0;
        }

        public static Matrix2D CreateScale(Vector2 scale)
        {
            Matrix2D result;
            CreateScale(scale, out result);
            return result;
        }

        public static void CreateScale(Vector2 scale, out Matrix2D result)
        {
            result.m11 = scale.X;
            result.m12 = 0;
            result.m13 = 0;
            result.m21 = 0;
            result.m22 = scale.Y;
            result.m23 = 0;
        }
    }
}
