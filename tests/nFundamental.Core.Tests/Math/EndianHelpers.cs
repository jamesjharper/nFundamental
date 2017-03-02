using System;
using Fundamental.Core.Memory;

namespace Fundamental.Core.Tests.Math
{
    public static class EndianHelpers
    {
        public static byte[] Int32Bytes(UInt32 x, Endianness endianness)
        {
            switch (endianness)
            {
                case Endianness.Big:
                    return BitConverter.GetBytes(ToBigEndianness(x));
                case Endianness.Little:
                    return BitConverter.GetBytes(ToLittleEndianness(x));
                default:
                    throw new ArgumentOutOfRangeException(nameof(endianness), endianness, null);
            }
        }
        public static byte[] Int32Bytes(UInt64 x, Endianness endianness)
        {
            switch (endianness)
            {
                case Endianness.Big:
                    return BitConverter.GetBytes(ToBigEndianness(x));
                case Endianness.Little:
                    return BitConverter.GetBytes(ToLittleEndianness(x));
                default:
                    throw new ArgumentOutOfRangeException(nameof(endianness), endianness, null);
            }
        }


        public static UInt32 ToLittleEndianness(UInt32 x)
        {
            return BitConverter.IsLittleEndian ? x : SwapEndianness(x);
        }

        public static UInt32 ToBigEndianness(UInt32 x)
        {
            return !BitConverter.IsLittleEndian ? x : SwapEndianness(x);
        }

        public static UInt64 ToLittleEndianness(UInt64 x)
        {
            return BitConverter.IsLittleEndian ? x : SwapEndianness(x);
        }

        public static UInt64 ToBigEndianness(UInt64 x)
        {
            return !BitConverter.IsLittleEndian ? x : SwapEndianness(x);
        }

        public static UInt32 SwapEndianness(UInt32 x)
        {
            return  ((x & 0x000000ff) << 24) + 
                    ((x & 0x0000ff00) << 8) + 
                    ((x & 0x00ff0000) >> 8) + 
                    ((x & 0xff000000) >> 24);
        }

        public static UInt64 SwapEndianness(UInt64 x)
        {
            x = (x & 0x00000000FFFFFFFF) << 32 | (x & 0xFFFFFFFF00000000) >> 32;
            x = (x & 0x0000FFFF0000FFFF) << 16 | (x & 0xFFFF0000FFFF0000) >> 16;
            x = (x & 0x00FF00FF00FF00FF) << 8 | (x & 0xFF00FF00FF00FF00) >> 8;
            return x;
        }
    }
}
