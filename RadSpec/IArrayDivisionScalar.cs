namespace RadSpec;

public interface IArrayDivisionScalar<TSelf, TElement> where TSelf : IArrayDivisionScalar<TSelf, TElement>
{
    abstract static TSelf operator /(TSelf lhs, TElement rhs);
    abstract static TSelf operator /(TElement lhs, TSelf rhs);
}
