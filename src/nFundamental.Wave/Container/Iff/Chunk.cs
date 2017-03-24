// ReSharper disable BuiltInTypeReferenceStyle

using System;
using System.IO;
using Fundamental.Wave.Container.Iff.Headers;

namespace Fundamental.Wave.Container.Iff
{
    

    //public class ChunkFactory<T> where T : Chunk
    //{
    //    T Read(Stream stream, IffStandard standardStandard)
    //    {
            
    //    }        
    //}

    public interface IChunkFactory
    {
        ChunkHeader CreateHeader();

        Chunk CreateInstance();


        HeaderMetaData WriteHeader(ChunkHeader chunkHeader, Stream stream, IffStandard standardStandard);

        void WriteData(Chunk chunk);

    }


    public class ChunkBase
    {
        public ChunkHeader Header { get; set; } = new ChunkHeader();

        public HeaderMetaData MetaData { get; set; }

        // <summary> The base stream </summary>
        public Stream BaseStream { get; set; }

        /// <summary> Gets the information file format type settings. </summary>
        public IffStandard IffStandard { get; set; }

        /// <summary> The header needs flushing </summary>
        private bool _headerNeedsFlushing = true;

        /// <summary>
        /// Gets the chunk identifier.
        /// </summary>
        /// <value>
        /// The chunk identifier.
        /// </value>
        public string ChunkId => Header.ChunkId;

        /// <summary> The Parent chunk </summary>
        public ChunkBase ParentChunk { get; set; }

        /// <summary> Invalidates this instance. </summary>
        public void FlagHeaderForFlush()
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

        /// <summary> Sets the length of the chuck. </summary>
        /// <param name="value">The value.</param>
        public void SetLength(long value)
        {
            if (Header.DataByteSize == value)
                return;


            // Make sure the chunk is at the end of the stream.
            // NOTE: this could end up byte shifting the entire stream.
            MoveChunkToEndOfStream();

            Header.DataByteSize = value;
            MetaData = MetaData?.AdjustLength(value);
            FlagHeaderForFlush();
        }


        /// <summary> Clears all buffers for this chunk, writes any changes to the chunk header </summary>
        public void Flush()
        {
            ForceFlushChildren();

            if (!HeaderRequiresFlush())
                return;

            ForceFlushHeader();
            ForceFlushParent();
        }



        /// <summary> Clears all buffers for this chunk, writes any changes to the chunk header, and calls flush on the this chunks children </summary>
        public virtual void FlushChildren()
        {
            ForceFlushChildren();

            if (!HeaderRequiresFlush())
                return;

            // Flush self
            ForceFlushHeader();
        }

        /// <summary> Calculate the byte size of the current content. </summary>
        public virtual long ContentSize => Header.DataByteSize;

        /// <summary> Gets the padded byte size of the current content. </summary>
        public long PaddedContentSize => HeaderMetaData.Packing.RoundUp(ContentSize);

        public long StartPosition => MetaData.StartLocation;

        public long EndPosition => MetaData.EndLocation;

        /// <summary> Calculates if this chunk is trailing. </summary>
        public bool CaculateIfTrailingChunk()
        {
            // If this chunk doesn't have parent,
            // then it must be trailing.
            if (ParentChunk == null)
                return true;

            // If is no meta data, then this chunk has never been written
            if (MetaData == null)
                return true;

            var groupChunk = ParentChunk as GroupChunk;
            if (groupChunk == null)
                return ParentChunk.CaculateIfTrailingChunk();

            if (!Equals(groupChunk.Last, this))
                return false;
            return ParentChunk.CaculateIfTrailingChunk();
        }

        public virtual void ShiftLocation(long vector)
        {
            MetaData = MetaData.ShiftLocation(vector);
        }

        /// <summary>
        /// Writes to stream.
        /// </summary>
        public void WriteToStream()
        {
            var length = ContentSize;
            SetLength(length);
            WriteChunk();

            // writing the data context may result in another header flush 
            if (_headerNeedsFlushing)
            {
                BaseStream.Position = MetaData.StartLocation;
                WriteHeader();
            }

            BaseStream.Position = MetaData.EndLocation;
            _headerNeedsFlushing = false;
        }


