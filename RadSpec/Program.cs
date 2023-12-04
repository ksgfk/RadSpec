using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
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

using FileStream f = File.OpenRead("/Users/admin/Desktop/Fantasy Dragon.ply");
// using FileStream f = File.OpenRead("D:\\download\\Dragon-ply\\Dragon-ply.ply");
//using FileStream f = File.OpenRead("D:\\download\\Patchwork chair.ply");
PlyReader r = new(f);

var sw = Stopwatch.StartNew();
r.ReadHeader();
r.ReadData();
sw.Stop();
Console.WriteLine($"ms {sw.ElapsedMilliseconds}");
var vert = r.Header!.Elements.Find(i => i.Name == "vertex")!;
var face = r.Header!.Elements.Find(i => i.Name == "face")!;
using StreamWriter d = File.CreateText("/Users/admin/Desktop/Fantasy Dragon.obj");
var t1 = MemoryMarshal.Cast<byte, Vector3>(vert.Data);
foreach (var i in t1)
{
    d.WriteLine($"v {i.X} {i.Y} {i.Z}");
}
var t2 = MemoryMarshal.Cast<byte, F>(face.Data);
foreach (var i in t2)
{
    d.WriteLine($"f {i.a + 1} {i.b + 1} {i.c + 1}");
}
Console.WriteLine($"{t1.Length} {t2.Length}");

struct F
{
    public int a;
    public int b;
    public int c;
}
