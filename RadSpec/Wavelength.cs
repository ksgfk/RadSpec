using System.Numerics;

namespace RadSpec;

public readonly struct Wavelength
{
    public readonly Vector4 Lambda;
    public readonly Vector4 Pdf;

    public Wavelength(Vector4 lambda, Vector4 pdf)
    {
        Lambda = lambda;
        Pdf = pdf;
    }
}
