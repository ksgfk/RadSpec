namespace RadSpec;

public readonly struct Ray3f(Vector3f o, Vector3f d, float maxT, float time, SampledWavelength wavelength)
{
    public readonly Vector3f O = o;
    public readonly float MaxT = maxT;
    public readonly Vector3f D = d;
    public readonly float Time = time;
    public readonly SampledWavelength Wavelength = wavelength;

    public Vector3f At(float t) => Fma(D, t, O);
}
