using System.Numerics;

namespace RadSpec;

public struct Vector3f : IVector<Vector3f, float>
{
    public Vector3 Value;

    public float X { readonly get => Value.X; set => Value.X = value; }
    public float Y { readonly get => Value.Y; set => Value.Y = value; }
    public float Z { readonly get => Value.Z; set => Value.Z = value; }
    public float this[int i] { readonly get => Value[i]; set => Value[i] = value; }

    public static int Count => 3;
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

    public override readonly bool Equals(object? obj) => Value.Equals(obj);
    public readonly bool Equals(Vector3f other) => this == other;
    public override readonly int GetHashCode() => Value.GetHashCode();
    public override readonly string ToString() => Value.ToString();

    public static Vector3f Abs(Vector3f v) => Vector3.Abs(v.Value);
    public static Vector3f Clamp(Vector3f v, Vector3f min, Vector3f max) => Vector3.Clamp(v.Value, min.Value, max.Value);
    public static Vector3f Max(Vector3f x, Vector3f y) => Vector3.Max(x.Value, y.Value);
    public static Vector3f Min(Vector3f x, Vector3f y) => Vector3.Min(x.Value, y.Value);
}
