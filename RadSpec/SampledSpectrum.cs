using System.Numerics;

namespace RadSpec;

public readonly struct SampledSpectrum
{
    private readonly Vector4 _value;

    public SampledSpectrum(float value)
    {
        _value = new Vector4(value);
    }

    public SampledSpectrum(float x, float y, float z, float w)
    {
        _value = new Vector4(x, y, z, w);
    }

    public SampledSpectrum(Vector4 value)
    {
        _value = value;
    }

    public static SampledSpectrum operator +(in SampledSpectrum lhs, in SampledSpectrum rhs)
    {
        return new SampledSpectrum(lhs._value + rhs._value);
    }

    public static SampledSpectrum operator -(in SampledSpectrum lhs, in SampledSpectrum rhs)
    {
        return new SampledSpectrum(lhs._value - rhs._value);
    }

    public static SampledSpectrum operator *(in SampledSpectrum lhs, in SampledSpectrum rhs)
    {
        return new SampledSpectrum(lhs._value * rhs._value);
    }

    public static SampledSpectrum operator /(in SampledSpectrum lhs, in SampledSpectrum rhs)
    {
        return new SampledSpectrum(lhs._value / rhs._value);
    }

    public Xyz ToXyz(SampledWavelength wavelength)
    {
        Vector4 cieX = Spectra.Cie1931X.Eval(wavelength).ToFloat4();
        Vector4 cieY = Spectra.Cie1931Y.Eval(wavelength).ToFloat4();
        Vector4 cieZ = Spectra.Cie1931Z.Eval(wavelength).ToFloat4();

        Vector4 specX = cieX * _value / wavelength.Pdf;
        Vector4 specY = cieY * _value / wavelength.Pdf;
        Vector4 specZ = cieZ * _value / wavelength.Pdf;

        float x = specX.Sum() / 4 / (float)Spectra.Cie1931IntegralY;
        float y = specY.Sum() / 4 / (float)Spectra.Cie1931IntegralY;
        float z = specZ.Sum() / 4 / (float)Spectra.Cie1931IntegralY;

        return new Xyz(x, y, z);
    }
}
