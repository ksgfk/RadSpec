namespace RadSpec.Reconstruction;

public class BoxReconstruction : IImageReconstruction
{
    public Vector2f Radius { get; }

    public BoxReconstruction() : this(new Vector2f(0.5f)) { }

    public BoxReconstruction(Vector2f radius)
    {
        Radius = radius;
    }

    public float Eval(Vector2f x)
    {
        return (float.Abs(x.X) <= Radius.X && float.Abs(x.Y) <= Radius.Y) ? 1 : 0;
    }

    public (Vector2f, float) Sample(Vector2f xi)
    {
        Vector2f p = Lerp(-Radius, Radius, xi);
        return (p, 1);
    }
}
