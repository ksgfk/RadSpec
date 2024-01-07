namespace RadSpec.ImageReconstruction;

public class BoxReconstruction : IImageReconstruction
{
    public Vector2f Radius { get; }
    public float Integral { get; }

    public BoxReconstruction() : this(Float2(0.5f)) { }

    public BoxReconstruction(Vector2f radius)
    {
        Radius = radius;
        Integral = 2 * radius.X * 2 * radius.Y; // 积分就是盒子面积
    }

    public float Eval(Vector2f x)
    {
        return (float.Abs(x.X) <= Radius.X && float.Abs(x.Y) <= Radius.Y) ? 1 : 0;
    }

    public (Vector2f value, float weight) Sample(Vector2f xi)
    {
        Vector2f p = Lerp(-Radius, Radius, xi);
        return (p, 1);
    }
}
