namespace RadSpec;

public readonly struct ShapeRef(int index)
{
    public readonly int Index = index;

    public static ShapeRef Invalid => new(-1);

    public static implicit operator int(ShapeRef v) => v.Index;
}

public interface IShape
{
    float SurfaceArea { get; }

    RayIntersectResult RayIntersect(Ray3f ray, int primitiveIndex);

    SurfaceInteraction ComputeSurfaceInteraction(Ray3f ray, in RayIntersectResult rir);
}
