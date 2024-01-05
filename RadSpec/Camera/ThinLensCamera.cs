namespace RadSpec.Camera;

public class ThinLensCamera : ICamera
{
    public Vector3f Position { get; }
    public Vector3f Target { get; }
    public Vector3f Up { get; }
    public float Fov { get; }
    public float Aspect { get; }
    public float Near { get; }
    public float Far { get; }

    private readonly Transform4f _cameraToWorld;
    private readonly Transform4f _screenToCamera;

    public ThinLensCamera(Vector3f position, Vector3f target, Vector3f up, float fov, float aspect, float near, float far)
    {
        Position = position;
        Target = target;
        Up = up;
        Fov = Radian(fov);
        Aspect = aspect;
        Near = near;
        Far = far;

        Transform4f worldToCamera = Transform4f.LookAt(Position, Target, Up);
        _cameraToWorld = Transform4f.Invert(worldToCamera);
        Transform4f cameraToScreen = Transform4f.Scale(0.5f, -0.5f * aspect, 1.0f) *
            Transform4f.Translate(1.0f, -1.0f / aspect, 0.0f) *
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
        throw new NotImplementedException();
    }
}
