using System.Runtime.CompilerServices;

namespace RadSpec;

public class Pcg32
{
    public const ulong PCG32_DEFAULT_STATE = 0x853c49e6748fea9bUL;
    public const ulong PCG32_DEFAULT_STREAM = 0xda3e39cb94b95bdbUL;
    public const ulong PCG32_MULT = 0x5851f42d4c957f2dUL;

    private ulong _state;
    private readonly ulong _inc;

    public Pcg32(ulong seqIndex = PCG32_DEFAULT_STREAM, ulong offset = PCG32_DEFAULT_STATE)
    {
        _state = 0;
        _inc = (seqIndex << 1) | 1;
        NextUInt32();
        _state += offset;
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
}