        /// <summary>
        /// Moves the chunk to end of stream.
        /// </summary>
        public bool MoveChunkToEndOfStream()
        {
            if (!MoveChunkToEndOfStreamInner())
                return false;
            BaseStream.Position = StartPosition;
            return true;
        }

        public bool MoveChunkToEndOfStreamInner()
        {
            var baseGroupChunk = ParentChunk as GroupChunk;

            var hasMovedParentChunk = false;
            var hasMovedInGroupChunk = false;

            if (baseGroupChunk != null)
                hasMovedInGroupChunk = baseGroupChunk.MoveChunkToEndOfStream(this);

            if (ParentChunk != null)
                hasMovedParentChunk = ParentChunk.MoveChunkToEndOfStreamInner();

            return hasMovedParentChunk || hasMovedInGroupChunk;
        }


        // Private methods

        //protected T AppendChild<T>(string chunkId, long dataByteSize) where T : Chunk, new()
        //{
        //    var c = new T
        //    {
        //        Header =
        //        {
        //            ChunkId = chunkId,
        //            DataByteSize = dataByteSize
        //        }
        //    };
        //    AppendChild(c);
        //    return c;
        //}

        protected void AppendChild(ChunkBase chunk)
        {
            chunk.BaseStream = BaseStream;
            chunk.IffStandard = IffStandard;
            chunk.ParentChunk = this;

            // Flush out any pending changes
            Flush();

            // As we are appending to the chunk, we have to make sure
            // we are at the end of the stream
            MoveChunkToEndOfStream();

            // Move to the end of this chunk
            BaseStream.Position = CalculateNewEndPosition();

            chunk.WriteHeader();
            FlagHeaderForFlush();
        }



        private void FlushParent()
        {
            if (!HeaderRequiresFlush())
            {
                BaseStream.Flush();
                return;
            }

            // Flush self
            ForceFlushHeader();
            ForceFlushParent();
        }

        protected void ForceFlushParent()
        {
            if (ParentChunk != null)
                ParentChunk.FlushParent();
            else
                BaseStream.Flush();
        }

        protected virtual void ForceFlushChildren()
        {
            // Do nothing, this can be overridden
        }

        protected virtual void ForceFlushHeader()
        {
            BaseStream.Position = MetaData.StartLocation;
            WriteToStream();
        }


        /// <summary> Calculates the end position. </summary>
        private long CalculateNewEndPosition()
        {
            return MetaData.DataLocation + PaddedContentSize;
        }

        #region Write

        protected virtual void WriteChunk()
        {
            WriteHeader();
            WriteData();
        }

        private void WriteHeader()
        {
            MetaData = Header.Write(BaseStream, IffStandard);
            _headerNeedsFlushing = false;
        }


        protected virtual void WriteData()
        {

        }

        #endregion

        #region Read

        private void ReadFromStream()
        {
            ReadChunk();
        }

        private void ReadChunk()
        {
            ReadHeader();
            ReadData();
        }

        private void ReadHeader()
        {
            MetaData = Header.Read(BaseStream, IffStandard);
            _headerNeedsFlushing = false;
        }


        protected virtual void ReadData()
        {
        }

        #endregion
    }

    public class Chunk : ChunkBase<Chunk>
    {
    }

    public class ChunkBase<T> where T : ChunkBase
    {

        public class FactoryBase<TChunk> : IChunkFactory where TChunk : ChunkBase, new()
        {

            public string Id { get; set; } = "JUNK";
            public long Size { get; set; }


            ChunkHeader IChunkFactory.CreateHeader()
            {
                return new ChunkHeader
                {
                    ChunkId = Id,
                    DataByteSize = Size
                };
            }

            ChunkBase IChunkFactory.CreateInstance() 
                => CreateInstance();
           

