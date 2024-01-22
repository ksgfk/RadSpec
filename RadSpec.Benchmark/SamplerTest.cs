using BenchmarkDotNet.Attributes;
using RadSpec.Sampler;

namespace RadSpec.Benchmark;

[DisassemblyDiagnoser(printInstructionAddresses: true)]
public class StratifiedSamplerTest
{
    private IndependentSampler _independent = null!;
    private StratifiedSampler _stratified = null!;

    [GlobalSetup]
    public void Setup()
    {
        _independent = new IndependentSampler(DateTime.Now.Ticks, 100);
        _stratified = new StratifiedSampler(100, DateTime.Now.Ticks, true);
    }

    [Benchmark(Baseline = true)]
    public Vector2f Independent() => _independent.Next2D();

    [Benchmark]
    public Vector2f Stratified() => _stratified.Next2D();
}
