//// ReSharper disable BuiltInTypeReferenceStyle

//using System;
//using System.IO;
//using System.Linq;

//using Fundamental.Core.Memory;
//using Fundamental.Wave.Container.Iff;
//using Fundamental.Wave.Format;

//namespace Fundamental.Wave.Container
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    public class WaveFileFormat
//    {

//        /// <summary>
//        /// The wave chunk identifier
//        /// </summary>
//        private const string WaveChunkId = "RIFF";

//        /// <summary>
//        /// The wave sub chunk identifier
//        /// </summary>
//        private const string WaveSubChunkId = "WAVE";

//        /// <summary>
//        /// The format sub chunk identifier
//        /// </summary>
//        private const string FormatSubChunkId = "fmt ";

//        /// <summary>
//        /// The audio sub chunk identifier
//        /// </summary>
//        private const string AudioSubChunkId = "data";

//        /// <summary>
//        /// The group chunk which hold the RIFF content
//        /// </summary>
//        private InterchangeFileFormatGroupChunk Iff { get; set; }

//        /// <summary>
//        /// Prevents a default instance of the <see cref="WaveFileFormat"/> class from being created.
//        /// </summary>
//        private WaveFileFormat()
//        {
//            Iff = InterchangeFileFormatGroupChunk.ToStream
//            (
//                WaveChunkId,
//                WaveSubChunkId,
//                Chunk.ToStream
//                (
//                    FormatSubChunkId
//                ),
//                Chunk.ToStream
//                (
//                    AudioSubChunkId
//                )
//            );
//        }

//        // Public 

//        /// <summary>
//        /// Gets or sets the format.
//        /// </summary>
//        /// <value>
//        /// The format.
//        /// </value>
//        public WaveFormat Format { get; protected set; }

//        /// <summary>
//        /// Gets or sets the size of the audio content.
//        /// </summary>
//        /// <value>
//        /// The size of the audio content.
//        /// </value>
//        public UInt32 AudioContentSize { get; set; }

//        /// <summary>
//        /// Gets the audio content location.
//        /// </summary>
//        /// <value>
//        /// The audio content location.
//        /// </value>
//        public Int64 AudioContentLocation { get; protected set; }

//        /// <summary>
//        /// Writes the specified stream.
//        /// </summary>
//        /// <param name="stream">The stream.</param>
//        /// <param name="endianness">The endianness.</param>
//        public void Write(Stream stream, Endianness endianness)
//        {
//            UpdateAudioChunkByteLength();
//            Iff.Write
//            (
//                stream,
//                endianness,
//                WriteSubChuck
//            );
//        }

//        /// <summary>
//        /// Reads the specified stream.
//        /// </summary>
//        /// <param name="stream">The stream.</param>
//        /// <param name="endianness">The endianness.</param>
//        /// <exception cref="System.FormatException">Wave file header expects WAVE Type multimedia Id</exception>
//        private void Read(Stream stream, Endianness endianness)
//        {
//            Iff = InterchangeFileFormatGroupChunk.Read
//            (
//                stream,
//                endianness,
//                (s, c) => ReadSubChuck(s, c, endianness)
//            );
//        }

//        /// <summary>
//        /// Reads the wave file format from the given stream.
//        /// </summary>
//        /// <param name="stream">The stream.</param>
//        /// <param name="endianness">The endianness.</param>
//        /// <returns></returns>
//        public static WaveFileFormat Read(Stream stream, Endianness endianness = Endianness.Little)
//        {
//            var wff = new WaveFileFormat();
//            wff.Read(stream, endianness);
//            return wff;
//        }

//        /// <summary>
//        /// Creates a new wave file format.
//        /// </summary>
//        /// <param name="waveFormat">The wave format.</param>
//        /// <param name="stream">The stream.</param>
//        /// <param name="endianness">The endianness (WAVE files by definition are little endian).</param>
//        /// <returns></returns>
//        public static WaveFileFormat ToStream(WaveFormat waveFormat, Stream stream, Endianness endianness = Endianness.Little)
//        {
//            var wff = new WaveFileFormat { Format = waveFormat };

//            // The wave file needs to be written immediately to the stream 
//            // so that the audio chuck has a valid data location
//            wff.Write(stream, endianness);

//            return wff;
//        }

//        // Private methods

//        #region Read

//        private void ReadSubChuck(Chunk chunk, Stream stream, Endianness endianness)
//        {
//            switch (chunk.ChunkId)
//            {
//                case FormatSubChunkId:
//                    ReadFormatChunck(chunk, stream, endianness);
//                    return;
//                case AudioSubChunkId:
//                    SetAudioChunck(chunk);
//                    return;
//            }
//        }

//        private void ReadFormatChunck(Chunk chunk, Stream stream, Endianness endianness)
//        {
//            // Read the format header
//            var chunkSize = checked((int) chunk.DataByteSize);
//            var formateBytes = stream.Read(chunkSize);

//            ReadFormatBytes(formateBytes, endianness);
//        }

//        private void ReadFormatBytes(byte[] bytes, Endianness endianness)
//        {
//            // We can only assume the endianness is the same as the source file format
//            var converter = endianness.ToBitConverter();
//            Format = WaveFormat.FromBytes(bytes, converter);
//        }

//        private void SetAudioChunck(Chunk chunk)
//        {
//            AudioContentSize = chunk.DataByteSize;
//            AudioContentLocation = chunk.DataLocation;
//        }

//        #endregion

//        #region Write 

//        private void UpdateAudioChunkByteLength()
//        {
//            var audioChuck = Iff.FirstOrDefault(x => x.ChunkId == AudioSubChunkId);
//            if (audioChuck == null)
//                throw new FormatException("Wave file is missing data chunk, file can not be append.");
//            audioChuck.DataByteSize = AudioContentSize;
//        }

//        private void WriteSubChuck(Chunk chunk, Stream stream)
//        {
//            switch (chunk.ChunkId)
//            {
//                case FormatSubChunkId:
//                    WriteFormatChunck(chunk, stream);
//                    return;
//                case AudioSubChunkId:
//                    SetAudioChunck(chunk);
//                    return;
//            }
//        }

//        private void WriteFormatChunck(Chunk chunk, Stream stream)
//        {
//            var formatBytes = GetFormatBytes();
//            stream.Write(formatBytes);
//            chunk.DataByteSize = checked ((uint)formatBytes.Length);
//        }

//        private byte[] GetFormatBytes()
//        {
//            return Format == null ? new byte[] { } : Format.ToBytes();
//        }

//        #endregion
//    }
//}
