using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
        public T At<T>(int i) where T : Chunk, new()
        {
            return this[i] as T;
        }

        public T Add<T>(string chunkId) where T : Chunk, new()
        {
            var c = CreateChild<T>(chunkId);
            ParseLocalChunk(c);
            AddLocalChunkArray(c);
            return c;
        }

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
        /// <returns></returns>
        public new static GroupChunk FromStream(Stream stream, IffStandard standardStandard)
        {
            return FromStream<GroupChunk>(stream, standardStandard);
        }


        // private methods


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

            Write(typeIdBytes, 0, typeIdBytes.Length);
        }

        private void WriteLocalChunks()
        {
            foreach (var chunk in LocalChunks)
                chunk.WriteToStream();

            // Update the byte size, just in case an chuck
            // didn't correctly calculate is size 
            DataByteSize = GetChunkByteSize();
        }

        // Read

        protected override void ReadData()
        {
            ReadTypeId();
            LocalChunks = ReadLocalChunks().ToArray();
        }

        private void ReadTypeId()
        {
            var typeIdBytes = new byte[4];
            Read(typeIdBytes, 0, 4);
            TypeId = Encoding.UTF8.GetString(typeIdBytes, 0, typeIdBytes.Length);
        }


        private IEnumerable<Chunk> ReadLocalChunks()
        {
            while (Position < Length)
                yield return ReadLocalChunk();
        }

        private Chunk ReadLocalChunk()
        {
            var startLocation = Position;
            var c = Chunk.FromStream(this, IffStandard);
            c = ParseLocalChunk(c);

            // Seek to the byte aligned end of the chunk
            Position = startLocation + c.PaddedDataByteSize + c.HeaderByteSize;
            return c;
        }

        protected virtual Chunk ParseLocalChunk(Chunk streamChunk)
        {
            // Override this method for specialized parsing of chunks
            return streamChunk;
        }

        // Chunk Methods

        private void AddLocalChunkArray(Chunk localChunk)
        {
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
    }
}
