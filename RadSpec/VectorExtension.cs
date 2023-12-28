namespace RadSpec;

public static class VectorExtension
{
    public static float Sum(this Vector4f float4)
    {
        return float4.X + float4.Y + float4.Z + float4.W;
    }
}
