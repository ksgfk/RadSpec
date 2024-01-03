using System.Numerics;

namespace RadSpec;

public struct Vector3f
{
    public Vector3 Value;

    public float X { readonly get => Value.X; set => Value.X = value; }
    public float Y { readonly get => Value.Y; set => Value.Y = value; }
    public float Z { readonly get => Value.Z; set => Value.Z = value; }
    public float this[int i] { readonly get => Value[i]; set => Value[i] = value; }

    public const int Count = 3;
    public static Vector3f Zero => new(0);
    public static Vector3f One => new(1);

    public Vector3f(float x, float y, float z)
    {
        Value = new Vector3(x, y, z);
    }

    public Vector3f(float v)
    {
        Value = new Vector3(v);
    }

    public Vector3f(Vector3 v)
    {
        Value = v;
    }

    public Vector3f(Vector2f xy, float z)
    {
        Value = new Vector3(xy.Value, z);
    }

    public Vector3f(float x, Vector2f yz)
    {
        Value = new Vector3(x, yz.X, yz.Y);
    }

    public static implicit operator Vector3(Vector3f v) => v.Value;
    public static implicit operator Vector3f(Vector3 v) => new(v);

    public static Vector3f operator +(Vector3f lhs, Vector3f rhs) => new(lhs.Value + rhs.Value);
    public static Vector3f operator -(Vector3f lhs, Vector3f rhs) => new(lhs.Value - rhs.Value);
    public static Vector3f operator *(Vector3f lhs, Vector3f rhs) => new(lhs.Value * rhs.Value);
    public static Vector3f operator /(Vector3f lhs, Vector3f rhs) => new(lhs.Value / rhs.Value);

    public static Vector3f operator +(Vector3f lhs, float rhs) => new(lhs.Value + new Vector3(rhs));
    public static Vector3f operator +(float lhs, Vector3f rhs) => new(new Vector3(lhs) + rhs.Value);

    public static Vector3f operator -(Vector3f lhs, float rhs) => new(lhs.Value - new Vector3(rhs));
    public static Vector3f operator -(float lhs, Vector3f rhs) => new(new Vector3(lhs) - rhs.Value);

    public static Vector3f operator *(Vector3f lhs, float rhs) => new(lhs.Value * new Vector3(rhs));
    public static Vector3f operator *(float lhs, Vector3f rhs) => new(new Vector3(lhs) * rhs.Value);

    public static Vector3f operator /(Vector3f lhs, float rhs) => new(lhs.Value / new Vector3(rhs));
    public static Vector3f operator /(float lhs, Vector3f rhs) => new(new Vector3(lhs) / rhs.Value);

    public static bool operator ==(Vector3f lhs, Vector3f rhs) => lhs.Value == rhs.Value;
    public static bool operator !=(Vector3f lhs, Vector3f rhs) => lhs.Value != rhs.Value;

    public static Vector3f operator -(Vector3f v) => -v.Value;

    public static Vector3f operator ++(Vector3f v) => v.Value + new Vector3(1);
    public static Vector3f operator --(Vector3f v) => v.Value - new Vector3(1);

    public override readonly bool Equals(object? obj) => (obj is Vector3f v) && Value.Equals(v);
    public readonly bool Equals(Vector3f other) => Value.Equals(other.Value);
    public override readonly int GetHashCode() => Value.GetHashCode();
    public override readonly string ToString() => Value.ToString();

    public static Vector3f Abs(Vector3f v) => Vector3.Abs(v.Value);
    public static Vector3f Clamp(Vector3f v, Vector3f min, Vector3f max) => Vector3.Clamp(v.Value, min.Value, max.Value);
    public static Vector3f Max(Vector3f x, Vector3f y) => Vector3.Max(x.Value, y.Value);
    public static Vector3f Min(Vector3f x, Vector3f y) => Vector3.Min(x.Value, y.Value);

