using System.Runtime.CompilerServices;

namespace RadSpec;

/**
 * https://www.pcg-random.org/
 * pbrt 和 mitsuba 都用这个作为随机数发生器，也不知道它强在哪里
 * benchmark 结果是比标准库内置用的 xoshiro128/256++ 快一些，确实是个优点（
 */
public class Pcg32
{
    public const ulong PCG32_DEFAULT_STATE = 0x853c49e6748fea9bUL;
    public const ulong PCG32_DEFAULT_STREAM = 0xda3e39cb94b95bdbUL;
    public const ulong PCG32_MULT = 0x5851f42d4c957f2dUL;

    private ulong _state;
    private ulong _inc;

    public Pcg32(ulong seqIndex = PCG32_DEFAULT_STREAM, ulong offset = PCG32_DEFAULT_STATE)
    {
        _state = 0;
        _inc = (seqIndex << 1) | 1;
        NextUInt32();
        _state += offset;
        NextUInt32();
    }

    public Pcg32(Pcg32 other)
    {
        _state = other._state;
        _inc = other._inc;
    }

    public void SetSeed(ulong seed)
    {
        _state = 0;
        _inc = (PCG32_DEFAULT_STREAM << 1) | 1;
        NextUInt32();
        _state += seed;
        NextUInt32();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint NextUInt32()
    {
        ulong oldState = _state;
        _state = oldState * PCG32_MULT + _inc;
        uint xorshifted = unchecked((uint)(((oldState >> 18) ^ oldState) >> 27));
        uint rot = unchecked((uint)(oldState >> 59));
        uint result = (xorshifted >> unchecked((int)rot)) | (xorshifted << ((-(int)rot) & 31));
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int NextInt32() => unchecked((int)NextUInt32());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong NextUInt64()
    {
        ulong v0 = NextUInt32();
        ulong v1 = NextUInt32();
        return v0 << 32 | v1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long NextInt64() => unchecked((long)NextUInt64());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float NextSingle()
    {
        uint value = NextUInt32();
        uint t = (value >> 9) | 0x3f800000u;
        return Unsafe.As<uint, float>(ref t) - 1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double NextDouble()
    {
        ulong value = NextUInt32();
        ulong t = (value << 20) | 0x3ff0000000000000ul;
        return Unsafe.As<ulong, double>(ref t) - 1;
    }

    public uint NextUInt32(uint max)
    {
        uint threshold = (~max + 1) % max;
        while (true)
        {
            uint result = NextUInt32();
            if (result >= threshold)
            {
                return result % max;
            }
        }
    }

    public int NextInt32(int max)
    {
        int threshold = (~max + 1) % max;
        while (true)
        {
            int result = NextInt32();
            if (result >= threshold)
            {
                return result % max;
            }
        }
    }

    public ulong NextUInt64(ulong max)
    {
        ulong threshold = (~max + 1) % max;
        while (true)
        {
            ulong result = NextUInt64();
            if (result >= threshold)
            {
                return result % max;
            }
        }
    }

    public long NextInt64(long max)
    {
        long threshold = (~max + 1) % max;
        while (true)
        {
            long result = NextInt64();
            if (result >= threshold)
            {
                return result % max;
            }
        }
    }

    public void Shuffle<T>(Span<T> list)
    {
        for (int i = list.Length - 1; i > 0; i--)
        {
            int target = NextInt32(i + 1);
            (list[i], list[target]) = (list[target], list[i]);
        }
    }

    public void Advance(long idelta)
    {
        ulong curMult = PCG32_MULT, curPlus = _inc, accMult = 1u;
        ulong accPlus = 0u, delta = (ulong)idelta;
        while (delta > 0)
        {
            if ((delta & 1) != 0)
            {
                accMult *= curMult;
                accPlus = accPlus * curMult + curPlus;
            }
            curPlus = (curMult + 1) * curPlus;
            curMult *= curMult;
            delta /= 2;
        }
        _state = accMult * _state + accPlus;
    }
}
