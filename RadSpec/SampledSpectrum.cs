using System.Numerics;

namespace RadSpec;

public struct SampledSpectrum
{
    private Vector4 _value;

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
}
