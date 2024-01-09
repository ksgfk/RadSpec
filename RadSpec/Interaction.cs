namespace RadSpec;

public readonly struct RayIntersectResult(float t, Vector2f uv, int primitiveIndex, int shapeIndex, ShapeRef instance)
{
    public readonly float T = t;
    public readonly int PrimitiveIndex = primitiveIndex;
    public readonly Vector2f UV = uv;
    public readonly int ShapeIndex = shapeIndex;
    public readonly ShapeRef Instance = instance;

    public bool IsHit => float.IsFinite(T);
    public static RayIntersectResult Miss => new(float.PositiveInfinity, new(), 0, 0);

    public RayIntersectResult(float t, Vector2f uv, int primitiveIndex, int shapeIndex) : this(t, uv, primitiveIndex, shapeIndex, ShapeRef.Invalid) { }
}

public struct SurfaceInteraction
{
    public Vector3f P;
    public float T;
    public Vector3f N;
    public float Time;
    public SampledWavelength Wavelength;
    public Vector2f UV;
    public CoordFrame Shading;
    public Vector3f dPdU, dPdV;
    public ShapeRef Shape;
    public Vector3f dNdU, dNdV;
    public int PrimitiveIndex;
    public Vector2f dUVdX, dUVdY;
    public Vector3f Wi;
    public ShapeRef Instance;
}
