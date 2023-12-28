using System.Numerics;

namespace RadSpec;

public struct Vector2d
{
    public double X, Y;

    public Vector2d(double x, double y)
    {
        X = x;
        Y = y;
    }

    public Vector2d(double value)
    {
        X = value;
        Y = value;
    }

    public Vector2d(Vector2 v)
    {
        X = v.X;
        Y = v.Y;
    }

    public readonly Vector2f ToFloat2() => new((float)X, (float)Y);

    public static Vector2d operator +(Vector2d lhs, Vector2d rhs) => new(lhs.X + rhs.X, lhs.Y + rhs.Y);
    public static Vector2d operator -(Vector2d lhs, Vector2d rhs) => new(lhs.X - rhs.X, lhs.Y - rhs.Y);
    public static Vector2d operator *(Vector2d lhs, Vector2d rhs) => new(lhs.X * rhs.X, lhs.Y * rhs.Y);
    public static Vector2d operator /(Vector2d lhs, Vector2d rhs) => new(lhs.X / rhs.X, lhs.Y / rhs.Y);

    public static Vector2d operator +(Vector2d lhs, double rhs) => new(lhs.X + rhs, lhs.Y + rhs);
    public static Vector2d operator +(double lhs, Vector2d rhs) => new(lhs + rhs.X, lhs + rhs.Y);

    public static Vector2d operator -(Vector2d lhs, double rhs) => new(lhs.X - rhs, lhs.Y - rhs);
    public static Vector2d operator -(double lhs, Vector2d rhs) => new(lhs - rhs.X, lhs - rhs.Y);

    public static Vector2d operator *(Vector2d lhs, double rhs) => new(lhs.X * rhs, lhs.Y * rhs);
    public static Vector2d operator *(double lhs, Vector2d rhs) => new(lhs * rhs.X, lhs * rhs.Y);

    public static Vector2d operator /(Vector2d lhs, double rhs) => new(lhs.X / rhs, lhs.Y / rhs);
    public static Vector2d operator /(double lhs, Vector2d rhs) => new(lhs / rhs.X, lhs / rhs.Y);

    public static bool operator ==(Vector2d lhs, Vector2d rhs) => lhs.X == rhs.X && lhs.Y == rhs.Y;
    public static bool operator !=(Vector2d lhs, Vector2d rhs) => lhs.X != rhs.X || lhs.Y != rhs.Y;

    public override readonly bool Equals(object? obj) => (obj is Vector2d v) && this == v;
    public override readonly int GetHashCode() => HashCode.Combine(X, Y);
    public override readonly string ToString() => $"<{X}, {Y}>";
}
