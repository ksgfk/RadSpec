namespace RadSpec;

public struct CoordFrame // 三维正交坐标系
{
    public Vector3f S, T, N;

    public CoordFrame(Vector3f v)
    {
        (S, T) = OrthoFrame(v);
        N = v;
    }
}
