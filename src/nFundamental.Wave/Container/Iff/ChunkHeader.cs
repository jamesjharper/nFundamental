using System;
using System.IO;
using System.Text;

using Fundamental.Core.Memory;
using Fundamental.Wave.Container.Iff.Headers;

namespace Fundamental.Wave.Container.Iff
{
    public class ChunkHeader
    {

        /// <summary> Gets or sets the chunk identifier. </summary>
        public string ChunkId { get; set; }

        /// <summary> The chunk content byte size </summary>
        public long DataByteSize { get; set; }

        /// <summary>
        /// Gets a value indicating whether [will32 bit over flow].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [will32 bit over flow]; otherwise, <c>false</c>.
        /// </value>
        private bool Requires64ExtentedHeaders => DataByteSize >= 0xFFFFFFFF;

        /// <summary> Writes the chuck to the stream specified stream </summary>
        /// <param name="stream">The target stream.</param>
        /// <param name="iffStandard">The IFF standard.</param>
        public HeaderMetaData Write(Stream stream, IffStandard iffStandard) => WriteChunk(stream, iffStandard);

        /// <summary> Reads chuck from the stream specified stream. </summary>
        /// <param name="stream">The target stream.</param>
        /// <param name="iffStandard">The IFF standard.</param>
        public HeaderMetaData Read(Stream stream, IffStandard iffStandard) => ReadChunk(stream, iffStandard);
        
        // Private methods

        #region Write

        protected virtual HeaderMetaData WriteChunk(Stream stream, IffStandard iffStandard)
        {
            var startPosition = stream.Position;
            WriteHeader(stream, iffStandard);
            var dataPosition = stream.Position;

            WriteData(stream, iffStandard);

            return new HeaderMetaData(startPosition, dataPosition, DataByteSize, Requires64ExtentedHeaders, iffStandard, stream, this);
        }

        private void WriteHeader(Stream stream, IffStandard iffStandard)
        {
            WriteChunkId(stream);
            WriteDataSize(stream, iffStandard);
        }

        private void WriteChunkId(Stream stream)
        {
            var chunkIdBytes = Encoding.UTF8.GetBytes(ChunkId);
            if (chunkIdBytes.Length != 4)
                throw new FormatException("Chunk Id must be exactly 4 chars long");

            stream.Write(chunkIdBytes);
        }

        private void WriteDataSize(Stream stream, IffStandard iffStandard)
        {
            var binaryWriter = stream.AsEndianWriter(iffStandard.ByteOrder);

            switch (iffStandard.AddressSize)
            {
                case AddressSize.UInt32:

                    var uint32Address = (uint)Math.Min(uint.MaxValue, DataByteSize);
                    binaryWriter.Write(uint32Address);
                    break;
                case AddressSize.UInt64:
                    var uint64Address = (ulong) DataByteSize;
                    binaryWriter.Write(uint64Address);

                    break;
                case AddressSize.UInt16:
                    var uint16Address = (ushort)Math.Min(uint.MaxValue, DataByteSize);
                    binaryWriter.Write(uint16Address);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        protected virtual void WriteData(Stream stream, IffStandard iffStandard)
        {

        }

        #endregion

        #region Read

        private HeaderMetaData ReadChunk(Stream stream, IffStandard iffStandard)
        {
            var startPosition = stream.Position;
            ReadHeader(stream, iffStandard);
            var dataPosition = stream.Position;
            ReadData(stream, iffStandard);

            return new HeaderMetaData(startPosition, dataPosition, DataByteSize, Requires64ExtentedHeaders, iffStandard, stream, this);
        }

        private void ReadHeader(Stream stream, IffStandard iffStandard)
        {
            ReadChunkId(stream);
            ReadDataSize(stream, iffStandard);
        }

        private void ReadChunkId(Stream stream)
        {
            var chunkIdBytes = stream.Read(4);
            ChunkId = Encoding.UTF8.GetString(chunkIdBytes, 0, chunkIdBytes.Length);
        }

        private void ReadDataSize(Stream stream, IffStandard iffStandard)
        {
            var binaryReader = stream.AsEndianReader(iffStandard.ByteOrder);
            switch (iffStandard.AddressSize)
            {
                case AddressSize.UInt32:
                    DataByteSize = binaryReader.ReadUInt32();
                    break;
                case AddressSize.UInt64:
                    // Truncate down to Int64.MaxValue as .net doesn't give native access to unsigned 64bit file cursors
                    var dataSize = binaryReader.ReadUInt64();
                    DataByteSize = (long)Math.Min(long.MaxValue, dataSize);
                    break;
                case AddressSize.UInt16:
                    DataByteSize = binaryReader.ReadUInt16();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected virtual void ReadData(Stream stream, IffStandard iffStandard)
        {
        }

        #endregion
    }
}
