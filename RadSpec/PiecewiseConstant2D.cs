namespace RadSpec;

/// <summary>
/// https://pbr-book.org/4ed/Sampling_Algorithms/Sampling_Multidimensional_Functions#Piecewise-Constant2DDistributions
/// 分段二维常数函数
/// 
/// （以后再细看吧，概率论和多元微积分都忘光了）
/// </summary>
public class PiecewiseConstant2D
{
    private readonly PiecewiseConstant1D[] _conditional;
    private readonly PiecewiseConstant1D _marginal;
    private readonly Vector2f _min;
    private readonly Vector2f _max;
    private readonly float _width;
    private readonly float _height;

    public float Integral => _marginal.Integral;
    public float Width => _width;
    public float Height => _height;
    public ReadOnlySpan<PiecewiseConstant1D> Conditional => _conditional;

    public PiecewiseConstant2D(ReadOnlySpan<float> f, int width, int height, Vector2f min, Vector2f max)
    {
        PiecewiseConstant1D[] conditional = new PiecewiseConstant1D[height];
        for (int i = 0; i < height; i++)
        {
            conditional[i] = new PiecewiseConstant1D(f.Slice(i * width, width), min.X, max.X);
        }
        float[] marginalFunc = new float[height];
        for (int i = 0; i < height; i++)
        {
            marginalFunc[i] = conditional[i].Integral;
        }
        PiecewiseConstant1D marginal = new(marginalFunc, min.Y, max.Y);
        _conditional = conditional;
        _marginal = marginal;
        _min = min;
        _max = max;
        _width = width;
        _height = height;
    }

    public (Vector2f value, float pdf, Vector2i offset) Sample(Vector2f xi)
    {
        var (v1, pdf1, offset1) = _marginal.Sample(xi.Y);
        var (v0, pdf0, offset0) = _conditional[offset1].Sample(xi.X);
        float pdf = pdf0 * pdf1;
        return (new(v0, v1), pdf, new(offset0, offset1));
    }

    public float Pdf(Vector2f value)
    {
        value = (value - _min) / (_max - _min);
        Vector2i offset = Floor(value * Float2(_width, _height)).AsInt2();
        return _conditional[offset.Y].Function[offset.X] / _marginal.Integral;
    }
}
