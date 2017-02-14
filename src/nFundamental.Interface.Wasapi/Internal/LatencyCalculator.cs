using System;


namespace Fundamental.Interface.Wasapi.Internal
{
    public class LatencyCalculator : ILatencyCalculator
    {
        /// <summary>
        /// The frame size
        /// </summary>
        private readonly int _frameSize;

        /// <summary>
        /// The sample rate
        /// </summary>
        private readonly ulong _sampleRate;

        /// <summary>
        /// Initializes a new instance of the <see cref="LatencyCalculator"/> class.
        /// </summary>
        /// <param name="frameSize">Size of the frame.</param>
        /// <param name="sampleRate">The sample rate.</param>
        public LatencyCalculator(int frameSize, ulong sampleRate)
        {
            _frameSize = frameSize;
            _sampleRate = sampleRate;
        }

        /// <summary>
        /// Gets the length of the buffer as a time span.
        /// </summary>
        /// <param name="byteSize">Size of the byte.</param>
        /// <returns></returns>
        public TimeSpan BytesToLatency(int byteSize)
        {
            var sampleCount = byteSize / _frameSize;
            return FramesToLatency(sampleCount);
        }

        /// <summary>
        /// Gets the length of the buffer as a time span.
        /// </summary>
        /// <param name="frameCount">Size of the buffer in frames.</param>
        /// <returns></returns>
        public TimeSpan FramesToLatency(int frameCount)
        {
            return FramesToLatency(checked((uint)frameCount));
        }

        /// <summary>
        /// Gets the length of the buffer as a time span.
        /// </summary>
        /// <param name="frameCount">The frame count.</param>
        /// <returns></returns>
        public TimeSpan FramesToLatency(uint frameCount)
        {
            const ulong ticksPerSecond = TimeSpan.TicksPerSecond;
            var samplePeriod = ticksPerSecond / _sampleRate;

            var numberOfTicks = samplePeriod * frameCount;
            return TimeSpan.FromTicks(checked((long)numberOfTicks));
        }
    }
}
