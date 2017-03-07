using System;
using System.IO;
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

        /// <summary>
        /// Performs an in place swap of bytes in the stream
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="position">The position.</param>
        /// <param name="mid">The mid.</param>
        /// <param name="end">The end.</param>
        public static void Swap(this Stream stream, int position, int mid, int end)
        {
            stream.Swap(position, mid, end, BufferSize);
        }

        /// <summary>
        /// Performs an in place swap of bytes in the stream
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="position">The position.</param>
        /// <param name="mid">The mid.</param>
        /// <param name="end">The end.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        public static void Swap(this Stream stream, int position, int mid, int end, int bufferSize)
        {
            var leftBuffer = new byte[bufferSize];
            var rightBuffer = new byte[bufferSize];
            var leftCursor = position;
            var rightCursor = (mid + end) - mid;

            var endPosition = position + mid + end;

            while (rightCursor != endPosition)
            {
                // Read left
                stream.Position = leftCursor;
                var leftBytesRead = stream.Read(leftBuffer, 0, leftBuffer.Length);

                // Read left
                stream.Position = rightCursor;
                var rightBytesRead = stream.Read(rightBuffer, 0, rightBuffer.Length);

                // Write left
                stream.Position = rightCursor;
                stream.Write(leftBuffer, 0, leftBytesRead);

                // Write right
                stream.Position = leftCursor;
                stream.Write(rightBuffer, 0, rightBytesRead);

                leftCursor += leftBytesRead;
                rightCursor += rightBytesRead;
            }
        }
    }
}
