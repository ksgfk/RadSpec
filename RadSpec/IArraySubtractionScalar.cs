namespace RadSpec;

public interface IArraySubtractionScalar<TSelf, TElement> where TSelf : IArraySubtractionScalar<TSelf, TElement>
{
    abstract static TSelf operator -(TSelf lhs, TElement rhs);
    abstract static TSelf operator -(TElement lhs, TSelf rhs);
}
