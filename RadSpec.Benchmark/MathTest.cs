using System.Numerics;
using BenchmarkDotNet.Attributes;

namespace RadSpec.Benchmark;

[DisassemblyDiagnoser(printInstructionAddresses: true)]
public class RadToDeg
{
    public const int Cnt = 1000;

    private double[] _a = null!;

    [GlobalSetup]
    public void Setup()
    {
        _a = new double[Cnt];
        for (int i = 0; i < _a.Length; i++)
        {
            _a[i] = Random.Shared.NextSingle();
        }
    }

    [Benchmark(Baseline = true)]
    public void Raw()
    {
        for (int i = 0; i < _a.Length; i++)
        {
            _a[i] = 180 / double.Pi * _a[i];
        }
    }

    [Benchmark]
    public void Generic()
    {
        for (int i = 0; i < _a.Length; i++)
        {
            _a[i] = Degree(_a[i]);
        }
    }

    private static T Degree<T>(T radian) where T : IFloatingPoint<T> => T.CreateChecked(180) / T.Pi * radian;
}

[DisassemblyDiagnoser(printInstructionAddresses: true)]
public class Erf
{
    public static IEnumerable<object> Gen()
    {
        yield return 0.21413;
    }

    [Benchmark(Baseline = true)]
    [ArgumentsSource(nameof(Gen))]
    public double Taylor(double x)
    {
        return (double)MathExt.ErfTaylor((decimal)x);
    }

    [Benchmark]
    [ArgumentsSource(nameof(Gen))]
    public double Fortran77(double x)
    {
        return MathExt.ErfFortran77(x);
    }

    [Benchmark]
    [ArgumentsSource(nameof(Gen))]
    public float MitsubaFloat(double x)
    {
        return MathExt.ErfMitsuba((float)x);
    }

    [Benchmark]
    [ArgumentsSource(nameof(Gen))]
    public double MitsubaDouble(double x)
    {
        return MathExt.ErfMitsuba(x);
    }
}

[DisassemblyDiagnoser(printInstructionAddresses: true)]
public class UniformToVisibleWavelength
{
    public static IEnumerable<object> Gen()
    {
        yield return 0.21413f;
    }

    [Benchmark(Baseline = true)]
    [ArgumentsSource(nameof(Gen))]
    public float Raw(float x) => RawFormula(x);

    [Benchmark]
    [ArgumentsSource(nameof(Gen))]
    public float Simple(float x) => Warp.UniformToVisibleWavelength(x);

    private static float RawFormula(double xi)
    {
        const double VisWaveA = 0.0072;
        const double VisWaveB = 538.0;

        double integral = 1 / VisWaveA * (double.Tanh(VisWaveA * (Spectra.LambdaMax - VisWaveB)) - double.Tanh(VisWaveA * (Spectra.LambdaMin - VisWaveB)));
        double invVisWaveIntegral = 1 / integral;
        const double a = VisWaveA;
        const double b = VisWaveB;
        double c = invVisWaveIntegral;
        double d = c * (1 / VisWaveA * double.Tanh(VisWaveA * (Spectra.LambdaMin - VisWaveB)));
        double result = 1 / a * double.Atanh(a / c * (xi + d)) + b;
        return (float)result;
    }
}
