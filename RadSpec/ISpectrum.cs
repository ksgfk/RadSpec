using System.Numerics;

namespace RadSpec;

public interface ISpectrum
{
    double Eval(double lambda);
}

public static class SpectrumUtility
{
    public static double InnerProduct(ISpectrum f, ISpectrum g)
    {
        double integral = 0;
        for (double lambda = Spectra.LambdaMin; lambda <= Spectra.LambdaMax; lambda++)
        {
            integral += f.Eval(lambda) * g.Eval(lambda);
        }
        return integral;
    }

    public static Xyz ToXyz(this ISpectrum s)
    {
        double x = InnerProduct(Spectra.Cie1931X, s);
        double y = InnerProduct(Spectra.Cie1931Y, s);
        double z = InnerProduct(Spectra.Cie1931Z, s);
        x /= Spectra.Cie1931IntegralY;
        y /= Spectra.Cie1931IntegralY;
        z /= Spectra.Cie1931IntegralY;
        return new Xyz((float)x, (float)y, (float)z);
    }
}
