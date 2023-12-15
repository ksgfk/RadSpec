using System.Numerics;
using RadSpec;
using RadSpec.Spectrum;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

using var img = new Image<Rgba32>(500, 500);
img.ProcessPixelRows((a) =>
{
    for (int i = 0; i < 500; i++)
    {
        double k = i * 20 + 1000;
        double lambdaMax = 2.8977721e-3 / k;
        double normal = 1 / Blackbody(lambdaMax * 1e9, k);
        double[] values = new double[Spectra.SampleCount];
        for (int lambda = Spectra.LambdaMin; lambda <= Spectra.LambdaMax; lambda++)
        {
            values[lambda - Spectra.LambdaMin] = Blackbody(lambda, k) * normal;
        }
        DenselySampledSpectrum spec = new(values);
        Xyz xyz = spec.ToXyz();
        Vector3 rgb = RgbColorSpace.Srgb.ToRgb(xyz);
        Console.WriteLine($"i:{i} xyz:{xyz} rgb:{rgb}");
        foreach (ref var j in a.GetRowSpan(i))
        {
            j.R = (byte)(float.Clamp(ColorUtility.ToSRGB(rgb.X), 0, 1) * byte.MaxValue);
            j.G = (byte)(float.Clamp(ColorUtility.ToSRGB(rgb.Y), 0, 1) * byte.MaxValue);
            j.B = (byte)(float.Clamp(ColorUtility.ToSRGB(rgb.Z), 0, 1) * byte.MaxValue);
            j.A = byte.MaxValue;
        }
    }
});
img.SaveAsPng("/Users/admin/Desktop/test.png");

static double Blackbody(double lambda, double T)
{
    if (T <= 0)
        return 0;
    const double c = 299792458.0;
    const double h = 6.62606957e-34;
    const double kb = 1.3806488e-23;
    double l = lambda * 1e-9f;
    double Le = (2 * h * c * c) / (Math.Pow(l, 5) * (Math.Exp((h * c) / (l * kb * T)) - 1));
    return Le;
}
