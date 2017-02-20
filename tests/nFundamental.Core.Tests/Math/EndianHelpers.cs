using System;

namespace Fundamental.Core.Tests.Math
{
    public static class EndianHelpers
    {

        public static byte[] ToLittleEndianBytes(UInt32 x)
        {
            return BitConverter.GetBytes(ToLittleEndianness(x));
        }

        public static byte[] ToBigEndianBytes(UInt32 x)
        {
            return BitConverter.GetBytes(ToBigEndianness(x));
        }



        public static UInt32 ToLittleEndianness(UInt32 x)
        {
            return BitConverter.IsLittleEndian ? x : SwapEndianness(x);
        }

        public static UInt32 ToBigEndianness(UInt32 x)
        {
            return !BitConverter.IsLittleEndian ? x : SwapEndianness(x);
        }

        public static UInt32 SwapEndianness(UInt32 x)
        {
            return ((x & 0x000000ff) << 24) +
                   ((x & 0x0000ff00) << 8) +
                   ((x & 0x00ff0000) >> 8) +
                   ((x & 0xff000000) >> 24);
        }

    }
}
