namespace RadSpec;

public readonly struct RayIntersectResult(float t, Vector2f uv, int primitiveIndex, ShapeRef shape, ShapeRef instance)
{
    public readonly float T = t;
    public readonly int PrimitiveIndex = primitiveIndex;
    public readonly Vector2f UV = uv;
    public readonly ShapeRef Shape = shape;
    public readonly ShapeRef Instance = instance;

    public bool IsHit => float.IsFinite(T);
    public static RayIntersectResult Miss => new(float.PositiveInfinity, default, default, default);

    public RayIntersectResult(float t, Vector2f uv, int primitiveIndex, ShapeRef shape) : this(t, uv, primitiveIndex, ShapeRef.Invalid, shape) { }

    public SurfaceInteraction ComputeSurfaceInteraction(Ray3f ray, Scene scene)
    {
        IShape shape = scene.GetShape(Shape);
        SurfaceInteraction si = shape.ComputeSurfaceInteraction(ray, in this);
        si.ComputeShadingFrame();
        si.Time = ray.Time;
        si.Wavelength = ray.Wavelength;
        si.Shape = Shape;
        si.PrimitiveIndex = PrimitiveIndex;
        si.Wi = -ray.D;
        si.Instance = Instance;
        return si;
    }
}

public struct SurfaceInteraction
{
    public Vector3f P;
    public float T;
    public Vector3f N;
    public float Time;
    public SampledWavelength Wavelength;
    public Vector2f UV;
    public Frame Shading;
    public Vector3f dPdU, dPdV; //曲面上点P的参数化偏导数，位于切平面内，但不一定正交
    public ShapeRef Shape;
    public Vector3f dNdU, dNdV; //曲面上法向量的参数化偏导数，记录表面法线的微分变化情况，叉乘可以得到法向量
    public int PrimitiveIndex;
    public Vector2f dUVdX, dUVdY;
    public Vector3f Wi;
    public ShapeRef Instance;

    public void ComputeShadingFrame()
    {
        if (dPdU == Vector3f.Zero)
        {
            var (s, _) = OrthoCoordFrame(Shading.N);
            Shading.S = Normalize(s);
        }
        else
        {
            Shading.S = Normalize(GramSchmidt(Shading.N, dPdU));
        }
        Shading.T = Cross(Shading.N, Shading.S);
    }
}
