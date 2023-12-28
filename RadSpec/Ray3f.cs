namespace RadSpec;

public readonly struct Ray3f
{
    public readonly Vector3f O;
    public readonly float MaxT;
    public readonly Vector3f D;
    public readonly float Time;
    public readonly SampledWavelength Wavelength;

    public Ray3f(Vector3f o, Vector3f d, float maxT, float time, SampledWavelength wavelength)
    {
        O = o;
        D = d;
        MaxT = maxT;
        Time = time;
        Wavelength = wavelength;
    }
}
