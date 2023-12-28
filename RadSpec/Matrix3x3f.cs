namespace RadSpec;

public struct Matrix3x3f
{
    public float M11;
    public float M12;
    public float M13;
    public float M21;
    public float M22;
    public float M23;
    public float M31;
    public float M32;
    public float M33;

    public Matrix3x3f(float m11, float m12, float m13, float m21, float m22, float m23, float m31, float m32, float m33)
    {
        M11 = m11;
        M12 = m12;
        M13 = m13;
        M21 = m21;
        M22 = m22;
        M23 = m23;
        M31 = m31;
        M32 = m32;
        M33 = m33;
    }

    public static Matrix3x3f FromDiagonal(float x, float y, float z)
    {
        return new(
            x, 0, 0,
            0, y, 0,
            0, 0, z);
    }

    public static Matrix3x3f FromDiagonal(Vector3f vec)
    {
        return new(
            vec.X, 0, 0,
            0, vec.Y, 0,
            0, 0, vec.Z);
    }

    public static Matrix3x3f Transpose(Matrix3x3f matrix)
    {
        return new Matrix3x3f(
            matrix.M11, matrix.M21, matrix.M31,
            matrix.M12, matrix.M22, matrix.M32,
            matrix.M13, matrix.M23, matrix.M33);
    }

    public static bool Invert(Matrix3x3f matrix, out Matrix3x3f result)
    {
        // 逆矩阵 = 伴随矩阵 / 矩阵的行列式
        float a = matrix.M11, b = matrix.M12, c = matrix.M13;
        float d = matrix.M21, e = matrix.M22, f = matrix.M23;
        float g = matrix.M31, h = matrix.M32, i = matrix.M33;

        // 代数余子式
        float c11 = e * i - f * h;
        float c12 = -(d * i - f * g);
        float c13 = d * h - e * g;
        float c21 = -(b * i - c * h);
        float c22 = a * i - g * c;
        float c23 = -(a * h - g * b);
        float c31 = b * f - c * e;
        float c32 = -(a * f - c * d);
        float c33 = a * e - b * d;

        // 矩阵的行列式
        float det = a * c11 + b * c12 + c * c13;

        if (MathF.Abs(det) < float.Epsilon)
        {
            result = new Matrix3x3f(float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN);
            return false;
        }

        float invDet = 1 / det;
        result = new Matrix3x3f(
            c11 * invDet, c21 * invDet, c31 * invDet,
            c12 * invDet, c22 * invDet, c32 * invDet,
            c13 * invDet, c23 * invDet, c33 * invDet);
        return true;
    }

    public static Vector3f Mul(Matrix3x3f matrix, Vector3f vec)
    {
        Vector3f result = new()
        {
            X = matrix.M11 * vec.X + matrix.M12 * vec.Y + matrix.M13 * vec.Z,
            Y = matrix.M21 * vec.X + matrix.M22 * vec.Y + matrix.M23 * vec.Z,
            Z = matrix.M31 * vec.X + matrix.M32 * vec.Y + matrix.M33 * vec.Z
        };
        return result;
    }

    public static Matrix3x3f Mul(Matrix3x3f l, Matrix3x3f r)
    {
        Matrix3x3f result = new()
        {
            M11 = l.M11 * r.M11 + l.M12 * r.M21 + l.M13 * r.M31,
            M12 = l.M11 * r.M12 + l.M12 * r.M22 + l.M13 * r.M32,
            M13 = l.M11 * r.M13 + l.M12 * r.M23 + l.M13 * r.M33,

            M21 = l.M21 * r.M11 + l.M22 * r.M21 + l.M23 * r.M31,
            M22 = l.M21 * r.M12 + l.M22 * r.M22 + l.M23 * r.M32,
            M23 = l.M21 * r.M13 + l.M22 * r.M23 + l.M23 * r.M33,

            M31 = l.M31 * r.M11 + l.M32 * r.M21 + l.M33 * r.M31,
            M32 = l.M31 * r.M12 + l.M32 * r.M22 + l.M33 * r.M32,
            M33 = l.M31 * r.M13 + l.M32 * r.M23 + l.M33 * r.M33,
        };
        return result;
    }

    public override readonly string ToString()
    {
        return $"{{ {{M11:{M11} M12:{M12} M13:{M13}}} {{M21:{M21} M22:{M22} M23:{M23}}} {{M31:{M31} M32:{M32} M33:{M33}}} }}";
    }
}
