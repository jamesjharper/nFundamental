using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Win32;

namespace Fundamental.Interface.Wasapi.Internal
{
    public class WasapiAudioRenderClientInterop
    {
        private readonly IAudioRenderClient _audioRenderClient;
        private readonly IAudioClient _audioClient;
        private readonly int _frameSize;

        private int _bufferSize = -1;

        private int _framesWrittenToBuffer;

        public WasapiAudioRenderClientInterop(IAudioRenderClient audioRenderClient, IAudioClient audioClient, int frameSize)
        {
            _audioRenderClient = audioRenderClient;
            _audioClient = audioClient;
            _frameSize = frameSize;
        }


        /// <summary>
        /// Writes the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public int Write(byte[] buffer, int offset, int length)
        {
            var lengthInFrames = length / _frameSize;

            var freeBufferSpace = GetFreeBufferSize();
            var frameToWrite = Math.Min(lengthInFrames, freeBufferSpace);
            var bytesToWrite = frameToWrite * _frameSize;

            var pBuffer = GetBuffer(frameToWrite);

            Marshal.Copy(buffer, offset, pBuffer, bytesToWrite);
            _framesWrittenToBuffer += frameToWrite;

            ReleaseBuffer();
            return bytesToWrite;
        }

        public void ReleaseBuffer()
        {
            if(_framesWrittenToBuffer == 0)
                return;

            _audioRenderClient.ReleaseBuffer(_framesWrittenToBuffer, AudioClientBufferFlags.None).ThrowIfFailed();
            _framesWrittenToBuffer = 0;
        }

        public int GetFreeBufferSize()
        {
            return GetBufferSize() - GetCurrentPadding();
        }

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

        private int GetBufferSize()
        {
            if (_bufferSize != -1)
                return _bufferSize;

            uint outInt;
            _audioClient.GetBufferSize(out outInt).ThrowIfFailed();
            _bufferSize = checked((int)outInt);
            return _bufferSize;
        }
    }
}
