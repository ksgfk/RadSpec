using System.Numerics;
using System.Runtime.CompilerServices;

namespace RadSpec;

public struct Vector3d
{
    public double X, Y, Z;

    public double this[int i]
    {
        readonly get => this.GetElement(i);
        set => this = this.WithElement(i, value);
    }

    public const int Count = 3;
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

    public Vector3d(Vector2d xy, float z) : this(xy.X, xy.Y, z) { }
    public Vector3d(float x, Vector2d yz) : this(x, yz.X, yz.Y) { }

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

    public static Vector3d Abs(Vector3d v) => new(double.Abs(v.X), double.Abs(v.Y), double.Abs(v.Z));
    public static Vector3d Clamp(Vector3d v, Vector3d min, Vector3d max) => new(double.Clamp(v.X, min.X, max.X), double.Clamp(v.Y, min.Y, max.Y), double.Clamp(v.Z, min.Z, max.Z));
    public static Vector3d Max(Vector3d x, Vector3d y) => new(double.Max(x.X, y.X), double.Max(x.Y, y.Y), double.Max(x.Z, y.Z));
    public static Vector3d Min(Vector3d x, Vector3d y) => new(double.Min(x.X, y.X), double.Min(x.Y, y.Y), double.Min(x.Z, y.Z));
    public static double Sum(Vector3d v) => v.X + v.Y + v.Z;

    public static Vector3d Floor(Vector3d v) => new(double.Floor(v.X), double.Floor(v.Y), double.Floor(v.Z));
    public static Vector3d Ceiling(Vector3d v) => new(double.Ceiling(v.X), double.Ceiling(v.Y), double.Ceiling(v.Z));
    public static Vector3d Lerp(Vector3d x, Vector3d y, double t) => new(double.Lerp(x.X, y.X, t), double.Lerp(x.Y, y.Y, t), double.Lerp(x.Z, y.Z, t));
    public static Vector3d Lerp(Vector3d x, Vector3d y, Vector3d t) => new(double.Lerp(x.X, y.X, t.X), double.Lerp(x.Y, y.Y, t.Y), double.Lerp(x.Z, y.Z, t.Z));
    public static Vector3d Normalize(Vector3d v) => v / Length(v);
    public static Vector3d Sqrt(Vector3d v) => new(double.Sqrt(v.X), double.Sqrt(v.Y), double.Sqrt(v.Z));
    public static Vector3d Fma(Vector3d a, Vector3d b, Vector3d c) => new(double.FusedMultiplyAdd(a.X, b.X, c.X), double.FusedMultiplyAdd(a.Y, b.Y, c.Y), double.FusedMultiplyAdd(a.Z, b.Z, c.Z));
    public static Vector3d Fma(double a, Vector3d b, Vector3d c) => new(double.FusedMultiplyAdd(a, b.X, c.X), double.FusedMultiplyAdd(a, b.Y, c.Y), double.FusedMultiplyAdd(a, b.Z, c.Z));
    public static Vector3d Fma(Vector3d a, double b, Vector3d c) => new(double.FusedMultiplyAdd(a.X, b, c.X), double.FusedMultiplyAdd(a.Y, b, c.Y), double.FusedMultiplyAdd(a.Z, b, c.Z));
    public static Vector3d Fma(Vector3d a, Vector3d b, double c) => new(double.FusedMultiplyAdd(a.X, b.X, c), double.FusedMultiplyAdd(a.Y, b.Y, c), double.FusedMultiplyAdd(a.Z, b.Z, c));
    public static Vector3d Fma(double a, double b, Vector3d c) => new(double.FusedMultiplyAdd(a, b, c.X), double.FusedMultiplyAdd(a, b, c.Y), double.FusedMultiplyAdd(a, b, c.Z));
    public static Vector3d Fma(double a, Vector3d b, double c) => new(double.FusedMultiplyAdd(a, b.X, c), double.FusedMultiplyAdd(a, b.Y, c), double.FusedMultiplyAdd(a, b.Z, c));
    public static Vector3d Fma(Vector3d a, double b, double c) => new(double.FusedMultiplyAdd(a.X, b, c), double.FusedMultiplyAdd(a.Y, b, c), double.FusedMultiplyAdd(a.Z, b, c));
    public static double Distance(Vector3d x, Vector3d y) => Length(x - y);
    public static double DistanceSquared(Vector3d x, Vector3d y) => LengthSquared(x - y);
    public static double Dot(Vector3d x, Vector3d y) => x.X * y.X + x.Y * y.Y + x.Z * y.Z;
    public static double AbsDot(Vector3d x, Vector3d y) => double.Abs(Dot(x, y));
    public static double Length(Vector3d v) => double.Sqrt(LengthSquared(v));
    public static double LengthSquared(Vector3d v) => Dot(v, v);
    public static double MinElement(Vector3d v) => double.Min(double.Min(v.X, v.Y), v.Z);
    public static double MaxElement(Vector3d v) => double.Max(double.Max(v.X, v.Y), v.Z);
    public static bool HasNaN(Vector3d v) => double.IsNaN(v.X) || double.IsNaN(v.Y) || double.IsNaN(v.Z);
    public static bool HasInf(Vector3d v) => double.IsInfinity(v.X) || double.IsInfinity(v.Y) || double.IsInfinity(v.Z);

    public static Vector3d Cross(Vector3d x, Vector3d y) => new(
        x.Y * y.Z - x.Z * y.Y,
        x.Z * y.X - x.X * y.Z,
        x.X * y.Y - x.Y * y.X);

    public readonly Vector3f AsFloat3() => new((float)X, (float)Y, (float)Z);
    public readonly Vector3i AsInt3() => new((int)X, (int)Y, (int)Z);

    public readonly Vector2d XY => new(X, Y);
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
