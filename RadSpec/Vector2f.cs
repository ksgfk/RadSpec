using System.Diagnostics;
using System.Numerics;

namespace RadSpec;

public struct Vector2f : IVector<Vector2f, float>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vector2 Value;

    public float X { readonly get => Value.X; set => Value.X = value; }
    public float Y { readonly get => Value.Y; set => Value.Y = value; }

    public float this[int i] { readonly get => Value[i]; set => Value[i] = value; }

    public static int Count => 2;
    public static Vector2f Zero => new(0);
    public static Vector2f One => new(1);

    public Vector2f(float x, float y)
    {
        Value = new Vector2(x, y);
    }

    public Vector2f(float v)
    {
        Value = new Vector2(v);
    }

    public Vector2f(Vector2 v)
    {
        Value = v;
    }

    public static implicit operator Vector2(Vector2f v) => v.Value;
    public static implicit operator Vector2f(Vector2 v) => new(v);

    public static Vector2f operator +(Vector2f lhs, Vector2f rhs) => new(lhs.Value + rhs.Value);
    public static Vector2f operator -(Vector2f lhs, Vector2f rhs) => new(lhs.Value - rhs.Value);
    public static Vector2f operator *(Vector2f lhs, Vector2f rhs) => new(lhs.Value * rhs.Value);
    public static Vector2f operator /(Vector2f lhs, Vector2f rhs) => new(lhs.Value / rhs.Value);

    public static Vector2f operator +(Vector2f lhs, float rhs) => new(lhs.Value + new Vector2(rhs));
    public static Vector2f operator +(float lhs, Vector2f rhs) => new(new Vector2(lhs) + rhs.Value);

    public static Vector2f operator -(Vector2f lhs, float rhs) => new(lhs.Value - new Vector2(rhs));
    public static Vector2f operator -(float lhs, Vector2f rhs) => new(new Vector2(lhs) - rhs.Value);

    public static Vector2f operator *(Vector2f lhs, float rhs) => new(lhs.Value * new Vector2(rhs));
    public static Vector2f operator *(float lhs, Vector2f rhs) => new(new Vector2(lhs) * rhs.Value);

    public static Vector2f operator /(Vector2f lhs, float rhs) => new(lhs.Value / new Vector2(rhs));
    public static Vector2f operator /(float lhs, Vector2f rhs) => new(new Vector2(lhs) / rhs.Value);

    public static Vector2f operator -(Vector2f v) => -v.Value;

    public static Vector2f operator ++(Vector2f v) => v.Value + new Vector2(1);
    public static Vector2f operator --(Vector2f v) => v.Value - new Vector2(1);

    public static bool operator ==(Vector2f lhs, Vector2f rhs) => lhs.Value == rhs.Value;
    public static bool operator !=(Vector2f lhs, Vector2f rhs) => lhs.Value != rhs.Value;

    public override readonly bool Equals(object? obj) => (obj is Vector2f v) && Value.Equals(v);
    public readonly bool Equals(Vector2f other) => Value.Equals(other.Value);
    public override readonly int GetHashCode() => Value.GetHashCode();
    public override readonly string ToString() => Value.ToString();

    public static Vector2f Abs(Vector2f v) => Vector2.Abs(v.Value);
    public static Vector2f Clamp(Vector2f v, Vector2f min, Vector2f max) => Vector2.Clamp(v.Value, min.Value, max.Value);
    public static Vector2f Max(Vector2f x, Vector2f y) => Vector2.Max(x.Value, y.Value);
    public static Vector2f Min(Vector2f x, Vector2f y) => Vector2.Min(x.Value, y.Value);

    public static Vector2f Floor(Vector2f v) => new(float.Floor(v.X), float.Floor(v.Y));
    public static Vector2f Ceiling(Vector2f v) => new(float.Ceiling(v.X), float.Ceiling(v.Y));
    public static Vector2f Lerp(Vector2f x, Vector2f y, float t) => Vector2.Lerp(x, y, t);
    public static Vector2f Lerp(Vector2f x, Vector2f y, Vector2f t) => new(float.Lerp(x.X, y.X, t.X), float.Lerp(x.Y, y.Y, t.Y));
    public static Vector2f Normalize(Vector2f v) => Vector2.Normalize(v);
    public static Vector2f Sqrt(Vector2f v) => Vector2.SquareRoot(v);
    public static Vector2f Fma(Vector2f a, Vector2f b, Vector2f c) => new(float.FusedMultiplyAdd(a.X, b.X, c.X), float.FusedMultiplyAdd(a.Y, b.Y, c.Y));
    public static Vector2f Fma(float a, Vector2f b, Vector2f c) => new(float.FusedMultiplyAdd(a, b.X, c.X), float.FusedMultiplyAdd(a, b.Y, c.Y));
    public static Vector2f Fma(Vector2f a, float b, Vector2f c) => new(float.FusedMultiplyAdd(a.X, b, c.X), float.FusedMultiplyAdd(a.Y, b, c.Y));
    public static Vector2f Fma(Vector2f a, Vector2f b, float c) => new(float.FusedMultiplyAdd(a.X, b.X, c), float.FusedMultiplyAdd(a.Y, b.Y, c));
    public static Vector2f Fma(float a, float b, Vector2f c) => new(float.FusedMultiplyAdd(a, b, c.X), float.FusedMultiplyAdd(a, b, c.Y));
    public static Vector2f Fma(float a, Vector2f b, float c) => new(float.FusedMultiplyAdd(a, b.X, c), float.FusedMultiplyAdd(a, b.Y, c));
    public static Vector2f Fma(Vector2f a, float b, float c) => new(float.FusedMultiplyAdd(a.X, b, c), float.FusedMultiplyAdd(a.Y, b, c));
    public static float Distance(Vector2f x, Vector2f y) => Vector2.Distance(x, y);
    public static float DistanceSquared(Vector2f x, Vector2f y) => Vector2.DistanceSquared(x, y);
    public static float Dot(Vector2f x, Vector2f y) => Vector2.Dot(x, y);
    public static float AbsDot(Vector2f x, Vector2f y) => float.Abs(Vector2.Dot(x, y));
    public static float Length(Vector2f v) => v.Value.Length();
    public static float LengthSquared(Vector2f v) => v.Value.LengthSquared();
    public static float Sum(Vector2f v) => v.X + v.Y;
    public static float MinElement(Vector2f v) => float.Min(v.X, v.Y);
    public static float MaxElement(Vector2f v) => float.Max(v.X, v.Y);
    public static bool HasNaN(Vector2f v) => float.IsNaN(v.X) || float.IsNaN(v.Y);
    public static bool HasInf(Vector2f v) => float.IsInfinity(v.X) || float.IsInfinity(v.Y);
    public static float Average(Vector2f v) => Sum(v) / Count;

    public readonly Vector2d AsDouble2() => new(X, Y);
    public readonly Vector2i AsInt2() => new((int)X, (int)Y);
}
