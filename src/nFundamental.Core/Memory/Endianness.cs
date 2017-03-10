using System;
using MiscUtil.Conversion;

namespace Fundamental.Core.Memory
{
    public enum Endianness
    {
        Little = 1,
        Big = 2
    }


    public static class EndiannessExtentions
    {
        public static EndianBitConverter ToBitConverter(this Endianness @this)
        {
            if (@this == Endianness.Big)
                return EndianBitConverter.Big;

            if (@this == Endianness.Little)
                return EndianBitConverter.Little;

            throw new ArgumentOutOfRangeException(nameof(@this), @this, null);
        }
    }
}