    public static Vector3f Floor(Vector3f v) => new(float.Floor(v.X), float.Floor(v.Y), float.Floor(v.Z));
    public static Vector3f Ceiling(Vector3f v) => new(float.Ceiling(v.X), float.Ceiling(v.Y), float.Ceiling(v.Z));
    public static Vector3f Lerp(Vector3f x, Vector3f y, float t) => Vector3.Lerp(x, y, t);
    public static Vector3f Lerp(Vector3f x, Vector3f y, Vector3f t) => new(float.Lerp(x.X, y.X, t.X), float.Lerp(x.Y, y.Y, t.Y), float.Lerp(x.Z, y.Z, t.Z));
    public static Vector3f Normalize(Vector3f v) => Vector3.Normalize(v);
    public static Vector3f Sqrt(Vector3f v) => Vector3.SquareRoot(v);
    public static Vector3f Fma(Vector3f a, Vector3f b, Vector3f c) => new(float.FusedMultiplyAdd(a.X, b.X, c.X), float.FusedMultiplyAdd(a.Y, b.Y, c.Y), float.FusedMultiplyAdd(a.Z, b.Z, c.Z));
    public static Vector3f Fma(float a, Vector3f b, Vector3f c) => new(float.FusedMultiplyAdd(a, b.X, c.X), float.FusedMultiplyAdd(a, b.Y, c.Y), float.FusedMultiplyAdd(a, b.Z, c.Z));
    public static Vector3f Fma(Vector3f a, float b, Vector3f c) => new(float.FusedMultiplyAdd(a.X, b, c.X), float.FusedMultiplyAdd(a.Y, b, c.Y), float.FusedMultiplyAdd(a.Z, b, c.Z));
    public static Vector3f Fma(Vector3f a, Vector3f b, float c) => new(float.FusedMultiplyAdd(a.X, b.X, c), float.FusedMultiplyAdd(a.Y, b.Y, c), float.FusedMultiplyAdd(a.Z, b.Z, c));
    public static Vector3f Fma(float a, float b, Vector3f c) => new(float.FusedMultiplyAdd(a, b, c.X), float.FusedMultiplyAdd(a, b, c.Y), float.FusedMultiplyAdd(a, b, c.Z));
    public static Vector3f Fma(float a, Vector3f b, float c) => new(float.FusedMultiplyAdd(a, b.X, c), float.FusedMultiplyAdd(a, b.Y, c), float.FusedMultiplyAdd(a, b.Z, c));
    public static Vector3f Fma(Vector3f a, float b, float c) => new(float.FusedMultiplyAdd(a.X, b, c), float.FusedMultiplyAdd(a.Y, b, c), float.FusedMultiplyAdd(a.Z, b, c));
    public static float Distance(Vector3f x, Vector3f y) => Vector3.Distance(x, y);
    public static float DistanceSquared(Vector3f x, Vector3f y) => Vector3.DistanceSquared(x, y);
    public static float Dot(Vector3f x, Vector3f y) => Vector3.Dot(x, y);
    public static float AbsDot(Vector3f x, Vector3f y) => float.Abs(Vector3.Dot(x, y));
    public static float Length(Vector3f v) => v.Value.Length();
    public static float LengthSquared(Vector3f v) => v.Value.LengthSquared();
    public static float Sum(Vector3f v) => v.X + v.Y + v.Z;
    public static float MinElement(Vector3f v) => float.Min(float.Min(v.X, v.Y), v.Z);
    public static float MaxElement(Vector3f v) => float.Max(float.Max(v.X, v.Y), v.Z);
    public static bool HasNaN(Vector3f v) => float.IsNaN(v.X) || float.IsNaN(v.Y) || float.IsNaN(v.Z);
    public static bool HasInf(Vector3f v) => float.IsInfinity(v.X) || float.IsInfinity(v.Y) || float.IsInfinity(v.Z);

    public static Vector3f Cross(Vector3f x, Vector3f y) => Vector3.Cross(x.Value, y.Value);

    public readonly Vector3d AsDouble3() => new(X, Y, Z);
    public readonly Vector3i AsInt3() => new((int)X, (int)Y, (int)Z);

    public readonly Vector2f XY => new(X, Y);
}
