namespace RadSpec.Sampler;

/**
 * 分层随机抽样是一种抽样方法。将总体划分为多个较小的层，各个层内的成员具有共同特征。再从各个层内抽样
 *
 * https://pbr-book.org/4ed/Sampling_and_Reconstruction/Stratified_Sampler
 * https://pbr-book.org/4ed/Monte_Carlo_Integration/Improving_Efficiency#StratifiedSampling
 * 经典且有效的方差减少技术系列基于仔细放置样本，以便更好地捕获被积函数的特征（或者更准确地说，不太可能错过重要特征）
 * 分层抽样将积分域细分为不重叠的区域。每个区域称为一个层 \Lambda_i，它们必须完全覆盖原始域 \Lambda
 * 为了从 \Lambda 中抽取样本，我们将根据每个层内的密度从每个 \Lambda_i 中抽取样本
 * 每层的样本使用蒙特卡洛估计出 F_i 后乘以类似权重一样的值（fractional volume of stratum i）再求和，就得到了原始 F
 * 层次也有维度灾难，D 维度中的完全分层（每个维度有 S 层）需要 S^D 个样本
 * 选择分层的维度时，应采用对被积函数值的影响最相关的维度进行分层的方式
 */
public class StratifiedSampler : ISampler
{
    public int SampleCount => throw new NotImplementedException();

    public void Advance()
    {
        throw new NotImplementedException();
    }

    public ISampler Clone()
    {
        throw new NotImplementedException();
    }

    public float Next1D()
    {
        throw new NotImplementedException();
    }

    public Vector2f Next2D()
    {
        throw new NotImplementedException();
    }

    public void SetSeed(int seed)
    {
        throw new NotImplementedException();
    }
}
