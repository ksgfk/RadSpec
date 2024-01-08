namespace RadSpec;

public readonly struct ShapeRef(int index)
{
    public readonly int Index = index;

    public static implicit operator int(ShapeRef v) => v.Index;
}

public interface IShape
{
    float SurfaceArea { get; }
}
