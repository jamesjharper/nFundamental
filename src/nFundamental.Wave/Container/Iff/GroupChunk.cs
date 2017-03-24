using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Fundamental.Core.Memory;
using Fundamental.Wave.Container.Iff.Headers;

namespace Fundamental.Wave.Container.Iff
{
    public class GroupChunk : Chunk
    {
        public new class FactoryBase<T> : Chunk.FactoryBase<T> where T : GroupChunk, new()
        {

            /// <summary>
            /// Gets or sets the chunk identifier.
            /// </summary>
            /// <value>
            /// The chunk identifier.
            /// </value>
            public string TypeId { get; set; }

            protected override T CreateInstance()
            {
                var i = base.CreateInstance();
                i.TypeId = TypeId;
                return i;
            }
        }

        public new class Factory : FactoryBase<GroupChunk> { }



        public static Chunk Write(Stream stream, IffStandard standard, Action<Factory> configure)
        {
            return Write<Factory>(stream, standard, configure);
        }






        /// <summary>
        /// Creates a new chunk.
        /// </summary>
        /// <param name="chunkId">The chunk identifier.</param>
        /// <param name="typeId">The type identifier.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="standardStandard">Type of the chunk standard chunk standard.</param>
        /// <returns></returns>
        /// <exception cref="System.FormatException">Type Id must be exactly 4 chars long</exception>
        public static GroupChunk ToStream(string chunkId, string typeId, Stream stream, IffStandard standardStandard)
        {
            var chunk = new GroupChunk
            {
                Header =
                {
                    ChunkId = chunkId,
                },
                TypeId = typeId,
                IffStandard = standardStandard,
                BaseStream = stream
            };
            chunk.WriteToStream();
            return chunk;
        }

        /// <summary>
        /// Reads a chunk from a stream using the given standard.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="standardStandard">Type of the chunk standard.</param>
        public new static GroupChunk FromStream(Stream stream, IffStandard standardStandard)
        {
            return FromStream<GroupChunk>(stream, standardStandard);
        }

        public new static T FromStream<T>(Stream stream, IffStandard standardStandard)
            where T : GroupChunk, new()
        {
            return Chunk.FromStream<T>(stream, standardStandard);
        }


        /// <summary>
        /// Gets or sets the chunk identifier.
        /// </summary>
        /// <value>
        /// The chunk identifier.
        /// </value>
        public string TypeId { get; protected set; }

        /// <summary>
        /// Gets the <see cref="Chunk"/> with the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="Chunk"/>.
        /// </value>
        /// <param name="i">The i.</param>
        /// <returns></returns>
        public Chunk this[int i] => LocalChunks[i];

        /// <summary>
        /// Gets the <see cref="Chunk"/> with the specified identifier.
        /// </summary>
        /// <value>
        /// The <see cref="Chunk"/>.
        /// </value>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Chunk this[string id]
        {
            get { return LocalChunks.First(x => x.ChunkId == id); }
        }

        /// <summary>
        /// Returns the last element of a chunk sequence, or a null if the sequence contains no elements. 
        /// </summary>
        public Chunk Last => LocalChunks.LastOrDefault();

        /// <summary>
        /// Returns the first element of a chunk sequence, or a null if the sequence contains no elements. 
        /// </summary>
        public Chunk First => LocalChunks.FirstOrDefault();

        /// <summary>
        /// Gets the count of sub chunks.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count => LocalChunks.Length;

        /// <summary>
        /// Gets or sets underlying the sub chunk list.
        /// </summary>
        /// <value>
        /// The sub chunks.
        /// </value>
        protected Chunk[] LocalChunks { get; set; } = { };

        /// <summary> Gets the byte size of the current content. </summary>
        public override long ContentSize => LocalChunkContentSize + (sizeof(byte) * 4);

        /// <summary> Gets the size of the local chunk content. </summary>
        public long LocalChunkContentSize => LocalChunks.Sum(x => x.PaddedContentSize + x.MetaData.HeaderByteSize);

        /// <summary>
        /// Gets the <see cref="Chunk"/> with the specified identifier.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="i">The i.</param>
        /// <returns></returns>
        public T At<T>(int i) where T : Chunk
        {
            return this[i] as T;
        }

        /// <summary>
        /// Adds the specified chunk with chunk identifier.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="chunkId">The chunk identifier.</param>
        /// <param name="dataByteSize">Size of the data byte.</param>
        /// <returns></returns>
        public T Add<T>(string chunkId, long dataByteSize = 0) where T : Chunk, new()
        {
            var result = CreateLocalChunk<T>(chunkId, dataByteSize);
            if (!(result is T))
                throw new InvalidCastException($"Resolved chuck was not of type {typeof(T)}");

            return (T) result;
        }


        /// <summary>
        /// Adds the specified chunk identifier.
        /// </summary>
        /// <param name="chunkId">The chunk identifier.</param>
        /// <param name="dataByteSize">Size of the data byte.</param>
        /// <returns></returns>
        public Chunk Add(string chunkId, long dataByteSize = 0) 
        {
            return Add<Chunk>(chunkId, dataByteSize);
        }

        /// <summary>
        /// Adds a group to the chunk group.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="chunkId">The chunk identifier.</param>
        /// <param name="typeId">The type identifier.</param>
        public T AddGroup<T>(string chunkId, string typeId) where T : GroupChunk
        {
            var result = CreateLocalChunk<T>(chunkId, /* Group header size */ 2);
            if (!(result is T))
                throw new InvalidCastException($"Resolved chuck was not of type {typeof(T)}");

            var gp = (T)result;
            gp.TypeId = typeId;
            gp.WriteData();
            return gp;
        }

