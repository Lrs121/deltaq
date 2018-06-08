using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace deltaq
{
    [DebuggerDisplay("{(long)this}")]
    public readonly ref struct CompactLong
    {
        public readonly ReadOnlySpan<byte> Value;
        public CompactLong(ReadOnlySpan<byte> value)
        {
            this.Value = value;
        }

        public static implicit operator CompactLong(long y)
        {
            //var l = MemoryMarshal.CreateSpan(ref y, 1);
            //var b = MemoryMarshal.Cast<long, byte>(l);
            var b = new byte[sizeof(long)];
            unchecked
            {
                if (y < 0)
                {
                    y = -y;

                    b[0] = (byte)y;
                    b[1] = (byte)(y >>= 8);
                    b[2] = (byte)(y >>= 8);
                    b[3] = (byte)(y >>= 8);
                    b[4] = (byte)(y >>= 8);
                    b[5] = (byte)(y >>= 8);
                    b[6] = (byte)(y >>= 8);
                    b[7] = (byte)((y >> 8) | 0x80);
                }
                else
                {
                    b[0] = (byte)y;
                    b[1] = (byte)(y >>= 8);
                    b[2] = (byte)(y >>= 8);
                    b[3] = (byte)(y >>= 8);
                    b[4] = (byte)(y >>= 8);
                    b[5] = (byte)(y >>= 8);
                    b[6] = (byte)(y >>= 8);
                    b[7] = (byte)(y >> 8);
                }
            }
            return new CompactLong(b);
        }

        public static implicit operator long(CompactLong x)
        {
            var b = x.Value;

            long y = b[7] & 0x7F;
            y <<= 8; y += b[6];
            y <<= 8; y += b[5];
            y <<= 8; y += b[4];
            y <<= 8; y += b[3];
            y <<= 8; y += b[2];
            y <<= 8; y += b[1];
            y <<= 8; y += b[0];

            return (b[7] & 0x80) != 0 ? -y : y;
        }
    }
}
