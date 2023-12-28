using System.Numerics;

namespace RadSpec;

public struct Vector3d
{
    public double X, Y, Z;

    public Vector3d(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Vector3d(double value)
    {
        X = value;
        Y = value;
        Z = value;
    }

    public Vector3d(Vector3 v)
    {
        X = v.X;
        Y = v.Y;
        Z = v.Z;
    }

    public readonly Vector3f ToFloat3() => new((float)X, (float)Y, (float)Z);

    public static Vector3d operator +(Vector3d lhs, Vector3d rhs) => new(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z);
    public static Vector3d operator -(Vector3d lhs, Vector3d rhs) => new(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z);
    public static Vector3d operator *(Vector3d lhs, Vector3d rhs) => new(lhs.X * rhs.X, lhs.Y * rhs.Y, lhs.Z * rhs.Z);
    public static Vector3d operator /(Vector3d lhs, Vector3d rhs) => new(lhs.X / rhs.X, lhs.Y / rhs.Y, lhs.Z / rhs.Z);

    public static Vector3d operator +(Vector3d lhs, double rhs) => new(lhs.X + rhs, lhs.Y + rhs, lhs.Z + rhs);
    public static Vector3d operator +(double lhs, Vector3d rhs) => new(lhs + rhs.X, lhs + rhs.Y, lhs + rhs.Z);

    public static Vector3d operator -(Vector3d lhs, double rhs) => new(lhs.X - rhs, lhs.Y - rhs, lhs.Z - rhs);
    public static Vector3d operator -(double lhs, Vector3d rhs) => new(lhs - rhs.X, lhs - rhs.Y, lhs - rhs.Z);

    public static Vector3d operator *(Vector3d lhs, double rhs) => new(lhs.X * rhs, lhs.Y * rhs, lhs.Z * rhs);
    public static Vector3d operator *(double lhs, Vector3d rhs) => new(lhs * rhs.X, lhs * rhs.Y, lhs * rhs.Z);

    public static Vector3d operator /(Vector3d lhs, double rhs) => new(lhs.X / rhs, lhs.Y / rhs, lhs.Z / rhs);
    public static Vector3d operator /(double lhs, Vector3d rhs) => new(lhs / rhs.X, lhs / rhs.Y, lhs / rhs.Z);

    public static bool operator ==(Vector3d lhs, Vector3d rhs) => lhs.X == rhs.X && lhs.Y == rhs.Y && lhs.Z == rhs.Z;
    public static bool operator !=(Vector3d lhs, Vector3d rhs) => lhs.X != rhs.X || lhs.Y != rhs.Y || lhs.Z != rhs.Z;

    public override readonly bool Equals(object? obj) => (obj is Vector3d v) && this == v;
    public override readonly int GetHashCode() => HashCode.Combine(X, Y, Z);
    public override readonly string ToString() => $"<{X}, {Y}, {Z}>";
}
