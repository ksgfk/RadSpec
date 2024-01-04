namespace RadSpec.ImageReconstruction;

public class GaussianReconstruction : IImageReconstruction
{
    private readonly Vector2f _radius;
    private readonly float _sigma;
    private readonly float _expX;
    private readonly float _expY;

    public Vector2f Radius => _radius;

    public GaussianReconstruction(Vector2f radius, float sigma)
    {
        _radius = radius;
        _sigma = sigma;
        _expX = Gaussian(_radius.X, 0, sigma);
        _expX = Gaussian(_radius.Y, 0, sigma);
    }

    public float Eval(Vector2f x)
    {
        return float.Max(0, Gaussian(x.X, 0, _sigma) - _expX) * float.Max(0, Gaussian(x.Y, 0, _sigma) - _expY);
    }

    public (Vector2f, float) Sample(Vector2f xi)
    {
        throw new NotImplementedException();
    }
}
