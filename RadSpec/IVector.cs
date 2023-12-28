using System.Numerics;

namespace RadSpec;

public interface IVector<TSelf, TElement>
    : IAdditionOperators<TSelf, TSelf, TSelf>,
      ISubtractionOperators<TSelf, TSelf, TSelf>,
      IMultiplyOperators<TSelf, TSelf, TSelf>,
      IDivisionOperators<TSelf, TSelf, TSelf>,
      IArrayAdditionScalar<TSelf, TElement>,
      IArraySubtractionScalar<TSelf, TElement>,
      IArrayMultiplyScalar<TSelf, TElement>,
      IArrayDivisionScalar<TSelf, TElement>,
      IUnaryNegationOperators<TSelf, TSelf>,
      IIncrementOperators<TSelf>,
      IDecrementOperators<TSelf>,
      IEqualityOperators<TSelf, TSelf, bool>,
      IEquatable<TSelf>
    where TSelf : IVector<TSelf, TElement>
{
    TElement this[int i] { get; set; }

    abstract static int Count { get; }
    abstract static TSelf Zero { get; }
    abstract static TSelf One { get; }

    abstract static TSelf Abs(TSelf v);
    abstract static TSelf Clamp(TSelf v, TSelf min, TSelf max);
    abstract static TSelf Max(TSelf x, TSelf y);
    abstract static TSelf Min(TSelf x, TSelf y);
}
