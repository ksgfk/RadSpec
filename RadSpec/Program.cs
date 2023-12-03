using System.Diagnostics;
using RadSpec;

// using FileStream f = File.OpenRead("/Users/admin/Downloads/buddha.obj");
// using WavefrontObjReader reader = new(f);

// Stopwatch sw = Stopwatch.StartNew();

// reader.Read();

// sw.Stop();

// Console.WriteLine(sw.ElapsedMilliseconds);

// if (reader.ErrorMessage != null)
// {
//     Console.WriteLine(reader.ErrorMessage);
// }
// Console.WriteLine(reader.Positions.Count);
// Console.WriteLine(reader.Faces.Count);

using FileStream f = File.OpenRead("D:\\download\\Dragon-ply\\Dragon-ply.ply");
//using FileStream f = File.OpenRead("D:\\download\\Patchwork chair.ply");
PlyReader r = new(f);
r.ReadHeader();
r.ReadData();
Console.WriteLine();
