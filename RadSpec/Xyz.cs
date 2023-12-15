using System.Numerics;

namespace RadSpec;

public readonly struct Xyz : IEquatable<Xyz>
{
    public readonly Vector3 Value;

    public float X => Value.X;
    public float Y => Value.Y;
    public float Z => Value.Z;

    public Xyz(Vector3 value)
    {
        Value = value;
    }

    public Xyz(float x, float y, float z)
    {
        Value = new Vector3(x, y, z);
    }

    /// <summary>
    /// chromaticity coordinate
    /// </summary>
    public Vector2 ChromaticityCoord()
    {
        return new Vector2(X / (X + Y + Z), Y / (X + Y + Z));
    }

    public static Xyz FromChromaticityCoord(Vector2 xy, float y = 1)
    {
        if (xy.Y == 0)
        {
            return new Xyz(0, 0, 0);
        }
        return new Xyz(xy.X * y / xy.Y, y, (1 - xy.X - xy.Y) * y / xy.Y);
    }

    public static implicit operator Vector3(Xyz xyz) => xyz.Value;

    public static implicit operator Xyz(Vector3 vec) => new Xyz(vec);

    public override string ToString() => $"<{X}, {Y}, {Z}>";
    public bool Equals(Xyz other) => Value.Equals(other.Value);
    public override bool Equals(object? obj) => (obj is Xyz other) && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();
    public static bool operator ==(Xyz left, Xyz right) => left.Value == right.Value;
    public static bool operator !=(Xyz left, Xyz right) => left.Value != right.Value;
}
