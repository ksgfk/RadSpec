namespace RadSpec;

public struct BoundingBox3f(Vector3f min, Vector3f max)
{
    public Vector3f Min = min;
    public Vector3f Max = max;

    public readonly bool IsValid => Max.X >= Min.X && Max.Y >= Min.Y && Max.Z >= Min.Z;
    public readonly bool IsCollapsed => Max.X == Min.X || Max.Y == Min.Y || Max.Z == Min.Z;
    public readonly Vector3f Diagonal => Max - Min; //对角线
    public readonly int MajorAxis
    {
        get
        {
            Vector3f d = Diagonal;
            return (d.Y > d.X) ? (d.Z > d.Y ? 2 : 1) : (d.Z > d.X ? 2 : 0);
        }
    }
    public readonly int MinorAxis
    {
        get
        {
            Vector3f d = Diagonal;
            return (d.Y < d.X) ? (d.Z < d.Y ? 2 : 1) : (d.Z < d.X ? 2 : 0);
        }
    }
    public readonly Vector3f Center => (Max + Min) * 0.5f;
    public readonly float SurfaceArea
    {
        get
        {
            Vector3f d = Diagonal;
            return (d.X * d.Y + d.X * d.Z + d.Y * d.Z) * 2;
        }
    }
    public readonly float Volume
    {
        get
        {
            Vector3f d = Diagonal;
            return d.X * d.Y * d.Z;
        }
    }

    public BoundingBox3f() : this(Float3(float.PositiveInfinity), Float3(float.NegativeInfinity)) { }
    public BoundingBox3f(Vector3f p) : this(p, p) { }

    public readonly bool Contains(Vector3f p) => p.X >= Min.X && p.Y >= Min.Y && p.Z >= Min.Z && p.X <= Max.X && p.Y <= Max.Y && p.Z <= Max.Z;
    public readonly bool ContainsExclusive(Vector3f p) => p.X > Min.X && p.Y > Min.Y && p.Z > Min.Z && p.X < Max.X && p.Y < Max.Y && p.Z < Max.Z;

    public readonly bool Contains(BoundingBox3f box) => box.Min.X >= Min.X && box.Min.Y >= Min.Y && box.Min.Z >= Min.Z && box.Max.X <= Max.X && box.Max.Y <= Max.Y && box.Max.Z <= Max.Z;
    public readonly bool ContainsExclusive(BoundingBox3f box) => box.Min.X > Min.X && box.Min.Y > Min.Y && box.Min.Z > Min.Z && box.Max.X < Max.X && box.Max.Y < Max.Y && box.Max.Z < Max.Z;

    public readonly bool Overlaps(BoundingBox3f box) => box.Min.X <= Max.X && box.Min.Y <= Max.Y && box.Min.Z <= Max.Z && box.Max.X >= Min.X && box.Max.Y >= Min.Y && box.Max.Z >= Min.Z;
    public readonly bool OverlapsExclusive(BoundingBox3f box) => box.Min.X < Max.X && box.Min.Y < Max.Y && box.Min.Z < Max.Z && box.Max.X > Min.X && box.Max.Y > Min.Y && box.Max.Z > Min.Z;

    public readonly float DistanceSquared(Vector3f p) => Sum(Sqr(Max(Float3(0), Max(Min - p, p - Max))));
    public readonly float Distance(Vector3f p) => float.Sqrt(DistanceSquared(p));

    public readonly float DistanceSquared(BoundingBox3f box) => Sum(Sqr(Max(Float3(0), Max(Min - box.Max, box.Min - Max))));
    public readonly float Distance(BoundingBox3f box) => float.Sqrt(DistanceSquared(box));

    public void Clip(BoundingBox3f box)
    {
        Min = Vector3f.Max(Min, box.Min);
        Max = Vector3f.Min(Max, box.Max);
    }

    public void Expand(Vector3f p)
    {
        Min = Vector3f.Min(Min, p);
        Max = Vector3f.Max(Max, p);
    }

    public void Expand(BoundingBox3f box)
    {
        Min = Vector3f.Min(Min, box.Min);
        Max = Vector3f.Max(Max, box.Max);
    }

    public static BoundingBox3f Merge(BoundingBox3f a, BoundingBox3f b) => new(Min(a.Min, b.Min), Max(a.Max, b.Max));

    public readonly (bool isHit, float minT, float maxT) RayIntersect(Vector3f o, Vector3f d)
    {
        /**
         * https://www.pbr-book.org/4ed/Shapes/Basic_Shape_Interface#RayndashBoundsIntersections
         * 包围盒就是三组面对面的 无穷远平面 包围起来的一块空间
         * 光线与6个平面相交，可以计算出6个t，进入包围盒的时间是最大的t，出包围盒时间是最小的t
         * 如果进入时间小于出时间，才与包围盒有交点
         * 且需要注意：
         * 当出时间小于0，其实就是盒子在光线反方向，那就不可能有交点
         * 当进入时间小于0且出时间大于等于0，说明光线起点在盒子里面
         * 因此：当且仅当进入时间小于等于出时间且出时间大于等于0，和AABB有交点
         */
        Vector3f invD = 1 / d;
        Vector3f t1 = (Min - o) * invD; //光线到3组平面的距离
        Vector3f t2 = (Max - o) * invD;
        Vector3f near = Min(t1, t2); //选出3组平面交点中较近与较远
        Vector3f far = Max(t1, t2);
        float minT = MaxElement(near);
        float maxT = MinElement(far);
        bool isHit = maxT >= minT && maxT >= 0;
        return (isHit, minT, maxT);
    }
}
