// ReSharper disable BuiltInTypeReferenceStyle

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiscUtil.IO;

namespace Fundamental.Wave.Container.Riff
{
    public class RiffHeader 
    {
        /// <summary>
        /// We expect the header to start with "RIFF"
        /// </summary>
        private static readonly byte[] RiffFileSignature = { 0x52, 0x49, 0x46, 0x46 };

        /// <summary>
        /// The total byte size of all the 
        /// </summary>
        public UInt32 ContentByteSize => (UInt32)Chunks.Sum(x => x.TotalByteSize);

        /// <summary>
        /// Gets the size of the header byte.
        /// </summary>
        /// <value>
        /// The size of the header byte.
        /// </value>
        public UInt32 HeaderByteSize =>  4  // Sig
                                       + 4  // type
							           + 4; // size

        /// <summary>
        /// Gets the total size of the byte.
        /// </summary>
        /// <value>
        /// The total size of the byte.
        /// </value>
        public UInt32 TotalByteSize => HeaderByteSize + ContentByteSize;

        /// <summary>
        /// Gets or sets the riff type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; set; } = "NONE";

        /// <summary>
        /// Gets the chunks.
        /// </summary>
        /// <value>
        /// The chunks.
        /// </value>
        public List<RiffChunk> Chunks  { get;  } = new List<RiffChunk>(); 

        /// <summary>
        /// Reads the binary fragment from the stream reader.
        /// </summary>
        /// <param name="binaryReader">The binary reader.</param>
        /// <exception cref="System.FormatException">Expected riff header was missing. check that the stream contains a valid header at this position.</exception>
        public void Read(EndianBinaryReader binaryReader)
        {
            var startPosition = binaryReader.BaseStream.Position;

            var fileSignature = binaryReader.ReadBytes(4);

            if(!RiffFileSignature.SequenceEqual(fileSignature))
                throw new FormatException("Expected riff header was missing. check that the stream contains a valid header at this position.");

            // Read the length of the riff chunk
            var byteSize = binaryReader.ReadUInt32();

            // Read the file content type
            var mmioBytes = binaryReader.ReadBytes(4);
            Type = Encoding.UTF8.GetString(mmioBytes, 0, mmioBytes.Length);

            var length = binaryReader.BaseStream.Length;
	        var chunkEndPosition = Math.Min(byteSize + 8, length - startPosition);

	        while (binaryReader.BaseStream.Length < chunkEndPosition)
	        {
	            var chunck = new RiffChunk();
                Chunks.Add(chunck);
                chunck.Read(binaryReader);

                // Go to the position of the next chunk
	            binaryReader.BaseStream.Position = chunck.ContentByteSize + chunck.Location;
	        }
        }

        /// <summary>
        /// Writes the binary fragment to the stream writer.
        /// </summary>
        /// <param name="binaryWriter">The binary writer.</param>
        /// <exception cref="System.FormatException">Riff type MMIO Id must be exactly 4 chars long</exception>
        public void Write(EndianBinaryWriter binaryWriter)
        {
            // Write Signature
            binaryWriter.Write(RiffFileSignature);

            // Write byte Size
            binaryWriter.Write(ContentByteSize  + 4);

            // Write riff type id
            var mmioBytes = Encoding.UTF8.GetBytes(Type);
            if (mmioBytes.Length != 4)
                throw new FormatException("Riff type MMIO Id must be exactly 4 chars long");

            binaryWriter.Write(mmioBytes);

            foreach (var chunk in Chunks)
            {
                chunk.Write(binaryWriter);

                // Go to the position of the next chunk
                binaryWriter.BaseStream.Position = chunk.ContentByteSize + chunk.Location;
            }
        }
    }
}
