using System.Numerics;

namespace RadSpec;

public readonly struct SampledWavelength
{
    public readonly Vector4 Lambda;
    public readonly Vector4 Pdf;

    public SampledWavelength(Vector4 lambda, Vector4 pdf)
    {
        Lambda = lambda;
        Pdf = pdf;
    }
}
