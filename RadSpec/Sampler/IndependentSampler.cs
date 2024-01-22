namespace RadSpec.Sampler;

public class IndependentSampler : ISampler
{
    private readonly long _seed;
    private readonly Pcg32 _rng;

    public int SampleCount { get; }

    public IndependentSampler(long seed, int sampleCount)
    {
        _seed = seed;
        SampleCount = sampleCount;
        _rng = new Pcg32(unchecked((ulong)_seed));
    }

    public ISampler Clone() => new IndependentSampler(_seed, SampleCount);

    public float Next1D() => _rng.NextSingle();

    public Vector2f Next2D()
    {
        float a = _rng.NextSingle();
        float b = _rng.NextSingle();
        return new Vector2f(a, b);
    }

    public void StartPixel(Vector2i pos, int sampleIndex, int dimension)
    {
        _rng.SetSeed((ulong)MurmurHash64A(_seed, pos.X, pos.Y));
    }
}
