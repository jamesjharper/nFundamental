namespace Fundamental.Core
{
    public interface IAudioSink
    {

        /// <summary>
        /// Writes the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        int Write(byte[] buffer, int offset, int length);
    }
}
