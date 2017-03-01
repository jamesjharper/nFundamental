// ReSharper disable BuiltInTypeReferenceStyle

using System;
using System.IO;
using System.Text;

using Fundamental.Core.Memory;

namespace Fundamental.Wave.Container.Iff
{
    public class InterchangeFileFormatChunk 
    {
        /// <summary>
        /// The chunk content byte size
        /// </summary>
        public UInt32 DataByteSize { get; set; }

        /// <summary>
        /// The size of the padded content byte.
        /// </summary>
        /// <value>
        /// The size of the padded content byte.
        /// </value>
        public UInt32 PaddedDataByteSize => DataByteSize + PaddingBytes;

        /// <summary>
        /// Gets the size of the header byte.
        /// </summary>
        /// <value>
        /// The size of the header byte.
        /// </value>
        public UInt32 HeaderByteSize =>  (sizeof(byte) * 4)  // Chunks Id
                                        + sizeof(UInt32);    // Data Size

        /// <summary>
        /// Gets the total size of the byte.
        /// </summary>
        /// <value>
        /// The total size of the byte.
        /// </value>
        public UInt32 TotalByteSize => HeaderByteSize + PaddedDataByteSize;

        /// <summary>
        /// Gets or sets the chunk identifier.
        /// </summary>
        /// <value>
        /// The chunk identifier.
        /// </value>
        public string ChunkId { get; protected set; }

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
        public Int64 StartLocation { get; private set; }

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
        public UInt32 PaddingBytes => DataByteSize % 2;

        /// <summary>
        /// Prevents a default instance of the <see cref="InterchangeFileFormatChunk"/> class from being created.
        /// </summary>
        private InterchangeFileFormatChunk()
        {
            
        }

        /// <summary>
        /// Reads the specified binary reader.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="endianness">The endianness.</param>
        private void Read(Stream stream, Endianness endianness)
        {
            StartLocation = stream.Position;
            var binaryReader = stream.AsEndianReader(endianness);

            ReadChunkId(binaryReader);
            ReadDataSize(binaryReader);
        }


        /// <summary>
        /// Writes the specified binary writer.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="endianness">The endianness.</param>
        /// <exception cref="System.FormatException">MMIO Id must be exactly 4 chars long</exception>
        public void Write(Stream stream, Endianness endianness)
        {
            StartLocation = stream.Position;
            var binaryWriter = stream.AsEndianWriter(endianness);

            WriteChunkId(binaryWriter);
            WriteDataSize(binaryWriter);
        }


        /// <summary>
        /// Reads a chunk from a stream using the given endianness.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="endianness">The endianness.</param>
        /// <returns></returns>
        public static InterchangeFileFormatChunk FromStream(Stream stream, Endianness endianness)
        {
            var iffc = new InterchangeFileFormatChunk();
            iffc.Read(stream, endianness);
            return iffc;
        }

        /// <summary>
        /// Creates a new chunk.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="dataByteSize">Size of the data byte.</param>
        /// <returns></returns>
        /// <exception cref="System.FormatException">Type Id must be exactly 4 chars long</exception>
        public static InterchangeFileFormatChunk Create(string id, int dataByteSize = 0)
        {
            if (id.Length != 4)
                throw new FormatException("Chunk Id must be exactly 4 chars long");
            return new InterchangeFileFormatChunk
            {
                ChunkId = id,
                DataByteSize = checked ((UInt32)dataByteSize)
            };
        }

        // Private methods

        private void WriteDataSize(MiscUtil.IO.EndianBinaryWriter binaryWriter)
        {
            binaryWriter.Write(DataByteSize);
        }

        private void WriteChunkId(MiscUtil.IO.EndianBinaryWriter binaryWriter)
        {
            var chunkIdBytes = Encoding.UTF8.GetBytes(ChunkId);
            if (chunkIdBytes.Length != 4)
                throw new FormatException("Chunk Id must be exactly 4 chars long");

            binaryWriter.Write(chunkIdBytes);
        }

        private void ReadDataSize(MiscUtil.IO.EndianBinaryReader binaryReader)
        {
            DataByteSize = binaryReader.ReadUInt32();
        }

        private void ReadChunkId(MiscUtil.IO.EndianBinaryReader binaryReader)
        {
            var chunkIdBytes = binaryReader.ReadBytes(4);
            ChunkId = Encoding.UTF8.GetString(chunkIdBytes, 0, chunkIdBytes.Length);
        }
    }
}
