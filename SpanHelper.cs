using System.Numerics;

namespace NonUniformCouponCollectorProblem;

public static class SpanHelper
{
    public static bool AreAllPositive(this Span<int> span)
    {
        var vectorSize = Vector<int>.Count;
        var zero = Vector<int>.Zero;
        int i;
        for (i = 0; i <= span.Length - vectorSize; i += vectorSize)
        {
            var part = new Vector<int>(span.Slice(i, vectorSize));

            if (!Vector.GreaterThanAll(part, zero))
            {
                return false;
            }
        }
        
        for (; i < span.Length; ++i)
        {
            if (span[i] <= 0)
            {
                return false;
            }
        }

        return true;
    }
}