using System;
using System.Collections.Generic;
using System.IO;

namespace Fundamental.Core.Memory
{
    public class CursorStreamSegment
    {

        /// <summary>
        /// The underlying stream
        /// </summary>
        private readonly Stream _stream;

        /// <summary>
        /// The segment
        /// </summary>
        private StreamSegment _segment;

        /// <summary>
        /// Initializes a new instance of the <see cref="CursorStreamSegment" /> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="position">The position.</param>
        /// <param name="length">The length.</param>
        /// <param name="procession">The procession.</param>
        /// <param name="buffer">The buffer.</param>
        public CursorStreamSegment(Stream stream, long position, int length, int procession, byte[] buffer)
        {
            _stream = stream;
            _segment = new StreamSegment
            {
                Data = buffer,
                Length = length,
                Order = procession
            };

            Position = position;
        }


        /// <summary>
        /// Gets the position at which this segment exists in the stream.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public long Position { get; }

        /// <summary>
        /// Gets the actual length of data content.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        public int Length => _segment.Length;

        /// <summary>
        /// Gets the procession.
        /// </summary>
        /// <value>
        /// The procession.
        /// </value>
        public int Order => _segment.Order;

        /// <summary>
        /// Swaps the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        public void Swap(CursorStreamSegment other)
        {
            Swap(this, other);
        }

        /// <summary>
        /// Swaps the specified a.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <exception cref="System.InvalidOperationException">Can only swap segments of the same size.</exception>
        public static void Swap(CursorStreamSegment a, CursorStreamSegment b)
        {
            var segmentA = a.Read();
        
            // If this is the same buffer instance, we have 
            // to clone the segment so that the buffer can be reused 
            // for the read of segment B
            if (ReferenceEquals(a._segment.Data, b._segment.Data))
                segmentA = segmentA.Clone();
 
            var segmentB = b.Read();

            // Swap content
            a.Write(segmentB);
            b.Write(segmentA);
        }

        /// <summary>
        /// Reads this instance.
        /// </summary>
        /// <returns></returns>
        public StreamSegment Read()
        {
            GotoPosition();
            _segment.Length = _stream.Read(_segment.Data, 0, _segment.Data.Length);
            return _segment;
        }

        /// <summary>
        /// Writes the specified buffer over the segment.
        /// </summary>
        /// <param name="segment">The segment.</param>
        /// <exception cref="System.InvalidOperationException">Can only write segments of the same size.</exception>
        public void Write(StreamSegment segment)
        {
            Write(segment.Data, 0, segment.Length);
        }

        /// <summary>
        /// Writes the specified buffer over the segment.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        public void Write(byte[] buffer)
        {
            Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Writes the specified buffer over the segment.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        /// <exception cref="System.InvalidOperationException">Can only write segments of the same size.</exception>
        public void Write(byte[] buffer, int offset, int count)
        {
            if (count != Length)
                throw new InvalidOperationException("Can only write segments of the same size.");

            GotoPosition();
            _stream.Write(buffer, offset, count);
        }

        /// <summary>
        /// Goes to the segments position.
        /// </summary>
        public void GotoPosition()
        {
            _stream.Position = Position;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is CursorStreamSegment))
                return false;

            return Equals((CursorStreamSegment)obj);
        }

        /// <summary>
        /// Determines whether the specified <see cref="CursorStreamSegment" />, is equal to this instance.
        /// </summary>
        /// <param name="other">The other object.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="CursorStreamSegment" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(CursorStreamSegment other)
        {
            return (Length == other.Length) && (Position == other.Position);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return Length.GetHashCode() * 397 ^ Position.GetHashCode();
            }
        }
    }

    public static class LazyStreamSegmentStreamExtentions
    {

        /// <summary>
        /// Gets a stream buffer enumerator.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <returns></returns>
        public static IEnumerable<CursorStreamSegment> CursoredEnumerate(this Stream stream, int bufferSize)
        {
            return stream.CursoredEnumerate(bufferSize, stream.Position, stream.Length);
        }


        /// <summary>
        /// Gets a stream buffer enumerator.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static IEnumerable<CursorStreamSegment> CursoredEnumerate(this Stream stream, long bufferSize, long length)
        {
            return stream.CursoredEnumerate(checked((int)bufferSize), stream.Position, length);
        }

        /// <summary>
        /// Gets a stream buffer enumerator.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="length">The length.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <returns></returns>
        public static IEnumerable<CursorStreamSegment> CursoredEnumerate(this Stream stream, int bufferSize, long length)
        {
            return stream.CursoredEnumerate(bufferSize, stream.Position, length);
        }

        /// <summary>
        /// Gets a stream buffer enumerator.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="startPosition">The stream position.</param>
        /// <param name="length">The length.</param>
        /// <param name="bufferSize">Size of the segment.</param>
        /// <returns></returns>
        public static IEnumerable<CursorStreamSegment> CursoredEnumerate(this Stream stream, int bufferSize, long startPosition, long length)
        {
            if (bufferSize == 0)
                throw new ArgumentException("Buffer size can not be equal to zero", nameof(bufferSize));

            // Make sure we don't go past the end
            var maxLength = System.Math.Min(startPosition + length, stream.Length);
            length = maxLength - startPosition;

            if (length == 0)
                yield break;

     
            var buffer = new byte[bufferSize];

            var endPosition = startPosition + length;
            var currentPosition = startPosition;
            var procession = 0;

            while (currentPosition != endPosition)
            {
                var byteReaming = (int)(endPosition - currentPosition);
                var predictedDataSize = System.Math.Min(bufferSize, byteReaming);
                procession++;

                yield return new CursorStreamSegment(stream, currentPosition, predictedDataSize, procession, buffer);
                currentPosition += predictedDataSize;
            }
        }
    }
}
