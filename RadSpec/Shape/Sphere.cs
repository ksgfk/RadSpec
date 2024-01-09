namespace RadSpec.Shape;

public class Sphere : IShape
{
    private readonly Transform4f _transform;

    public float Radius { get; }
    public Vector3f Center { get; }
    public Matrix4x4f Rotate { get; }
    public float SurfaceArea => 4 * float.Pi * Sqr(Radius);

    public Sphere(float radius, Vector3f center, Matrix4x4f rotate)
    {
        Radius = radius;
        Center = center;
        Rotate = rotate;

        _transform = Transform4f.Translate(center) *
            Transform4f.Scale(Float3(radius)) *
            Transform4f.FromMatrix(rotate);
    }

    public SurfaceInteraction ComputeSurfaceInteraction(Ray3f ray, in RayIntersectResult rir)
    {
        throw new NotImplementedException();
    }

    public RayIntersectResult RayIntersect(Ray3f ray, int primitiveIndex)
    {
        /** 
         * https://pbr-book.org/4ed/Shapes/Spheres#IntersectionTests
         * 首先球的隐式表达为:
         * (P - C)^2 - r^2 = 0 (1), 其中 p 表示球面上一点的坐标, c表示球心坐标, r表示半径
         * 射线的表达式为:
         * r(t) = O + t * D (2)
         * 球面上一点可以用射线与这一点相交来代替, 所以将球公式 (1) 改写:
         * (O + t * D - C)^2 - r^2 = 0 (3)
         * 除了 t, 也就是射线从起点出发, 经过的距离, 其他参数都是已知的
         * 我们将公式 (3) 展开
         * (3) = (t * D + (O - C))^2 - r^2
         *     = (t * D)^2 + 2 * t * D * (O - C) + (O - C)^2 - r^2
         *     = D^2 * t^2 + 2 * D * (O - C) * t + (O - C)^2 - r^2 (4)
         * 很容易发现这是个标准的一元二次方程, 且
         * a = D^2
         * b = 2 * D * (O - C)
         * c = (O - C)^2 - r^2
         * 解出t1, t2就可以求出射线与球是否有交点, 最近的交点就是最小的那个解
         *
         * 此外
         * https://www.realtimerendering.com/intersections.html
         * 这里给出了进一步优化的文章与demo
         *
         * 更进一步
         * https://github.com/mitsuba-renderer/mitsuba3/blob/f49fbb33738f8b154c3f7aa20f52632c7234957a/src/shapes/sphere.cpp#L495
         * mitsuba给出了数值上更稳健的实现，通过虚拟一个垂直于光线的平面实现
         */
        Vector3f l = ray.O - Center;
        Vector3f d = ray.D;
        float planeT = Dot(-l, d) / Length(d);
        if (planeT == 0 && ray.O != Center)
        {
            return RayIntersectResult.Miss;
        }
        Vector3f planeP = ray.At(planeT);
        if (Length(planeP - Center) > Radius)
        {
            return RayIntersectResult.Miss;
        }
        Vector3f o = planeP - Center;
        float a = LengthSquared(ray.D);
        float b = 2 * Dot(o, ray.D);
        float c = LengthSquared(o) - Sqr(Radius);
        var (isFind, nearT, farT) = SolveQuadratic(a, b, c);
        nearT += planeT;
        farT += planeT;
        bool outBounds = !(nearT <= ray.MaxT && farT >= 0);
        bool inBounds = nearT < 0 && farT > ray.MaxT;
        if (!isFind || outBounds || inBounds)
        {
            return RayIntersectResult.Miss;
        }
        float t = nearT < 0 ? farT : nearT;
        return new RayIntersectResult(t, new(), 0, 0);
    }
}
