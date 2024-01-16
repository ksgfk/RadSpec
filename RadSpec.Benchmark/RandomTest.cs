using BenchmarkDotNet.Attributes;

namespace RadSpec.Benchmark;

[DisassemblyDiagnoser(printInstructionAddresses: true)]
public class RandomTest
{
    [Benchmark(Baseline = true)]
    public float Std()
    {
        return new Random().NextSingle();
    }

    [Benchmark]
    public float Pcg32()
    {
        return new Pcg32().NextSingle();
    }
}
