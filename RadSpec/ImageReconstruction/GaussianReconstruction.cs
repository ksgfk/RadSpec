namespace RadSpec.ImageReconstruction;

public class GaussianReconstruction : IImageReconstruction
{
    private readonly PiecewiseConstant2D _dist;
    private readonly float[] _eval;
    private readonly Vector2f _radius;
    private readonly float _sigma;
    private readonly float _expX;
    private readonly float _expY;

    private const int SampleCount = 32;

    public Vector2f Radius => _radius;

    public GaussianReconstruction(Vector2f radius, float sigma)
    {
        _radius = radius;
        _sigma = sigma;
        _expX = Gaussian(_radius.X, 0, sigma);
        _expY = Gaussian(_radius.Y, 0, sigma);

        Vector2f min = -_radius;
        Vector2f max = _radius;
        Vector2i sample = Floor(_radius * new Vector2f(SampleCount)).AsInt2();
        float[] pdf = new float[sample.X * sample.Y];
        for (int j = 0; j < sample.Y; j++)
        {
            for (int i = 0; i < sample.X; i++)
            {
                Vector2f offset = (new Vector2f(i, j) + new Vector2f(0.5f)) / sample.AsFloat2();
                Vector2f p = Lerp(min, max, offset);
                pdf[i + j * sample.X] = Eval(p);
            }
        }
        _dist = new PiecewiseConstant2D(pdf, sample.X, sample.Y, min, max);
        _eval = pdf;
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
