// ReSharper disable BuiltInTypeReferenceStyle

using System;
using System.IO;
using System.Linq;
using Fundamental.Core.Memory;
using Fundamental.Wave.Container.Iff;
using Fundamental.Wave.Format;

using MiscUtil.IO;

namespace Fundamental.Wave.Container
{
    /// <summary>
    /// 
    /// </summary>
    public class WaveFileFormat
    {

        /// <summary>
        /// The wave chunk identifier
        /// </summary>
        private const string WaveChunkId = "RIFF";

        /// <summary>
        /// The wave sub chunk identifier
        /// </summary>
        private const string WaveSubChunkId = "WAVE";

        /// <summary>
        /// The format sub chunk identifier
        /// </summary>
        private const string FormatSubChunkId = "fmt ";

        /// <summary>
        /// The audio sub chunk identifier
        /// </summary>
        private const string AudioSubChunkId = "data";

        /// <summary>
        /// The format chunk
        /// </summary>
        private InterchangeFileFormatChunk _formatChunk = new InterchangeFileFormatChunk { TypeId = FormatSubChunkId };

        /// <summary>
        /// The audio chunk
        /// </summary>
        private InterchangeFileFormatChunk _audioChunk = new InterchangeFileFormatChunk { TypeId = AudioSubChunkId };

         // Public 

        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        /// <value>
        /// The format.
        /// </value>
        public WaveFormat Format { get; set; }

        /// <summary>
        /// Gets or sets the size of the audio content.
        /// </summary>
        /// <value>
        /// The size of the audio content.
        /// </value>
        public UInt32 AudioContentSize
        {
            get { return _audioChunk.ContentByteSize; }
            set { _audioChunk.ContentByteSize = value; }
        }

        /// <summary>
        /// Gets the audio content location.
        /// </summary>
        /// <value>
        /// The audio content location.
        /// </value>
        public Int64 AudioContentLocation => _audioChunk.Location;

        /// <summary>
        /// Writes the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="endianness">The endianness.</param>
        public void Write(Stream stream, Endianness endianness)
        {
            // Get the byte size of the format header 
            var formatBytes = GetFormatBytes();

            // Set the content size on the chuck
            _formatChunk.ContentByteSize = checked((uint)formatBytes.Length);

            var iff = new InterchangeFileFormatListChunk();
            iff.Chunks.Add(_formatChunk);
            iff.Chunks.Add(_audioChunk);

            // Goto the location of the format chunk
            stream.Position = _formatChunk.Location;

            // Write the wave bytes to the location
            stream.Write(formatBytes);
        }

        /// <summary>
        /// Reads the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="endianness">The endianness.</param>
        /// <exception cref="System.FormatException">Wave file header expects WAVE Type multimedia Id</exception>
        public void Read(Stream stream, Endianness endianness)
        {
            var iff = new InterchangeFileFormatListChunk();
            iff.Read(stream, endianness);


            if (iff.TypeId != WaveChunkId)
                throw new FormatException("Wave file header expects WAVE Type multimedia Id");


            if (iff.SubTypeId != WaveSubChunkId)
                throw new FormatException("Wave file header expects WAVE Type multimedia Id");


            var formatChunk = iff.Chunks.First(x => x.TypeId == FormatSubChunkId);
            var audioChunk = iff.Chunks.First(x => x.TypeId == AudioSubChunkId);

            SetFormatChunck(formatChunk, stream, endianness);
            SetAudioChunck(audioChunk);
        }

        private void SetFormatChunck(InterchangeFileFormatChunk chunk, Stream stream, Endianness endianness)
        {
            _formatChunk = chunk;

            // Goto the location of the chunk
            stream.Position = chunk.Location;

            // Read the format header
            var formateBytes = stream.Read(checked((int)chunk.ContentByteSize));

            SetFormatBytes(formateBytes, endianness);
        }


        private void SetFormatBytes(byte[] bytes, Endianness endianness)
        {
            // We can only assume the endianness is the same as the source file format
            var converter = endianness.ToBitConverter();
            Format = WaveFormat.FromBytes(bytes, converter);
        }

        private byte[] GetFormatBytes()
        {
            return Format == null ? new byte[] {} : Format.ToBytes();
        }

        private void SetAudioChunck(InterchangeFileFormatChunk chunk)
        {
            _audioChunk = chunk;
        }
    }
}
