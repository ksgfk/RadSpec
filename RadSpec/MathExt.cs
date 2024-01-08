using System.Numerics;

namespace RadSpec;

public static partial class MathExt
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

    public static (bool, float, float) SolveQuadratic(float a, float b, float c)
    {
        bool isLinearCase = a == 0.0f;
        bool validLinear = isLinearCase && b != 0.0f;
        float x0 = -c / b, x1 = -c / b;
        float discrim = b * b - 4 * a * c;
        bool validQuadratic = !isLinearCase && (discrim >= 0.0);
        if (validQuadratic)
        {
            float rootDiscrim = float.Sqrt(discrim);
            float temp = -0.5f * (b + float.CopySign(rootDiscrim, b));
            float x0p = temp / a;
            float x1p = c / temp;
            float x0m = float.Min(x0p, x1p);
            float x1m = float.Max(x0p, x1p);
            x0 = isLinearCase ? x0 : x0m;
            x1 = isLinearCase ? x0 : x1m;
        }
        return (validLinear || validQuadratic, x0, x1);
    }

    public static float Gaussian(float x, float mu, float sigma)
    {
        return 1 / float.Sqrt(2 * float.Pi * sigma * sigma) * float.Exp(-Sqr(x - mu) / (2 * sigma * sigma));
    }

    public static float GaussianIntegral(float x0, float x1, float mu, float sigma)
    {
        float sigmaRoot2 = sigma * float.Sqrt(2);
        return 0.5f * (Erf((mu - x0) / sigmaRoot2) - Erf((mu - x1) / sigmaRoot2));
    }

    public static float Erf(float x) => ErfMitsuba(x);

    public static double Erf(double x) => ErfMitsuba(x);

    public static decimal ErfTaylor(decimal x)
    {
        const int level = 24;
        decimal a = (decimal)(2.0 / double.Sqrt(double.Pi));
        decimal result = 0;
        for (int n = 0; n <= level; n++)
        {
            decimal mol = (n % 2 == 0 ? 1 : -1) * MyPow(x, 2 * n + 1);
            decimal deno = Factorial(n) * (2 * n + 1);
            result += mol / deno;
        }
        return a * result;

        static decimal Factorial(decimal x)
        {
            decimal r = 1;
            for (decimal i = 1; i <= x; i++)
            {
                r *= i;
            }
            return r;
        }
        static decimal MyPow(decimal x, decimal y)
        {
            decimal result = 1;
            for (int i = 0; i < y; i++)
            {
                result *= x;
            }
            return result;
        }
    }

    public static double ErfFortran77(double x)
    {
        // Numerical Recipes in Fortran 77: The Art of Scientific Computing (ISBN 0-521-43064-X), 1992, page 214, Cambridge University Press.
        double t = 1 / (1 + 0.5 * double.Abs(x));
        double v = -Sqr(x) -
            1.26551223 +
            1.00002368 * t +
            0.37409196 * Sqr(t) +
            0.09678418 * Pow3(t) -
            0.18628806 * Pow4(t) +
            0.27886807 * Pow5(t) -
            1.13520398 * Pow6(t) +
            1.48851587 * Pow7(t) -
            0.82215223 * Pow8(t) +
            0.17087277 * Pow9(t);
        double tuo = t * double.Exp(v);
        return x >= 0 ? 1 - tuo : tuo - 1;
    }

    public static float ErfMitsuba(float x)
    {
        // https://github.com/mitsuba-renderer/drjit/blob/4e9b718a7c3012c61b51196afde960e0c0aa160d/include/drjit/math.h#L1473
        const double a0 = 1.1283791065216064453125, a1 = -0.3761232197284698486328125,
            a2 = 0.11280174553394317626953125, a3 = -0.0267112739384174346923828125,
            a4 = 0.004917544312775135040283203125, a5 = -0.000563142239116132259368896484375;
        const double b0 = -1.62828218936920166015625, b1 = -0.916246235370635986328125,
            b2 = -0.1535955369472503662109375, b3 = 0.034695319831371307373046875,
            b4 = -0.0054983342997729778289794921875, b5 = 0.0005341702490113675594329833984375,
            b6 = -2.3643011445528827607631683349609e-05;
        float xa = float.Abs(x), x2 = Sqr(x);
        float c0 = Estrin(x2, (float)a0, (float)a1, (float)a2, (float)a3, (float)a4, (float)a5);
        float c1 = Estrin(xa, (float)b0, (float)b1, (float)b2, (float)b3, (float)b4, (float)b5, (float)b6);
        float xb = 1 - float.Exp2(c1 * xa);
        return xa < 1 ? x * c0 : float.CopySign(float.IsFinite(xb) ? xb : 1, x);
    }

    public static double ErfMitsuba(double x)
    {
        // https://github.com/mitsuba-renderer/drjit/blob/4e9b718a7c3012c61b51196afde960e0c0aa160d/include/drjit/math.h#L1473
        const double m0 = 1.1283791670955125585606992899557, m1 = -0.37612638903183515104444722965127,
            m2 = 0.11283791670944179341695701168646, m3 = -0.02686617064311137836885023943978,
            m4 = 0.0052239776061183892941208739557624, m5 = -0.0008548325929314459434293915762737,
            m6 = 0.0001205529357690069807698338144597, m7 = -1.4924712302009882718112875055194e-05,
            m8 = 1.644713157127994174807206900768e-06, m9 = -1.6206313758500693870599990240727e-07,
            m10 = 1.371098039806271639693233826713e-08, m11 = -7.7794684890268289513111458479896e-10;
        const double n0 = -1.6279070192506333025761477983906, n1 = -3.0913875441905238616868700773921,
            n2 = -2.7782812837275869810582662466913, n3 = -1.4815783978163847844911060747108,
            n4 = -0.49526071908541530186553814019135, n5 = -0.10079526078777582831502712679139,
            n6 = -0.010893819805040573164833794805872, n7 = -0.00039123080868543183086574743789754;
        const double t0 = 1, t1 = 1.3348055144562560592191857722355,
            t2 = 0.86249541285595099360250515019288, t3 = 0.31887932626961806859000603253662,
            t4 = 0.068581121190434468637420195591403, t5 = 0.0075454453261318936430068760046197,
            t6 = 0.00027123875407616894646356531239917, t7 = -4.7260463372207654373298275842126e-10;
        double xa = double.Abs(x), x2 = Sqr(x);
        double c0 = Estrin(x2, m0, m1, m2, m3, m4, m5, m6, m7, m8, m9, m10, m11);
        double c1 = Estrin(xa, n0, n1, n2, n3, n4, n5, n6, n7) / Estrin(xa, t0, t1, t2, t3, t4, t5, t6, t7);
        double xb = 1 - double.Exp2(c1 * xa);
        return xa < 1 ? x * c0 : double.CopySign(double.IsFinite(xb) ? xb : 1, x);
    }
}

