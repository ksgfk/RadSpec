namespace RadSpec;

public interface ISpectrum
{
    float Eval(float lambda);

    SampledSpectrum Sample(Wavelength wavelength);
}

public class UniformSpectrum : ISpectrum
{
    private readonly float _value;
    public UniformSpectrum(float value) { _value = value; }
    public float Eval(float lambda) => _value;
    public SampledSpectrum Sample(Wavelength wavelength) => new(_value);
}

public class DenselySampledSpectrum : ISpectrum
{
    private readonly int _lambdaMin;
    private readonly int _lambdaMax;
    private readonly float[] _data;

    public int LambdaMin => _lambdaMin;
    public int LambdaMax => _lambdaMax;

    public DenselySampledSpectrum(float[] data, int lambdaMin, int lambdaMax)
    {
        _data = data;
        _lambdaMin = lambdaMin;
        _lambdaMax = lambdaMax;
        if (_data.Length != lambdaMax - lambdaMin)
        {
            throw new ArgumentOutOfRangeException(nameof(data));
        }
    }

    public float Eval(float lambda)
    {
        int lambdaInt = (int)MathF.Round(lambda);
        int offset = lambdaInt - _lambdaMin;
        float result = (uint)offset >= _data.Length ? 0 : _data[offset];
        return result;
    }

    public SampledSpectrum Sample(Wavelength wavelength)
    {
        float x = Eval(wavelength.Lambda.X);
        float y = Eval(wavelength.Lambda.Y);
        float z = Eval(wavelength.Lambda.Z);
        float w = Eval(wavelength.Lambda.W);
        return new SampledSpectrum(x, y, z, w);
    }
}
