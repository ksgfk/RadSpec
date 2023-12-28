namespace RadSpec;

public interface IArrayAdditionScalar<TSelf, TElement> where TSelf : IArrayAdditionScalar<TSelf, TElement>
{
    abstract static TSelf operator +(TSelf lhs, TElement rhs);
    abstract static TSelf operator +(TElement lhs, TSelf rhs);
}
