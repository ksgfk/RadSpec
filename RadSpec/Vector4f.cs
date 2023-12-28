using System.Numerics;

namespace RadSpec;

public struct Vector4f : IVector<Vector4f, float>
{
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

    public override readonly bool Equals(object? obj) => Value.Equals(obj);
    public readonly bool Equals(Vector4f other) => this == other;
    public override readonly int GetHashCode() => Value.GetHashCode();
    public override readonly string ToString() => Value.ToString();

    public static Vector4f Abs(Vector4f v) => Vector4.Abs(v.Value);
    public static Vector4f Clamp(Vector4f v, Vector4f min, Vector4f max) => Vector4.Clamp(v.Value, min.Value, max.Value);
    public static Vector4f Max(Vector4f x, Vector4f y) => Vector4.Max(x.Value, y.Value);
    public static Vector4f Min(Vector4f x, Vector4f y) => Vector4.Min(x.Value, y.Value);
}
