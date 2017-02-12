using System;
using System.Runtime.InteropServices;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Win32;

namespace Fundamental.Interface.Wasapi.Internal
{
    public class WasapiAudioCaptureClientInterop : IWasapiAudioCaptureClientInterop
    {
        /// <summary>
        /// The audio capture client
        /// </summary>
        private readonly IAudioCaptureClient _audioCaptureClient;

        /// <summary>
        /// The frame size
        /// </summary>
        private readonly int _frameSize;

        /// <summary>
        /// The sample size
        /// </summary>
        private readonly int _sampleRate;

        /// <summary>
        /// The p data
        /// </summary>
        private IntPtr _pData;

        /// <summary>
        /// The p data offset
        /// </summary>
        private int _pDataOffset;

        /// <summary>
        /// The current audio frames in buffer
        /// </summary>
        private uint _currentAudioFramesInBuffer;

        /// <summary>
        /// The current buffer flags
        /// </summary>
        private AudioClientBufferFlags _currentBufferFlags;

        /// <summary>
        /// The device position
        /// </summary>
        private UInt64 _devicePosition;

        /// <summary>
        /// The QPC position
        /// </summary>
        private UInt64 _qpcPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiAudioCaptureClientInterop"/> class.
        /// </summary>
        /// <param name="audioCaptureClient">The audio capture client.</param>
        /// <param name="frameSize"></param>
        /// <param name="sampleRate"></param>
        public WasapiAudioCaptureClientInterop(IAudioCaptureClient audioCaptureClient, int frameSize, int sampleRate)
        {
            _audioCaptureClient = audioCaptureClient;
            _frameSize = frameSize;
            _sampleRate = sampleRate;
        }


        /// <summary>
        /// Updates the buffer.
        /// </summary>
        public void UpdateBuffer()
        {
            // Get the available data in the shared buffer.
            _audioCaptureClient.GetBuffer(out _pData, out _currentAudioFramesInBuffer, out _currentBufferFlags,
                out _devicePosition, out _qpcPosition).ThrowIfFailed();
            _pDataOffset = 0;
            if ((_currentBufferFlags & AudioClientBufferFlags.Silent) != 0)
            {
                _pData = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Releases the buffer.
        /// </summary>
        public void ReleaseBuffer()
        {
            if (_currentAudioFramesInBuffer == 0)
                return;

            _audioCaptureClient.ReleaseBuffer(_currentAudioFramesInBuffer);
            _currentAudioFramesInBuffer = 0;
        }

        /// <summary>
        /// Gets the bytes remaining.
        /// </summary>
        /// <returns></returns>
        public int GetFramesRemaining()
        {
            uint packetLength;
            _audioCaptureClient.GetNextPacketSize(out packetLength);
            return checked((int) packetLength);
        }


        /// <summary>
        /// Writes the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public int Read(byte[] buffer, int offset, int length)
        {
            var lengthInFrames = length/_frameSize;

            var framesWritten = Math.Min(lengthInFrames, (int) _currentAudioFramesInBuffer);
            var bytesWritten = framesWritten*_frameSize;

            Marshal.Copy(_pData + _pDataOffset, buffer, offset, bytesWritten);

            _pDataOffset += bytesWritten;

            // Release the number of frames read from the buffer
            _audioCaptureClient.ReleaseBuffer((uint) framesWritten);

            return bytesWritten;
        }

        /// <summary>
        /// Gets the buffer size.
        /// </summary>
        /// <returns></returns>
        public int GetBufferFrameCount()
        {
            return checked((int) _currentAudioFramesInBuffer);
        }

        /// <summary>
        /// Gets the buffer size.
        /// </summary>
        /// <returns></returns>
        public int GetBufferByteSize()
        {
            return checked((int) _currentAudioFramesInBuffer * _frameSize);
        }


        /// <summary>
        /// Gets the length of the buffer as a time span.
        /// </summary>
        /// <param name="byteSize">Size of the byte.</param>
        /// <returns></returns>
        public TimeSpan BytesToLatency(int byteSize)
        {
            var sampleCount = byteSize /_frameSize;
            return FramesToLatency(sampleCount);
        }

        /// <summary>
        /// Gets the length of the buffer as a time span.
        /// </summary>
        /// <param name="frameCount">Size of the buffer in frames.</param>
        /// <returns></returns>
        public TimeSpan FramesToLatency(int frameCount)
        {
            const ulong ticksPerSecond = TimeSpan.TicksPerSecond;

            var samplePeriod = ticksPerSecond / (ulong)_sampleRate;

            var numberOfTicks = samplePeriod * (ulong)frameCount;
            return TimeSpan.FromTicks(checked((long)numberOfTicks));
        }
    }
}
