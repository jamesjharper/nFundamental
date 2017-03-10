using System;
using System.Collections.Generic;
using System.IO;

namespace Fundamental.Core.Memory
{
    /// <summary>
    /// Represents a segment of stream content
    /// </summary>
    public struct StreamSegment
    {
        /// <summary>
        /// The byte buffer of the stream content
        /// </summary>
        public byte[] Data;

        /// <summary>
        /// The actual length of the byte buffer content. 
        /// </summary>
        public int Length;

        /// <summary>
        /// The order in which segment was created
        /// </summary>
        public int Order;

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public StreamSegment Clone()
        {
            var clone = new StreamSegment
            {
                Length = Length,
                Order = Order,
                Data = new byte[Data.Length]
            };
            Array.Copy(Data, clone.Data, Length);
            return clone;
        }
    }

    public static class StreamSegmentStreamExtentions
    {

        /// <summary>
        /// Gets a stream buffer enumerator of the target stream.
        /// Please note, the buffer is recycled after each iteration!
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="bufferSize">Size of the segment.</param>
        /// <returns></returns>
        public static IEnumerable<StreamSegment> Enumerate(this Stream stream, int bufferSize)
        {
            var segment = new StreamSegment { Data = new byte[bufferSize] };

            while ((segment.Length = stream.Read(segment.Data, 0, segment.Data.Length)) != 0)
            {
                segment.Order ++;
                yield return segment;
            }
        }
    }
}
