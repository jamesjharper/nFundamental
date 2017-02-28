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
        /// The chunk byte size
        /// </summary>
        public UInt32 ContentByteSize { get; set; }

        /// <summary>
        /// Gets the size of the header byte.
        /// </summary>
        /// <value>
        /// The size of the header byte.
        /// </value>
        public UInt32 HeaderByteSize => 4  // MMIO id
							          + 4; // chunk size

        /// <summary>
        /// Gets the total size of the byte.
        /// </summary>
        /// <value>
        /// The total size of the byte.
        /// </value>
        public UInt32 TotalByteSize => HeaderByteSize + ContentByteSize;

        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public Int64 Location { get; private set; }

        /// <summary>
        /// The MMIO identifier
        /// </summary>
        public string TypeId { get; set; } = "NONE";

        /// <summary>
        /// Reads the specified binary reader.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="endianness">The endianness.</param>
        public void Read(Stream stream, Endianness endianness)
        {
            var binaryReader = stream.AsEndianReader(endianness);

            // Read the MMIO id string. This is actually a 4 char string
            // but is used as an id of the RIFF chunk type
            var mmioBytes = binaryReader.ReadBytes(4);
            TypeId = Encoding.UTF8.GetString(mmioBytes, 0, mmioBytes.Length);

            // Read the length of the riff chunk
            ContentByteSize = binaryReader.ReadUInt32();

            Location = binaryReader.BaseStream.Position;
        }

        /// <summary>
        /// Writes the specified binary writer.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="endianness">The endianness.</param>
        /// <exception cref="System.FormatException">MMIO Id must be exactly 4 chars long</exception>
        public void Write(Stream stream, Endianness endianness)
        {
            // Write MMIO Id
            var mmioBytes = Encoding.UTF8.GetBytes(TypeId);
            if(mmioBytes.Length != 4)
                throw new FormatException("Type Id must be exactly 4 chars long");

            var binaryWriter = stream.AsEndianWriter(endianness);
            binaryWriter.Write(mmioBytes);

            // Write Chunk Size
            binaryWriter.Write(ContentByteSize);

            Location = binaryWriter.BaseStream.Position;
        }


        /// <summary>
        /// Reads from stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="endianness">The endianness.</param>
        /// <returns></returns>
        public static InterchangeFileFormatChunk ReadFromStream(Stream stream, Endianness endianness)
        {
            var rc = new InterchangeFileFormatChunk();
            rc.Read(stream, endianness);
            return rc;
        }
    }
}
