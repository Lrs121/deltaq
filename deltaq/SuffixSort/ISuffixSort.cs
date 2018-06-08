using System;

namespace deltaq.SuffixSort
{
    public interface ISuffixSort
    {
        Span<byte> Sort(ReadOnlySpan<byte> buffer);
    }
}