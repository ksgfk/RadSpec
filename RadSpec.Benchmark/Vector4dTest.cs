// using System.Numerics;
// using BenchmarkDotNet.Attributes;

// namespace RadSpec.Benchmark;

// [DisassemblyDiagnoser(printInstructionAddresses: true)]
// public class Vector4dTest
// {
//     private struct Scalar4d
//     {
//         public double X, Y, Z, W;
//     }

//     public const int Len = 1024;

//     public static IEnumerable<object[]> StdVecData()
//     {
//         var a = new Vector3[Len];
//         for (int i = 0; i < a.Length; i++)
//         {
//             a[i] = Random.Shared.NextVector3();
//         }
//         var b = new Vector3[Len];
//         for (int i = 0; i < b.Length; i++)
//         {
//             b[i] = Random.Shared.NextVector3();
//         }
//         yield return new object[] { a, b };
//     }

//     public static IEnumerable<object[]> RadVecData()
//     {
//         var a = new Vector3f[Len];
//         for (int i = 0; i < a.Length; i++)
//         {
//             a[i] = Random.Shared.NextVector3();
//         }
//         var b = new Vector3f[Len];
//         for (int i = 0; i < b.Length; i++)
//         {
//             b[i] = Random.Shared.NextVector3();
//         }
//         yield return new object[] { a, b };
//     }


//     [Benchmark(Baseline = true)]
//     [ArgumentsSource(nameof(StdVecData))]
//     public Vector3[] StdAdd(Vector3[] a, Vector3[] b)
//     {
//         if (a.Length != b.Length)
//         {
//             return null!;
//         }
//         int len = a.Length;
//         Vector3[] t = new Vector3[len];
//         for (int i = 0; i < len; i++)
//         {
//             t[i] = a[i] + b[i];
//         }
//         return t;
//     }

//     [Benchmark]
//     [ArgumentsSource(nameof(RadVecData))]
//     public Vector3f[] RadAdd(Vector3f[] a, Vector3f[] b)
//     {
//         if (a.Length != b.Length)
//         {
//             return null!;
//         }
//         int len = a.Length;
//         Vector3f[] t = new Vector3f[len];
//         for (int i = 0; i < len; i++)
//         {
//             t[i] = a[i] + b[i];
//         }
//         return t;
//     }
// }
