namespace RadSpec;

public readonly struct SampledSpectrum
{
    private readonly Vector4f _value;

    public SampledSpectrum(float value)
    {
        _value = new Vector4f(value);
    }

    public SampledSpectrum(float x, float y, float z, float w)
    {
        _value = new Vector4f(x, y, z, w);
    }

    public SampledSpectrum(Vector4f value)
    {
        _value = value;
    }

    public static SampledSpectrum operator +(in SampledSpectrum lhs, in SampledSpectrum rhs) => new(lhs._value + rhs._value);
    public static SampledSpectrum operator -(in SampledSpectrum lhs, in SampledSpectrum rhs) => new(lhs._value - rhs._value);
    public static SampledSpectrum operator *(in SampledSpectrum lhs, in SampledSpectrum rhs) => new(lhs._value * rhs._value);
    public static SampledSpectrum operator /(in SampledSpectrum lhs, in SampledSpectrum rhs) => new(lhs._value / rhs._value);

    public Xyz ToXyz(SampledWavelength wavelength)
    {
        Vector4f cieX = Spectra.Cie1931X.Eval(wavelength).AsFloat4();
        Vector4f cieY = Spectra.Cie1931Y.Eval(wavelength).AsFloat4();
        Vector4f cieZ = Spectra.Cie1931Z.Eval(wavelength).AsFloat4();

        Vector4f specX = cieX * _value / wavelength.Pdf;
        Vector4f specY = cieY * _value / wavelength.Pdf;
        Vector4f specZ = cieZ * _value / wavelength.Pdf;

        float x = Sum(specX) / 4 / (float)Spectra.Cie1931IntegralY;
        float y = Sum(specY) / 4 / (float)Spectra.Cie1931IntegralY;
        float z = Sum(specZ) / 4 / (float)Spectra.Cie1931IntegralY;

        return new Xyz(x, y, z);
    }
}
