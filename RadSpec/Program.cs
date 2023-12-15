using RadSpec;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.PixelFormats;

var t = Spectra.CieIllumD65.ToXyz();
Console.WriteLine(t);

RgbColorSpace srgb = new(
    new(0.64f, 0.33f),
    new(0.3f, 0.6f),
    new(0.15f, 0.06f),
    Spectra.CieIllumD65);
Console.WriteLine(srgb.ToRgbMatrix);
Console.WriteLine(srgb.ToXyzMatrix);
