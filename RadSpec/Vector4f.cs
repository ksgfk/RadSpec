using System.Numerics;

namespace RadSpec;

public struct Vector4f
{
    public Vector4 Value;

    public float X { readonly get => Value.X; set => Value.X = value; }
    public float Y { readonly get => Value.Y; set => Value.Y = value; }
    public float Z { readonly get => Value.Z; set => Value.Z = value; }
    public float W { readonly get => Value.W; set => Value.W = value; }
    public float this[int i] { readonly get => Value[i]; set => Value[i] = value; }

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
    public override readonly bool Equals(object? obj) => Value.Equals(obj);
    public override readonly int GetHashCode() => Value.GetHashCode();
    public override readonly string ToString() => Value.ToString();
}
