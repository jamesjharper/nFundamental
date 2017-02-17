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
        TimeSpan BytesToLatency(ulong byteSize);

        /// <summary>
        /// Gets the length of the buffer as a time span.
        /// </summary>
        /// <param name="frameCount">Size of the buffer in frames.</param>
        /// <returns></returns>
        TimeSpan FramesToLatency(ulong frameCount);

        /// <summary>
        /// Latencies to frames.
        /// </summary>
        /// <param name="latency">The latency.</param>
        /// <returns></returns>
        ulong LatencyToFrames(TimeSpan latency);

        /// <summary>
        /// Latencies to bytes.
        /// </summary>
        /// <param name="latency">The latency.</param>
        /// <returns></returns>
        ulong LatencyToBytes(TimeSpan latency);
    }

    public static class LatencyCalculatorExtentions
    {
        /// <summary>
        /// Gets the length of the buffer as a time span.
        /// </summary>
        /// <param name="this">The extension method target.</param>
        /// <param name="byteSize">Size of the byte.</param>
        /// <returns></returns>
        public static TimeSpan BytesToLatency(this ILatencyCalculator @this, long byteSize)
        {
            return @this.BytesToLatency(checked ((ulong) byteSize));
        }

        /// <summary>
        /// Gets the length of the buffer as a time span.
        /// </summary>
        /// <param name="this">The extension method target.</param>
        /// <param name="frameCount">Size of the buffer in frames.</param>
        /// <returns></returns>
        public static TimeSpan FramesToLatency(this ILatencyCalculator @this, long frameCount)
        {
            return @this.FramesToLatency(checked((ulong)frameCount));
        }

        /// <summary>
        /// Bytes the align latency.
        /// </summary>
        /// <param name="this">The extension method target.</param>
        /// <param name="timeSpan">The time span.</param>
        /// <returns></returns>
        public static TimeSpan ByteAlignLatency(this ILatencyCalculator @this, TimeSpan timeSpan)
        {
            var bytes = @this.LatencyToBytes(timeSpan);
            return @this.BytesToLatency(bytes);
        }
    }
}