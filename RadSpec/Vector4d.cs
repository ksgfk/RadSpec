using System.Numerics;

namespace RadSpec;

public struct Vector4d
{
    public double X, Y, Z, W;

    public Vector4d(double x, double y, double z, double w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public Vector4d(double value)
    {
        X = value;
        Y = value;
        Z = value;
        W = value;
    }

    public Vector4d(Vector4 v)
    {
        X = v.X;
        Y = v.Y;
        Z = v.Z;
        W = v.W;
    }

    public readonly Vector4f ToFloat4() => new((float)X, (float)Y, (float)Z, (float)W);

    public static Vector4d operator +(Vector4d lhs, Vector4d rhs) => new(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z, lhs.W + rhs.W);
    public static Vector4d operator -(Vector4d lhs, Vector4d rhs) => new(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z, lhs.W - rhs.W);
    public static Vector4d operator *(Vector4d lhs, Vector4d rhs) => new(lhs.X * rhs.X, lhs.Y * rhs.Y, lhs.Z * rhs.Z, lhs.W * rhs.W);
    public static Vector4d operator /(Vector4d lhs, Vector4d rhs) => new(lhs.X / rhs.X, lhs.Y / rhs.Y, lhs.Z / rhs.Z, lhs.W / rhs.W);

    public static Vector4d operator +(Vector4d lhs, double rhs) => new(lhs.X + rhs, lhs.Y + rhs, lhs.Z + rhs, lhs.W + rhs);
    public static Vector4d operator +(double lhs, Vector4d rhs) => new(lhs + rhs.X, lhs + rhs.Y, lhs + rhs.Z, lhs + rhs.W);

    public static Vector4d operator -(Vector4d lhs, double rhs) => new(lhs.X - rhs, lhs.Y - rhs, lhs.Z - rhs, lhs.W - rhs);
    public static Vector4d operator -(double lhs, Vector4d rhs) => new(lhs - rhs.X, lhs - rhs.Y, lhs - rhs.Z, lhs - rhs.W);

    public static Vector4d operator *(Vector4d lhs, double rhs) => new(lhs.X * rhs, lhs.Y * rhs, lhs.Z * rhs, lhs.W * rhs);
    public static Vector4d operator *(double lhs, Vector4d rhs) => new(lhs * rhs.X, lhs * rhs.Y, lhs * rhs.Z, lhs * rhs.W);

    public static Vector4d operator /(Vector4d lhs, double rhs) => new(lhs.X / rhs, lhs.Y / rhs, lhs.Z / rhs, lhs.W / rhs);
    public static Vector4d operator /(double lhs, Vector4d rhs) => new(lhs / rhs.X, lhs / rhs.Y, lhs / rhs.Z, lhs / rhs.W);

    public static bool operator ==(Vector4d lhs, Vector4d rhs) => lhs.X == rhs.X && lhs.Y == rhs.Y && lhs.Z == rhs.Z && lhs.W == rhs.W;
    public static bool operator !=(Vector4d lhs, Vector4d rhs) => lhs.X != rhs.X || lhs.Y != rhs.Y || lhs.Z != rhs.Z || lhs.W != rhs.W;

    public override readonly bool Equals(object? obj) => (obj is Vector4d v) && this == v;
    public override readonly int GetHashCode() => HashCode.Combine(X, Y, Z, W);
    public override readonly string ToString() => $"<{X}, {Y}, {Z}, {W}>";
}
