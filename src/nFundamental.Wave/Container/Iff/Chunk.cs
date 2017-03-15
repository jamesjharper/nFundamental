// ReSharper disable BuiltInTypeReferenceStyle

using System;
using System.IO;
using System.Text;

using Fundamental.Core.Memory;

namespace Fundamental.Wave.Container.Iff
{
    public class Chunk 
    {
        /// <summary>
        /// Reads a chunk from a stream using the given standard.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="standardStandard">Type of the chunk standard.</param>
        /// <returns></returns>
        public static Chunk FromStream(Chunk parent, IffStandard standardStandard)
        {
            var chunk = new Chunk
            {
                IffStandard = standardStandard,
                BaseStream = parent.BaseStream,
                ParentChunk = parent
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
        public static Chunk FromStream(Stream stream, IffStandard standardStandard)
        {
            var chunk = new Chunk
            {
                IffStandard = standardStandard,
                BaseStream = stream,
                ParentChunk = (stream as ChunkStreamAdapter)?.Chunk
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
                ParentChunk = (stream as ChunkStreamAdapter)?.Chunk
            };
            chunk.ReadFromStream();
            return chunk;
        }

        /// <summary> Creates a value new chunk. </summary>
        /// <param name="id">The chunk identifier.</param>
        /// <param name="stream">The target stream .</param>
        /// <param name="standardStandard">Type of the chunk standard chunk standard.</param>
        public static Chunk Create(string id, Stream stream, IffStandard standardStandard)
        {
            return Create<Chunk>(id, stream, standardStandard);
        }

        /// <summary> Creates a value new chunk. </summary>
        /// <param name="id">The chunk identifier.</param>
        /// <param name="stream">The target stream .</param>
        /// <param name="standardStandard">Type of the chunk standard chunk standard.</param>
        protected static T Create<T>(string id, Stream stream, IffStandard standardStandard)
            where T : Chunk, new()
        {
            var chunk = new T
            {
                ChunkId = id,
                IffStandard = standardStandard,
                BaseStream = stream,
                ParentChunk = (stream as ChunkStreamAdapter)?.Chunk
            };
            return chunk;
        }

        /// <summary>
        /// The header needs flushing
        /// </summary>
        private bool _headerNeedsFlushing = true;

        /// <summary>
        /// The packing
        /// </summary>
        private readonly PackingCalculator _packing = PackingCalculator.Int16;


        /// <summary>
        /// The pending initial write
        /// </summary>
        private bool _pendingInitialWrite = true;

        /// <summary>
        /// The base stream
        /// </summary>
        public Stream BaseStream { get; set; }

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
        public string ChunkId { get; set; }

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
                ParentChunk = ParentChunk,
                _pendingInitialWrite = _pendingInitialWrite
            };

            BaseStream.Position = DataLocation;
            c.ReadData();

            return c;
        }

        /// <summary>
        /// Sets the length.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetLength(long value)
        {
            if (DataByteSize == value)
                return;

            // Make sure the chunk is at the end of the stream.
            // NOTE: this could end up byte shifting the entire stream.
            MoveChunkToEndOfStream();

            DataByteSize = value;
            _headerNeedsFlushing = true;
        }


        /// <summary>
        /// When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        public void Flush()
        {
            if(!HeaderRequireFlush())
                return;
            
            BaseStream.Position = StartLocation;
            WriteToStream();
            BaseStream.Flush();
        }


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
            _pendingInitialWrite = false;
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
 

        // Private methods
   

        protected T CreateChild<T>(string chunkId) where T : Chunk, new()
        {
            return new T
            {
                ChunkId = chunkId,
                BaseStream = BaseStream,
                IffStandard = IffStandard,
                ParentChunk =  this,
                _pendingInitialWrite = true
            };
        }

        protected void ReadFromStream()
        {
            StartLocation = BaseStream.Position;
            ReadChunk();
            _headerNeedsFlushing = false;
            _pendingInitialWrite = false;
        }


        #region Write

        protected virtual void WriteChunk()
        {
            WriteHeader();
            WriteData();
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
                    if (IffStandard.Has64BitLookupChunk && DataByteSize == 0xFFFFFFFF)
                    {
                        DataByteSize = 0xFFFFFFFF - 1;
                        IsRf64 = true;
                    }
                     

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
