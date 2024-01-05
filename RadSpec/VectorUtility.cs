namespace RadSpec;

public static class VectorUtility
{
    public static Vector2f Float2(float x, float y) => new(x, y);
    public static Vector2f Float2(float v) => new(v);
    public static Vector3f Float3(float x, float y, float z) => new(x, y, z);
    public static Vector3f Float3(float v) => new(v);
    public static Vector3f Float3(Vector2f xy, float z) => new(xy, z);
    public static Vector3f Float3(float x, Vector2f yz) => new(x, yz);
    public static Vector4f Float4(float x, float y, float z, float w) => new(x, y, z, w);
    public static Vector4f Float4(float v) => new(v);
    public static Vector4f Float4(Vector2f xy, float z, float w) => new(xy, z, w);
    public static Vector4f Float4(float x, Vector2f yz, float w) => new(x, yz, w);
    public static Vector4f Float4(float x, float y, Vector2f zw) => new(x, y, zw);
    public static Vector4f Float4(Vector2f xy, Vector2f zw) => new(xy, zw);
    public static Vector4f Float4(Vector3f xyz, float w) => new(xyz, w);
    public static Vector4f Float4(float x, Vector3f yzw) => new(x, yzw);

    public static Vector2d Double2(double x, double y) => new(x, y);
    public static Vector2d Double2(double v) => new(v);
    public static Vector3d Double3(double x, double y, double z) => new(x, y, z);
    public static Vector3d Double3(double v) => new(v);
    public static Vector3d Double3(Vector2d xy, double z) => new(xy, z);
    public static Vector3d Double3(double x, Vector2d yz) => new(x, yz);
    public static Vector4d Double4(double x, double y, double z, double w) => new(x, y, z, w);
    public static Vector4d Double4(double v) => new(v);
    public static Vector4d Double4(Vector2d xy, double z, double w) => new(xy, z, w);
    public static Vector4d Double4(double x, Vector2d yz, double w) => new(x, yz, w);
    public static Vector4d Double4(double x, double y, Vector2d zw) => new(x, y, zw);
    public static Vector4d Double4(Vector2d xy, Vector2d zw) => new(xy, zw);
    public static Vector4d Double4(Vector3d xyz, double w) => new(xyz, w);
    public static Vector4d Double4(double x, Vector3d yzw) => new(x, yzw);

    public static Vector2i Int2(int x, int y) => new(x, y);
    public static Vector2i Int2(int v) => new(v);
    public static Vector3i Int3(int x, int y, int z) => new(x, y, z);
    public static Vector3i Int3(int v) => new(v);
    public static Vector3i Int3(Vector2i xy, int z) => new(xy, z);
    public static Vector3i Int3(int x, Vector2i yz) => new(x, yz);
    public static Vector4i Int4(int x, int y, int z, int w) => new(x, y, z, w);
    public static Vector4i Int4(int v) => new(v);
    public static Vector4i Int4(Vector2i xy, int z, int w) => new(xy, z, w);
    public static Vector4i Int4(int x, Vector2i yz, int w) => new(x, yz, w);
    public static Vector4i Int4(int x, int y, Vector2i zw) => new(x, y, zw);
    public static Vector4i Int4(Vector2i xy, Vector2i zw) => new(xy, zw);
    public static Vector4i Int4(Vector3i xyz, int w) => new(xyz, w);
    public static Vector4i Int4(int x, Vector3i yzw) => new(x, yzw);
}
