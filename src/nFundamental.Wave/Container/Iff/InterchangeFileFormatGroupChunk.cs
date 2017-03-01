// ReSharper disable BuiltInTypeReferenceStyle

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Fundamental.Core.Memory;

namespace Fundamental.Wave.Container.Iff
{
    public class InterchangeFileFormatGroupChunk : IEnumerable<InterchangeFileFormatChunk>
    {

        /// <summary>
        /// Gets the size of the header byte.
        /// </summary>
        /// <value>
        /// The size of the header byte.
        /// </value>
        public UInt32 HeaderByteSize => (sizeof(byte) * 4)  // Chunks Id
                                      + sizeof(UInt32)      // Data Size
							          + (sizeof(byte) * 4); // Type Id

        /// <summary>
        /// Gets the total size of the byte.
        /// </summary>
        /// <value>
        /// The total size of the byte.
        /// </value>
        public UInt32 TotalByteSize => HeaderByteSize + DataByteSize;
        
        /// <summary>
        /// Gets the start location.
        /// </summary>
        /// <value>
        /// The start location.
        /// </value>
        public Int64 StartLocation { get; private set; }

        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public Int64 DataLocation => StartLocation + HeaderByteSize;

        /// <summary>
        /// Gets the end location.
        /// </summary>
        /// <value>
        /// The start location.
        /// </value>
        public Int64 EndLocation => StartLocation + HeaderByteSize + DataByteSize;

        /// <summary>
        /// Gets or sets the type identifier.
        /// </summary>
        /// <value>
        /// The type identifier.
        /// </value>
        public string ChunkId { get; protected set; }

        /// <summary>
        /// Gets or sets the sub type identifier.
        /// </summary>
        /// <value>
        /// The sub type identifier.
        /// </value>
        public string TypeId { get; protected set; }

        /// <summary>
        /// The total byte size of all the 
        /// </summary>
        public UInt32 DataByteSize => checked((UInt32)SubChunks.Sum(x => x.TotalByteSize));

        /// <summary>
        /// Gets the <see cref="InterchangeFileFormatChunk"/> with the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="InterchangeFileFormatChunk"/>.
        /// </value>
        /// <param name="i">The i.</param>
        /// <returns></returns>
        public InterchangeFileFormatChunk this[int i] => SubChunks[i];

        /// <summary>
        /// Gets the <see cref="InterchangeFileFormatChunk"/> with the specified identifier.
        /// </summary>
        /// <value>
        /// The <see cref="InterchangeFileFormatChunk"/>.
        /// </value>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public InterchangeFileFormatChunk this[string id]
        {
            get { return SubChunks.First(x => x.ChunkId == id); }
        }

        /// <summary>
        /// Gets the count of sub chunks.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count => SubChunks.Count;

        /// <summary>
        /// Gets or sets underlying the sub chunk list.
        /// </summary>
        /// <value>
        /// The sub chunks.
        /// </value>
        protected IList<InterchangeFileFormatChunk> SubChunks { get; set; } = new List<InterchangeFileFormatChunk>();

        /// <summary>
        /// Reads the binary fragment from the stream reader.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="endianness">The endianness.</param>
        /// <param name="readChunkData"></param>
        /// <exception cref="System.FormatException">Expected riff header was missing. check that the stream contains a valid header at this position.</exception>
        private void Read(Stream stream, Endianness endianness, Action<InterchangeFileFormatChunk, Stream> readChunkData = null)
        {
            var binaryReader = stream.AsEndianReader(endianness);

            StartLocation = stream.Position;
            var length = stream.Length;

            ReadChunkId(binaryReader);
            var byteSize = ReadDataSize(binaryReader);
            ReadTypeId(binaryReader);

            var chunkEndPosition = GetMaxPossibleChunkEndPosition(byteSize, StartLocation, length);
            ReadSubChunks(stream, endianness, readChunkData, chunkEndPosition);
        }

      
        /// <summary>
        /// Writes the binary fragment to the stream writer.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="endianness">The endianness.</param>
        /// <param name="writeChunkData">The write chunk data.</param>
        /// <exception cref="System.FormatException">Riff type MMIO Id must be exactly 4 chars long</exception>
        public void Write(Stream stream, Endianness endianness, Action<InterchangeFileFormatChunk, Stream> writeChunkData = null)
        {
            var binaryWriter = stream.AsEndianWriter(endianness);

            var startLocation = stream.Position;
            var currentSubChunkSize = DataByteSize;

            WriteIffChunkHeader(binaryWriter);
            WriteTypeId(binaryWriter);
            WriteSubChunks(stream, endianness, writeChunkData);

            // Update the IFF header if the total size changed as a result of written the content
            if (currentSubChunkSize == DataByteSize) 
                return;

            var endLocation = stream.Position;
            stream.Position = startLocation;
            WriteIffChunkHeader(binaryWriter);
            stream.Position = endLocation;
        }

        /// <summary>
        /// Reads a chunk group from a stream using the given endianness.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="endianness">The endianness.</param>
        /// <param name="readChunkData">The read chunk data.</param>
        /// <returns></returns>
        public static InterchangeFileFormatGroupChunk FromStream(Stream stream, Endianness endianness, Action<InterchangeFileFormatChunk, Stream> readChunkData = null)
        {
            var rh = new InterchangeFileFormatGroupChunk();
            rh.Read(stream, endianness, readChunkData);
            return rh;
        }

