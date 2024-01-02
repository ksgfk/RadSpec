using System.Numerics;
using System.Runtime.CompilerServices;

namespace RadSpec;

public struct Vector4d
{
    public double X, Y, Z, W;

    public double this[int i]
    {
        readonly get => this.GetElement(i);
        set => this = this.WithElement(i, value);
    }

    public const int Count = 4;
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

    public Vector4d(Vector2d xy, double z, double w) : this(xy.X, xy.Y, z, w) { }
    public Vector4d(double x, Vector2d yz, double w) : this(x, yz.X, yz.Y, w) { }
    public Vector4d(double x, double y, Vector2d zw) : this(x, y, zw.X, zw.Y) { }
    public Vector4d(Vector3d xyz, double w) : this(xyz.X, xyz.Y, xyz.Z, w) { }
    public Vector4d(double x, Vector3d yzw) : this(x, yzw.X, yzw.Y, yzw.Z) { }

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

    public static Vector4d Abs(Vector4d v) => new(double.Abs(v.X), double.Abs(v.Y), double.Abs(v.Z), double.Abs(v.W));
    public static Vector4d Clamp(Vector4d v, Vector4d min, Vector4d max) => new(double.Clamp(v.X, min.X, max.X), double.Clamp(v.Y, min.Y, max.Y), double.Clamp(v.Z, min.Z, max.Z), double.Clamp(v.W, min.W, max.W));
    public static Vector4d Max(Vector4d x, Vector4d y) => new(double.Max(x.X, y.X), double.Max(x.Y, y.Y), double.Max(x.Z, y.Z), double.Max(x.W, y.W));
    public static Vector4d Min(Vector4d x, Vector4d y) => new(double.Min(x.X, y.X), double.Min(x.Y, y.Y), double.Min(x.Z, y.Z), double.Min(x.W, y.W));
    public static double Sum(Vector4d v) => v.X + v.Y + v.Z + v.W;

    public static Vector4d Floor(Vector4d v) => new(double.Floor(v.X), double.Floor(v.Y), double.Floor(v.Z), double.Floor(v.W));
    public static Vector4d Ceiling(Vector4d v) => new(double.Ceiling(v.X), double.Ceiling(v.Y), double.Ceiling(v.Z), double.Ceiling(v.W));
    public static Vector4d Lerp(Vector4d x, Vector4d y, double t) => new(double.Lerp(x.X, y.X, t), double.Lerp(x.Y, y.Y, t), double.Lerp(x.Z, y.Z, t), double.Lerp(x.W, y.W, t));
    public static Vector4d Normalize(Vector4d v) => v / Length(v);
    public static Vector4d Sqrt(Vector4d v) => new(double.Sqrt(v.X), double.Sqrt(v.Y), double.Sqrt(v.Z), double.Sqrt(v.W));
    public static Vector4d Fma(Vector4d a, Vector4d b, Vector4d c) => new(double.FusedMultiplyAdd(a.X, b.X, c.X), double.FusedMultiplyAdd(a.Y, b.Y, c.Y), double.FusedMultiplyAdd(a.Z, b.Z, c.Z), double.FusedMultiplyAdd(a.W, b.W, c.W));
    public static Vector4d Fma(double a, Vector4d b, Vector4d c) => new(double.FusedMultiplyAdd(a, b.X, c.X), double.FusedMultiplyAdd(a, b.Y, c.Y), double.FusedMultiplyAdd(a, b.Z, c.Z), double.FusedMultiplyAdd(a, b.W, c.W));
    public static Vector4d Fma(Vector4d a, double b, Vector4d c) => new(double.FusedMultiplyAdd(a.X, b, c.X), double.FusedMultiplyAdd(a.Y, b, c.Y), double.FusedMultiplyAdd(a.Z, b, c.Z), double.FusedMultiplyAdd(a.W, b, c.W));
    public static Vector4d Fma(Vector4d a, Vector4d b, double c) => new(double.FusedMultiplyAdd(a.X, b.X, c), double.FusedMultiplyAdd(a.Y, b.Y, c), double.FusedMultiplyAdd(a.Z, b.Z, c), double.FusedMultiplyAdd(a.W, b.W, c));
    public static Vector4d Fma(double a, double b, Vector4d c) => new(double.FusedMultiplyAdd(a, b, c.X), double.FusedMultiplyAdd(a, b, c.Y), double.FusedMultiplyAdd(a, b, c.Z), double.FusedMultiplyAdd(a, b, c.W));
    public static Vector4d Fma(double a, Vector4d b, double c) => new(double.FusedMultiplyAdd(a, b.X, c), double.FusedMultiplyAdd(a, b.Y, c), double.FusedMultiplyAdd(a, b.Z, c), double.FusedMultiplyAdd(a, b.W, c));
    public static Vector4d Fma(Vector4d a, double b, double c) => new(double.FusedMultiplyAdd(a.X, b, c), double.FusedMultiplyAdd(a.Y, b, c), double.FusedMultiplyAdd(a.Z, b, c), double.FusedMultiplyAdd(a.W, b, c));
    public static double Distance(Vector4d x, Vector4d y) => Length(x - y);
    public static double DistanceSquared(Vector4d x, Vector4d y) => LengthSquared(x - y);
    public static double Dot(Vector4d x, Vector4d y) => x.X * y.X + x.Y * y.Y + x.Z * y.Z + x.W * y.W;
    public static double AbsDot(Vector4d x, Vector4d y) => double.Abs(Dot(x, y));
    public static double Length(Vector4d v) => double.Sqrt(LengthSquared(v));
    public static double LengthSquared(Vector4d v) => Dot(v, v);
    public static double MinElement(Vector4d v) => double.Min(double.Min(double.Min(v.X, v.Y), v.Z), v.W);
    public static double MaxElement(Vector4d v) => double.Max(double.Max(double.Max(v.X, v.Y), v.Z), v.W);
    public static bool HasNaN(Vector4d v) => double.IsNaN(v.X) || double.IsNaN(v.Y) || double.IsNaN(v.Z) || double.IsNaN(v.W);
    public static bool HasInf(Vector4d v) => double.IsInfinity(v.X) || double.IsInfinity(v.Y) || double.IsInfinity(v.Z) || double.IsInfinity(v.W);

    public readonly Vector4f AsFloat4() => new((float)X, (float)Y, (float)Z, (float)W);
    public readonly Vector4i AsInt4() => new((int)X, (int)Y, (int)Z, (int)W);

    public readonly Vector3d XYZ => new(X, Y, Z);
    public readonly Vector2d XY => new(X, Y);
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
