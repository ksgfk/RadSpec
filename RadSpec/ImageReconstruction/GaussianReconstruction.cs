namespace RadSpec.ImageReconstruction;

public class GaussianReconstruction : IImageReconstruction
{
    private readonly PiecewiseConstant2D _dist;
    private readonly Vector2f _radius;
    private readonly float _sigma;
    private readonly float _expX;
    private readonly float _expY;

    private const int SampleCount = 32;

    public Vector2f Radius => _radius;
    public float Integral => throw new NotImplementedException();

    public GaussianReconstruction(Vector2f radius, float sigma)
    {
        _radius = radius;
        _sigma = sigma;
        _expX = Gaussian(_radius.X, 0, sigma);
        _expY = Gaussian(_radius.Y, 0, sigma);

        /**
         * https://pbr-book.org/4ed/Sampling_and_Reconstruction/Image_Reconstruction#FilterSampler
         * 
         * 并非所有过滤器都能够轻松地从其过滤器函数的分布中进行采样。高斯分布的cdf无法用初等函数表示，因此找不到反函数，反演法无效
         * 因此采样滤波器权重转换为分段函数作为pdf
         * （以下是pbrt机翻）
         * Sample() 方法的实现中有两个重要的细节。 首先，该实现不使用 Filter::Evaluate() 来评估过滤器函数，而是使用 f 中的表格版本。 
         * 通过使用滤波器函数的分段常数近似，可以确保对于常数 c，采样点 p' 的返回权重 f/p 始终为 +-c。 
         * 如果不这样做，由于采样分布与过滤器函数不完全成比例，非常量过滤器函数的返回权重将会发生变化 - 请参见图 8.50，它说明了该问题。
         * 第二个重要细节是 PiecewiseConstant2D::Sample() 返回的样本的整数坐标用于索引 f 以进行过滤函数评估。
         * 相反，如果将点 p 在每个维度上按 f 数组的大小放大并转换为整数，则由于浮点舍入误差，结果有时会与 PiecewiseConstant2D 采样期间计算的整数坐标不同。同样，会产生方差，因为 f/p 的比率可能不是 +-c。
         * 
         * 总之为了避免采样、浮点数带来的误差，这是好的方法（
         */
        Vector2f min = -_radius;
        Vector2f max = _radius;
        Vector2i sample = Floor(_radius * Float2(SampleCount)).AsInt2();
        float[] pdf = new float[sample.X * sample.Y];
        for (int j = 0; j < sample.Y; j++)
        {
            for (int i = 0; i < sample.X; i++)
            {
                Vector2f offset = (Float2(i, j) + Float2(0.5f)) / sample.AsFloat2();
                Vector2f p = Lerp(min, max, offset);
                pdf[i + j * sample.X] = Eval(p);
            }
        }
        _dist = new PiecewiseConstant2D(pdf, sample.X, sample.Y, min, max);
    }

    public float Eval(Vector2f x)
    {
        return float.Max(0, Gaussian(x.X, 0, _sigma) - _expX) * float.Max(0, Gaussian(x.Y, 0, _sigma) - _expY);
    }

    public (Vector2f value, float weight) Sample(Vector2f xi)
    {
        var (value, pdf, offset) = _dist.Sample(xi);
        float weight = _dist.Conditional[offset.Y].Function[offset.X] / pdf;
        return (value, weight);
    }
}
