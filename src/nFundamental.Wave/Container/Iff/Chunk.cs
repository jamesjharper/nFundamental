// ReSharper disable BuiltInTypeReferenceStyle

using System;
using System.IO;
using System.Text;

using Fundamental.Core.Memory;

namespace Fundamental.Wave.Container.Iff
{
    public class Chunk : Stream
    {

        /// <summary>
        /// The header needs flushing
        /// </summary>
        private bool _headerNeedsFlushing;

        /// <summary>
        /// The base stream
        /// </summary>
        protected Stream BaseStream { get; set; }

        /// <summary>
        /// Gets or sets the chunk identifier.
        /// </summary>
        /// <value>
        /// The chunk identifier.
        /// </value>
        public string ChunkId { get; protected set; }

        /// <summary>
        /// Gets the information file format type settings.
        /// </summary>
        /// <value>
        /// The information file format type settings.
        /// </value>
        public IffStandard IffStandard { get; set;  }

        /// <summary>
        /// Gets the total size of the byte.
        /// </summary>
        /// <value>
        /// The total size of the byte.
        /// </value>
        public Int64 TotalByteSize => HeaderByteSize + PaddedDataByteSize;

        /// <summary>
        /// Gets a value indicating whether this instance is r F64.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is r F64; otherwise, <c>false</c>.
        /// </value>
        public bool IsRf64 { get; private set; }

        /// <summary>
        /// The size of the padded content byte.
        /// </summary>
        /// <value>
        /// The size of the padded content byte.
        /// </value>
        public Int64 PaddedDataByteSize => DataByteSize + PaddingBytes;

        /// <summary>
        /// The chunk content byte size
        /// </summary>
        public Int64 DataByteSize { get; set; }

