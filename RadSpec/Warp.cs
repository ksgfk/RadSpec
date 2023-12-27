using static RadSpec.MathExt;

namespace RadSpec;

/// <summary>
/// https://ksgfk.github.io/2022/09/08/MonteCarlo/#Inversion-Method
/// </summary>
public static class Warp
{
    private static readonly double InvVisWaveIntegral;
    private const double VisWaveA = 0.0072;
    private const double VisWaveB = 538.0;

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
        double integral = 1 / VisWaveA * (Math.Tanh(VisWaveA * (Spectra.LambdaMax - VisWaveB)) - Math.Tanh(VisWaveA * (Spectra.LambdaMin - VisWaveB)));
        InvVisWaveIntegral = 1 / integral;
    }

    public static float UniformToVisibleWavelength(float xi)
    {
        const double a = VisWaveA;
        const double b = VisWaveB;
        double c = InvVisWaveIntegral;
        double d = c * (1 / VisWaveA * Math.Tanh(VisWaveA * (Spectra.LambdaMin - VisWaveB)));
        double result = 1 / a * Math.Atanh(a / c * (xi + d)) + b;
        return (float)result;
    }

    public static float UniformToVisibleWavelengthPdf(float lambda)
    {
        if (lambda < 360 || lambda > 830)
        {
            return 0;
        }
        double result = InvVisWaveIntegral / Sqr(Math.Cosh(VisWaveA * (lambda - VisWaveB)));
        return (float)result;
    }
}