public static partial class MathExt
{
    public static T Pow3<T>(T v) where T : INumber<T> => Sqr(v) * v;

    public static T Pow4<T>(T v) where T : INumber<T> => Pow3(v) * v;

    public static T Pow5<T>(T v) where T : INumber<T> => Pow4(v) * v;

    public static T Pow6<T>(T v) where T : INumber<T> => Pow5(v) * v;

    public static T Pow7<T>(T v) where T : INumber<T> => Pow6(v) * v;

    public static T Pow8<T>(T v) where T : INumber<T> => Pow7(v) * v;

    public static T Pow9<T>(T v) where T : INumber<T> => Pow8(v) * v;
}

public static partial class MathExt
{
    public static float Estrin(float x, float a0, float a1, float a2, float a3, float a4, float a5, float a6, float a7, float a8, float a9, float a10, float a11)
    {
        float b0 = float.FusedMultiplyAdd(x, a1, a0);
        float b1 = float.FusedMultiplyAdd(x, a3, a2);
        float b2 = float.FusedMultiplyAdd(x, a5, a4);
        float b3 = float.FusedMultiplyAdd(x, a7, a6);
        float b4 = float.FusedMultiplyAdd(x, a9, a8);
        float b5 = float.FusedMultiplyAdd(x, a11, a10);
        return Estrin(Sqr(x), b0, b1, b2, b3, b4, b5);
    }

    public static float Estrin(float x, float a0, float a1, float a2, float a3, float a4, float a5, float a6, float a7)
    {
        float b0 = float.FusedMultiplyAdd(x, a1, a0);
        float b1 = float.FusedMultiplyAdd(x, a3, a2);
        float b2 = float.FusedMultiplyAdd(x, a5, a4);
        float b3 = float.FusedMultiplyAdd(x, a7, a6);
        return Estrin(Sqr(x), b0, b1, b2, b3);
    }

