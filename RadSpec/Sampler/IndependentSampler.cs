namespace RadSpec.Sampler;

public class IndependentSampler : ISampler
{
    private readonly Pcg32 _rng;

    public int SampleCount { get; }

    public IndependentSampler(ulong seed, int sampleCount) : this(new Pcg32(offset: seed), sampleCount) { }

    private IndependentSampler(Pcg32 rng, int sampleCount)
    {
        _rng = rng;
        SampleCount = sampleCount;
    }

    public void Advance() { }

    public ISampler Clone() => new IndependentSampler(new Pcg32(_rng), SampleCount);

    public float Next1D() => _rng.NextSingle();

    public Vector2f Next2D()
    {
        float a = _rng.NextSingle();
        float b = _rng.NextSingle();
        return new Vector2f(a, b);
    }

    public void SetSeed(int seed) => _rng.SetSeed((ulong)seed);
}
