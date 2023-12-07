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
}
