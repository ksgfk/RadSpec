namespace RadSpec;

public readonly struct Transform4f
{
    private readonly Matrix4x4f _mat;
    private readonly Matrix4x4f _inv;

    public Matrix4x4f Matrix => _mat;
    public Matrix4x4f InvMatrix => _inv;

    public Vector3f Translation => new(_mat.M14, _mat.M24, _mat.M34);

    public Transform4f(Matrix4x4f mat, Matrix4x4f inv)
    {
        _mat = mat;
        _inv = inv;
    }

    public Vector3f ApplyAffine(Vector3f v)
    {
        Vector4f t = _mat * new Vector4f(v, 1);
        return t.XYZ / t.W;
    }

    public Vector3f ApplyLinear(Vector3f v)
    {
        return new Vector3f(
            _mat.M11 * v.X + _mat.M12 * v.Y + _mat.M13 * v.Z,
            _mat.M21 * v.X + _mat.M22 * v.Y + _mat.M23 * v.Z,
            _mat.M31 * v.X + _mat.M32 * v.Y + _mat.M33 * v.Z);
    }

    public static Transform4f Translate(float x, float y, float z)
    {
        Matrix4x4f mat = new(
            1, 0, 0, x,
            0, 1, 0, y,
            0, 0, 1, z,
            0, 0, 0, 1);
        Matrix4x4f inv = new(
            1, 0, 0, -x,
            0, 1, 0, -y,
            0, 0, 1, -z,
            0, 0, 0, 1);
        return new Transform4f(mat, inv);
    }

    public static Transform4f Translate(Vector3f v)
    {
        Matrix4x4f mat = new(
            1, 0, 0, v.X,
            0, 1, 0, v.Y,
            0, 0, 1, v.Z,
            0, 0, 0, 1);
        Matrix4x4f inv = new(
            1, 0, 0, -v.X,
            0, 1, 0, -v.Y,
            0, 0, 1, -v.Z,
            0, 0, 0, 1);
        return new Transform4f(mat, inv);
    }

    public static Transform4f Scale(float x, float y, float z)
    {
        Matrix4x4f mat = FromDiagonal(new Vector4f(x, y, z, 1));
        Matrix4x4f inv = FromDiagonal(new Vector4f(1 / x, 1 / y, 1 / z, 1));
        return new Transform4f(mat, inv);
    }

    public static Transform4f Scale(Vector3f v)
    {
        Matrix4x4f mat = FromDiagonal(new Vector4f(v, 1));
        Matrix4x4f inv = FromDiagonal(new Vector4f(1 / v, 1));
        return new Transform4f(mat, inv);
    }

    public static Transform4f Perspective(float fov, float near, float far)
    {
        float recip = 1 / (far - near);
        float tan = Tan(fov / 2);
        float cot = 1 / tan;
        Matrix4x4f mat = new(
            cot, 0, 0, 0,
            0, cot, 0, 0,
            0, 0, far * recip, -near * far * recip,
            0, 0, 1, 0);
        Matrix4x4f inv = new(
            tan, 0, 0, 0,
            0, tan, 0, 0,
            0, 0, 0, 1,
            0, 0, (near - far) / (far * near), 1 / near);
        return new Transform4f(mat, inv);
    }

    public static Transform4f LookAt(Vector3f pos, Vector3f target, Vector3f up)
    {
        Vector3f dir = Normalize(target - pos);
        Vector3f right = Normalize(Cross(Normalize(up), dir));
        Vector3f newUp = Cross(dir, right);
        Matrix4x4f mat = new(
            right.X, newUp.X, dir.X, pos.X,
            right.Y, newUp.Y, dir.Y, pos.Y,
            right.Z, newUp.Z, dir.Z, pos.Z,
            0, 0, 0, 1);
        Matrix4x4f.Invert(mat, out Matrix4x4f inv);
        return new Transform4f(inv, mat);
    }

    public static Transform4f Multiply(Transform4f a, Transform4f b) => new(a.Matrix * b.Matrix, b.InvMatrix * a.InvMatrix); //注意到逆矩阵是反过来乘的

    public static Transform4f Invert(Transform4f v) => new(v._inv, v._mat);

    public static Transform4f operator *(Transform4f a, Transform4f b) => Multiply(a, b);
}
