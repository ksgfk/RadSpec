using System.Numerics;

namespace RadSpec;

public struct Vector4d
{
    public double X, Y, Z, W;

    public Vector4d(double x, double y, double z, double w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public Vector4d(double value)
    {
        X = value;
        Y = value;
        Z = value;
        W = value;
    }

    public Vector4d(Vector4 float4)
    {
        X = float4.X;
        Y = float4.Y;
        Z = float4.Z;
        W = float4.W;
    }

    public readonly Vector4 ToFloat4() => new((float)X, (float)Y, (float)Z, (float)W);
}
