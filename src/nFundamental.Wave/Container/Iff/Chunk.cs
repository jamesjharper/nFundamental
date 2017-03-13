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
        private bool _headerNeedsFlushing = true;

        /// <summary>
        /// The packing
        /// </summary>
        private readonly PackingCalculator _packing = PackingCalculator.Int16;

        /// <summary>
        /// The position
        /// </summary>
        private long _cursor;

        /// <summary>
        /// The base stream
        /// </summary>
        protected Stream BaseStream { get; set; }

        /// <summary>
        /// The Parent chunk
        /// </summary>
        protected Chunk ParentChunk { get; set; }

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
        public Int64 PaddedDataByteSize => _packing.RoundUp(DataByteSize);

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
        public Int64 HeaderByteSize => AddressByteSize + (sizeof(byte) * 4);

        /// <summary>
        /// Gets the size of the header byte.
        /// </summary>
        /// <value>
        /// The size of the header byte.
        /// </value>
        public Int64 AddressByteSize
        {
            get
            {
                switch (IffStandard.AddressSize)
                {
                    case AddressSize.UInt32:
                        return sizeof(UInt32);
                    case AddressSize.UInt64:
                        return sizeof(UInt64);
                    case AddressSize.UInt16:
                        return sizeof(UInt16);
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
        /// Invalidates this instance.
        /// </summary>
        public virtual void FlagHeaderForFlush()
        {
            _headerNeedsFlushing = true;
        }

        /// <summary>
        /// Headers the require flush.
        /// </summary>
        /// <returns></returns>
        public virtual bool HeaderRequireFlush()
        {
            return _headerNeedsFlushing;
        }

        /// <summary>
        /// Serialize as.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T As<T>() where  T : Chunk, new()
        {
            var c = new T
            {
                StartLocation = StartLocation,
                DataByteSize = DataByteSize,
                ChunkId = ChunkId,
                BaseStream = BaseStream,
                IffStandard = IffStandard,
                IsRf64 = IsRf64,
                ParentChunk = ParentChunk
            };

            BaseStream.Position = DataLocation;
            c.ReadData();

            return c;
        }

        #region Stream Impl

        /// <summary>
        /// When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        public override void Flush()
        {
            if(!HeaderRequireFlush())
                return;
            
            BaseStream.Position = StartLocation;
            WriteToStream();
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
            // Please don't do this.
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
            // If the cursor has been moved, we want to move it 
            // the cursor location of this particular chunk 
            // NOTE: reading and writing to chunks is very not 
            // thread safe.
            if (ActualPosition != _cursor)
                ActualPosition = _cursor;

            var remainingBytes = (int) (DataByteSize - ActualPosition);
            var readableBytes = Math.Min(count, remainingBytes);
            var bytesRead = BaseStream.Read(buffer, offset, readableBytes);

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
            // If the cursor has been moved, we want to move it 
            // the cursor location of this particular chunk 
            // NOTE: reading and writing to chunks is very not 
            // thread safe.
            if (ActualPosition != _cursor)
                ActualPosition = _cursor;

            var remainingBytes = DataByteSize - ActualPosition;
            if (remainingBytes < count)
            {
                var incressSize = count - remainingBytes;
                ExpandChunkSize(incressSize);
            }


            BaseStream.Write(buffer, offset, count);

            // Sync up both cursor and position
            _cursor = ActualPosition;
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
        /// The cursor position for this chunk
        /// </summary>
        public override long Position
        {
            get { return _cursor; }
            set
            {
                _cursor = value;
                BaseStream.Position = value + DataLocation;
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
            get { return BaseStream.Position - DataLocation;  }
            set { Position = value; }
        }


        #endregion

        /// <summary>
        /// Gets the byte size of the current content.
        /// </summary>
        /// <returns></returns>
        public virtual long CaculateContentSize()
        {
            return DataByteSize;
        }

        /// <summary>
        /// Gets the padded byte size of the current content.
        /// </summary>
        /// <returns></returns>
        public virtual long CaculatePaddedContentSize()
        {
            return _packing.RoundUp(CaculateContentSize());
        }

        /// <summary>
        /// Calculates if this chunk is trailing.
        /// </summary>
        /// <returns></returns>
        public bool CaculateIfTrailingChunk()
        {
            // If this chunk doesn't have parent,
            // then it must be trailing.
            if (ParentChunk == null)
                return true;

            var groupChunk = ParentChunk as GroupChunk;

            if (groupChunk == null)
                return ParentChunk.CaculateIfTrailingChunk();

            // Check if the chunk is the last in the list.
            return groupChunk.Last == this;
        }

        /// <summary>
        /// Writes to stream.
        /// </summary>
        public void WriteToStream()
        {
            StartLocation = BaseStream.Position;
            DataByteSize = CaculateContentSize();
            WriteChunk();

            // writing the data context may result in another header flush 
            if (_headerNeedsFlushing)
            {
                BaseStream.Position = StartLocation;
                WriteHeader();
            }

            BaseStream.Position = EndLocation;
            _headerNeedsFlushing = false;
        }
 
        /// <summary>
        /// Moves the chunk to end of stream.
        /// </summary>
        public void MoveChunkToEndOfStream()
        {
            var baseGroupChunk = ParentChunk as GroupChunk;
            baseGroupChunk?.MoveChunkToEndOfStream(this);
            ParentChunk?.MoveChunkToEndOfStream();
        }
 
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
                BaseStream = stream,
                ParentChunk = stream as Chunk 
            };
            chunk.ReadFromStream();
            return chunk;
        }

        /// <summary>
        /// Reads a chunk from a stream using the given standard.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="standardStandard">Type of the chunk standard.</param>
        /// <returns></returns>
        public static T FromStream<T>(Stream stream, IffStandard standardStandard)
            where T : Chunk, new()
        {
            var chunk = new T
            {
                IffStandard = standardStandard,
                BaseStream = stream,
                ParentChunk =  stream as Chunk
            };
            chunk.ReadFromStream();
            return chunk;
        }

        /// <summary>
        /// Creates a new chunk.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="standardStandard">Type of the chunk standard chunk standard.</param>
        /// <returns></returns>
        /// <exception cref="System.FormatException">Type Id must be exactly 4 chars long</exception>
        public static Chunk Create(string id, Stream stream, IffStandard standardStandard)
        {
            var chunk = new Chunk
            {
                ChunkId = id,
                IffStandard = standardStandard,
                BaseStream = stream,
                ParentChunk = stream as Chunk
            };
            return chunk;
        }

        // Private methods
   
        protected void ReadFromStream()
        {
            StartLocation = BaseStream.Position;
            ReadChunk();
            _headerNeedsFlushing = false;
        }

        protected T CreateChild<T>(string chunkId) where T : Chunk, new()
        {
            return new T
            {
                ChunkId = chunkId,
                BaseStream = BaseStream,
                IffStandard = IffStandard,
                ParentChunk =  this
            };
        }

        private void ExpandChunkSize(long count)
        {
            // Make sure the chunk is at the end of the stream.
            // NOTE: this could end up byte shifting the entire stream.
            MoveChunkToEndOfStream();

            DataByteSize += count;
            _headerNeedsFlushing = true;
        }

        #region Write

        protected virtual void WriteChunk()
        {
            WriteHeader();

            // save out the cursor position so that 
            // it can be set to the start of the stream for the writing process 
            var cursor = _cursor;
            _cursor = 0;
            WriteData();
            _cursor = cursor;
        }

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

        private void ReadChunk()
        {
            ReadHeader();
            ReadData();
        }

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
