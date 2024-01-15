namespace RadSpec;

public readonly struct ShapeRef(int index) : IEquatable<ShapeRef>
{
    public readonly int Index = index;

    public static ShapeRef Invalid => new(-1);

    public static implicit operator int(ShapeRef v) => v.Index;

    public static bool operator ==(ShapeRef l, ShapeRef r) => l.Index == r.Index;
    public static bool operator !=(ShapeRef l, ShapeRef r) => l.Index != r.Index;

    public bool Equals(ShapeRef other) => this == other;
    public override bool Equals(object? obj) => obj is ShapeRef sref && Equals(sref);
    public override int GetHashCode() => Index.GetHashCode();
    public override string ToString() => Index.ToString();
}

public interface IShape
{
    ShapeRef Self { get; }
    float SurfaceArea { get; }
    BoundingBox3f AllWorldBound { get; }

    void SetSelfRef(ShapeRef shape);

    BoundingBox3f GetPrimitiveWorldBound(int primitiveIndex);

    RayIntersectResult RayIntersect(Ray3f ray, int primitiveIndex);

    SurfaceInteraction ComputeSurfaceInteraction(Ray3f ray, in RayIntersectResult rir);
}

public static class ShapeUtility
{
    public static void CheckIsInit<T>(T shape) where T : IShape
    {
        if (shape.Self != ShapeRef.Invalid)
        {
            throw new InvalidOperationException("shape already initialized");
        }
    }
}
