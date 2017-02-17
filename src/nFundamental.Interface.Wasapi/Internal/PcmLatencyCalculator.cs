using System;


namespace Fundamental.Interface.Wasapi.Internal
{
    public class PcmLatencyCalculator : ILatencyCalculator
    {
        /// <summary>
        /// The frame size
        /// </summary>
        private readonly ulong _frameSize;

        /// <summary>
        /// The length of a single sample in ticks
        /// </summary>
        private readonly double _sampleTickPeriod; 

        /// <summary>
        /// Initializes a new instance of the <see cref="PcmLatencyCalculator"/> class.
        /// </summary>
        /// <param name="frameSize">Size of the frame.</param>
        /// <param name="sampleRate">The sample rate.</param>
        public PcmLatencyCalculator(int frameSize, int sampleRate)
        {
            _frameSize = checked((ulong) frameSize);

            const double ticksPerSecond = TimeSpan.TicksPerSecond;
            _sampleTickPeriod = ticksPerSecond / sampleRate; 
        }

        /// <summary>
        /// Gets the length of the buffer as a time span.
        /// </summary>
        /// <param name="byteSize">Size of the byte.</param>
        /// <returns></returns>
        public TimeSpan BytesToLatency(ulong byteSize)
        {
            var sampleCount = byteSize / _frameSize;
            return FramesToLatency(sampleCount);
        }

        /// <summary>
        /// Gets the length of the buffer as a time span.
        /// </summary>
        /// <param name="frameCount">The frame count.</param>
        /// <returns></returns>
        public TimeSpan FramesToLatency(ulong frameCount)
        {
            var numberOfTicks = _sampleTickPeriod * frameCount;
            return TimeSpan.FromTicks(checked((long)numberOfTicks));
        }

        /// <summary>
        /// Latencies to frames.
        /// </summary>
        /// <param name="latency">The latency.</param>
        /// <returns></returns>
        public ulong LatencyToFrames(TimeSpan latency)
        {
            return checked((ulong) (latency.Ticks / _sampleTickPeriod));
        }

        /// <summary>
        /// Latencies to bytes.
        /// </summary>
        /// <param name="latency">The latency.</param>
        /// <returns></returns>
        public ulong LatencyToBytes(TimeSpan latency)
        {
            return LatencyToFrames(latency) * _frameSize;
        }


    }
}
