// ReSharper disable BuiltInTypeReferenceStyle

using System;
using System.IO;
using System.Text;

using Fundamental.Core.Memory;

namespace Fundamental.Wave.Container.Iff
{
    public class Chunk 
    {
        /// <summary> Reads a chunk from a stream using the given IFF standard.</summary>
        /// <param name="parent">The parent.</param>
        /// <param name="standard">The IFF standard.</param>
        public static Chunk FromStream(Chunk parent, IffStandard standard)
        {
            var chunk = new Chunk
            {
                IffStandard = standard,
                BaseStream = parent.BaseStream,
                ParentChunk = parent
            };
            chunk.ReadFromStream();
            return chunk;
        }

        /// <summary> Reads a chunk from a stream using the given IFF standard. </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="standard">The IFF standard.</param>
        public static Chunk FromStream(Stream stream, IffStandard standard)
        {
            return FromStream<Chunk>(stream, standard);
        }

        /// <summary> Reads a chunk from a stream using the given IFF standard. </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="standard">The IFF standard.</param>
        public static T FromStream<T>(Stream stream, IffStandard standard)
            where T : Chunk, new()
        {
            var chunk = new T();
            FromStream(chunk, stream, standard);
            return chunk;
        }

        /// <summary> Reads a chunk from a stream using the given IFF standard. </summary>
        /// <param name="chunk">The chunk.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="standard">The IFF standard.</param>
        protected static void FromStream(Chunk chunk, Stream stream, IffStandard standard)
        {
            chunk.IffStandard = standard;
            chunk.BaseStream = stream;
            chunk.ParentChunk = (stream as ChunkStreamAdapter)?.Chunk;
            chunk.ReadFromStream();
        }

        /// <summary> Creates a value new chunk. </summary>
        /// <param name="id">The chunk identifier.</param>
        /// <param name="stream">The target stream .</param>
        /// <param name="standard">The IFF standard.</param>
        public static Chunk ToStream(string id, Stream stream, IffStandard standard)
        {
            return ToStream<Chunk>(id, stream, standard);
        }

        /// <summary> Creates a value new chunk. </summary>
        /// <param name="id">The chunk identifier.</param>
        /// <param name="stream">The target stream .</param>
        /// <param name="standard">The IFF standard.</param>
        protected static T ToStream<T>(string id, Stream stream, IffStandard standard)
            where T : Chunk, new()
        {
            var chunk = new T();
            ToStream(chunk, id, stream, standard);
            return chunk;
        }

        /// <summary> Creates a value new chunk. </summary>
        /// <param name="chunk">The chunk.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="standard">The IFF standard.</param>
        protected static void ToStream(Chunk chunk, string id, Stream stream, IffStandard standard)
        {
            chunk.ChunkId = id;
            chunk.IffStandard = standard;
            chunk.BaseStream = stream;
            chunk.ParentChunk = (stream as ChunkStreamAdapter)?.Chunk;
            chunk.WriteToStream();
        }

        /// <summary> The header needs flushing </summary>
        private bool _headerNeedsFlushing = true;

        /// <summary> The packing calculator </summary>
        private readonly PackingCalculator _packing = PackingCalculator.Int16;

        /// <summary> The base stream </summary>
        public Stream BaseStream { get; set; }

        /// <summary> The Parent chunk </summary>
        protected Chunk ParentChunk { get; set; }

        /// <summary> Gets or sets the chunk identifier. </summary>
        public string ChunkId { get; set; }

        /// <summary> Gets the information file format type settings. </summary>
        public IffStandard IffStandard { get; set;  }

        /// <summary>  Gets a value indicating whether this instance is RF64. </summary>
        /// <value>
        ///   <c>true</c> if this instance is RF64; otherwise, <c>false</c>.
        /// </value>
        public bool IsRf64 { get; private set; }

        /// <summary> The size of the padded content byte. </summary>
        public Int64 PaddedDataByteSize => _packing.RoundUp(DataByteSize);

        /// <summary> The chunk content byte size </summary>
        public Int64 DataByteSize { get; set; }

        /// <summary> The size of the header byte. </summary>
        public Int64 HeaderByteSize => AddressByteSize + (sizeof(byte) * 4);

        /// <value>
        /// The address size.
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

