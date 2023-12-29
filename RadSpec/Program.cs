using System.Numerics;
using RadSpec;
using RadSpec.Spectrum;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

for (int i = 0; i < 50; i++)
{
    var lambda = Warp.UniformToVisibleWavelength(i * 0.01f);
    var pdf = Warp.UniformToVisibleWavelengthPdf(lambda);
    Console.WriteLine($"sample lambda: {lambda}, pdf: {pdf}");
}
{
    var a = new Matrix4x4f(
        1, 3, 2, 4,
        5, -2, 1, 4,
        5, 1, 6, 7,
        1, 4, 3, 5);
    var b = new Matrix4x4(
        1, 3, 2, 4,
        5, -2, 1, 4,
        5, 1, 6, 7,
        1, 4, 3, 5);
    var c = new Vector4f(-2, 5, 3, 7);

    Console.WriteLine(c * a);
    Console.WriteLine(Vector4.Transform(c.Value, b));
}
{
    var a = new Matrix4x4f(
        1, 3, 2, 0,
        5, -2, 1, 0,
        5, 1, 6, 0,
        0, 0, 0, 1);
    var b = new Matrix4x4f(
        -2, 1, 5, 0,
        1, -2, 5, 0,
        3, 3, 4, 0,
        0, 0, 0, 1);
    var c = new Matrix3x3f(
        1, 3, 2,
        5, -2, 1,
        5, 1, 6);
    var d = new Matrix3x3f(
        -2, 1, 5,
        1, -2, 5,
        3, 3, 4);
    Console.WriteLine(a * b);
    Console.WriteLine(c * d);
}
{
    object t1 = new Vector2f(1, 2);
    var t2 = new Vector2f(1, 2);
    Console.WriteLine(t2.Equals(t1));
}
{
    object t1 = new Vector3f(1, 2, 3);
    var t2 = new Vector3f(1, 2, 3);
    Console.WriteLine(t2.Equals(t1));
}
{
    object t1 = new Vector4f(1, 2, 3, 4);
    var t2 = new Vector4f(1, 2, 3, 4);
    Console.WriteLine(t2.Equals(t1));
}
{
    var a = new Matrix3x3f(
        -2, 1, 5,
        1, -2, 5,
        3, 3, 4);
    var b = new Matrix3x3f(
        -2, 1, 5,
        1, -2, 5,
        3, 3, 4);
    Console.WriteLine(a == b);
    Console.WriteLine(a != b);
}
{
    var a = new Matrix3x3f(
        -1, 1, 5,
        1, -2, 5,
        3, 3, 4);
    var b = new Matrix3x3f(
        -2, 1, 5,
        1, -2, 5,
        3, 3, 4);
    Console.WriteLine(a == b);
    Console.WriteLine(a != b);
}

// double r = 1 / 0.0072 * Math.Tanh(0.0072 * (830 - 538)) - 1 / 0.0072 * Math.Tanh(0.0072 * (360 - 538));
// double c = 1 /r;
// Console.WriteLine(c);
// double a = 0.0072;
// double d = c / a * Math.Tanh(a * (360 - 538));
// Console.WriteLine(c / 0.0072);
// Console.WriteLine(d);
// Console.WriteLine(a * 538);

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
        Vector3f rgb = RgbColorSpace.Srgb.ToRgb(xyz);
        Console.WriteLine($"i:{i} xyz:{xyz} rgb:{rgb}");
        foreach (ref var j in a.GetRowSpan(i))
        {
            j.R = (byte)(float.Clamp(ColorUtility.ToSrgb(rgb.X), 0, 1) * byte.MaxValue);
            j.G = (byte)(float.Clamp(ColorUtility.ToSrgb(rgb.Y), 0, 1) * byte.MaxValue);
            j.B = (byte)(float.Clamp(ColorUtility.ToSrgb(rgb.Z), 0, 1) * byte.MaxValue);
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
