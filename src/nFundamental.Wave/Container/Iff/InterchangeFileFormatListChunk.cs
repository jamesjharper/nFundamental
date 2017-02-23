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
        public UInt32 SubChunkByteSize => (UInt32)Chunks.Sum(x => x.TotalByteSize);

        /// <summary>
        /// Gets the size of the header byte.
        /// </summary>
        /// <value>
        /// The size of the header byte.
        /// </value>
        public UInt32 ChunkByteSize =>  4  // Sig
                                      + 4  // type
							          + 4; // size

        /// <summary>
        /// Gets the total size of the byte.
        /// </summary>
        /// <value>
        /// The total size of the byte.
        /// </value>
        public UInt32 TotalByteSize => ChunkByteSize + SubChunkByteSize;

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
        /// <exception cref="System.FormatException">Expected riff header was missing. check that the stream contains a valid header at this position.</exception>
        public void Read(Stream stream, Endianness endianness)
        {
            var binaryReader = stream.AsEndianReader(endianness);
            var startPosition = binaryReader.BaseStream.Position;

            var typeBytes = binaryReader.ReadBytes(4);
            TypeId = Encoding.UTF8.GetString(typeBytes, 0, typeBytes.Length);

            // Read the length of the riff chunk
            var byteSize = binaryReader.ReadUInt32();

            // Read the file content type
            var subTypeBytes = binaryReader.ReadBytes(4);
            SubTypeId = Encoding.UTF8.GetString(subTypeBytes, 0, subTypeBytes.Length);

            var length = binaryReader.BaseStream.Length;
	        var chunkEndPosition = Math.Min(byteSize + 8, length - startPosition);

            Chunks.Clear();

            while (binaryReader.BaseStream.Position < chunkEndPosition)
	        {
	            var chunck = new InterchangeFileFormatChunk();
                Chunks.Add(chunck);
                chunck.Read(stream, endianness);

                // Go to the position of the next chunk
	            binaryReader.BaseStream.Position = chunck.ContentByteSize + chunck.Location;
	        }
        }

        /// <summary>
        /// Writes the binary fragment to the stream writer.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="endianness">The endianness.</param>
        /// <exception cref="System.FormatException">Riff type MMIO Id must be exactly 4 chars long</exception>
        public void Write(Stream stream, Endianness endianness)
        {
            var binaryWriter = stream.AsEndianWriter(endianness);

            // Write Signature
            var typeIdBytes = Encoding.UTF8.GetBytes(TypeId);
            if (typeIdBytes.Length != 4)
                throw new FormatException("IFF type Id must be exactly 4 chars long");
            binaryWriter.Write(typeIdBytes);

            // Write byte Size
            binaryWriter.Write(SubChunkByteSize  + 4);

            // Write riff type id
            var mmioBytes = Encoding.UTF8.GetBytes(SubTypeId);
            if (mmioBytes.Length != 4)
                throw new FormatException("IFF sub type Id must be exactly 4 chars long");

            binaryWriter.Write(mmioBytes);

            foreach (var chunk in Chunks)
            {
                chunk.Write(stream, endianness);

                // Go to the position of the next chunk
                binaryWriter.BaseStream.Position = chunk.ContentByteSize + chunk.Location;
            }
        }

        /// <summary>
        /// Reads from stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="endianness">The endianness.</param>
        /// <returns></returns>
        public static InterchangeFileFormatListChunk ReadFromStream(Stream stream, Endianness endianness)
        {
            var rh = new InterchangeFileFormatListChunk();
            rh.Read(stream, endianness);
            return rh;
        }
    }
}
