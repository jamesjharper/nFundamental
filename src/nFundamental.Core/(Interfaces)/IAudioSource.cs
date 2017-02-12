namespace Fundamental.Core
{
    public interface IAudioSource
    {

        /// <summary>
        /// Reads the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="length">The length.</param>
        int Read(byte[] buffer, int offset, int length);

    }
}
