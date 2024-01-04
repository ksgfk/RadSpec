using System.Numerics;

namespace RadSpec.Test;

public class VectorEqualityComparer<TValue, TDelta> : IEqualityComparer<TValue>
    where TValue : struct, IVector<TValue, TDelta>
    where TDelta : struct, IFloatingPoint<TDelta>
{
    private readonly TDelta _delta;

    public VectorEqualityComparer(TDelta delta)
    {
        _delta = delta;
    }

    public bool Equals(TValue x, TValue y)
    {
        TValue v = TValue.Abs(x - y);
        for (int i = 0; i < TValue.Count; i++)
        {
            if (v[i] > _delta)
            {
                return false;
            }
        }
        return true;
    }

    public int GetHashCode(TValue obj)
    {
        return obj.GetHashCode();
    }
}
