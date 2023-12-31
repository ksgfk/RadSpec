namespace RadSpec.ImageReconstruction;

public class BoxReconstruction : IImageReconstruction
{
    public Vector2f Radius { get; }
    public float Integral => 2 * Radius.X * 2 * Radius.Y; // 积分就是盒子面积

    public BoxReconstruction() : this(Float2(0.5f)) { }

    public BoxReconstruction(Vector2f radius)
    {
        Radius = radius;
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