        public GroupChunk AddGroup(string chunkId, string typeId)
        {
            return AddGroup<GroupChunk>(chunkId, typeId);
        }


        /// <summary>
        /// Moves the chunk to end of stream.
        /// </summary>
        public bool MoveChunkToEndOfStream(Chunk subChunk)
        {
            // If this chunk is already is the at the end of 
            // the sequence, then we have nothing to do
            if(subChunk.CaculateIfTrailingChunk())
                return false;
            
            var left = new[] { subChunk };
            var right = GetChucksAfter(subChunk).ToArray();

            // if the left list is empty, then the give chunk is 
            // already at the end of the stream.
            if (right.Length == 0)
                return false;

            SwapChunks(left, right);
            EnsureChunksAreInPositionOrder();
            return true;
        }


        // private methods

        protected override void ForceFlushChildren()
        {
            foreach (var chunk in LocalChunks)
                chunk.FlushChildren();
        }


        #region Write Data

        protected override void WriteData()
        {
            WriteTypeId();
            WriteLocalChunks();
        }

        private void WriteTypeId()
        {
            var typeIdBytes = Encoding.UTF8.GetBytes(TypeId);
            if (typeIdBytes.Length != 4)
                throw new FormatException("IFF type Id must be exactly 4 bytes long");

            BaseStream.Write(typeIdBytes, 0, typeIdBytes.Length);
        }

        private void WriteLocalChunks()
        {
            foreach (var chunk in LocalChunks)
            {
                // Use flush instead of WriteData as it
                // we be lazy if the content is already up to date
                chunk.FlushChildren();
            }

            BaseStream.Position = EndPosition;
        }

        #endregion

        #region Read Data

        protected override void ReadData()
        {
            ReadTypeId();
            LocalChunks = ReadLocalChunks().ToArray();
        }

        private void ReadTypeId()
        {
            var typeIdBytes = new byte[4];
            BaseStream.Read(typeIdBytes, 0, 4);
            TypeId = Encoding.UTF8.GetString(typeIdBytes, 0, typeIdBytes.Length);
        }


        private IEnumerable<Chunk> ReadLocalChunks()
        {
            while (BaseStream.Position < MetaData.EndLocation)
                yield return ReadLocalChunk();
        }

        private Chunk ReadLocalChunk()
        {
            var startLocation = BaseStream.Position;
            var childChunk = FromStream(this, GetTypeResolver<Chunk>()); 

            // Seek to the byte aligned end of the chunk
            BaseStream.Position = startLocation + childChunk.MetaData.PaddedDataByteSize + childChunk.MetaData.HeaderByteSize;
            return childChunk;
        }

        private Chunk CreateLocalChunk<TDefault>(string chunkId, long dataByteSize)
        {
            var chunk = ToStream(this, GetTypeResolver<TDefault>(), chunkId, dataByteSize);
            AddLocalChunkArray(chunk);
            return chunk;
        }

        protected  Func<ChunkHeader, Type> GetTypeResolver<TDefault>()
        {
            return (h) => ResolveLocalChunkType(h) ?? typeof(TDefault);
        }

        protected virtual Type ResolveLocalChunkType(ChunkHeader header)
        {
            // Override this method for specialized parsing of chunks
            return null;
        }

        #endregion


        #region Chunk ordering

        private IEnumerable<Chunk> GetChucksAfter(Chunk subChunk)
        {
            var chunkIndex = GetChunkIndex(subChunk);
            if (chunkIndex == -1)
                throw new InvalidOperationException("GetChucksAfter(Chunk c) failed as given chunk does not belong to this chunk group");

            for (var i = chunkIndex + 1; i < LocalChunks.Length; i++)
                yield return LocalChunks[i];
        }

        private int GetChunkIndex(Chunk subChunk)
        {
            for (var i = 0; i < LocalChunks.Length; i++)
            {
                if (Equals(subChunk, LocalChunks[i]))
                    return i;
            }

            return -1;
        }


        private void SwapChunks(Chunk[] leftChunks, Chunk[] rightChunks)
        {
            var leftStart = leftChunks.First().StartPosition;
            var leftEnd = leftChunks.Last().EndPosition;

            var rigthStart = rightChunks.First().StartPosition;
            var rightEnd = rightChunks.Last().EndPosition;

            if (leftEnd != rigthStart)
                throw new InvalidOperationException("Can only swap contiguous chunk orders");

            var vector = checked((uint)(rightEnd - rigthStart));
            var length = checked((uint)(rightEnd - leftStart));

            BaseStream.Position = leftStart;
            BaseStream.Rotate(vector, length);

            // Update chunk positions
            var left = vector;
            var right = length - vector;

            foreach (var leftChunk in leftChunks)
                leftChunk.ShiftLocation(left);

            foreach (var rigthChunk in rightChunks)
                rigthChunk.ShiftLocation(right * -1);

        }

        public override void ShiftLocation(long vector)
        {
            base.ShiftLocation(vector);

            foreach (var localChunk in LocalChunks)
            {
                localChunk.ShiftLocation(vector);
            }
        }

        private void EnsureChunksAreInPositionOrder()
        {
            LocalChunks = LocalChunks.OrderBy(x => x.MetaData.StartLocation).ToArray();
        }

        #endregion

        // Chunk Methods

        private void AddLocalChunkArray(Chunk localChunk)
        {
            FlagHeaderForFlush();
            LocalChunks = LocalChunks.Concat(new [] {localChunk}).ToArray();
        }


        public override bool HeaderRequiresFlush()
        {
            return base.HeaderRequiresFlush() || LocalChunks.Any(localChunk => localChunk.HeaderRequiresFlush());
        }
    }
}