        /// <summary> Gets the data segment location in the base stream </summary>
        public Int64 DataLocation => StartLocation + HeaderByteSize;

        /// <summary> Gets the start segment location in the base stream </summary>
        public Int64 StartLocation { get; set; }

        /// <summary>
        /// Gets the end location.
        /// Note: EA IFF 85 Standard for Interchange Format Files states that
        /// chucks should be 16bit aligned 
        /// </summary>
        public Int64 EndLocation => StartLocation + HeaderByteSize + PaddedDataByteSize;

        /// <summary> Invalidates this instance. </summary>
        public virtual void FlagHeaderForFlush()
        {
            _headerNeedsFlushing = true;
            var p = ParentChunk;
            while (p != null)
            {
                p._headerNeedsFlushing = true;
                p = p.ParentChunk;
            }
        }

        /// <summary> Returns whether the chunk headers requires flushing or not. </summary>
        public virtual bool HeaderRequiresFlush()
        {
            return _headerNeedsFlushing;
        }

        /// <summary> Casts the steam to the given type. </summary>
        /// <typeparam name="T"></typeparam>
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
            };

            BaseStream.Position = DataLocation;
            c.ReadData();

            return c;
        }

        /// <summary> Sets the length of the chuck. </summary>
        /// <param name="value">The value.</param>
        public void SetLength(long value)
        {
            if (DataByteSize == value)
                return;

            // Make sure the chunk is at the end of the stream.
            // NOTE: this could end up byte shifting the entire stream.
            MoveChunkToEndOfStream();

            DataByteSize = value;
            FlagHeaderForFlush();
        }


        /// <summary> Clears all buffers for this chunk, writes any changes to the chunk header </summary>
        public void Flush()
        {
            FlushChildrenInner();

            if (!HeaderRequiresFlush())
                return;

            FlushInner();
            FlushParentInner();
        }

        /// <summary> Clears all buffers for this chunk, writes any changes to the chunk header, and calls flush on the this chunks parent </summary>
        public void FlushParent()
        {
            if (!HeaderRequiresFlush())
            {
                BaseStream.Flush(); 
                return;
            }

            // Flush self
            FlushInner();
            FlushParentInner();
        }

        /// <summary> Clears all buffers for this chunk, writes any changes to the chunk header, and calls flush on the this chunks children </summary>
        public virtual void FlushChildren()
        {
            FlushChildrenInner();

            if (!HeaderRequiresFlush())
                return;

            // Flush self
            FlushInner();
        }

        /// <summary> Calculate the byte size of the current content. </summary>
        public virtual long CalculateContentSize()
        {
            return DataByteSize;
        }

        /// <summary> Gets the padded byte size of the current content. </summary>
        public virtual long CalculatePaddedContentSize()
        {
            return _packing.RoundUp(CalculateContentSize());
        }

        /// <summary> Calculates the end position. </summary>
        public long CalculateEndPosition()
        {
            return StartLocation + HeaderByteSize + CalculatePaddedContentSize();
        }

        /// <summary> Calculates if this chunk is trailing. </summary>
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
            DataByteSize = CalculateContentSize();
            StartLocation = BaseStream.Position;
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
 

        // Private methods

        protected T CreateChild<T>(string chunkId, Int64 dataByteSize) where T : Chunk, new()
        {
            var c = new T { ChunkId = chunkId, DataByteSize = dataByteSize };
            CreateChild(c);
            return c;
        }

        protected void CreateChild(Chunk chunk)
        {
            chunk.BaseStream = BaseStream;
            chunk.IffStandard = IffStandard;
            chunk.ParentChunk = this;

            // As we are appending to the chunk, we have to make sure
            // we are at the end of the stream
            MoveChunkToEndOfStream();

            // Move to the end of this chunk
            BaseStream.Position = CalculateEndPosition();

            chunk.WriteToStream();
        }

        protected void ReadFromStream()
        {
            StartLocation = BaseStream.Position;
            ReadChunk();
            _headerNeedsFlushing = false;
        }

        protected void FlushParentInner()
        {
            if (ParentChunk != null)
                ParentChunk.FlushParent();
            else
                BaseStream.Flush();
        }

        protected virtual void FlushChildrenInner()
        {
            // Do nothing, this can be overridden
        }

        protected virtual void FlushInner()
        {
            BaseStream.Position = StartLocation;
            WriteToStream();
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
