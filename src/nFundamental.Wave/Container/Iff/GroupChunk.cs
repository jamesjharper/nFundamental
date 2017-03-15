using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Fundamental.Core.Memory;

namespace Fundamental.Wave.Container.Iff
{
    public class GroupChunk : Chunk
    {

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

        /// <summary>
        /// Gets the byte size of the current content.
        /// </summary>
        /// <returns></returns>
        public override long CaculateContentSize()
        {
            return GetChunkByteSize();
        }

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
        /// <returns></returns>
        public T Add<T>(string chunkId) where T : Chunk, new()
        {
            var c = CreateChild<T>(chunkId);
            ParseLocalChunk(c);
            AddLocalChunkArray(c);
            return c;
        }

        /// <summary>
        /// Adds a group to the chunk group.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="chunkId">The chunk identifier.</param>
        /// <param name="typeId">The type identifier.</param>
        /// <returns></returns>
        public T AddGroup<T>(string chunkId, string typeId) where T : GroupChunk, new()
        {
            var c = CreateChild<T>(chunkId);
            c.TypeId = typeId;
            ParseLocalChunk(c);
            AddLocalChunkArray(c);
            return c;
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
        public static GroupChunk Create(string chunkId, string typeId, Stream stream, IffStandard standardStandard)
        {
            var chunk = new GroupChunk
            {
                ChunkId = chunkId,
                TypeId = typeId,
                IffStandard = standardStandard,
                BaseStream = stream
            };
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
        /// Moves the chunk to end of stream.
        /// </summary>
        public void MoveChunkToEndOfStream(Chunk subChunk)
        {
            // If this chunk is already is the at the end of 
            // the sequence, then we have nothing to do
            if(subChunk.CaculateIfTrailingChunk())
                return;
            
            var left = new[] { subChunk };
            var right = GetChucksAfter(subChunk).ToArray();

            // if the left list is empty, then the give chunk is 
            // already at the end of the stream.
            if(right.Length == 0)
                return;

            SwapChunks(left, right);
            EnsureChunksAreInPositionOrder();
        }


        // private methods

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
                chunk.WriteToStream();

            // Update the byte size, just in case an chuck
            // didn't correctly calculate is size 
            DataByteSize = GetChunkByteSize();
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
            while (BaseStream.Position < EndLocation)
                yield return ReadLocalChunk();
        }

        private Chunk ReadLocalChunk()
        {
            var startLocation = BaseStream.Position;
            var c = FromStream(this, IffStandard);
            c = ParseLocalChunk(c);

            // Seek to the byte aligned end of the chunk
            BaseStream.Position = startLocation + c.PaddedDataByteSize + c.HeaderByteSize;
            return c;
        }

        protected virtual Chunk ParseLocalChunk(Chunk streamChunk)
        {
            // Override this method for specialized parsing of chunks
            return streamChunk;
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
                if (subChunk == LocalChunks[i])
                    return i;
            }

            return -1;
        }


        private void SwapChunks(Chunk[] leftChunks, Chunk[] rightChunks)
        {
            var leftStart = leftChunks.First().StartLocation;
            var leftEnd = leftChunks.Last().EndLocation;

            var rigthStart = rightChunks.First().StartLocation;
            var rightEnd = rightChunks.Last().EndLocation;

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
                leftChunk.StartLocation += left;

            foreach (var rigthChunk in rightChunks)
                rigthChunk.StartLocation -= right;
        }

        private void EnsureChunksAreInPositionOrder()
        {
            LocalChunks = LocalChunks.OrderBy(x => x.StartLocation).ToArray();
        }

        #endregion


        // Chunk Methods

        private void AddLocalChunkArray(Chunk localChunk)
        {
            FlagHeaderForFlush();
            LocalChunks = LocalChunks.Concat(new [] {localChunk}).ToArray();
        }

        private long GetLocalChunkByteSize()
        {
            return LocalChunks.Sum(x => x.CaculatePaddedContentSize() + x.HeaderByteSize);
        }

        private long GetChunkByteSize()
        {
            return GetLocalChunkByteSize() + (sizeof(byte) * 4); // Plus  the type Id bytes
        }

        public override bool HeaderRequireFlush()
        {
            return base.HeaderRequireFlush() || LocalChunks.Any(localChunk => localChunk.HeaderRequireFlush());
        }
    }
}
