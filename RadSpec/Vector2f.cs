using System.Numerics;

namespace RadSpec;

public struct Vector2f
{
    public Vector2 Value;

    public float X { readonly get => Value.X; set => Value.X = value; }
    public float Y { readonly get => Value.Y; set => Value.Y = value; }
    public float this[int i] { readonly get => Value[i]; set => Value[i] = value; }

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

    public static bool operator ==(Vector2f lhs, Vector2f rhs) => lhs.Value == rhs.Value;
    public static bool operator !=(Vector2f lhs, Vector2f rhs) => lhs.Value != rhs.Value;
    public override readonly bool Equals(object? obj) => Value.Equals(obj);
    public override readonly int GetHashCode() => Value.GetHashCode();
    public override readonly string ToString() => Value.ToString();
}
