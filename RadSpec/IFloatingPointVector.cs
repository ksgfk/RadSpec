using System.Numerics;

namespace RadSpec;

public interface IFloatingPointVector<TSelf, TElement> : IVector<TSelf, TElement>
    where TSelf : IFloatingPointVector<TSelf, TElement>
    where TElement : IFloatingPoint<TElement>
{
    abstract static TSelf Floor(TSelf v);
    abstract static TSelf Ceiling(TSelf v);
    abstract static TSelf Lerp(TSelf x, TSelf y, TElement t);
    abstract static TSelf Normalize(TSelf v);
    abstract static TSelf Sqrt(TSelf v);
    abstract static TSelf Fma(TSelf x, TSelf y, TSelf z);
    abstract static TSelf Fma(TElement x, TSelf y, TSelf z);
    abstract static TSelf Fma(TSelf x, TElement y, TSelf z);
    abstract static TSelf Fma(TSelf x, TSelf y, TElement z);
    abstract static TSelf Fma(TElement x, TElement y, TSelf z);
    abstract static TSelf Fma(TElement x, TSelf y, TElement z);
    abstract static TSelf Fma(TSelf x, TElement y, TElement z);
    abstract static TElement Distance(TSelf x, TSelf y);
    abstract static TElement DistanceSquared(TSelf x, TSelf y);
    abstract static TElement Dot(TSelf x, TSelf y);
    abstract static TElement AbsDot(TSelf x, TSelf y);
    abstract static TElement Length(TSelf v);
    abstract static TElement LengthSquared(TSelf v);
    abstract static TElement Sum(TSelf v);
    abstract static TElement MinElement(TSelf v);
    abstract static TElement MaxElement(TSelf v);
}
