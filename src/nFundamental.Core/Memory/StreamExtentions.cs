using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MiscUtil.Conversion;
using MiscUtil.IO;

namespace Fundamental.Core.Memory
{
    public  static class StreamExtentions
    {

        /// <summary>
        /// The buffer size
        /// </summary>
        private const int BufferSize = 32 * 1024;

        /// <summary>
        /// Gets the endian writer.
        /// </summary>
        /// <param name="stream">The stream target of the extension method.</param>
        /// <param name="endianness">The endianness.</param>
        /// <returns></returns>
        public static EndianBinaryWriter AsEndianWriter(this Stream stream, Endianness endianness)
        {
            if (endianness == Endianness.Big)
                return new EndianBinaryWriter(EndianBitConverter.Big, stream);
         
            if (endianness == Endianness.Little)
                return new EndianBinaryWriter(EndianBitConverter.Little, stream);

            throw new ArgumentOutOfRangeException(nameof(endianness), endianness, null);
        }

        /// <summary>
        /// Gets the endian reader.
        /// </summary>
        /// <param name="stream">The stream target of the extension method.</param>
        /// <param name="endianness">The endianness.</param>
        /// <returns></returns>
        public static EndianBinaryReader AsEndianReader(this Stream stream, Endianness endianness)
        {
            if (endianness == Endianness.Big)
                return new EndianBinaryReader(EndianBitConverter.Big, stream);

            if (endianness == Endianness.Little)
                return new EndianBinaryReader(EndianBitConverter.Little, stream);

            throw new ArgumentOutOfRangeException(nameof(endianness), endianness, null);
        }

        // Read helpers

        /// <summary>
        /// Reads the bytes.
        /// </summary>
        /// <param name="stream">The stream target of the extension method.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public static byte[] Read(this Stream stream, int count)
        {
            int bytesRead;
            var bytes = stream.Read(count, out bytesRead);
            if(bytesRead != count)
                Array.Resize(ref bytes, bytesRead);

            return bytes;
        }


        /// <summary>
        /// Reads the bytes.
        /// </summary>
        /// <param name="stream">The stream target of the extension method.</param>
        /// <param name="count">The count.</param>
        /// <param name="byteRead">The byte read.</param>
        /// <returns></returns>
        public static byte[] Read(this Stream stream, int count, out int byteRead)
        {
            var bytes = new byte[count];
            byteRead = stream.Read(bytes, 0, count);
            return bytes;
        }

        /// <summary>
        /// Writes the bytes.
        /// </summary>
        /// <param name="stream">The stream target of the extension method.</param>
        /// <param name="bytes">The bytes.</param>
        public static void Write(this Stream stream, byte[] bytes)
        { 
            stream.Write(bytes, 0, bytes.Length);
        }

        #region Rotate

        /// <summary>
        /// Rotates the order of the elements in such a way that the element pointed by middle becomes the new first element.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="shiftVector">The shift vector.</param>
        /// <param name="length">The length.</param>
        public static void Rotate(this Stream stream, int shiftVector, long length)
        {
            stream.Rotate(shiftVector, checked((uint)length), BufferSize);
        }

        /// <summary>
        /// Rotates the order of the elements in such a way that the element pointed by middle becomes the new first element.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="shiftVector">The shift vector.</param>
        /// <param name="length">The length.</param>
        public static void Rotate(this Stream stream, int shiftVector, int length)
        {
            stream.Rotate(shiftVector, checked((uint)length), BufferSize);
        }

        /// <summary>
        /// Rotates the specified shift vector.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="shiftVector">The shift vector.</param>
        /// <param name="length">The length.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        public static void Rotate(this Stream stream, int shiftVector, int length, int bufferSize)
        {
            stream.Rotate(shiftVector, checked((uint)length), checked((uint)bufferSize));
        }

        /// <summary>
        /// Rotates the order of the elements in such a way that the element pointed by middle becomes the new first element.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="shiftVector">The shift vector.</param>
        /// <param name="length">The length.</param>
        public static void Rotate(this Stream stream, long shiftVector, long length)
        {
            stream.Rotate(shiftVector, checked((uint)length), BufferSize);
        }

        /// <summary>
        /// Rotates the order of the elements in such a way that the element pointed by middle becomes the new first element.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="shiftVector">The shift vector.</param>
        /// <param name="length">The length.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        public static void Rotate(this Stream stream, long shiftVector, uint length, uint bufferSize)
        {
            // Calculate the wrap around shift vector, if the given vector was negative
            var asbShiftVector = ToAbsoluteVector(shiftVector, length);

            if (asbShiftVector == 0)
                return;

            if (asbShiftVector == length)
                return;

            var alignedBufferSize = CalculateAlignedBufferSize(asbShiftVector, length, bufferSize);

            var left = stream.CursoredEnumerate(alignedBufferSize, length).ToArray();
            var right = stream.CursoredEnumerate(alignedBufferSize, length).ToArray();
            var midIndex = CalculateSegmentIndex(length, asbShiftVector, alignedBufferSize);
            RotateInner(left, right, midIndex);
        }

        private static uint CalculateSegmentIndex(uint length, uint asbShiftVector, uint alignedBufferSize)
        {
            return (length - asbShiftVector) / alignedBufferSize;
        }

        private static uint ToAbsoluteVector(long shiftVector, uint length)
        {
            return (uint)(shiftVector < 0 ? length - (shiftVector * -1) : shiftVector);
        }

        private static uint CalculateAlignedBufferSize(uint shiftVector, uint length, uint bufferSize)
        {
            var left = length - shiftVector;
            var right = shiftVector;

            var packing = PackingCalculator.AlignToGreatestCommonDivisor(left, right);

            if (bufferSize >= packing.PackageSize)
                return  packing.PackageSize;

            var bufferCount = packing.PackageSize / bufferSize;
            return packing.PackageSize / bufferCount;
        }

        private static void RotateInner(CursorStreamSegment[] left, CursorStreamSegment[] right, uint middleOffset)
        {
            var first = 0L;
            var middle = middleOffset;
            var next = middle;
            var last = (uint)left.Length;

            while (first != next)
            {
                CursorStreamSegment.Swap(left[first++], right[next++]);

                if (next == last) 
                    next = middle;
                else if (first == middle) 
                    middle = next;
            }            
        }

        #endregion
    }
}
