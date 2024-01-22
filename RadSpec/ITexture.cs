namespace RadSpec;

public interface ITexture
{
    Vector2i Resolution { get; }

    SampledSpectrum Eval(ref readonly SurfaceInteraction si);

    SampledWavelength SampleSpectrum(ref readonly SurfaceInteraction si, Vector4f xi);
}
