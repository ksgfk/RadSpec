using System.Numerics;

namespace RadSpec;

public class RgbColorSpace
{
    public Vector2 R { get; }
    public Vector2 G { get; }
    public Vector2 B { get; }

    public RgbColorSpace(Vector2 r, Vector2 g, Vector2 b)
    {
        R = r;
        G = g;
        B = b;
    }
}
