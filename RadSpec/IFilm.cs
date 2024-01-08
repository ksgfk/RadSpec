namespace RadSpec;

public interface IFilm
{
    Vector2i Resolution { get; }

    IImageReconstruction Reconstruction { get; }

    void AddSample(Vector2i pos, SampledSpectrum value, SampledWavelength wavelength, float weight);

    Vector3f GetPixelRgb(Vector2i pos);
}
