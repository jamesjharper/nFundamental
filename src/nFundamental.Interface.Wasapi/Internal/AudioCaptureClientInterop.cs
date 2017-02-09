using System;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Win32;

namespace Fundamental.Interface.Wasapi.Internal
{
    public class AudioCaptureClientInterop
    {
        /// <summary>
        /// The audio capture client
        /// </summary>
        private readonly IAudioCaptureClient _audioCaptureClient;


        /// <summary>
        /// The p data
        /// </summary>
        private IntPtr _pData;

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
        /// Initializes a new instance of the <see cref="AudioCaptureClientInterop"/> class.
        /// </summary>
        /// <param name="audioCaptureClient">The audio capture client.</param>
        public AudioCaptureClientInterop(IAudioCaptureClient audioCaptureClient)
        {
            _audioCaptureClient = audioCaptureClient;
        }


        /// <summary>
        /// Updates the buffer.
        /// </summary>
        public void UpdateBuffer()
        {
            // Get the available data in the shared buffer.

             _audioCaptureClient.GetBuffer(out _pData, out _currentAudioFramesInBuffer, out _currentBufferFlags, out _devicePosition, out _qpcPosition).ThrowIfFailed();

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
        public uint GetFramesRemaining()
        {
            uint packetLength ;
            _audioCaptureClient.GetNextPacketSize(out packetLength);
            return packetLength;
        }

        /// <summary>
        /// Gets the buffer.
        /// </summary>
        /// <returns></returns>
        public IntPtr GetBuffer()
        {
            return _pData;
        }

        /// <summary>
        /// Gets the buffer size.
        /// </summary>
        /// <returns></returns>
        public uint GetBufferFrameCount()
        {
            return _currentAudioFramesInBuffer;
        }
    }
}
