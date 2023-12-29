using System.Runtime.CompilerServices;

namespace RadSpec;

public struct Vector3i
{
    public int X, Y, Z;

    public int this[int i]
    {
        readonly get => this.GetElement(i);
        set => this = this.WithElement(i, value);
    }

    public const int Count = 3;
    public static Vector3i Zero => new(0);
    public static Vector3i One => new(1);

    public Vector3i(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Vector3i(int value)
    {
        X = value;
        Y = value;
        Z = value;
    }

    public Vector3i(Vector2i xy, int z) : this(xy.X, xy.Y, z) { }
    public Vector3i(int x, Vector2i yz) : this(x, yz.X, yz.Y) { }

    public static Vector3i operator +(Vector3i lhs, Vector3i rhs) => new(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z);
    public static Vector3i operator -(Vector3i lhs, Vector3i rhs) => new(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z);
    public static Vector3i operator *(Vector3i lhs, Vector3i rhs) => new(lhs.X * rhs.X, lhs.Y * rhs.Y, lhs.Z * rhs.Z);
    public static Vector3i operator /(Vector3i lhs, Vector3i rhs) => new(lhs.X / rhs.X, lhs.Y / rhs.Y, lhs.Z / rhs.Z);

    public static Vector3i operator +(Vector3i lhs, int rhs) => new(lhs.X + rhs, lhs.Y + rhs, lhs.Z + rhs);
    public static Vector3i operator +(int lhs, Vector3i rhs) => new(lhs + rhs.X, lhs + rhs.Y, lhs + rhs.Z);

    public static Vector3i operator -(Vector3i lhs, int rhs) => new(lhs.X - rhs, lhs.Y - rhs, lhs.Z - rhs);
    public static Vector3i operator -(int lhs, Vector3i rhs) => new(lhs - rhs.X, lhs - rhs.Y, lhs - rhs.Z);

    public static Vector3i operator *(Vector3i lhs, int rhs) => new(lhs.X * rhs, lhs.Y * rhs, lhs.Z * rhs);
    public static Vector3i operator *(int lhs, Vector3i rhs) => new(lhs * rhs.X, lhs * rhs.Y, lhs * rhs.Z);

    public static Vector3i operator /(Vector3i lhs, int rhs) => new(lhs.X / rhs, lhs.Y / rhs, lhs.Z / rhs);
    public static Vector3i operator /(int lhs, Vector3i rhs) => new(lhs / rhs.X, lhs / rhs.Y, lhs / rhs.Z);

    public static bool operator ==(Vector3i lhs, Vector3i rhs) => lhs.X == rhs.X && lhs.Y == rhs.Y && lhs.Z == rhs.Z;
    public static bool operator !=(Vector3i lhs, Vector3i rhs) => lhs.X != rhs.X || lhs.Y != rhs.Y || lhs.Z != rhs.Z;

    public static Vector3i operator -(Vector3i v) => new(-v.X, -v.Y, -v.Z);

    public static Vector3i operator ++(Vector3i v) => new(v.X + 1, v.Y + 1, v.Z + 1);
    public static Vector3i operator --(Vector3i v) => new(v.X - 1, v.Y - 1, v.Z - 1);

    public override readonly bool Equals(object? obj) => (obj is Vector3i v) && this == v;
    public readonly bool Equals(Vector3i other) => this == other;
    public override readonly int GetHashCode() => HashCode.Combine(X, Y, Z);
    public override readonly string ToString() => $"<{X}, {Y}, {Z}>";

    public static Vector3i Abs(Vector3i v) => new(int.Abs(v.X), int.Abs(v.Y), int.Abs(v.Z));
    public static Vector3i Clamp(Vector3i v, Vector3i min, Vector3i max) => new(int.Clamp(v.X, min.X, max.X), int.Clamp(v.Y, min.Y, max.Y), int.Clamp(v.Z, min.Z, max.Z));
    public static Vector3i Max(Vector3i x, Vector3i y) => new(int.Max(x.X, y.X), int.Max(x.Y, y.Y), int.Max(x.Z, y.Z));
    public static Vector3i Min(Vector3i x, Vector3i y) => new(int.Min(x.X, y.X), int.Min(x.Y, y.Y), int.Min(x.Z, y.Z));
    public static int Sum(Vector3i v) => v.X + v.Y + v.Z;
    public static int MinElement(Vector3i v) => int.Min(int.Min(v.X, v.Y), v.Z);
    public static int MaxElement(Vector3i v) => int.Max(int.Max(v.X, v.Y), v.Z);

    public readonly Vector3d AsFloat3() => new(X, Y, Z);
    public readonly Vector3d AsDouble3() => new(X, Y, Z);

    public readonly Vector2i XY => new(X, Y);
}

public static partial class VectorExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int GetElement(this Vector3i vector, int index)
    {
        if ((uint)index >= (uint)Vector3i.Count)
        {
            ThrowUtility.ArgumentOutOfRange(nameof(index));
        }
        return vector.GetElementUnsafe(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector3i WithElement(this Vector3i vector, int index, int value)
    {
        if ((uint)index >= (uint)Vector3i.Count)
        {
            ThrowUtility.ArgumentOutOfRange(nameof(index));
        }
        Vector3i result = vector;
        result.SetElementUnsafe(index, value);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetElementUnsafe(in this Vector3i vector, int index)
    {
        ref int address = ref Unsafe.AsRef(in vector.X);
        return Unsafe.Add(ref address, index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SetElementUnsafe(ref this Vector3i vector, int index, int value)
    {
        Unsafe.Add(ref vector.X, index) = value;
    }
}
