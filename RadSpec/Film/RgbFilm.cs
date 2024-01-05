namespace RadSpec;

public class RgbFilm : IFilm
{
    private readonly Vector4d[] _pixels;

    public Vector2i Resolution { get; }
    public IImageReconstruction Reconstruction { get; }

    public RgbFilm(Vector2i resolution, IImageReconstruction reconstruction)
    {
        Resolution = resolution;
        Reconstruction = reconstruction;

        _pixels = new Vector4d[Resolution.X * Resolution.Y];
    }

    public void AddSample(Vector2i pos, SampledSpectrum value, SampledWavelength wavelength, float weight)
    {
        throw new NotImplementedException();
    }
}
