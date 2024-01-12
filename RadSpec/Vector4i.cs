using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace RadSpec;

public struct Vector4i : IVector<Vector4i, int>
{
    public int X, Y, Z, W;

    public int this[int i]
    {
        readonly get => this.GetElement(i);
        set => this = this.WithElement(i, value);
    }

    public static int Count => 4;
    public static Vector4i Zero => new(0);
    public static Vector4i One => new(1);

    public Vector4i(int x, int y, int z, int w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public Vector4i(int value)
    {
        X = value;
        Y = value;
        Z = value;
        W = value;
    }

    public Vector4i(Vector2i xy, int z, int w) : this(xy.X, xy.Y, z, w) { }
    public Vector4i(int x, Vector2i yz, int w) : this(x, yz.X, yz.Y, w) { }
    public Vector4i(int x, int y, Vector2i zw) : this(x, y, zw.X, zw.Y) { }
    public Vector4i(Vector2i xy, Vector2i zw) : this(xy.X, xy.Y, zw.X, zw.Y) { }
    public Vector4i(Vector3i xyz, int w) : this(xyz.X, xyz.Y, xyz.Z, w) { }
    public Vector4i(int x, Vector3i yzw) : this(x, yzw.X, yzw.Y, yzw.Z) { }

    public static Vector4i operator +(Vector4i lhs, Vector4i rhs) => new(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z, lhs.W + rhs.W);
    public static Vector4i operator -(Vector4i lhs, Vector4i rhs) => new(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z, lhs.W - rhs.W);
    public static Vector4i operator *(Vector4i lhs, Vector4i rhs) => new(lhs.X * rhs.X, lhs.Y * rhs.Y, lhs.Z * rhs.Z, lhs.W * rhs.W);
    public static Vector4i operator /(Vector4i lhs, Vector4i rhs) => new(lhs.X / rhs.X, lhs.Y / rhs.Y, lhs.Z / rhs.Z, lhs.W / rhs.W);

    public static Vector4i operator +(Vector4i lhs, int rhs) => new(lhs.X + rhs, lhs.Y + rhs, lhs.Z + rhs, lhs.W + rhs);
    public static Vector4i operator +(int lhs, Vector4i rhs) => new(lhs + rhs.X, lhs + rhs.Y, lhs + rhs.Z, lhs + rhs.W);

    public static Vector4i operator -(Vector4i lhs, int rhs) => new(lhs.X - rhs, lhs.Y - rhs, lhs.Z - rhs, lhs.W - rhs);
    public static Vector4i operator -(int lhs, Vector4i rhs) => new(lhs - rhs.X, lhs - rhs.Y, lhs - rhs.Z, lhs - rhs.W);

    public static Vector4i operator *(Vector4i lhs, int rhs) => new(lhs.X * rhs, lhs.Y * rhs, lhs.Z * rhs, lhs.W * rhs);
    public static Vector4i operator *(int lhs, Vector4i rhs) => new(lhs * rhs.X, lhs * rhs.Y, lhs * rhs.Z, lhs * rhs.W);

    public static Vector4i operator /(Vector4i lhs, int rhs) => new(lhs.X / rhs, lhs.Y / rhs, lhs.Z / rhs, lhs.W / rhs);
    public static Vector4i operator /(int lhs, Vector4i rhs) => new(lhs / rhs.X, lhs / rhs.Y, lhs / rhs.Z, lhs / rhs.W);

    public static bool operator ==(Vector4i lhs, Vector4i rhs) => lhs.X == rhs.X && lhs.Y == rhs.Y && lhs.Z == rhs.Z && lhs.W == rhs.W;
    public static bool operator !=(Vector4i lhs, Vector4i rhs) => lhs.X != rhs.X || lhs.Y != rhs.Y || lhs.Z != rhs.Z || lhs.W != rhs.W;

    public static Vector4i operator -(Vector4i v) => new(-v.X, -v.Y, -v.Z, -v.W);

    public static Vector4i operator ++(Vector4i v) => new(v.X + 1, v.Y + 1, v.Z + 1, v.W + 1);
    public static Vector4i operator --(Vector4i v) => new(v.X - 1, v.Y - 1, v.Z - 1, v.W - 1);

    public override readonly bool Equals(object? obj) => (obj is Vector4i v) && this == v;
    public readonly bool Equals(Vector4i other) => this == other;
    public override readonly int GetHashCode() => HashCode.Combine(X, Y, Z, W);
    public override readonly string ToString() => $"<{X}, {Y}, {Z}, {W}>";

    public static Vector4i Abs(Vector4i v) => new(int.Abs(v.X), int.Abs(v.Y), int.Abs(v.Z), int.Abs(v.W));
    public static Vector4i Clamp(Vector4i v, Vector4i min, Vector4i max) => new(int.Clamp(v.X, min.X, max.X), int.Clamp(v.Y, min.Y, max.Y), int.Clamp(v.Z, min.Z, max.Z), int.Clamp(v.W, min.W, max.W));
    public static Vector4i Max(Vector4i x, Vector4i y) => new(int.Max(x.X, y.X), int.Max(x.Y, y.Y), int.Max(x.Z, y.Z), int.Max(x.W, y.W));
    public static Vector4i Min(Vector4i x, Vector4i y) => new(int.Min(x.X, y.X), int.Min(x.Y, y.Y), int.Min(x.Z, y.Z), int.Min(x.W, y.W));
    public static int Sum(Vector4i v) => v.X + v.Y + v.Z + v.W;
    public static int MinElement(Vector4i v) => int.Min(int.Min(int.Min(v.X, v.Y), v.Z), v.W);
    public static int MaxElement(Vector4i v) => int.Max(int.Max(int.Max(v.X, v.Y), v.Z), v.W);

    public readonly Vector4f AsFloat4() => new(X, Y, Z, W);
    public readonly Vector4d AsDouble4() => new(X, Y, Z, W);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Vector3i XYZ => new(X, Y, Z);
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Vector2i XY => new(X, Y);
}

public static partial class VectorExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int GetElement(this Vector4i vector, int index)
    {
        if ((uint)index >= (uint)Vector4i.Count)
        {
            ThrowUtility.ArgumentOutOfRange(nameof(index));
        }
        return vector.GetElementUnsafe(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector4i WithElement(this Vector4i vector, int index, int value)
    {
        if ((uint)index >= (uint)Vector4i.Count)
        {
            ThrowUtility.ArgumentOutOfRange(nameof(index));
        }
        Vector4i result = vector;
        result.SetElementUnsafe(index, value);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetElementUnsafe(in this Vector4i vector, int index)
    {
        ref int address = ref Unsafe.AsRef(in vector.X);
        return Unsafe.Add(ref address, index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SetElementUnsafe(ref this Vector4i vector, int index, int value)
    {
        Unsafe.Add(ref vector.X, index) = value;
    }
}
