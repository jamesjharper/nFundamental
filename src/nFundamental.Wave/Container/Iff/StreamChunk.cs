using System.IO;

namespace Fundamental.Wave.Container.Iff
{
    public class StreamChunk : Chunk
    {
        /// <summary>
        /// The adapter
        /// </summary>
        private ChunkStreamAdapter _adapter;

        /// <summary>
        /// Gets the stream.
        /// </summary>
        /// <value>
        /// The stream.
        /// </value>
        public ChunkStreamAdapter Stream => _adapter ?? (_adapter = new ChunkStreamAdapter(this));

        /// <summary>
        /// When overridden in a derived class, sets the position within the current stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the <paramref name="origin" /> parameter.</param>
        /// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin" /> indicating the reference point used to obtain the new position.</param>
        /// <returns>
        /// The new position within the current stream.
        /// </returns>
        public long Seek(long offset, SeekOrigin origin) => Stream.Seek(offset, origin);

        /// <summary> Reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read. </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between <paramref name="offset" /> and (<paramref name="offset" /> + <paramref name="count" /> - 1) replaced by the bytes read from the current source.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>
        /// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.
        /// </returns>
        public int Read(byte[] buffer, int offset, int count) => Stream.Read(buffer, offset, count);

        /// <summary> Reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read. </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between <paramref name="offset" /> and (<paramref name="offset" /> + <paramref name="count" /> - 1) replaced by the bytes read from the current source.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>
        /// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.
        /// </returns>
        public int Read(byte[] buffer) => Stream.Read(buffer, 0, buffer.Length);


        /// <summary> Writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written. </summary>
        /// <param name="buffer">An array of bytes. This method copies <paramref name="count" /> bytes from <paramref name="buffer" /> to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">count - Unable to write past the end of the chunk</exception>
        public void Write(byte[] buffer, int offset, int count) => Stream.Write(buffer, offset, count);

        /// <summary> Writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written. </summary>
        /// <param name="buffer">The buffer.</param>
        public void Write(byte[] buffer) => Stream.Write(buffer, 0, buffer.Length);

        /// <summary>
        /// When overridden in a derived class, gets the length in bytes of the stream.
        /// </summary>
        public long Length => Stream.Length;

        /// <summary>
        /// The cursor position for this chunk
        /// </summary>
        public long Position
        {
            get { return Stream.Position; }
            set { Stream.Position = value; }
        }

        /// <summary> Reads a chunk from a stream using the given standard. </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="standardStandard">Type of the chunk standard.</param>
        /// <returns></returns>
        public new static StreamChunk FromStream(Stream stream, IffStandard standardStandard) 
            => Chunk.FromStream<StreamChunk>(stream, standardStandard);

        /// <summary> Creates a value new chunk. </summary>
        /// <param name="id">The chunk identifier.</param>
        /// <param name="stream">The target stream .</param>
        /// <param name="standardStandard">Type of the chunk standard chunk standard.</param>
        public new static StreamChunk Create(string id, Stream stream, IffStandard standardStandard)
        {
            return Create<StreamChunk>(id, stream, standardStandard);
        }
    }
}
