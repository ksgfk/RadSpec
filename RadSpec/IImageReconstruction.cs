namespace RadSpec;

public interface IImageReconstruction
{
    Vector2f Radius { get; }

    float Eval(Vector2f x);

    (Vector2f, float) Sample(Vector2f xi);
}
