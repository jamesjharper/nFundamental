namespace Fundamental.Interface.Wasapi.Internal
{
    public interface IWasapiAudioRenderClientInterop
    {
        /// <summary>
        /// Gets the size of the free space remaining in the render buffer.
        /// </summary>
        /// <returns></returns>
        int GetFreeBufferByteSize();

        /// <summary>
        /// Releases the current content in the render buffer.
        /// </summary>
        void ReleaseBuffer();

        /// <summary>
        /// Writes the given bytes to the render buffer
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        int Write(byte[] buffer, int offset, int length);
    }
}