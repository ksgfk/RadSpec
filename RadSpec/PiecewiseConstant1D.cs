namespace RadSpec;

/// <summary>
/// https://pbr-book.org/4ed/Sampling_Algorithms/Sampling_1D_Functions#SamplingPiecewise-Constant1DFunctions
/// 分段一维常数函数
/// </summary>
public class PiecewiseConstant1D
{
    private readonly float[] _pdf;
    private readonly float[] _cdf;
    private readonly float _min;
    private readonly float _max;
    private readonly float _integral;

    public float Count => _pdf.Length;
    public float Integral => _integral;
    public ReadOnlySpan<float> Function => _pdf;

    public PiecewiseConstant1D(ReadOnlySpan<float> f, float min, float max)
    {
        float[] func = f.ToArray();
        foreach (ref float i in func.AsSpan())
        {
            i = float.Abs(i);
        }
        float[] cdf = new float[func.Length + 1];
        cdf[0] = 0;
        int n = f.Length;
        for (int i = 1; i < n + 1; i++) //根据定义计算cdf
        {
            cdf[i] = cdf[i - 1] + func[i - 1] * (max - min) / n;
        }
        float integral = cdf[n];
        if (integral == 0)
        {
            for (int i = 1; i < n + 1; i++)
            {
                cdf[i] /= (float)i / n;
            }
        }
        else
        {
            foreach (ref float i in cdf.AsSpan())
            {
                i /= integral;
            }
        }
        _pdf = func;
        _cdf = cdf;
        _min = min;
        _max = max;
        _integral = integral;
    }

    public (float value, float pdf, int offset) Sample(float xi)
    {
        int i = Array.BinarySearch(_cdf, xi);
        int offset;
        if (i < 0)
        {
            int n = ~i;
            offset = n - 1;
        }
        else
        {
            offset = i;
        }
        float du = xi - _cdf[offset];
        if (_cdf[offset + 1] - _cdf[offset] > 0)
        {
            du /= _cdf[offset + 1] - _cdf[offset];
        }
        float pdf = _integral > 0 ? _pdf[offset] / _integral : 0;
        float value = float.Lerp(_min, _max, (offset + du) / _pdf.Length);
        return (value, pdf, offset);
    }

    public float Pdf(float value)
    {
        value = (value - _min) / (_max - _min);
        int offset = (int)float.Floor(value * _pdf.Length);
        return _pdf[offset] / _integral;
    }
}
