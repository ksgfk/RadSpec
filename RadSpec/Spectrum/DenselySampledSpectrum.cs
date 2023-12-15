namespace RadSpec.Spectrum;

public class DenselySampledSpectrum : ISpectrum
{
    private readonly int _lambdaMin;
    private readonly int _lambdaMax;
    private readonly double[] _values;

    public int LambdaMin => _lambdaMin;
    public int LambdaMax => _lambdaMax;

    public DenselySampledSpectrum(ISpectrum spec, int lambdaMin = Spectra.LambdaMin, int lambdaMax = Spectra.LambdaMax)
    {
        _lambdaMin = lambdaMin;
        _lambdaMax = lambdaMax;
        _values = new double[lambdaMax - lambdaMin + 1];
        for (int i = 0; i < _values.Length; i++)
        {
            _values[i] = spec.Eval(i + _lambdaMin);
        }
    }

    public double Eval(double lambda)
    {
        double t = double.Round(lambda);
        int i = (int)t - _lambdaMin;
        return i < 0 || i >= _values.Length ? 0 : _values[i];
    }
}
