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
        /**
         * 首先回忆一下球面坐标系和直角坐标系的转化
         * x = r\sin\theta\cos\phi
         * y = r\sin\theta\sin\phi
         * z = r\cos\theta
         * 反正\phi是横着转的轴的平面上的角度，\theta是竖着转的轴的平面上的角度，两个平面互相垂直
         * 
         * 众所周知，\phi\in[0,2\pi]，\theta\in[0,\pi]，我们可以通过线性变换，将它们都缩小到[0,1]范围
         * u = \frac{\phi}{2\pi}
         * v = \frac{\theta}{\pi}
         *
         * 计算dpdu：
         * \frac{\partial P_x}{\partial u} = \frac{\partial}{\partial u}(r\sin\theta\cos\phi)
         *                                 = r\sin\theta\frac{\partial}{cos\phi}
         *                                 = r\sin\theta(-\sin\phi)
         * 巧了，看看球面坐标系和直角坐标系的转化的第二条，可以等价替换成 -y
         * 同理可得
         * \frac{\partial P_y}{\partial u} = r\sin\theta\sin\phi = x
         * \frac{\partial P_z}{\partial u} = 0
         * 因此
         * \frac{\partial P}{\partial u} = (-y, x, 0)
         * 计算dpdv，注意z可以等价替换r\cos\theta：
         * \frac{\partial P_x}{\partial v} = r\cos\theta\cos\phi = z\cos\phi
         * \frac{\partial P_y}{\partial v} = r\cos\theta\sin\phi = z\sin\phi
         * \frac{\partial P_y}{\partial v} = r(-\sin\theta)
         * 因此
         * \frac{\partial P}{\partial v} = (z\cos\phi, z\sin\phi, -r\sin\theta))
         *
         * 怎么求sin{\theta}：
         * 看xy平面，根据sin函数定义，有
         * \sin\phi = \frac{y}{r} = \frac{y}{\sqrt{x^2+y^2}}
         * 看球面坐标系和直角坐标系的转化第二个公式，稍微变换一下让sin\theta在左边
         * \sin\theta=\frac{y}{r\sin\phi}
         * 带入\sin\phi整理，有
         * \sin\theta=\frac{\sqrt{x^2+y^2}}{r}
         */
        float t = rir.T;
        Vector3f n = Normalize(ray.At(t) - Center);
        Vector3f worldP = Fma(n, Radius, Center);
        Vector3f localP = Transform4f.Invert(_transform).ApplyAffine(worldP);

        float theta = AngleBetweenUnitZ(localP);
        float phi = float.Atan2(localP.Y, localP.X);
        if (phi < 0)
        {
            phi += 2 * float.Pi;
        }
        Vector2f uv = Float2(phi / (2 * float.Pi), theta / float.Pi);

        Vector3f dpdu = Float3(-localP.Y, localP.X, 0);
        float zRadius = float.Sqrt(Sqr(localP.X) + Sqr(localP.Y));
        Vector3f dpdv;
        if (zRadius == 0)
        {
            dpdv = Float3(1, 0, 0);
        }
        else
        {
            float sinPhi = localP.Y / zRadius;
            float cosPhi = localP.X / zRadius;
            dpdv = Float3(localP.Z * cosPhi, localP.Z * sinPhi, -zRadius); //Z分量看上面注释，r\sin\theta = \sqrt{x^2+y^2}
        }
        dpdu = _transform.ApplyLinear(dpdu * 2 * float.Pi);
        dpdv = _transform.ApplyLinear(dpdv * float.Pi);

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
         * 更进一步
         * https://github.com/mitsuba-renderer/mitsuba3/blob/f49fbb33738f8b154c3f7aa20f52632c7234957a/src/shapes/sphere.cpp#L495
         * mitsuba给出了数值上更稳健的实现，通过虚拟一个垂直于光线且包含圆心的平面实现
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
