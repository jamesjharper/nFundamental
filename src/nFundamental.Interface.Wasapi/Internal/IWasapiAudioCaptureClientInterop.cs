using System;

namespace Fundamental.Interface.Wasapi.Internal
{
    public interface IWasapiAudioCaptureClientInterop
    {
        /// <summary>
        /// Gets the size of the buffer byte.
        /// </summary>
        /// <returns></returns>
        int GetBufferByteSize();

        /// <summary>
        /// Gets the buffer frame count.
        /// </summary>
        /// <returns></returns>
        int GetBufferFrameCount();

        /// <summary>
        /// Gets the frames remaining.
        /// </summary>
        /// <returns></returns>
        int GetFramesRemaining();

        /// <summary>
        /// Releases the buffer.
        /// </summary>
        void ReleaseBuffer();

        /// <summary>
        /// Updates the buffer.
        /// </summary>
        void UpdateBuffer();

        /// <summary>
        /// Writes the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        int Read(byte[] buffer, int offset, int length);

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
        TimeSpan FramesToLatency(int frameCount);
    }
}