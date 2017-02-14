using System;
using System.Runtime.InteropServices;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Win32;

namespace Fundamental.Interface.Wasapi.Internal
{
    public class WasapiAudioRenderClientInterop : IWasapiAudioRenderClientInterop
    {
        /// <summary>
        /// The audio render client
        /// </summary>
        private readonly IAudioRenderClient _audioRenderClient;

        /// <summary>
        /// The audio client
        /// </summary>
        private readonly IAudioClient _audioClient;

        /// <summary>
        /// The frame size
        /// </summary>
        private readonly int _frameSize;

        /// <summary>
        /// The buffer size
        /// </summary>
        private int _bufferSize;

        /// <summary>
        /// The frames written to buffer
        /// </summary>
        private int _framesWrittenToBuffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiAudioRenderClientInterop"/> class.
        /// </summary>
        /// <param name="audioRenderClient">The audio render client.</param>
        /// <param name="audioClient">The audio client.</param>
        /// <param name="frameSize">Size of the frame.</param>
        public WasapiAudioRenderClientInterop(IAudioRenderClient audioRenderClient, IAudioClient audioClient, int frameSize)
        {
            _audioRenderClient = audioRenderClient;
            _audioClient = audioClient;
            _frameSize = frameSize;
        }


        /// <summary>
        /// Writes the given bytes to the render buffer
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public int Write(byte[] buffer, int offset, int length)
        {
            var lengthInFrames = length / _frameSize;

            var freeBufferFrames = GetFreeBufferFrameSize();
            var frameToWrite = Math.Min(lengthInFrames, freeBufferFrames);
            var bytesToWrite = frameToWrite * _frameSize;

            var pBuffer = GetBuffer(frameToWrite);

            if (bytesToWrite == 0) 
                return bytesToWrite;

            Marshal.Copy(buffer, offset, pBuffer, bytesToWrite);
            _framesWrittenToBuffer += frameToWrite;
            return bytesToWrite;
        }

        /// <summary>
        /// Releases the current content in the render buffer.
        /// </summary>
        public void ReleaseBuffer()
        {
           // if(_framesWrittenToBuffer == 0)
            //    return;

            _audioRenderClient.ReleaseBuffer(_framesWrittenToBuffer, AudioClientBufferFlags.None).ThrowIfFailed();
            _framesWrittenToBuffer = 0;
        }

        /// <summary>
        /// Gets the byte size of the free space remaining in the render buffer.
        /// </summary>
        /// <returns></returns>
        public int GetFreeBufferByteSize()
        {
            return GetBufferByteSize() - (GetCurrentPadding() * _frameSize);
        }

        /// <summary>
        /// Gets the frame count of the free space remaining in the render buffer.
        /// </summary>
        /// <returns></returns>
        public int GetFreeBufferFrameSize()
        {
            return GetBufferFrameSize() - GetCurrentPadding();
        }

        // Private Methods

        private int GetCurrentPadding()
        {
            uint outInt;
            _audioClient.GetCurrentPadding(out outInt).ThrowIfFailed();
            return checked((int)outInt);
        }

        private IntPtr GetBuffer(int numberOfrequestedFrames)
        {
            IntPtr buffer;
            _audioRenderClient.GetBuffer(numberOfrequestedFrames, out buffer);
            return buffer;
        }

        private int GetBufferByteSize()
        {
            return GetBufferFrameSize() * _frameSize;
        }


        private int GetBufferFrameSize()
        {
            if (_bufferSize != 0)
                return _bufferSize;

            uint outInt;
            _audioClient.GetBufferSize(out outInt).ThrowIfFailed();
            _bufferSize = checked((int)outInt);
            return _bufferSize;
        }
    }
}
