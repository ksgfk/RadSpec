namespace RadSpec;

public interface IScene
{
    public IShape GetShape(ShapeRef shape);

    public SurfaceInteraction RayIntersect(Ray3f ray);

    public bool RayTest(Ray3f ray);
}
