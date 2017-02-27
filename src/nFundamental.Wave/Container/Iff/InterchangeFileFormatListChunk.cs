// ReSharper disable BuiltInTypeReferenceStyle

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Fundamental.Core.Memory;

namespace Fundamental.Wave.Container.Iff
{

    public class InterchangeFileFormatListChunk 
    {

        /// <summary>
        /// The total byte size of all the 
        /// </summary>
        public UInt32 SubChunkByteSize =>  checked ((UInt32)Chunks.Sum(x => x.TotalByteSize));

        /// <summary>
        /// Gets the size of the header byte.
        /// </summary>
        /// <value>
        /// The size of the header byte.
        /// </value>
        public UInt32 HeaderByteSize => 4  // Sig
                                      + 4  // type
							          + 4; // size

        /// <summary>
        /// Gets the total size of the byte.
        /// </summary>
        /// <value>
        /// The total size of the byte.
        /// </value>
        public UInt32 TotalByteSize => HeaderByteSize + SubChunkByteSize;

        /// <summary>
        /// Gets or sets the type identifier.
        /// </summary>
        /// <value>
        /// The type identifier.
        /// </value>
        public string TypeId { get; set; } = "LIST";

        /// <summary>
        /// Gets or sets the sub type identifier.
        /// </summary>
        /// <value>
        /// The sub type identifier.
        /// </value>
        public string SubTypeId { get; set; } = "NONE";

        /// <summary>
        /// Gets the chunks.
        /// </summary>
        /// <value>
        /// The chunks.
        /// </value>
        public List<InterchangeFileFormatChunk> Chunks  { get;  } = new List<InterchangeFileFormatChunk>();

        /// <summary>
        /// Reads the binary fragment from the stream reader.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="endianness">The endianness.</param>
        /// <param name="readChunkData"></param>
        /// <exception cref="System.FormatException">Expected riff header was missing. check that the stream contains a valid header at this position.</exception>
        public void Read(Stream stream, Endianness endianness, Action<InterchangeFileFormatChunk, Stream> readChunkData = null)
        {
            var binaryReader = stream.AsEndianReader(endianness);

            var startLocation = stream.Position;
            var length = stream.Length;

            ReadTypeId(binaryReader);
            var byteSize = ReadContentSize(binaryReader);
            ReadSubTypeId(binaryReader);

            var chunkEndPosition = GetMaxPossibleChunkEndPosition(byteSize, startLocation, length);
            ReadChucks(stream, endianness, readChunkData, chunkEndPosition);
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
            var currentSubChunkSize = SubChunkByteSize;

            WriteIffHeader(binaryWriter);
            WriteSubType(binaryWriter);
            WriteChunks(stream, endianness, writeChunkData);

            // Update the IFF header if the total size changed as a result of written the content
            if (currentSubChunkSize == SubChunkByteSize) 
                return;

            var endLocation = stream.Position;
            stream.Position = startLocation;
            WriteIffHeader(binaryWriter);
            stream.Position = endLocation;
        }


        /// <summary>
        /// Reads from stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="endianness">The endianness.</param>
        /// <param name="readChunkData">The read chunk data.</param>
        /// <returns></returns>
        public static InterchangeFileFormatListChunk ReadFromStream(Stream stream, Endianness endianness, Action<InterchangeFileFormatChunk, Stream> readChunkData = null)
        {
            var rh = new InterchangeFileFormatListChunk();
            rh.Read(stream, endianness, readChunkData);
            return rh;
        }

        // Private Methods

        #region Write

        private void WriteIffHeader(MiscUtil.IO.EndianBinaryWriter binaryWriter)
        {
            WriteTypeId(binaryWriter);
            WriteTotalByteSize(binaryWriter);
        }

        private void WriteSubType(MiscUtil.IO.EndianBinaryWriter binaryWriter)
        {
            var mmioBytes = Encoding.UTF8.GetBytes(SubTypeId);
            if (mmioBytes.Length != 4)
                throw new FormatException("IFF sub type Id must be exactly 4 chars long");

            binaryWriter.Write(mmioBytes);
        }

        private void WriteTotalByteSize(MiscUtil.IO.EndianBinaryWriter binaryWriter)
        {
            binaryWriter.Write(SubChunkByteSize + 4);
        }

        private void WriteTypeId(MiscUtil.IO.EndianBinaryWriter binaryWriter)
        {
            var typeIdBytes = Encoding.UTF8.GetBytes(TypeId);
            if (typeIdBytes.Length != 4)
                throw new FormatException("IFF type Id must be exactly 4 chars long");
            binaryWriter.Write(typeIdBytes);
        }

        private void WriteChunks(Stream stream, Endianness endianness, Action<InterchangeFileFormatChunk, Stream> writeChunkData)
        {
            foreach (var chunk in Chunks)
            {
                WriteChunk(stream, endianness, chunk, writeChunkData);
            }
        }

        private static void WriteChunk(Stream stream, Endianness endianness, InterchangeFileFormatChunk chunk, Action<InterchangeFileFormatChunk, Stream> writeChunkData)
        {

            chunk.Write(stream, endianness);

            WriteChunkData(stream, endianness, chunk, writeChunkData);

            // Go to the position of the next chunk
            stream.Position = chunk.ContentByteSize + chunk.Location;
        }

        private static void WriteChunkData(Stream stream, Endianness endianness, InterchangeFileFormatChunk chunk, Action<InterchangeFileFormatChunk, Stream> writeChunkData)
        {
            var fragmentWritePosition = stream.Position;
            var currentSize = chunk.ContentByteSize;

            if (writeChunkData == null)
                return;

            writeChunkData.Invoke(chunk, stream);

            // If the chunk size didn't change we have nothing more to do
            if (currentSize == chunk.ContentByteSize)
                return;

            // If the chunk size changed then we need to update the size in the header
            stream.Position = fragmentWritePosition;
            chunk.Write(stream, endianness);
        }

        #endregion

        #region Read

        private void ReadTypeId(MiscUtil.IO.EndianBinaryReader binaryReader)
        {
            var typeBytes = binaryReader.ReadBytes(4);
            TypeId = Encoding.UTF8.GetString(typeBytes, 0, typeBytes.Length);
        }

        private void ReadChucks(Stream stream, Endianness endianness, Action<InterchangeFileFormatChunk, Stream> readChunkData, long chunkEndPosition)
        {
            Chunks.Clear();
            while (stream.Position < chunkEndPosition)
                ReadChunk(stream, endianness, readChunkData);
        }

        private void ReadChunk(Stream stream, Endianness endianness, Action<InterchangeFileFormatChunk, Stream> readChunkData)
        {
            var chunk = new InterchangeFileFormatChunk();
            Chunks.Add(chunk);
            chunk.Read(stream, endianness);

            readChunkData?.Invoke(chunk, stream);

            // Go to the position of the next chunk
            stream.Position = chunk.ContentByteSize + chunk.Location;
        }

        private static long GetMaxPossibleChunkEndPosition(uint byteSize, long startLocation, long maxLength)
        {
            var chunkEndPosition = Math.Min(byteSize + 8, maxLength - startLocation);
            return chunkEndPosition;
        }

        private void ReadSubTypeId(MiscUtil.IO.EndianBinaryReader binaryReader)
        {
            var subTypeBytes = binaryReader.ReadBytes(4);
            SubTypeId = Encoding.UTF8.GetString(subTypeBytes, 0, subTypeBytes.Length);
        }

        private static uint ReadContentSize(MiscUtil.IO.EndianBinaryReader binaryReader)
        {
            return binaryReader.ReadUInt32();
        }

        #endregion
    }
}
