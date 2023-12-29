using System.Runtime.CompilerServices;

namespace RadSpec;

public struct Vector2i
{
    public int X, Y;

    public int this[int i]
    {
        readonly get => this.GetElement(i);
        set => this = this.WithElement(i, value);
    }

    public const int Count = 2;
    public static Vector2i Zero => new(0);
    public static Vector2i One => new(1);

    public Vector2i(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Vector2i(int value)
    {
        X = value;
        Y = value;
    }

    public static Vector2i operator +(Vector2i lhs, Vector2i rhs) => new(lhs.X + rhs.X, lhs.Y + rhs.Y);
    public static Vector2i operator -(Vector2i lhs, Vector2i rhs) => new(lhs.X - rhs.X, lhs.Y - rhs.Y);
    public static Vector2i operator *(Vector2i lhs, Vector2i rhs) => new(lhs.X * rhs.X, lhs.Y * rhs.Y);
    public static Vector2i operator /(Vector2i lhs, Vector2i rhs) => new(lhs.X / rhs.X, lhs.Y / rhs.Y);

    public static Vector2i operator +(Vector2i lhs, int rhs) => new(lhs.X + rhs, lhs.Y + rhs);
    public static Vector2i operator +(int lhs, Vector2i rhs) => new(lhs + rhs.X, lhs + rhs.Y);

    public static Vector2i operator -(Vector2i lhs, int rhs) => new(lhs.X - rhs, lhs.Y - rhs);
    public static Vector2i operator -(int lhs, Vector2i rhs) => new(lhs - rhs.X, lhs - rhs.Y);

    public static Vector2i operator *(Vector2i lhs, int rhs) => new(lhs.X * rhs, lhs.Y * rhs);
    public static Vector2i operator *(int lhs, Vector2i rhs) => new(lhs * rhs.X, lhs * rhs.Y);

    public static Vector2i operator /(Vector2i lhs, int rhs) => new(lhs.X / rhs, lhs.Y / rhs);
    public static Vector2i operator /(int lhs, Vector2i rhs) => new(lhs / rhs.X, lhs / rhs.Y);

    public static bool operator ==(Vector2i lhs, Vector2i rhs) => lhs.X == rhs.X && lhs.Y == rhs.Y;
    public static bool operator !=(Vector2i lhs, Vector2i rhs) => lhs.X != rhs.X || lhs.Y != rhs.Y;

    public static Vector2i operator -(Vector2i v) => new(-v.X, -v.Y);

    public static Vector2i operator ++(Vector2i v) => new(v.X + 1, v.Y + 1);
    public static Vector2i operator --(Vector2i v) => new(v.X - 1, v.Y - 1);

    public override readonly bool Equals(object? obj) => (obj is Vector2i v) && this == v;
    public readonly bool Equals(Vector2i other) => this == other;
    public override readonly int GetHashCode() => HashCode.Combine(X, Y);
    public override readonly string ToString() => $"<{X}, {Y}>";

    public static Vector2i Abs(Vector2i v) => new(int.Abs(v.X), int.Abs(v.Y));
    public static Vector2i Clamp(Vector2i v, Vector2i min, Vector2i max) => new(int.Clamp(v.X, min.X, max.X), int.Clamp(v.Y, min.Y, max.Y));
    public static Vector2i Max(Vector2i x, Vector2i y) => new(int.Max(x.X, y.X), int.Max(x.Y, y.Y));
    public static Vector2i Min(Vector2i x, Vector2i y) => new(int.Min(x.X, y.X), int.Min(x.Y, y.Y));
    public static int Sum(Vector2i v) => v.X + v.Y;
    public static int MinElement(Vector2i v) => int.Min(v.X, v.Y);
    public static int MaxElement(Vector2i v) => int.Max(v.X, v.Y);

    public readonly Vector2f AsFloat2() => new(X, Y);
    public readonly Vector2d AsDouble2() => new(X, Y);
}

public static partial class VectorExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int GetElement(this Vector2i vector, int index)
    {
        if ((uint)index >= (uint)Vector2i.Count)
        {
            ThrowUtility.ArgumentOutOfRange(nameof(index));
        }
        return vector.GetElementUnsafe(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector2i WithElement(this Vector2i vector, int index, int value)
    {
        if ((uint)index >= (uint)Vector2i.Count)
        {
            ThrowUtility.ArgumentOutOfRange(nameof(index));
        }
        Vector2i result = vector;
        result.SetElementUnsafe(index, value);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetElementUnsafe(in this Vector2i vector, int index)
    {
        ref int address = ref Unsafe.AsRef(in vector.X);
        return Unsafe.Add(ref address, index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SetElementUnsafe(ref this Vector2i vector, int index, int value)
    {
        Unsafe.Add(ref vector.X, index) = value;
    }
}
