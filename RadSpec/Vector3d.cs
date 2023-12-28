using System.Numerics;
using System.Runtime.CompilerServices;

namespace RadSpec;

public struct Vector3d : IVector<Vector3d, double>
{
    public double X, Y, Z;

    public double this[int i]
    {
        readonly get => this.GetElement(i);
        set => this = this.WithElement(i, value);
    }

    public static int Count => 4;
    public static Vector3d Zero => new(0);
    public static Vector3d One => new(1);

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

    public static Vector3d operator -(Vector3d v) => new(-v.X, -v.Y, -v.Z);

    public static Vector3d operator ++(Vector3d v) => new(v.X + 1, v.Y + 1, v.Z + 1);
    public static Vector3d operator --(Vector3d v) => new(v.X - 1, v.Y - 1, v.Z - 1);

    public override readonly bool Equals(object? obj) => (obj is Vector3d v) && this == v;
    public readonly bool Equals(Vector3d other) => this == other;
    public override readonly int GetHashCode() => HashCode.Combine(X, Y, Z);
    public override readonly string ToString() => $"<{X}, {Y}, {Z}>";

    public static Vector3d Abs(Vector3d v) => new(Math.Abs(v.X), Math.Abs(v.Y), Math.Abs(v.Z));
    public static Vector3d Clamp(Vector3d v, Vector3d min, Vector3d max) => new(Math.Clamp(v.X, min.X, max.X), Math.Clamp(v.Y, min.Y, max.Y), Math.Clamp(v.Z, min.Z, max.Z));
    public static Vector3d Max(Vector3d x, Vector3d y) => new(Math.Max(x.X, y.X), Math.Max(x.Y, y.Y), Math.Max(x.Z, y.Z));
    public static Vector3d Min(Vector3d x, Vector3d y) => new(Math.Min(x.X, y.X), Math.Min(x.Y, y.Y), Math.Min(x.Z, y.Z));
}

public static partial class VectorExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static double GetElement(this Vector3d vector, int index)
    {
        if ((uint)index >= (uint)Vector3d.Count)
        {
            ThrowUtility.ArgumentOutOfRange(nameof(index));
        }
        return vector.GetElementUnsafe(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector3d WithElement(this Vector3d vector, int index, double value)
    {
        if ((uint)index >= (uint)Vector3d.Count)
        {
            ThrowUtility.ArgumentOutOfRange(nameof(index));
        }
        Vector3d result = vector;
        result.SetElementUnsafe(index, value);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double GetElementUnsafe(in this Vector3d vector, int index)
    {
        ref double address = ref Unsafe.AsRef(in vector.X);
        return Unsafe.Add(ref address, index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SetElementUnsafe(ref this Vector3d vector, int index, double value)
    {
        Unsafe.Add(ref vector.X, index) = value;
    }
}
