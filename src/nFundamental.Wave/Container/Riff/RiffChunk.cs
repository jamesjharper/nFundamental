// ReSharper disable BuiltInTypeReferenceStyle

using System;
using System.Text;
using MiscUtil.IO;

namespace Fundamental.Wave.Container.Riff
{
    public class RiffChunk 
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
        public string MmioId { get; set; } = "NONE";

        /// <summary>
        /// Reads the specified binary reader.
        /// </summary>
        /// <param name="binaryReader">The binary reader.</param>
        public void Read(EndianBinaryReader binaryReader)
        {
            // Read the MMIO id string. This is actually a 4 char string
            // but is used as an id of the RIFF chunk type
            var mmioBytes = binaryReader.ReadBytes(4);
            MmioId = Encoding.UTF8.GetString(mmioBytes, 0, mmioBytes.Length);

            // Read the length of the riff chunk
            ContentByteSize = binaryReader.ReadUInt32();

            Location = binaryReader.BaseStream.Position;
        }

        /// <summary>
        /// Writes the specified binary writer.
        /// </summary>
        /// <param name="binaryWriter">The binary writer.</param>
        public void Write(EndianBinaryWriter binaryWriter)
        {
            // Write MMIO Id
            var mmioBytes = Encoding.UTF8.GetBytes(MmioId);
            if(mmioBytes.Length != 4)
                throw new FormatException("MMIO Id must be exactly 4 chars long");

            binaryWriter.Write(mmioBytes);

            // Write Chunk Size
            binaryWriter.Write(ContentByteSize);

            Location = binaryWriter.BaseStream.Position;
        }


        /// <summary>
        /// Reads from stream.
        /// </summary>
        /// <param name="binaryReader">The binary reader.</param>
        /// <returns></returns>
        public static RiffChunk ReadFromStream(EndianBinaryReader binaryReader)
        {
            var rc = new RiffChunk();
            rc.Read(binaryReader);
            return rc;
        }
    }
}
