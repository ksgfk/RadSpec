using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using BenchmarkDotNet.Attributes;

namespace RadSpec.Benchmark;

[DisassemblyDiagnoser(printInstructionAddresses: true)]
public class Vector3fTest
{
    protected unsafe struct Vec3SoA : IDisposable
    {
        public float* X;
        public float* Y;
        public float* Z;

        public Vec3SoA(int length)
        {
            X = (float*)NativeMemory.AlignedAlloc(sizeof(float) * (nuint)length, 32);
            Y = (float*)NativeMemory.AlignedAlloc(sizeof(float) * (nuint)length, 32);
            Z = (float*)NativeMemory.AlignedAlloc(sizeof(float) * (nuint)length, 32);
        }

        public void Dispose()
        {
            if (X != null)
            {
                NativeMemory.AlignedFree(X);
                X = null;
            }
            if (Y != null)
            {
                NativeMemory.AlignedFree(Y);
                Y = null;
            }
            if (Z != null)
            {
                NativeMemory.AlignedFree(Z);
                Z = null;
            }
        }
    }

    public const int Len = 1538;

    protected Vector3[] _stdA = null!;
    protected Vector3[] _stdB = null!;
    protected Vector3[] _stdC = null!;

    protected Vector3f[] _radA = null!;
    protected Vector3f[] _radB = null!;
    protected Vector3f[] _radC = null!;

    protected Vec3SoA _soaA = default;
    protected Vec3SoA _soaB = default;
    protected Vec3SoA _soaC = default;

    [GlobalSetup]
    public void Setup()
    {
        {
            _stdA = new Vector3[Len];
            for (int i = 0; i < _stdA.Length; i++)
            {
                _stdA[i] = Random.Shared.NextVector3();
            }
            _stdB = new Vector3[Len];
            for (int i = 0; i < _stdB.Length; i++)
            {
                _stdB[i] = Random.Shared.NextVector3();
            }
            _stdC = new Vector3[Len];
        }
        {
            _radA = new Vector3f[Len];
            for (int i = 0; i < _radA.Length; i++)
            {
                _radA[i] = Random.Shared.NextVector3();
            }
            _radB = new Vector3f[Len];
            for (int i = 0; i < _radB.Length; i++)
            {
                _radB[i] = Random.Shared.NextVector3();
            }
            _radC = new Vector3f[Len];
        }
        {
            _soaA = new Vec3SoA(Len);
            _soaB = new Vec3SoA(Len);
            _soaC = new Vec3SoA(Len);
            for (int i = 0; i < Len; i++)
            {
                unsafe
                {
                    _soaA.X[i] = Random.Shared.NextSingle();
                    _soaA.Y[i] = Random.Shared.NextSingle();
                    _soaA.Z[i] = Random.Shared.NextSingle();
                    _soaB.X[i] = Random.Shared.NextSingle();
                    _soaB.Y[i] = Random.Shared.NextSingle();
                    _soaB.Z[i] = Random.Shared.NextSingle();
                }
            }
        }
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _soaA.Dispose();
        _soaB.Dispose();
    }
}

public class Vector3fAdd : Vector3fTest
{
    [Benchmark(Baseline = true)]
    public void StdAdd()
    {
        if (_stdA.Length != _stdB.Length || _stdA.Length != _stdC.Length)
        {
            return;
        }
        int len = _stdA.Length;
        for (int i = 0; i < len; i++)
        {
            _stdC[i] = _stdA[i] + _stdB[i];
        }
    }

    [Benchmark]
    public void RadAdd()
    {
        if (_radA.Length != _radB.Length || _radA.Length != _radC.Length)
        {
            return;
        }
        int len = _radA.Length;
        for (int i = 0; i < len; i++)
        {
            _radC[i] = _radA[i] + _radB[i];
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe void Fallback(float* ax, float* bx, float* rx)
    {
        for (int i = 0; i < Len; i++)
        {
            *rx = *ax + *bx;
            ax++;
            bx++;
            rx++;
        }
    }

    [Benchmark]
    public unsafe void SoAAdd256()
    {
        Call(_soaA.X, _soaB.X, _soaC.X);
        Call(_soaA.Y, _soaB.Y, _soaC.Y);
        Call(_soaA.Z, _soaB.Z, _soaC.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void Call(float* ax, float* bx, float* rx)
        {
            if (Vector256.IsHardwareAccelerated)
            {
                int remain = Len / 8 * 8;
                for (int i = 0; i < Len; i += 8)
                {
                    var x1 = Vector256.LoadAligned(ax);
                    var x2 = Vector256.LoadAligned(bx);
                    (x1 + x2).StoreAligned(rx);
                    ax += 8;
                    bx += 8;
                    rx += 8;
                }
                for (int i = remain; i < Len; i++)
                {
                    *rx = *ax + *bx;
                    ax++;
                    bx++;
                    rx++;
                }
            }
            else
            {
                Fallback(ax, bx, rx);
            }
        }
    }

    [Benchmark]
    public unsafe void SoAAdd128()
    {
        Call(_soaA.X, _soaB.X, _soaC.X);
        Call(_soaA.Y, _soaB.Y, _soaC.Y);
        Call(_soaA.Z, _soaB.Z, _soaC.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void Call(float* ax, float* bx, float* rx)
        {
            if (Vector128.IsHardwareAccelerated)
            {
                int remain = Len / 4 * 4;
                for (int i = 0; i < Len; i += 4)
                {
                    var x1 = Vector128.LoadAligned(ax);
                    var x2 = Vector128.LoadAligned(bx);
                    (x1 + x2).StoreAligned(rx);
                    ax += 4;
                    bx += 4;
                    rx += 4;
                }
                for (int i = remain; i < Len; i++)
                {
                    *rx = *ax + *bx;
                    ax++;
                    bx++;
                    rx++;
                }
            }
            else
            {
                Fallback(ax, bx, rx);
            }
        }
    }

    [Benchmark]
    public unsafe void SoAAdd64()
    {
        Call(_soaA.X, _soaB.X, _soaC.X);
        Call(_soaA.Y, _soaB.Y, _soaC.Y);
        Call(_soaA.Z, _soaB.Z, _soaC.Z);

        static void Call(float* ax, float* bx, float* rx)
        {
            if (Vector64.IsHardwareAccelerated)
            {
                int remain = Len / 2 * 2;
                for (int i = 0; i < Len; i += 2)
                {
                    var x1 = Vector64.LoadAligned(ax);
                    var x2 = Vector64.LoadAligned(bx);
                    (x1 + x2).StoreAligned(rx);
                    ax += 2;
                    bx += 2;
                    rx += 2;
                }
                for (int i = remain; i < Len; i++)
                {
                    *rx = *ax + *bx;
                    ax++;
                    bx++;
                    rx++;
                }
            }
            else
            {
                Fallback(ax, bx, rx);
            }
        }
    }

    [Benchmark]
    public unsafe void AddFallback()
    {
        Fallback(_soaA.X, _soaB.X, _soaC.X);
        Fallback(_soaA.Y, _soaB.Y, _soaC.Y);
        Fallback(_soaA.Z, _soaB.Z, _soaC.Z);
    }
}
