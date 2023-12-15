using RadSpec;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.PixelFormats;

// float[] t = [1.141f, 2.315f, 4.451f, 5.121f, 6.134f];

// int i = Array.BinarySearch(t, 2);
// int j = ~i;
// Console.WriteLine(i);
// Console.WriteLine(j);

var t = Spectra.CieIllumD65.ToXYZ();
Console.WriteLine(t);
