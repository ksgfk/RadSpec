using System.Numerics;

namespace RadSpec;

public struct Matrix4x4f : IEquatable<Matrix4x4f>
{
    public Matrix4x4 Value;

    public float M11 { readonly get => Value.M11; set => Value.M11 = value; }
    public float M12 { readonly get => Value.M12; set => Value.M12 = value; }
    public float M13 { readonly get => Value.M13; set => Value.M13 = value; }
    public float M14 { readonly get => Value.M14; set => Value.M14 = value; }
    public float M21 { readonly get => Value.M21; set => Value.M21 = value; }
    public float M22 { readonly get => Value.M22; set => Value.M22 = value; }
    public float M23 { readonly get => Value.M23; set => Value.M23 = value; }
    public float M24 { readonly get => Value.M24; set => Value.M24 = value; }
    public float M31 { readonly get => Value.M31; set => Value.M31 = value; }
    public float M32 { readonly get => Value.M32; set => Value.M32 = value; }
    public float M33 { readonly get => Value.M33; set => Value.M33 = value; }
    public float M34 { readonly get => Value.M34; set => Value.M34 = value; }
    public float M41 { readonly get => Value.M41; set => Value.M41 = value; }
    public float M42 { readonly get => Value.M42; set => Value.M42 = value; }
    public float M43 { readonly get => Value.M43; set => Value.M43 = value; }
    public float M44 { readonly get => Value.M44; set => Value.M44 = value; }

    public Matrix4x4f(float m11, float m12, float m13, float m14, float m21, float m22, float m23, float m24, float m31, float m32, float m33, float m34, float m41, float m42, float m43, float m44)
    {
        Value = new Matrix4x4(
            m11, m12, m13, m14,
            m21, m22, m23, m24,
            m31, m32, m33, m34,
            m41, m42, m43, m44);
    }

    public Matrix4x4f(Matrix4x4 v)
    {
        Value = v;
    }

    public static Matrix4x4f FromDiagonal(float x, float y, float z, float w)
    {
        return new(
            x, 0, 0, 0,
            0, y, 0, 0,
            0, 0, z, 0,
            0, 0, 0, w);
    }

    public static Matrix4x4f FromDiagonal(Vector4f vec)
    {
        return new(
            vec.X, 0, 0, 0,
            0, vec.Y, 0, 0,
            0, 0, vec.Z, 0,
            0, 0, 0, vec.W);
    }

    public static Matrix4x4f Transpose(Matrix4x4f matrix) => Matrix4x4.Transpose(matrix.Value);
    public static bool Invert(Matrix4x4f matrix, out Matrix4x4f result) => Matrix4x4.Invert(matrix, out result.Value);
    public static Vector4f Multiply(Matrix4x4f matrix, Vector4f vec)
    {
        Vector4f result = new()
        {
            X = matrix.M11 * vec.X + matrix.M12 * vec.Y + matrix.M13 * vec.Z + matrix.M14 * vec.W,
            Y = matrix.M21 * vec.X + matrix.M22 * vec.Y + matrix.M23 * vec.Z + matrix.M24 * vec.W,
            Z = matrix.M31 * vec.X + matrix.M32 * vec.Y + matrix.M33 * vec.Z + matrix.M34 * vec.W,
            W = matrix.M41 * vec.X + matrix.M42 * vec.Y + matrix.M43 * vec.Z + matrix.M44 * vec.W,
        };
        return result;
    }
    public static Vector4f Multiply(Vector4f vec, Matrix4x4f matrix) => Vector4.Transform(vec, matrix);
    public static Matrix4x4f Multiply(Matrix4x4f a, Matrix4x4f b) => a.Value * b.Value;

    public static implicit operator Matrix4x4f(Matrix4x4 v) => new(v);
    public static implicit operator Matrix4x4(Matrix4x4f v) => v.Value;

    public static Matrix4x4f operator *(Matrix4x4f a, Matrix4x4f b) => Multiply(a, b);
    public static Vector4f operator *(Matrix4x4f a, Vector4f b) => Multiply(a, b);
    public static Vector4f operator *(Vector4f a, Matrix4x4f b) => Multiply(a, b);

    public override readonly string ToString() => Value.ToString();
    public readonly bool Equals(Matrix4x4f other) => Value.Equals(other.Value);
    public override readonly bool Equals(object? obj) => (obj is Matrix4x4f mat) && Value.Equals(mat.Value);
    public override readonly int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(Matrix4x4f lhs, Matrix4x4f rhs) => lhs.Value == rhs.Value;
    public static bool operator !=(Matrix4x4f lhs, Matrix4x4f rhs) => lhs.Value != rhs.Value;
}
