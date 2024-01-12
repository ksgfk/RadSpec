using RadSpec;
using RadSpec.Camera;
using RadSpec.ImageReconstruction;
using RadSpec.Shape;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

BoundingBox3f a = new(Float3(-1), Float3(1));
Vector3f p = Float3(0,0,-2);
Vector3f look = Float3(-1,-1,-1);
var t = a.RayIntersect(p, Normalize(look - p));
Console.WriteLine(t);

// BoundingBox3f b = new(Float3(-1, -1, 1), Float3(0, 0, 2));
// float t = a.Distance(b);
// Console.WriteLine(t);

// Sphere sphere = new(3, Float3(5, -1, 0), Matrix4x4f.Identity);
// const int width = 1280, height = 720;
// ThinLensCamera c = new(new(0, 0, -10), new(0, 0, 0), new(0, 1, 0), 90, (float)width / height, 0.1f, 100);
// using var img = new Image<Rgba32>(width, height);
// img.ProcessPixelRows((a) =>
// {
//     for (int j = 0; j < height; j++)
//     {
//         var row = a.GetRowSpan(j);
//         for (int i = 0; i < row.Length; i++)
//         {
//             // float x = -1 + 2.0f / height * j;
//             // float y = -1 + 2.0f / width * i;
//             // Ray3f ray = new(Float3(x, y, -10), Float3(0, 0, 1), float.PositiveInfinity, 0, default);
//             Ray3f ray = c.SampleRay(0, default, new Vector2f(i / (width - 1.0f), j / (height - 1.0f)), default);
//             var rir = sphere.RayIntersect(ray, 0);
//             if (rir.IsHit)
//             {
//                 var si = sphere.ComputeSurfaceInteraction(ray, in rir);
//                 // Console.WriteLine($"{si.P} {si.N} {si.UV} {si.dPdU} {si.dPdV} {si.dNdU} {si.dNdV}");
//                 // row[i] = new Rgba32(255, 255, 255, 255);
//                 float t = float.Max(0, Dot(si.N, Normalize(Float3(1, 1, -1))));
//                 row[i] = new Rgba32(t, t, t, 255);
//             }
//             else
//             {
//                 row[i] = new Rgba32(0, 0, 0, 255);
//             }
//         }
//     }
// });
// img.SaveAsPng("/Users/admin/Desktop/test.png");
// Console.WriteLine("DONE");

// static (bool, float) SphereIntersect(Ray3f ray, Vector3f center, float radius)
// {
//     Vector3f o = ray.O - center;
//     float a = LengthSquared(ray.D);
//     float b = 2 * Dot(o, ray.D);
//     float c = LengthSquared(o) - Sqr(radius);
//     var (isFind, nearT, farT) = SolveQuadratic(a, b, c);
//     bool outBounds = !(nearT <= ray.MaxT && farT >= 0);
//     bool inBounds = nearT < 0 && farT > ray.MaxT;
//     bool isHit = isFind && !outBounds && !inBounds;
//     float t = nearT < 0 ? farT : nearT;
//     return (isHit, t);
// }

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
