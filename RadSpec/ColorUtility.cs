namespace RadSpec;

public static class ColorUtility
{
    public static float ToSrgb(float value)
    {
        return value <= 0.0031308f ? value * 12.92f : (1.0f + 0.055f) * MathF.Pow(value, 1.0f / 2.4f) - 0.055f;
    }

    public static float ToLinear(float value)
    {
        return value <= 0.04045f ? value * (1.0f / 12.92f) : MathF.Pow((value + 0.055f) * (1.0f / 1.055f), 2.4f);
    }
}
