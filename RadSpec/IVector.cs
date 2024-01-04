using System.Numerics;

namespace RadSpec;

public interface IVector<TSelf, TElement> :
    IAdditionOperators<TSelf, TSelf, TSelf>,
    ISubtractionOperators<TSelf, TSelf, TSelf>,
    IMultiplyOperators<TSelf, TSelf, TSelf>,
    IDivisionOperators<TSelf, TSelf, TSelf>,
    IEqualityOperators<TSelf, TSelf, bool>,
    IEquatable<TSelf>
    where TSelf : IVector<TSelf, TElement>
    where TElement : INumber<TElement>
{
    abstract static int Count { get; }

    TElement this[int index] { get; set; }

    abstract static TSelf Abs(TSelf v);
}