            HeaderMetaData IChunkFactory.WriteHeader(ChunkHeader chunkHeader, Stream stream, IffStandard standard) 
                => chunkHeader.Write(stream, standard);
            

            void IChunkFactory.WriteData(ChunkBase chunk) 
                => WriteData((T)chunk);

            protected virtual T CreateInstance()
                => new T();


            protected virtual void WriteData(T chunk)
                => chunk.WriteData();

        }

        public class Factory : FactoryBase<T> { }


        #region static Factory methods



        public static Chunk Write<T>(Chunk parent, Action<T> configure) where T : IChunkFactory, new()
        {
            var factory = new T();
            configure.Invoke(factory);
            return Write(parent, factory);
        }

        public static Chunk Write<T>(Stream stream, IffStandard standard, Action<T> configure) where T : IChunkFactory, new()
        {
            var factory = new T();
            configure.Invoke(factory);
            return Write(stream, standard, factory);
        }


        public static Chunk Write(Stream stream, IffStandard standard, Action<Factory> configure)
        {
            return Write<Factory>(stream, standard, configure);
        }

        public static Chunk Write(Chunk parent, IChunkFactory chunkFactory)
        {
            return WriteInner(parent.BaseStream, parent.IffStandard, chunkFactory, parent);
        }


        public static Chunk Write(Stream stream, IffStandard standard, IChunkFactory chunkFactory)
        {
            return WriteInner(stream, standard, chunkFactory, null);
        }

        private static Chunk WriteInner(Stream stream, IffStandard standard, IChunkFactory chunkFactory, Chunk parent)
        {
            var header = chunkFactory.CreateHeader();
            var chunk = chunkFactory.CreateInstance();

            var headerMetaData = chunkFactory.WriteHeader(header, stream, standard);
            chunk.Header = header;
            chunk.MetaData = headerMetaData;
            chunk.IffStandard = standard;
            chunk.BaseStream = stream;
            chunk.ParentChunk = parent;
            chunk._headerNeedsFlushing = false;
            chunkFactory.WriteData(chunk);

            return chunk;
        }










        public static Chunk FromStream(Chunk parent, Func<ChunkHeader, Type> resolveType)
        {
            var header = new ChunkHeader();
            var headerMetaData = header.Read(parent.BaseStream, parent.IffStandard);
            var type = resolveType(header);

            var newInstance = Activator.CreateInstance(type) as Chunk;
            if (newInstance == null)
                throw new InvalidCastException($"{type} chunk types must inherit from type Chunk");

            newInstance.Header = header;
            newInstance.MetaData = headerMetaData;
            newInstance.IffStandard = parent.IffStandard;
            newInstance.BaseStream = parent.BaseStream;
            newInstance.ParentChunk = parent;
            newInstance._headerNeedsFlushing = false;
            newInstance.ReadData();
            return newInstance;
        }


        public static Chunk ToStream(Chunk parent, Func<ChunkHeader, Type> resolveType, string chunkId, long dataByteSize = 0)
        {
            var header = new ChunkHeader
            {
                ChunkId = chunkId,
                DataByteSize = dataByteSize
            };

            var type = resolveType(header);
            return ToStream(parent, type, chunkId, dataByteSize);
        }

        public static Chunk ToStream(Chunk parent, Type type, string chunkId, long dataByteSize = 0)
        {
            var newInstance = Activator.CreateInstance(type) as Chunk;
            if (newInstance == null)
                throw new InvalidCastException($"{type} chunk types must inherit from type Chunk");

            newInstance.Header.ChunkId = chunkId;
            newInstance.Header.DataByteSize = dataByteSize;

            if(parent != null)
                parent.AppendChild(newInstance);
            else
                newInstance.WriteToStream();

            return newInstance;
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
            chunk.Header.ChunkId = id;
            chunk.IffStandard = standard;
            chunk.BaseStream = stream;
            chunk.ParentChunk = (stream as ChunkStreamAdapter)?.Chunk;
            chunk.WriteToStream();
        }

        #endregion

        
    }
}
