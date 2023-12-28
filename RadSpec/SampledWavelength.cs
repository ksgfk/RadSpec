namespace RadSpec;

public readonly struct SampledWavelength
{
    public readonly Vector4f Lambda;
    public readonly Vector4f Pdf;

    public SampledWavelength(Vector4f lambda, Vector4f pdf)
    {
        Lambda = lambda;
        Pdf = pdf;
    }
}
