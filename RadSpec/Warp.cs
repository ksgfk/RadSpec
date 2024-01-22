namespace RadSpec;

/// <summary>
/// https://ksgfk.github.io/2022/09/08/MonteCarlo/#Inversion-Method
/// </summary>
public static class Warp
{
    // private const double InvVisWaveIntegral = 0.0039398042293244066;
    // private const double VisWaveA = 0.0072;
    // private const double VisWaveB = 538.0;

    static Warp()
    {
        /**
         * https://pbr-book.org/4ed/Cameras_and_Film/Film_and_Imaging#eq:sample-wavelengths-pdf
         * 预计算pdf公式内的定积分
         * 令 t=A(\lambda - B)
         * 则 \lambda = \frac{t}{A} + B
         * 换元 \frac{1}{A}\int_{360}^{830}\frac{1}{\cosh^2(t)}\mathrm{d}t
         * 根据积分表 \tanh^{'}(x)=\frac{1}{\cosh^{2}(x)}, 得到原函数
         * 回代, 得到结果 \frac{1}{A}(\tanh(A(830-B))-\tanh(A(360-B)))
         *
         * 主要难点就是不知道双曲函数的积分表...阿巴阿巴阿巴
         */
        // double integral = 1 / VisWaveA * (Tanh(VisWaveA * (Spectra.LambdaMax - VisWaveB)) - Tanh(VisWaveA * (Spectra.LambdaMin - VisWaveB)));
        // InvVisWaveIntegral = 1 / integral;
    }

    public static float UniformToVisibleWavelength(float xi)
    {
        // const double a = VisWaveA;
        // const double b = VisWaveB;
        // const double c = InvVisWaveIntegral;
        // double d = c * (1 / VisWaveA * Tanh(VisWaveA * (Spectra.LambdaMin - VisWaveB)));
        // double result = 1 / a * Atanh(a / c * (xi + d)) + b;
        return 538 - 138.888889f * float.Atanh(0.85691062f - 1.82750197f * xi);
    }

    public static Vector4f UniformToVisibleWavelength(Vector4f xi)
    {
        return Float4(538) - Float4(138.888889f) * Atanh(0.85691062f - 1.82750197f * xi);
    }

    public static float UniformToVisibleWavelengthPdf(float lambda)
    {
        // if (lambda < Spectra.LambdaMin || lambda > Spectra.LambdaMax)
        // {
        //     return 0;
        // }
        // double result = InvVisWaveIntegral / Sqr(Cosh(VisWaveA * (lambda - VisWaveB)));
        // return (float)result;
        if (lambda < Spectra.LambdaMin || lambda > Spectra.LambdaMax)
        {
            return 0;
        }
        return 0.0039398042f / Sqr(Cosh(0.0072f * (lambda - 538)));
    }

    public static Vector4f UniformToVisibleWavelengthPdf(Vector4f lambda)
    {
        Vector4f value = Float4(0.0039398042f) / Sqr(Cosh(0.0072f * (lambda - 538)));
        value.X = Edge(lambda.X, value.X);
        value.Y = Edge(lambda.Y, value.Y);
        value.Z = Edge(lambda.Z, value.Z);
        value.W = Edge(lambda.W, value.W);
        return value;

        static float Edge(float lambda, float value) => lambda < Spectra.LambdaMin || lambda > Spectra.LambdaMax ? 0 : value;
    }
}
