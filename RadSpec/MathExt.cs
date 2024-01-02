using System.Numerics;

namespace RadSpec;

public static class MathExt
{
    /// <summary>
    /// Pow(v, 2)
    /// </summary>
    public static T Sqr<T>(T v) where T : INumber<T> => v * v;

    // CreateChecked系列生成的代码就是piece of shit, 这个接口的静态虚函数在数学运算还是基本没法用
    // public static T Degree<T>(T radian) where T : IFloatingPoint<T> => T.CreateChecked(180) / T.Pi * radian;
    public static float Degree(float radian) => 180 / float.Pi * radian;
    public static double Degree(double radian) => 180 / double.Pi * radian;

    // public static T Radian<T>(T degree) where T : IFloatingPoint<T> => T.Pi / T.CreateChecked(180) * degree;
    public static float Radian(float degree) => float.Pi / 180 * degree;
    public static double Radian(double degree) => double.Pi / 180 * degree;
}
