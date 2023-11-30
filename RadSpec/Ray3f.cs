using System.Numerics;

namespace RadSpec;

public readonly struct Ray3f
{
    public readonly Vector3 O;
    public readonly float MaxT;
    public readonly Vector3 D;
    public readonly float Time;
    public readonly Wavelength Wave;

    public Ray3f(Vector3 o, Vector3 d, float maxT, float time, Wavelength wave)
    {
        O = o;
        D = d;
        MaxT = maxT;
        Time = time;
        Wave = wave;
    }
}
