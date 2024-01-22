namespace RadSpec.Sampler;

/**
 * 分层随机抽样是一种抽样方法。将总体划分为多个较小的层，各个层内的成员具有共同特征。再从各个层内抽样
 *
 * https://pbr-book.org/4ed/Sampling_and_Reconstruction/Stratified_Sampler
 * https://pbr-book.org/4ed/Monte_Carlo_Integration/Improving_Efficiency#StratifiedSampling
 * （以下都是机翻）
 * 经典且有效的方差减少技术系列基于仔细放置样本，以便更好地捕获被积函数的特征（或者更准确地说，不太可能错过重要特征）
 * 分层抽样将积分域细分为不重叠的区域。每个区域称为一个层 \Lambda_i，它们必须完全覆盖原始域 \Lambda
 * 为了从 \Lambda 中抽取样本，我们将根据每个层内的密度从每个 \Lambda_i 中抽取样本
 * 每层的样本使用蒙特卡洛估计出 F_i 后乘以类似权重一样的值（fractional volume of stratum i）再求和，就得到了原始 F
 * 层次也有维度灾难，D 维度中的完全分层（每个维度有 S 层）需要 S^D 个样本
 * 选择分层的维度时，应采用对被积函数值的影响最相关的维度进行分层的方式
 *
 * 好了，写了这么多，实际上pbrt里使用的分层只有将采样空间[0,1)划分成更小的方块，让点的分布更加均匀，分块和蒙特卡洛采样次数关联
 */
public class StratifiedSampler : ISampler
{
    private readonly long _seed;
    private readonly bool _isJitter;
    private readonly int _resolution;
    private readonly Pcg32 _rng;

    private Vector2i _pixel;
    private int _sampleIndex;
    private int _dim;

    public int SampleCount { get; }

    public StratifiedSampler(int sampleCount, long seed, bool isJitter)
    {
        SampleCount = sampleCount;
        _seed = seed;
        _isJitter = isJitter;
        _resolution = 1;
        while (Sqr(_resolution) < SampleCount)
        {
            _resolution++;
        }
        if (Sqr(_resolution) != SampleCount)
        {
            throw new ArgumentOutOfRangeException(nameof(sampleCount));
        }
        _rng = new Pcg32(unchecked((ulong)_seed));
    }

    public StratifiedSampler(StratifiedSampler other)
    {
        SampleCount = other.SampleCount;
        _seed = other._seed;
        _isJitter = other._isJitter;
        _rng = new Pcg32(other._rng);
        _pixel = other._pixel;
        _sampleIndex = other._sampleIndex;
        _dim = other._dim;
    }

    public ISampler Clone() => new StratifiedSampler(this);

    public float Next1D()
    {
        long hash = MurmurHash64A(0, _pixel.X << 32 | _pixel.Y, _dim, _seed);
        int stratum = (int)PermuteKensler((uint)_sampleIndex, (uint)SampleCount, (uint)hash);
        _dim++;
        float delta = _isJitter ? _rng.NextSingle() : 0.5f;
        return (stratum + delta) / SampleCount;
    }

    public Vector2f Next2D()
    {
        long hash = MurmurHash64A(0, _pixel.X << 32 | _pixel.Y, _dim, _seed);
        int stratum = (int)PermuteKensler((uint)_sampleIndex, (uint)SampleCount, (uint)hash);
        _dim += 2;
        int x = stratum % _resolution;
        int y = stratum / _resolution;
        float dx = _isJitter ? _rng.NextSingle() : 0.5f;
        float dy = _isJitter ? _rng.NextSingle() : 0.5f;
        return new Vector2f(x + dx, y + dy) / _resolution;
    }

    public void StartPixel(Vector2i pos, int sampleIndex, int dimension)
    {
        _pixel = pos;
        _sampleIndex = sampleIndex;
        _dim = dimension;
        _rng.SetSeed((ulong)MurmurHash64A(_seed, pos.X, pos.Y));
    }
}
