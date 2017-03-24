using System;
using System.IO;

namespace Fundamental.Wave.Container.Iff
{
    public class ChunkStreamAdapter : Stream
    {

        /// <summary>
        /// The position
        /// </summary>
        private long _cursor;


        /// <summary>
        /// Initializes a new instance of the <see cref="ChunkStreamAdapter"/> class.
        /// </summary>
        /// <param name="chunk">The chunk.</param>
        public ChunkStreamAdapter(Chunk chunk)
        {
            Chunk = chunk;
        }

        /// <summary>
        /// When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        public override void Flush()
        {
            Chunk.Flush();
        }

        /// <summary>
        /// When overridden in a derived class, sets the position within the current stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the <paramref name="origin" /> parameter.</param>
        /// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin" /> indicating the reference point used to obtain the new position.</param>
        /// <returns>
        /// The new position within the current stream.
        /// </returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    return BaseStream.Seek(offset + Chunk.MetaData.StartLocation, origin);
                case SeekOrigin.End:
                    return BaseStream.Seek(Chunk.MetaData.EndLocation - offset, SeekOrigin.Begin);
                case SeekOrigin.Current:
                    return BaseStream.Seek(offset, SeekOrigin.Current);
                default:
                    throw new ArgumentOutOfRangeException(nameof(origin), origin, null);
            }
        }

        /// <summary>
        /// When overridden in a derived class, sets the length of the current stream.
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes.</param>
        public override void SetLength(long value)
        {
            Chunk.SetLength(value);
        }

        /// <summary>
        /// When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between <paramref name="offset" /> and (<paramref name="offset" /> + <paramref name="count" /> - 1) replaced by the bytes read from the current source.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>
        /// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.
        /// </returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            // If the cursor has been moved, we want to move it 
            // the cursor location of this particular chunk 
            // NOTE: reading and writing to chunks is very not 
            // thread safe.
            if (ActualPosition != _cursor)
                ActualPosition = _cursor;

            var remainingBytes = (int)(Length - ActualPosition);
            var readableBytes = Math.Min(count, remainingBytes);
            var bytesRead = Chunk.BaseStream.Read(buffer, offset, readableBytes);

            // Sync up both cursor and position
            _cursor = ActualPosition;
            return bytesRead;
        }

        /// <summary>
        /// When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies <paramref name="count" /> bytes from <paramref name="buffer" /> to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">count - Unable to write past the end of the chunk</exception>
        public override void Write(byte[] buffer, int offset, int count)
        {
            var remainingBytes = Length - _cursor;
            if (remainingBytes < count)
            {
                var incressSize = count - remainingBytes;
                Chunk.SetLength(Length + incressSize);
            }

            // If the cursor has been moved, we want to move it 
            // the cursor location of this particular chunk 
            // NOTE: reading and writing to chunks is very not 
            // thread safe.
            if (ActualPosition != _cursor)
                ActualPosition = _cursor;

            BaseStream.Write(buffer, offset, count);


            // Sync up both cursor and position
            _cursor = ActualPosition;
        }


        /// <summary>
        /// When overridden in a derived class, gets the length in bytes of the stream.
        /// </summary>
        public override long Length => Chunk.ContentSize;


        /// <summary>
        /// The cursor position for this chunk
        /// </summary>
        public override long Position
        {
            get { return _cursor; }
            set
            {
                _cursor = value;
                BaseStream.Position = value + Chunk.MetaData.DataLocation;
            }
        }

        /// <summary>
        /// Gets or sets the actual position.
        /// </summary>
        /// <value>
        /// The actual position.
        /// </value>
        public long ActualPosition
        {
            get { return BaseStream.Position - Chunk.MetaData.DataLocation; }
            set { Position = value; }
        }

        /// <summary>
        /// Gets the base stream.
        /// </summary>
        /// <value>
        /// The base stream.
        /// </value>
        public Stream BaseStream => Chunk.BaseStream;

        /// <summary>
        /// Gets the chunk which content is being streamed.
        /// </summary>
        /// <value>
        /// The chunk.
        /// </value>
        public Chunk Chunk { get; }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports reading.
        /// </summary>
        public override bool CanRead => Chunk.BaseStream.CanRead;

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports seeking.
        /// </summary>
        public override bool CanSeek => Chunk.BaseStream.CanSeek;

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports writing.
        /// </summary>
        public override bool CanWrite => Chunk.BaseStream.CanWrite;

    }
}
