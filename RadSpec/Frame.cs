namespace RadSpec;

public struct Frame // 三维正交坐标系
{
    public Vector3f S, T, N;

    public Frame(Vector3f v)
    {
        (S, T) = OrthoCoordFrame(v);
        N = v;
    }
}
