namespace RadSpec;

public readonly struct Xyz : IEquatable<Xyz>
{
    private readonly Vector3f _value;

    public float X => _value.X;
    public float Y => _value.Y;
    public float Z => _value.Z;

    public Xyz(Vector3f value)
    {
        _value = value;
    }

    public Xyz(float x, float y, float z)
    {
        _value = Float3(x, y, z);
    }

    /// <summary>
    /// chromaticity coordinate, 描述相对于白色的相对色彩
    /// </summary>
    public Vector2f ChromaticityCoord()
    {
        return Float2(X / (X + Y + Z), Y / (X + Y + Z));
    }

    public static Xyz FromChromaticityCoord(Vector2f xy, float y = 1)
    {
        if (xy.Y == 0)
        {
            return new Xyz(0, 0, 0);
        }
        return new Xyz(xy.X * y / xy.Y, y, (1 - xy.X - xy.Y) * y / xy.Y);
    }

    public static implicit operator Vector3f(Xyz xyz) => xyz._value;
    public static implicit operator Xyz(Vector3f vec) => new(vec);

    public static bool operator ==(Xyz left, Xyz right) => left._value == right._value;
    public static bool operator !=(Xyz left, Xyz right) => left._value != right._value;
    public bool Equals(Xyz other) => _value.Equals(other._value);
    public override bool Equals(object? obj) => (obj is Xyz other) && Equals(other);
    public override int GetHashCode() => _value.GetHashCode();
    public override string ToString() => $"<{X}, {Y}, {Z}>";
}
