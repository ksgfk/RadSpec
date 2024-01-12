using System.Diagnostics;
using System.Numerics;

namespace RadSpec;

public struct Vector4f : IVector<Vector4f, float>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector4 Value;

    public float X { readonly get => Value.X; set => Value.X = value; }
    public float Y { readonly get => Value.Y; set => Value.Y = value; }
    public float Z { readonly get => Value.Z; set => Value.Z = value; }
    public float W { readonly get => Value.W; set => Value.W = value; }
    public float this[int i] { readonly get => Value[i]; set => Value[i] = value; }

    public static int Count => 4;
    public static Vector4f Zero => new(0);
    public static Vector4f One => new(1);

    public Vector4f(float x, float y, float z, float w)
    {
        Value = new Vector4(x, y, z, w);
    }

    public Vector4f(float v)
    {
        Value = new Vector4(v);
    }

    public Vector4f(Vector4 v)
    {
        Value = v;
    }

    public Vector4f(Vector2f xy, float z, float w)
    {
        Value = new Vector4(xy.Value, z, w);
    }

    public Vector4f(float x, Vector2f yz, float w)
    {
        Value = new Vector4(x, yz.X, yz.Y, w);
    }

    public Vector4f(float x, float y, Vector2f zw)
    {
        Value = new Vector4(x, y, zw.X, zw.Y);
    }

    public Vector4f(Vector2f xy, Vector2f zw)
    {
        Value = new Vector4(xy.Value, zw.X, zw.Y);
    }

    public Vector4f(Vector3f xyz, float w)
    {
        Value = new Vector4(xyz.Value, w);
    }

    public Vector4f(float x, Vector3f yzw)
    {
        Value = new Vector4(x, yzw.X, yzw.Y, yzw.Z);
    }

    public static implicit operator Vector4(Vector4f v) => v.Value;
    public static implicit operator Vector4f(Vector4 v) => new(v);

    public static Vector4f operator +(Vector4f lhs, Vector4f rhs) => new(lhs.Value + rhs.Value);
    public static Vector4f operator -(Vector4f lhs, Vector4f rhs) => new(lhs.Value - rhs.Value);
    public static Vector4f operator *(Vector4f lhs, Vector4f rhs) => new(lhs.Value * rhs.Value);
    public static Vector4f operator /(Vector4f lhs, Vector4f rhs) => new(lhs.Value / rhs.Value);

    public static Vector4f operator +(Vector4f lhs, float rhs) => new(lhs.Value + new Vector4(rhs));
    public static Vector4f operator +(float lhs, Vector4f rhs) => new(new Vector4(lhs) + rhs.Value);

    public static Vector4f operator -(Vector4f lhs, float rhs) => new(lhs.Value - new Vector4(rhs));
    public static Vector4f operator -(float lhs, Vector4f rhs) => new(new Vector4(lhs) - rhs.Value);

    public static Vector4f operator *(Vector4f lhs, float rhs) => new(lhs.Value * new Vector4(rhs));
    public static Vector4f operator *(float lhs, Vector4f rhs) => new(new Vector4(lhs) * rhs.Value);

    public static Vector4f operator /(Vector4f lhs, float rhs) => new(lhs.Value / new Vector4(rhs));
    public static Vector4f operator /(float lhs, Vector4f rhs) => new(new Vector4(lhs) / rhs.Value);

    public static bool operator ==(Vector4f lhs, Vector4f rhs) => lhs.Value == rhs.Value;
    public static bool operator !=(Vector4f lhs, Vector4f rhs) => lhs.Value != rhs.Value;

    public static Vector4f operator -(Vector4f v) => -v.Value;

    public static Vector4f operator ++(Vector4f v) => v.Value + new Vector4(1);
    public static Vector4f operator --(Vector4f v) => v.Value - new Vector4(1);

    public override readonly bool Equals(object? obj) => (obj is Vector4f v) && Value.Equals(v);
    public readonly bool Equals(Vector4f other) => Value.Equals(other.Value);
    public override readonly int GetHashCode() => Value.GetHashCode();
    public override readonly string ToString() => Value.ToString();

    public static Vector4f Abs(Vector4f v) => Vector4.Abs(v.Value);
    public static Vector4f Clamp(Vector4f v, Vector4f min, Vector4f max) => Vector4.Clamp(v.Value, min.Value, max.Value);
    public static Vector4f Max(Vector4f x, Vector4f y) => Vector4.Max(x.Value, y.Value);
    public static Vector4f Min(Vector4f x, Vector4f y) => Vector4.Min(x.Value, y.Value);

    public static Vector4f Floor(Vector4f v) => new(float.Floor(v.X), float.Floor(v.Y), float.Floor(v.Z), float.Floor(v.W));
    public static Vector4f Ceiling(Vector4f v) => new(float.Ceiling(v.X), float.Ceiling(v.Y), float.Ceiling(v.Z), float.Ceiling(v.W));
    public static Vector4f Lerp(Vector4f x, Vector4f y, float t) => Vector4.Lerp(x, y, t);
    public static Vector4f Lerp(Vector4f x, Vector4f y, Vector4f t) => new(float.Lerp(x.X, y.X, t.X), float.Lerp(x.Y, y.Y, t.Y), float.Lerp(x.Z, y.Z, t.Z), float.Lerp(x.W, y.W, t.W));
    public static Vector4f Normalize(Vector4f v) => Vector4.Normalize(v);
    public static Vector4f Sqrt(Vector4f v) => Vector4.SquareRoot(v);
    public static Vector4f Fma(Vector4f a, Vector4f b, Vector4f c) => new(float.FusedMultiplyAdd(a.X, b.X, c.X), float.FusedMultiplyAdd(a.Y, b.Y, c.Y), float.FusedMultiplyAdd(a.Z, b.Z, c.Z), float.FusedMultiplyAdd(a.W, b.W, c.W));
    public static Vector4f Fma(float a, Vector4f b, Vector4f c) => new(float.FusedMultiplyAdd(a, b.X, c.X), float.FusedMultiplyAdd(a, b.Y, c.Y), float.FusedMultiplyAdd(a, b.Z, c.Z), float.FusedMultiplyAdd(a, b.W, c.W));
    public static Vector4f Fma(Vector4f a, float b, Vector4f c) => new(float.FusedMultiplyAdd(a.X, b, c.X), float.FusedMultiplyAdd(a.Y, b, c.Y), float.FusedMultiplyAdd(a.Z, b, c.Z), float.FusedMultiplyAdd(a.W, b, c.W));
    public static Vector4f Fma(Vector4f a, Vector4f b, float c) => new(float.FusedMultiplyAdd(a.X, b.X, c), float.FusedMultiplyAdd(a.Y, b.Y, c), float.FusedMultiplyAdd(a.Z, b.Z, c), float.FusedMultiplyAdd(a.W, b.W, c));
    public static Vector4f Fma(float a, float b, Vector4f c) => new(float.FusedMultiplyAdd(a, b, c.X), float.FusedMultiplyAdd(a, b, c.Y), float.FusedMultiplyAdd(a, b, c.Z), float.FusedMultiplyAdd(a, b, c.W));
    public static Vector4f Fma(float a, Vector4f b, float c) => new(float.FusedMultiplyAdd(a, b.X, c), float.FusedMultiplyAdd(a, b.Y, c), float.FusedMultiplyAdd(a, b.Z, c), float.FusedMultiplyAdd(a, b.W, c));
    public static Vector4f Fma(Vector4f a, float b, float c) => new(float.FusedMultiplyAdd(a.X, b, c), float.FusedMultiplyAdd(a.Y, b, c), float.FusedMultiplyAdd(a.Z, b, c), float.FusedMultiplyAdd(a.W, b, c));
    public static float Distance(Vector4f x, Vector4f y) => Vector4.Distance(x, y);
    public static float DistanceSquared(Vector4f x, Vector4f y) => Vector4.DistanceSquared(x, y);
    public static float Dot(Vector4f x, Vector4f y) => Vector4.Dot(x, y);
    public static float AbsDot(Vector4f x, Vector4f y) => float.Abs(Vector4.Dot(x, y));
    public static float Length(Vector4f v) => v.Value.Length();
    public static float LengthSquared(Vector4f v) => v.Value.LengthSquared();
    public static float Sum(Vector4f v) => v.X + v.Y + v.Z + v.W;
    public static float MinElement(Vector4f v) => float.Min(float.Min(float.Min(v.X, v.Y), v.Z), v.W);
    public static float MaxElement(Vector4f v) => float.Max(float.Max(float.Max(v.X, v.Y), v.Z), v.W);
    public static bool HasNaN(Vector4f v) => float.IsNaN(v.X) || float.IsNaN(v.Y) || float.IsNaN(v.Z) || float.IsNaN(v.W);
    public static bool HasInf(Vector4f v) => float.IsInfinity(v.X) || float.IsInfinity(v.Y) || float.IsInfinity(v.Z) || float.IsInfinity(v.W);
    public static float Average(Vector4f v) => Sum(v) / Count;

    public readonly Vector4d AsDouble4() => new(X, Y, Z, W);
    public readonly Vector4i AsInt4() => new((int)X, (int)Y, (int)Z, (int)W);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Vector3f XYZ => new(X, Y, Z);
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Vector2f XY => new(X, Y);
}
