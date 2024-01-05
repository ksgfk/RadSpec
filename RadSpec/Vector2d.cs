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

    public static Vector2d Abs(Vector2d v) => new(double.Abs(v.X), double.Abs(v.Y));
    public static Vector2d Clamp(Vector2d v, Vector2d min, Vector2d max) => new(double.Clamp(v.X, min.X, max.X), double.Clamp(v.Y, min.Y, max.Y));
    public static Vector2d Max(Vector2d x, Vector2d y) => new(double.Max(x.X, y.X), double.Max(x.Y, y.Y));
    public static Vector2d Min(Vector2d x, Vector2d y) => new(double.Min(x.X, y.X), double.Min(x.Y, y.Y));
    public static double Sum(Vector2d v) => v.X + v.Y;

    public static Vector2d Floor(Vector2d v) => new(double.Floor(v.X), double.Floor(v.Y));
    public static Vector2d Ceiling(Vector2d v) => new(double.Ceiling(v.X), double.Ceiling(v.Y));
    public static Vector2d Lerp(Vector2d x, Vector2d y, double t) => new(double.Lerp(x.X, y.X, t), double.Lerp(x.Y, y.Y, t));
    public static Vector2d Lerp(Vector2d x, Vector2d y, Vector2d t) => new(double.Lerp(x.X, y.X, t.X), double.Lerp(x.Y, y.Y, t.Y));
    public static Vector2d Normalize(Vector2d v) => v / Length(v);
    public static Vector2d Sqrt(Vector2d v) => new(double.Sqrt(v.X), double.Sqrt(v.Y));
    public static Vector2d Fma(Vector2d a, Vector2d b, Vector2d c) => new(double.FusedMultiplyAdd(a.X, b.X, c.X), double.FusedMultiplyAdd(a.Y, b.Y, c.Y));
    public static Vector2d Fma(double a, Vector2d b, Vector2d c) => new(double.FusedMultiplyAdd(a, b.X, c.X), double.FusedMultiplyAdd(a, b.Y, c.Y));
    public static Vector2d Fma(Vector2d a, double b, Vector2d c) => new(double.FusedMultiplyAdd(a.X, b, c.X), double.FusedMultiplyAdd(a.Y, b, c.Y));
    public static Vector2d Fma(Vector2d a, Vector2d b, double c) => new(double.FusedMultiplyAdd(a.X, b.X, c), double.FusedMultiplyAdd(a.Y, b.Y, c));
    public static Vector2d Fma(double a, double b, Vector2d c) => new(double.FusedMultiplyAdd(a, b, c.X), double.FusedMultiplyAdd(a, b, c.Y));
    public static Vector2d Fma(double a, Vector2d b, double c) => new(double.FusedMultiplyAdd(a, b.X, c), double.FusedMultiplyAdd(a, b.Y, c));
    public static Vector2d Fma(Vector2d a, double b, double c) => new(double.FusedMultiplyAdd(a.X, b, c), double.FusedMultiplyAdd(a.Y, b, c));
    public static double Distance(Vector2d x, Vector2d y) => Length(x - y);
    public static double DistanceSquared(Vector2d x, Vector2d y) => LengthSquared(x - y);
    public static double Dot(Vector2d x, Vector2d y) => x.X * y.X + x.Y * y.Y;
    public static double AbsDot(Vector2d x, Vector2d y) => double.Abs(Dot(x, y));
    public static double Length(Vector2d v) => double.Sqrt(LengthSquared(v));
    public static double LengthSquared(Vector2d v) => Dot(v, v);
    public static double MinElement(Vector2d v) => double.Min(v.X, v.Y);
    public static double MaxElement(Vector2d v) => double.Max(v.X, v.Y);
    public static bool HasNaN(Vector2d v) => double.IsNaN(v.X) || double.IsNaN(v.Y);
    public static bool HasInf(Vector2d v) => double.IsInfinity(v.X) || double.IsInfinity(v.Y);
    public static double Average(Vector2d v) => Sum(v) / Count;

    public readonly Vector2f AsFloat2() => new((float)X, (float)Y);
    public readonly Vector2i AsInt2() => new((int)X, (int)Y);
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
