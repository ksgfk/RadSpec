using BenchmarkDotNet.Attributes;

namespace RadSpec.Benchmark;

[DisassemblyDiagnoser(printInstructionAddresses: true)]
public class RadToDeg
{
    public const int Cnt = 1000;

    private double[] _a = null!;

    [GlobalSetup]
    public void Setup()
    {
        _a = new double[Cnt];
        for (int i = 0; i < _a.Length; i++)
        {
            _a[i] = Random.Shared.NextSingle();
        }
    }

    [Benchmark(Baseline = true)]
    public void Raw()
    {
        for (int i = 0; i < _a.Length; i++)
        {
            _a[i] = 180 / double.Pi * _a[i];
        }
    }

    [Benchmark]
    public void Generic()
    {
        for (int i = 0; i < _a.Length; i++)
        {
            _a[i] = MathExt.Degree(_a[i]);
        }
    }
}