        /// <summary>
        /// Creates a new chunk.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="typeId">The type identifier.</param>
        /// <param name="chunks">The chunks.</param>
        /// <returns></returns>
        /// <exception cref="System.FormatException">Type Id must be exactly 4 chars long</exception>
        public static InterchangeFileFormatGroupChunk Create(string id, string typeId, params InterchangeFileFormatChunk[] chunks)
        {
            if (id.Length != 4)
                throw new FormatException("Chunk Id must be exactly 4 chars long");

            if (typeId.Length != 4)
                throw new FormatException("Type Id must be exactly 4 chars long");

            return new InterchangeFileFormatGroupChunk
            {
                ChunkId = id,
                TypeId = typeId,
                SubChunks = chunks
            };
        }

        // Private Methods

        #region Write

        private void WriteIffChunkHeader(MiscUtil.IO.EndianBinaryWriter binaryWriter)
        {
            WriteChunkId(binaryWriter);
            WriteTotalByteSize(binaryWriter);
        }

        private void WriteTypeId(MiscUtil.IO.EndianBinaryWriter binaryWriter)
        {
            var typeIdBytes = Encoding.UTF8.GetBytes(TypeId);
            if (typeIdBytes.Length != 4)
                throw new FormatException("IFF type Id must be exactly 4 chars long");

            binaryWriter.Write(typeIdBytes);
        }

        private void WriteTotalByteSize(MiscUtil.IO.EndianBinaryWriter binaryWriter)
        {
            binaryWriter.Write(DataByteSize + 4);
        }

        private void WriteChunkId(MiscUtil.IO.EndianBinaryWriter binaryWriter)
        {
            var chunkIdBytes = Encoding.UTF8.GetBytes(ChunkId);
            if (chunkIdBytes.Length != 4)
                throw new FormatException("IFF chunk Id must be exactly 4 chars long");
            binaryWriter.Write(chunkIdBytes);
        }

        private void WriteSubChunks(Stream stream, Endianness endianness, Action<InterchangeFileFormatChunk, Stream> writeChunkData)
        {
            foreach (var chunk in SubChunks)
            {
                WriteSubChunk(stream, endianness, chunk, writeChunkData);
            }
        }

        private static void WriteSubChunk(Stream stream, Endianness endianness, InterchangeFileFormatChunk chunk, Action<InterchangeFileFormatChunk, Stream> writeChunkData)
        {
            chunk.Write(stream, endianness);

            WriteSubChunkData(stream, endianness, chunk, writeChunkData);

            // Go to the position of the next chunk
            stream.Position = chunk.EndLocation;
        }

        private static void WriteSubChunkData(Stream stream, Endianness endianness, InterchangeFileFormatChunk chunk, Action<InterchangeFileFormatChunk, Stream> writeChunkData)
        {
            var currentSize = chunk.DataByteSize;

            if (writeChunkData == null)
                return;

            writeChunkData.Invoke(chunk, stream);

            // If the chunk size didn't change we have nothing more to do
            if (currentSize == chunk.DataByteSize)
                return;

            // If the chunk size changed then we need to update the size in the header
            stream.Position = chunk.StartLocation;
            chunk.Write(stream, endianness);
        }


        #endregion

        #region Read

        private void ReadChunkId(MiscUtil.IO.EndianBinaryReader binaryReader)
        {
            var typeBytes = binaryReader.ReadBytes(4);
            ChunkId = Encoding.UTF8.GetString(typeBytes, 0, typeBytes.Length);
        }
        private void ReadTypeId(MiscUtil.IO.EndianBinaryReader binaryReader)
        {
            var subTypeBytes = binaryReader.ReadBytes(4);
            TypeId = Encoding.UTF8.GetString(subTypeBytes, 0, subTypeBytes.Length);
        }

        private static uint ReadDataSize(MiscUtil.IO.EndianBinaryReader binaryReader)
        {
            return binaryReader.ReadUInt32();
        }

        private void ReadSubChunks(Stream stream, Endianness endianness, Action<InterchangeFileFormatChunk, Stream> readChunkData, long chunkEndPosition)
        {
            while (stream.Position < chunkEndPosition)
                ReadSubChunk(stream, endianness, readChunkData);
        }

        private void ReadSubChunk(Stream stream, Endianness endianness, Action<InterchangeFileFormatChunk, Stream> readChunkData)
        {
            var chunk = InterchangeFileFormatChunk.FromStream(stream, endianness);

            SubChunks.Add(chunk);

            readChunkData?.Invoke(chunk, stream);

            // Go to the position of the next chunk
            stream.Position = chunk.EndLocation;
        }

        private static long GetMaxPossibleChunkEndPosition(uint byteSize, long startLocation, long maxLength)
        {
            var chunkEndPosition = Math.Min(byteSize + 8, maxLength - startLocation);
            return chunkEndPosition;
        }

        #endregion
        
        #region IEnumerable<InterchangeFileFormatChunk> Impl

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<InterchangeFileFormatChunk> GetEnumerator()
        {
            return SubChunks.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