    public static float Estrin(float x, float a0, float a1, float a2, float a3, float a4, float a5, float a6)
    {
        float b0 = float.FusedMultiplyAdd(x, a1, a0);
        float b1 = float.FusedMultiplyAdd(x, a3, a2);
        float b2 = float.FusedMultiplyAdd(x, a5, a4);
        float b3 = a6;
        return Estrin(Sqr(x), b0, b1, b2, b3);
    }

    public static float Estrin(float x, float a0, float a1, float a2, float a3, float a4, float a5)
    {
        float b0 = float.FusedMultiplyAdd(x, a1, a0);
        float b1 = float.FusedMultiplyAdd(x, a3, a2);
        float b2 = float.FusedMultiplyAdd(x, a5, a4);
        return Estrin(Sqr(x), b0, b1, b2);
    }

    public static float Estrin(float x, float a0, float a1, float a2, float a3)
    {
        float b0 = float.FusedMultiplyAdd(x, a1, a0);
        float b1 = float.FusedMultiplyAdd(x, a3, a2);
        return Estrin(Sqr(x), b0, b1);
    }

    public static float Estrin(float x, float a0, float a1, float a2)
    {
        float b0 = float.FusedMultiplyAdd(x, a1, a0);
        float b1 = a2;
        return Estrin(Sqr(x), b0, b1);
    }

    public static float Estrin(float x, float a0, float a1) => float.FusedMultiplyAdd(x, a1, a0);

    public static double Estrin(double x, double a0, double a1, double a2, double a3, double a4, double a5, double a6, double a7, double a8, double a9, double a10, double a11)
    {
        double b0 = double.FusedMultiplyAdd(x, a1, a0);
        double b1 = double.FusedMultiplyAdd(x, a3, a2);
        double b2 = double.FusedMultiplyAdd(x, a5, a4);
        double b3 = double.FusedMultiplyAdd(x, a7, a6);
        double b4 = double.FusedMultiplyAdd(x, a9, a8);
        double b5 = double.FusedMultiplyAdd(x, a11, a10);
        return Estrin(Sqr(x), b0, b1, b2, b3, b4, b5);
    }

    public static double Estrin(double x, double a0, double a1, double a2, double a3, double a4, double a5, double a6, double a7)
    {
        double b0 = double.FusedMultiplyAdd(x, a1, a0);
        double b1 = double.FusedMultiplyAdd(x, a3, a2);
        double b2 = double.FusedMultiplyAdd(x, a5, a4);
        double b3 = double.FusedMultiplyAdd(x, a7, a6);
        return Estrin(Sqr(x), b0, b1, b2, b3);
    }

    public static double Estrin(double x, double a0, double a1, double a2, double a3, double a4, double a5, double a6)
    {
        double b0 = double.FusedMultiplyAdd(x, a1, a0);
        double b1 = double.FusedMultiplyAdd(x, a3, a2);
        double b2 = double.FusedMultiplyAdd(x, a5, a4);
        double b3 = a6;
        return Estrin(Sqr(x), b0, b1, b2, b3);
    }

    public static double Estrin(double x, double a0, double a1, double a2, double a3, double a4, double a5)
    {
        double b0 = double.FusedMultiplyAdd(x, a1, a0);
        double b1 = double.FusedMultiplyAdd(x, a3, a2);
        double b2 = double.FusedMultiplyAdd(x, a5, a4);
        return Estrin(Sqr(x), b0, b1, b2);
    }

    public static double Estrin(double x, double a0, double a1, double a2, double a3)
    {
        double b0 = double.FusedMultiplyAdd(x, a1, a0);
        double b1 = double.FusedMultiplyAdd(x, a3, a2);
        return Estrin(Sqr(x), b0, b1);
    }

    public static double Estrin(double x, double a0, double a1, double a2)
    {
        double b0 = double.FusedMultiplyAdd(x, a1, a0);
        double b1 = a2;
        return Estrin(Sqr(x), b0, b1);
    }

    public static double Estrin(double x, double a0, double a1) => double.FusedMultiplyAdd(x, a1, a0);
}
