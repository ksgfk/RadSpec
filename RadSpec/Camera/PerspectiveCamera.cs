namespace RadSpec.Camera;

public class PerspectiveCamera : ICamera
{
    public Vector3f Position { get; }
    public Vector3f Target { get; }
    public Vector3f Up { get; }
    public float Fov { get; }
    public float Aspect { get; }
    public float Near { get; }
    public float Far { get; }
    public IFilm Film { get; }
    public ISampler Sampler { get; }

    private readonly Transform4f _cameraToWorld;
    private readonly Transform4f _screenToCamera;

    public PerspectiveCamera(IFilm film, ISampler sampler, Vector3f position, Vector3f target, Vector3f up, float fov, float near, float far)
    {
        Position = position;
        Target = target;
        Up = up;
        Fov = Radian(fov);
        Aspect = (float)film.Resolution.X / film.Resolution.Y;
        Near = near;
        Far = far;
        Film = film;
        Sampler = sampler;

        Transform4f worldToCamera = Transform4f.LookAt(Position, Target, Up);
        _cameraToWorld = Transform4f.Invert(worldToCamera);
        Transform4f cameraToScreen = Transform4f.Scale(0.5f, -0.5f * Aspect, 1.0f) *
            Transform4f.Translate(1.0f, -1.0f / Aspect, 0.0f) *
            Transform4f.Perspective(Fov, Near, Far);
        _screenToCamera = Transform4f.Invert(cameraToScreen);
    }

    public Ray3f SampleRay(float time, float xi1, Vector2f xi2, Vector2f xi3)
    {
        Vector3f nearPlane = _screenToCamera.ApplyAffine(Float3(xi2, 0));
        Vector3f cameraDir = Normalize(nearPlane);
        Vector3f worldDir = Normalize(_cameraToWorld.ApplyLinear(cameraDir));
        float nearT = Near / worldDir.Z;
        float farT = Far / worldDir.Z;
        Vector3f worldPos = _cameraToWorld.Translation + worldDir * nearT;
        float maxT = farT - nearT;
        return new Ray3f(worldPos, worldDir, maxT, 0, default);
    }

    public SampledWavelength SampleWavelengths(float xi)
    {
        Vector4f shift = Float4(0, 1.0f / 4, 2.0f / 4, 3.0f / 4);
        Vector4f value = Float4(xi) + shift;
        if (value.X > 1) value.X -= 1;
        if (value.Y > 1) value.Y -= 1;
        if (value.Z > 1) value.Z -= 1;
        if (value.W > 1) value.W -= 1;
        Vector4f lambda = Warp.UniformToVisibleWavelength(value);
        Vector4f pdf = Warp.UniformToVisibleWavelengthPdf(lambda);
        return new SampledWavelength(lambda, pdf);
    }
}
