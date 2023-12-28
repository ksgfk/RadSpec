using System.Numerics;
using System.Runtime.CompilerServices;

namespace RadSpec;

public struct Vector4d : IVector<Vector4d, double>
{
    public double X, Y, Z, W;

    public double this[int i]
    {
        readonly get => this.GetElement(i);
        set => this = this.WithElement(i, value);
    }

    public static int Count => 4;
    public static Vector4d Zero => new(0);
    public static Vector4d One => new(1);

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

    public static Vector4d operator -(Vector4d v) => new(-v.X, -v.Y, -v.Z, -v.W);

    public static Vector4d operator ++(Vector4d v) => new(v.X + 1, v.Y + 1, v.Z + 1, v.W + 1);
    public static Vector4d operator --(Vector4d v) => new(v.X - 1, v.Y - 1, v.Z - 1, v.W - 1);

    public override readonly bool Equals(object? obj) => (obj is Vector4d v) && this == v;
    public readonly bool Equals(Vector4d other) => this == other;
    public override readonly int GetHashCode() => HashCode.Combine(X, Y, Z, W);
    public override readonly string ToString() => $"<{X}, {Y}, {Z}, {W}>";

    public static Vector4d Abs(Vector4d v) => new(Math.Abs(v.X), Math.Abs(v.Y), Math.Abs(v.Z), Math.Abs(v.W));
    public static Vector4d Clamp(Vector4d v, Vector4d min, Vector4d max) => new(Math.Clamp(v.X, min.X, max.X), Math.Clamp(v.Y, min.Y, max.Y), Math.Clamp(v.Z, min.Z, max.Z), Math.Clamp(v.W, min.W, max.W));
    public static Vector4d Max(Vector4d x, Vector4d y) => new(Math.Max(x.X, y.X), Math.Max(x.Y, y.Y), Math.Max(x.Z, y.Z), Math.Max(x.W, y.W));
    public static Vector4d Min(Vector4d x, Vector4d y) => new(Math.Min(x.X, y.X), Math.Min(x.Y, y.Y), Math.Min(x.Z, y.Z), Math.Min(x.W, y.W));
}

public static partial class VectorExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static double GetElement(this Vector4d vector, int index)
    {
        if ((uint)index >= (uint)Vector4d.Count)
        {
            ThrowUtility.ArgumentOutOfRange(nameof(index));
        }
        return vector.GetElementUnsafe(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector4d WithElement(this Vector4d vector, int index, double value)
    {
        if ((uint)index >= (uint)Vector4d.Count)
        {
            ThrowUtility.ArgumentOutOfRange(nameof(index));
        }
        Vector4d result = vector;
        result.SetElementUnsafe(index, value);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double GetElementUnsafe(in this Vector4d vector, int index)
    {
        ref double address = ref Unsafe.AsRef(in vector.X);
        return Unsafe.Add(ref address, index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SetElementUnsafe(ref this Vector4d vector, int index, double value)
    {
        Unsafe.Add(ref vector.X, index) = value;
    }
}
