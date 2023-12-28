namespace RadSpec;

public interface IArrayMultiplyScalar<TSelf, TElement> where TSelf : IArrayMultiplyScalar<TSelf, TElement>
{
    abstract static TSelf operator *(TSelf lhs, TElement rhs);
    abstract static TSelf operator *(TElement lhs, TSelf rhs);
}
