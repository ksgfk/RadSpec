using System.Numerics;

namespace RadSpec.Benchmark;

public static class VectorExtension
{
    public static Vector3 NextVector3(this Random rand)
    {
        return new Vector3(rand.NextSingle(), rand.NextSingle(), rand.NextSingle());
    }
}
