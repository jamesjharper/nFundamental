using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MiscUtil.Conversion;
using MiscUtil.IO;

namespace Fundamental.Core.Memory
{
    public static class EndianBitConverterExtentions
    {

        /// <summary>
        /// Gets the endianness convert for the given value.
        /// </summary>
        /// <param name="endianness">The endianness.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">endianness - null</exception>
        public static EndianBitConverter AsConverter(this Endianness endianness)
        {
            if (endianness == Endianness.Big)
                return EndianBitConverter.Big;

            if (endianness == Endianness.Little)
                return EndianBitConverter.Little;

            throw new ArgumentOutOfRangeException(nameof(endianness), endianness, null);
        }
    }
}
