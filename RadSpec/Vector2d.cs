using System.Numerics;
using System.Runtime.CompilerServices;

namespace RadSpec;

public struct Vector2d : IVector<Vector2d, double>
{
    public double X, Y;

    public double this[int i]
    {
        readonly get => this.GetElement(i);
        set => this = this.WithElement(i, value);
    }

    public static int Count => 2;
    public static Vector2d Zero => new(0);
    public static Vector2d One => new(1);

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

    public static Vector2d operator -(Vector2d v) => new(-v.X, -v.Y);

    public static Vector2d operator ++(Vector2d v) => new(v.X + 1, v.Y + 1);
    public static Vector2d operator --(Vector2d v) => new(v.X - 1, v.Y - 1);

    public override readonly bool Equals(object? obj) => (obj is Vector2d v) && this == v;
    public readonly bool Equals(Vector2d other) => this == other;
    public override readonly int GetHashCode() => HashCode.Combine(X, Y);
    public override readonly string ToString() => $"<{X}, {Y}>";

    public static Vector2d Abs(Vector2d v) => new(Math.Abs(v.X), Math.Abs(v.Y));
    public static Vector2d Clamp(Vector2d v, Vector2d min, Vector2d max) => new(Math.Clamp(v.X, min.X, max.X), Math.Clamp(v.Y, min.Y, max.Y));
    public static Vector2d Max(Vector2d x, Vector2d y) => new(Math.Max(x.X, y.X), Math.Max(x.Y, y.Y));
    public static Vector2d Min(Vector2d x, Vector2d y) => new(Math.Min(x.X, y.X), Math.Min(x.Y, y.Y));
}

public static partial class VectorExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static double GetElement(this Vector2d vector, int index)
    {
        if ((uint)index >= (uint)Vector2d.Count)
        {
            ThrowUtility.ArgumentOutOfRange(nameof(index));
        }
        return vector.GetElementUnsafe(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector2d WithElement(this Vector2d vector, int index, double value)
    {
        if ((uint)index >= (uint)Vector2d.Count)
        {
            ThrowUtility.ArgumentOutOfRange(nameof(index));
        }
        Vector2d result = vector;
        result.SetElementUnsafe(index, value);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double GetElementUnsafe(in this Vector2d vector, int index)
    {
        ref double address = ref Unsafe.AsRef(in vector.X);
        return Unsafe.Add(ref address, index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SetElementUnsafe(ref this Vector2d vector, int index, double value)
    {
        Unsafe.Add(ref vector.X, index) = value;
    }
}
