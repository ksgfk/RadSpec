namespace RadSpec.Spectrum;

public class PiecewiseLinearSpectrum : ISpectrum
{
    private readonly double[] _lambdas;
    private readonly double[] _values;

    public PiecewiseLinearSpectrum(double[] values, double[] lambdas, bool isNormalize)
    {
        _values = values;
        _lambdas = lambdas;
        if (isNormalize)
        {
            Normalize();
        }
    }

    public PiecewiseLinearSpectrum(SpectraData[] data, bool isNormalize)
    {
        _lambdas = new double[data.Length];
        _values = new double[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            _lambdas[i] = data[i].Lambda;
            _values[i] = data[i].Value;
        }
        if (isNormalize)
        {
            Normalize();
        }
    }

    public double Eval(double lambda)
    {
        if (_lambdas.Length == 0 || lambda < _lambdas[0] || lambda > _lambdas[^1])
        {
            return 0;
        }
        int i = Array.BinarySearch(_lambdas, lambda);
        double result;
        if (i < 0)
        {
            int n = ~i;
            double start = _values[n - 1];
            double end = _values[n];
            double t = (lambda - start) / (end - start);
            result = double.Lerp(start, end, t);
        }
        else
        {
            result = _values[i];
        }
        return result;
    }

    private void Normalize()
    {
        double coeff = Spectra.Cie1931IntegralY / SpectrumUtility.InnerProduct(Spectra.Cie1931Y, this);
        foreach (ref double i in _values.AsSpan())
        {
            i *= coeff;
        }
    }
}