        /// <summary>
        /// Gets the size of the header byte.
        /// </summary>
        /// <value>
        /// The size of the header byte.
        /// </value>
        public Int64 HeaderByteSize
        {
            get
            {
                const int size = (sizeof(byte) * 4); // Chunks Id
                switch (IffStandard.AddressSize)
                {
                    case AddressSize.UInt32:
                        return size + sizeof(UInt32);
                    case AddressSize.UInt64:
                        return size + sizeof(UInt64);
                    case AddressSize.UInt16:
                        return size + sizeof(UInt16);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public Int64 DataLocation => StartLocation + HeaderByteSize;

        /// <summary>
        /// Gets the start location.
        /// </summary>
        /// <value>
        /// The start location.
        /// </value>
        public Int64 StartLocation { get; set; }

        /// <summary>
        /// Gets the end location.
        /// Note: EA IFF 85 Standard for Interchange Format Files states that
        /// chucks should be 16bit aligned 
        /// </summary>
        /// <value>
        /// The start location.
        /// </value>
        public Int64 EndLocation => StartLocation + HeaderByteSize + PaddedDataByteSize;

        /// <summary>
        /// Gets the padding bytes.
        /// </summary>
        /// <value>
        /// The padding bytes.
        /// </value>
        public Int32 PaddingBytes => (Int32)(DataByteSize % 2);

        /// <summary>
        /// Invalidates this instance.
        /// </summary>
        protected void FlagHeaderForFlush()
        {
            _headerNeedsFlushing = true;
        }

        /// <summary>
        /// Seeks to end of chunk.
        /// </summary>
        public void SeekToEndOfChunk()
        {
            Position = PaddedDataByteSize;
        }

        /// <summary>
        /// Serialize as.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T As<T>() where  T : Chunk, new()
        {
            return new T
            {
                StartLocation = StartLocation,
                DataByteSize = DataByteSize,
                ChunkId =  ChunkId
            };
        }

        #region Stream Impl

        /// <summary>
        /// When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        public override void Flush()
        {
            if(!_headerNeedsFlushing)
                return;
            BaseStream.Position = StartLocation;
            Write();
            BaseStream.Flush();
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
                    return BaseStream.Seek(offset + StartLocation, origin);
                case SeekOrigin.End:
                    return BaseStream.Seek(EndLocation - offset, SeekOrigin.Begin);
                case SeekOrigin.Current:
                    return BaseStream.Seek(offset, SeekOrigin.Current);
                default:
                    throw new ArgumentOutOfRangeException(nameof(origin), origin, null);
            }
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
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
            var remainingBytes = (int) (DataByteSize - Position);
            var readableBytes = Math.Min(count, remainingBytes);
            return BaseStream.Read(buffer, offset, readableBytes);
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
            var remainingBytes = DataByteSize - Position;
            if (remainingBytes < count)
            {
                var newBytesWritten = count - remainingBytes;
                DataByteSize += newBytesWritten;
                _headerNeedsFlushing = true;
            }

            BaseStream.Write(buffer, offset, count);
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports reading.
        /// </summary>
        public override bool CanRead => BaseStream.CanRead;

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports seeking.
        /// </summary>
        public override bool CanSeek => BaseStream.CanSeek;

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports writing.
        /// </summary>
        public override bool CanWrite => BaseStream.CanWrite;

        /// <summary>
        /// When overridden in a derived class, gets the length in bytes of the stream.
        /// </summary>
        public override long Length => DataByteSize;

        /// <summary>
        /// When overridden in a derived class, gets or sets the position within the current stream.
        /// </summary>
        public override long Position
        {
            get { return BaseStream.Position - DataLocation; }
            set { BaseStream.Position = value + DataLocation; }
        }

        #endregion

        /// <summary>
        /// Reads a chunk from a stream using the given standard.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="standardStandard">Type of the chunk standard.</param>
        /// <returns></returns>
        public static Chunk FromStream(Stream stream, IffStandard standardStandard)
        {
            var chunk = new Chunk
            {
                IffStandard = standardStandard,
                BaseStream = stream
            };
            chunk.Read();
            return chunk;
        }

        /// <summary>
        /// Creates a new chunk.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="standardStandard">Type of the chunk standard chunk standard.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.FormatException">Type Id must be exactly 4 chars long</exception>
        public static Chunk Create(string id, Stream stream, IffStandard standardStandard)
        {
            var chunk = new Chunk
            {
                ChunkId = id,
                IffStandard = standardStandard,
                BaseStream = stream
            };

            chunk.Write();
            return chunk;
        }

        // Private methods

        protected void Read()
        {
            StartLocation = BaseStream.Position;
            ReadHeader();
            ReadData();
            _headerNeedsFlushing = false;
        }

        protected void Write()
        {
            StartLocation = BaseStream.Position;
            WriteHeader();
            WriteData();

            // writing the data context may result in another header flush 
            if (_headerNeedsFlushing)
            {
                BaseStream.Position = StartLocation;
                WriteHeader();
                BaseStream.Position = EndLocation;
            }
            _headerNeedsFlushing = false;
        }

        #region Write

        private void WriteHeader()
        {
            WriteChunkId();
            WriteDataSize();
            _headerNeedsFlushing = false;
        }

        private void WriteChunkId()
        {
            var chunkIdBytes = Encoding.UTF8.GetBytes(ChunkId);
            if (chunkIdBytes.Length != 4)
                throw new FormatException("Chunk Id must be exactly 4 chars long");

            BaseStream.Write(chunkIdBytes);
        }

        private void WriteDataSize()
        {
            var binaryWriter = BaseStream.AsEndianWriter(IffStandard.ByteOrder);

            switch (IffStandard.AddressSize)
            {
                case AddressSize.UInt32:

                    var uint32Address = (UInt32)Math.Min(UInt32.MaxValue, DataByteSize);
                    binaryWriter.Write(uint32Address);

                    // If we are supporting RF64 and the data size is equal to 0xFFFFFFFF
                    // Then the actual length is save in another chunk external to this one
                    if (IffStandard.Has64BitLookupChunk && uint32Address == UInt32.MaxValue)
                        IsRf64 = true;

                    break;
                case AddressSize.UInt64:
                    var uint64Address = (UInt64) DataByteSize;
                    binaryWriter.Write(uint64Address);

                    break;
                case AddressSize.UInt16:
                    var uint16Address = (UInt16)Math.Min(UInt16.MaxValue, DataByteSize);
                    binaryWriter.Write(uint16Address);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected virtual void WriteData()
        {
        }

        #endregion

        #region Read
        private void ReadHeader()
        {
            ReadChunkId();
            ReadDataSize();
        }

        private void ReadChunkId()
        {
            var chunkIdBytes = BaseStream.Read(4);
            ChunkId = Encoding.UTF8.GetString(chunkIdBytes, 0, chunkIdBytes.Length);
        }

        private void ReadDataSize()
        {
            var binaryReader = BaseStream.AsEndianReader(IffStandard.ByteOrder);
            switch (IffStandard.AddressSize)
            {
                case AddressSize.UInt32:
                    DataByteSize = binaryReader.ReadUInt32();

                    // If we are supporting RF64 and the data size is equal to 0xFFFFFFFF
                    // Then the actual length is save in another chunk external to this one
                    if (IffStandard.Has64BitLookupChunk && DataByteSize == UInt32.MaxValue)
                        IsRf64 = true;

                    break;
                case AddressSize.UInt64:
                    // Truncate down to Int64.MaxValue as .net doesn't give native access to unsigned 64bit file cursors
                    var dataSize = binaryReader.ReadUInt64();
                    DataByteSize = (Int64) Math.Min(Int64.MaxValue, dataSize);
                    break;
                case AddressSize.UInt16:
                    DataByteSize = binaryReader.ReadUInt16();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected virtual void ReadData()
        {
        }

        #endregion
    }
}
