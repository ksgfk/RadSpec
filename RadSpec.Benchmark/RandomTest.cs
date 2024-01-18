using BenchmarkDotNet.Attributes;

namespace RadSpec.Benchmark;

public class RandomTestBase
{
    protected Random _std = null!;
    protected Pcg32 _pcg = null!;

    [GlobalSetup]
    public void Setup()
    {
        _std = new Random();
        _pcg = new Pcg32();
    }
}

[DisassemblyDiagnoser(printInstructionAddresses: true)]
public class RandomFloat : RandomTestBase
{
    [Benchmark(Baseline = true)]
    public float Std()
    {
        return _std.NextSingle();
    }

    [Benchmark]
    public float Pcg32()
    {
        return _pcg.NextSingle();
    }
}

[DisassemblyDiagnoser(printInstructionAddresses: true)]
public class RandomInt : RandomTestBase
{
    [Benchmark(Baseline = true)]
    public int Std() => _std.Next();

    [Benchmark]
    public int Pcg32() => _pcg.NextInt32();
}

[DisassemblyDiagnoser(printInstructionAddresses: true)]
public class RandomRangeInt : RandomTestBase
{
    public static IEnumerable<object> GetNumber()
    {
        yield return 10;
        yield return 100000;
        yield return int.MaxValue;
    }

    [Benchmark(Baseline = true)]
    [ArgumentsSource(nameof(GetNumber))]
    public int Std(int max) => _std.Next(max);

    [Benchmark]
    [ArgumentsSource(nameof(GetNumber))]
    public int Pcg32(int max) => _pcg.NextInt32(max);
}
