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
        // 根据转换公式
        // XYZ = (光谱乘以三刺激曲线)在可见光波长范围的积分 除以 Y在可见光波长范围的积分
        // 这里积分用黎曼和代替
        double x = InnerProduct(Spectra.Cie1931X, s);
        double y = InnerProduct(Spectra.Cie1931Y, s);
        double z = InnerProduct(Spectra.Cie1931Z, s);
        x /= Spectra.Cie1931IntegralY;
        y /= Spectra.Cie1931IntegralY;
        z /= Spectra.Cie1931IntegralY;
        return new Xyz((float)x, (float)y, (float)z);
    }

    public static Vector4d Eval(this ISpectrum s, SampledWavelength wavelength)
    {
        double x = s.Eval(wavelength.Lambda.X);
        double y = s.Eval(wavelength.Lambda.Y);
        double z = s.Eval(wavelength.Lambda.Z);
        double w = s.Eval(wavelength.Lambda.W);
        return Double4(x, y, z, w);
    }
}
