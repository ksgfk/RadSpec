using System.Numerics;

namespace RadSpec;

public static class MathExt
{
    /// <summary>
    /// Pow(v, 2)
    /// </summary>
    public static T Sqr<T>(T v) where T : INumber<T> => v * v;
}
