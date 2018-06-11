using deltaq;
using System.Runtime.InteropServices;
using Xunit;

namespace deltaq_tests
{
    public class CompactLongTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(long.MinValue)]
        [InlineData(long.MinValue + 1)]
        [InlineData(long.MaxValue)]
        [InlineData(long.MaxValue - 1)]
        public void CastCompactLong(long l)
        {
            CompactLong cl = l;
            long dummy_long = cl;
            Assert.Equal<long>(cl, l);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(long.MinValue)]
        [InlineData(long.MinValue + 1)]
        [InlineData(long.MaxValue)]
        [InlineData(long.MaxValue - 1)]
        public void SameAsBsdiffOfftout(long x)
        {
            var b = new byte[sizeof(long)];

            long y;
            if (x < 0) y = -x; else y = x;

            b[0] = (byte)(y % 256); y -= b[0];
            y = y / 256; b[1] = (byte)(y % 256); y -= b[1];
            y = y / 256; b[2] = (byte)(y % 256); y -= b[2];
            y = y / 256; b[3] = (byte)(y % 256); y -= b[3];
            y = y / 256; b[4] = (byte)(y % 256); y -= b[4];
            y = y / 256; b[5] = (byte)(y % 256); y -= b[5];
            y = y / 256; b[6] = (byte)(y % 256); y -= b[6];
            y = y / 256; b[7] = (byte)(y % 256);

            if (x < 0) b[7] |= 0x80;

            CompactLong cl = new CompactLong(b);
            Assert.Equal<long>(x, cl);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(long.MinValue)]
        [InlineData(long.MinValue + 1)]
        [InlineData(long.MaxValue)]
        [InlineData(long.MaxValue - 1)]
        public void SameAsBsdiffOfftin(long x)
        {
            CompactLong cl = x;
            var b = cl.Value;

            long y;
            y = b[7] & 0x7F;
            y = y * 256; y += b[6];
            y = y * 256; y += b[5];
            y = y * 256; y += b[4];
            y = y * 256; y += b[3];
            y = y * 256; y += b[2];
            y = y * 256; y += b[1];
            y = y * 256; y += b[0];

            if ((b[7] & 0x80) != 0) y = -y;

            Assert.Equal<long>(y, cl);
        }
    }
}
