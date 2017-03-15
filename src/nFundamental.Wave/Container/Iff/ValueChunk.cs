using System;
using System.IO;

namespace Fundamental.Wave.Container.Iff
{
    public abstract class ValueChunk : Chunk
    {

        /// <summary>
        /// The data needs calculating
        /// </summary>
        private bool _dataNeedsCalculating = true;

        /// <summary>
        /// Gets the raw bytes which are written to the stream.
        /// </summary>
        /// <value>
        /// The raw bytes.
        /// </value>
        public byte[] RawBytes { get; private set; } = {};

        /// <summary>
        /// Gets the bytes which will be written to the stream.
        /// </summary>
        /// <returns></returns>
        protected abstract byte[] GetValueBytes();

        /// <summary>
        /// Parses the bytes read from the stream.
        /// </summary>
        /// <param name="data">The data.</param>
        protected abstract void ReadValueBytes(byte[] data);

        /// <summary>
        /// Gets the byte size of the current content.
        /// </summary>
        /// <returns></returns>
        public override long CaculateContentSize()
        {
            CalculateBytes();
            return RawBytes.Length;
        }
  
        /// <summary>
        /// Flags the header for flush.
        /// </summary>
        public override void FlagHeaderForFlush()
        {
            _dataNeedsCalculating = true;
            base.FlagHeaderForFlush();
        }

        /// <summary> Reads a chunk from a stream using the given standard. </summary>
        /// <param name="stream">The source stream.</param>
        /// <param name="standardStandard">Type of the chunk standard.</param>
        public new static T FromStream<T>(Stream stream, IffStandard standardStandard)
            where T : ValueChunk, new()
        {
            return Chunk.FromStream<T>(stream, standardStandard);
        }

        /// <summary> Creates a value new chunk. </summary>
        /// <param name="id">The chunk identifier.</param>
        /// <param name="stream">The target stream .</param>
        /// <param name="standardStandard">Type of the chunk standard chunk standard.</param>
        public new static T Create<T>(string id, Stream stream, IffStandard standardStandard)
            where T : ValueChunk, new()
        {
            return Chunk.Create<T>(id, stream, standardStandard);
        }

        // private methods

        private void CalculateBytes()
        {
            if (_dataNeedsCalculating)
                RawBytes = GetValueBytes();
                
            if(RawBytes == null)
                RawBytes = new byte[] {};

            _dataNeedsCalculating = false;
        }

        // Write

        protected override void WriteData()
        {
            CalculateBytes();
            SetLength(RawBytes.Length);
            BaseStream.Write(RawBytes, 0, RawBytes.Length);
        }

        // Read

        protected override void ReadData()
        {
            if(RawBytes.Length != DataByteSize)
                RawBytes = new byte[DataByteSize];

            BaseStream.Read(RawBytes, 0, (int)DataByteSize);
            ReadValueBytes(RawBytes);
            _dataNeedsCalculating = false;
        }
    }
}
