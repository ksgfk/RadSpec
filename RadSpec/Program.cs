using RadSpec;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

const int width = 20, height = 10;
float aspect = (float)width / height;

var a = Transform4f.Scale(new Vector3f(0.5f, -0.5f * aspect, 1.0f));
Console.WriteLine("a");
Console.WriteLine(a.Mat);
Console.WriteLine(a.Inv);
Console.WriteLine();

var b = Transform4f.Translate(new Vector3f(1.0f, -1.0f / aspect, 0.0f));
Console.WriteLine("b");
Console.WriteLine(b.Mat);
Console.WriteLine(b.Inv);
Console.WriteLine();

var c = Transform4f.Perspective(Radian(30), 0.1f, 100.0f);
Console.WriteLine("c");
Console.WriteLine(c.Mat);
Console.WriteLine(c.Inv);
Console.WriteLine();

var f = a * b;
Console.WriteLine("f");
Console.WriteLine(f.Mat);
Console.WriteLine(f.Inv);
Console.WriteLine();

var g = f * c;
Console.WriteLine("g");
Console.WriteLine(g.Mat);
Console.WriteLine(g.Inv);
Console.WriteLine();

var e = Transform4f.Invert(g);
Console.WriteLine("e");
Console.WriteLine(e.Mat);
Console.WriteLine(e.Inv);
Console.WriteLine();

Console.WriteLine(e.ApplyAffine(new Vector3f(0, 0, 0)));
Console.WriteLine(e.ApplyAffine(new Vector3f(1, 1, 0)));
// for (int j = 0; j < height; j++)
// {
//     for (int i = 0; i < width; i++)
//     {
//         var cam = screenToCamera.ApplyAffine(new Vector3f((float)i / (width - 1), (float)j / (height - 1), 0));
//         Console.WriteLine($"i:{i} j:{j} {cam}");
//     }
// }

// using var img = new Image<Rgba32>(width, height);
// img.ProcessPixelRows((a) =>
// {
//     for (int j = 0; j < height; j++)
//     {
//         var row = a.GetRowSpan(j);
//         for (int i = 0; i < row.Length; i++)
//         {
//             var cam = screenToCamera.ApplyAffine(new Vector3f(i, j, 0));
//         }
//     }
// });
// img.SaveAsPng("/Users/admin/Desktop/test.png");

// using var img = new Image<Rgba32>(500, 500);
// img.ProcessPixelRows((a) =>
// {
//     for (int i = 0; i < 500; i++)
//     {
//         double k = i * 20 + 1000;
//         double lambdaMax = 2.8977721e-3 / k;
//         double normal = 1 / Blackbody(lambdaMax * 1e9, k);
//         double[] values = new double[Spectra.SampleCount];
//         for (int lambda = Spectra.LambdaMin; lambda <= Spectra.LambdaMax; lambda++)
//         {
//             values[lambda - Spectra.LambdaMin] = Blackbody(lambda, k) * normal;
//         }
//         DenselySampledSpectrum spec = new(values);
//         Xyz xyz = spec.ToXyz();
//         Vector3f rgb = RgbColorSpace.Srgb.ToRgb(xyz);
//         Console.WriteLine($"i:{i} xyz:{xyz} rgb:{rgb}");
//         foreach (ref var j in a.GetRowSpan(i))
//         {
//             j.R = (byte)(float.Clamp(ColorUtility.ToSrgb(rgb.X), 0, 1) * byte.MaxValue);
//             j.G = (byte)(float.Clamp(ColorUtility.ToSrgb(rgb.Y), 0, 1) * byte.MaxValue);
//             j.B = (byte)(float.Clamp(ColorUtility.ToSrgb(rgb.Z), 0, 1) * byte.MaxValue);
//             j.A = byte.MaxValue;
//         }
//     }
// });
// img.SaveAsPng("/Users/admin/Desktop/test.png");

// static double Blackbody(double lambda, double T)
// {
//     if (T <= 0)
//         return 0;
//     const double c = 299792458.0;
//     const double h = 6.62606957e-34;
//     const double kb = 1.3806488e-23;
//     double l = lambda * 1e-9f;
//     double Le = (2 * h * c * c) / (Math.Pow(l, 5) * (Math.Exp((h * c) / (l * kb * T)) - 1));
//     return Le;
// }
