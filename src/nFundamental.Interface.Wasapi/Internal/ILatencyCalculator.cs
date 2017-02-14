using System;

namespace Fundamental.Interface.Wasapi.Internal
{
    public interface ILatencyCalculator
    {
        /// <summary>
        /// Gets the length of the buffer as a time span.
        /// </summary>
        /// <param name="byteSize">Size of the byte.</param>
        /// <returns></returns>
        TimeSpan BytesToLatency(int byteSize);

        /// <summary>
        /// Gets the length of the buffer as a time span.
        /// </summary>
        /// <param name="frameCount">Size of the buffer in frames.</param>
        /// <returns></returns>
        TimeSpan FramesToLatency(uint frameCount);

        /// <summary>
        /// Gets the length of the buffer as a time span.
        /// </summary>
        /// <param name="frameCount">Size of the buffer in frames.</param>
        /// <returns></returns>
        TimeSpan FramesToLatency(int frameCount);
    }
}