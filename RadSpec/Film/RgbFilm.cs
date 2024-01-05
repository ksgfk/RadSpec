using RadSpec.Spectrum;

namespace RadSpec;

public class RgbFilm : IFilm
{
    private readonly Vector4d[] _pixels;

    public Vector2i Resolution { get; }
    public DenselySampledSpectrum RedResponse { get; }
    public DenselySampledSpectrum GreenResponse { get; }
    public DenselySampledSpectrum BlueResponse { get; }
    public IImageReconstruction Reconstruction { get; }

    public RgbFilm(
        Vector2i resolution,
        IImageReconstruction reconstruction,
        ISpectrum redResponse,
        ISpectrum greenResponse,
        ISpectrum blueResponse)
    {
        Resolution = resolution;
        Reconstruction = reconstruction;
        RedResponse = new DenselySampledSpectrum(redResponse);
        GreenResponse = new DenselySampledSpectrum(greenResponse);
        BlueResponse = new DenselySampledSpectrum(blueResponse);

        _pixels = new Vector4d[Resolution.X * Resolution.Y];
    }

    public void AddSample(Vector2i pos, SampledSpectrum value, SampledWavelength wavelength, float weight)
    {
        var l = value / wavelength.Pdf;
        Vector4f rBar = RedResponse.Eval(wavelength).AsFloat4();
        Vector4f gBar = GreenResponse.Eval(wavelength).AsFloat4();
        Vector4f bBar = BlueResponse.Eval(wavelength).AsFloat4();
        float r = Average(rBar * l);
        float g = Average(gBar * l);
        float b = Average(bBar * l);
        ref Vector4d pixel = ref _pixels[pos.X + pos.Y * Resolution.X];
        pixel.X += r * weight;
        pixel.Y += g * weight;
        pixel.Z += b * weight;
        pixel.W += weight;
    }
}
