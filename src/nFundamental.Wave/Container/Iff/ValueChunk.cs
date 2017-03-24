using System;
using System.IO;

namespace Fundamental.Wave.Container.Iff
{
    public abstract class ValueChunk : Chunk
    {

        public new class FactoryBase<T> : Chunk.FactoryBase<T> where T : ValueChunk, new()
        {

        }


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

        /// <summary> Gets the byte size of the current content. </summary>
        public override long ContentSize => GetValueBytes()?.Length ?? 0;

        /// <summary> Returns whether the chunk headers requires flushing or not. </summary>
        public override bool HeaderRequiresFlush()
        {
            return base.HeaderRequiresFlush() || ContentSize != base.ContentSize;
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
        public new static T ToStream<T>(string id, Stream stream, IffStandard standardStandard)
            where T : ValueChunk, new()
        {
            return Chunk.ToStream<T>(id, stream, standardStandard);
        }

        // private methods

        // Write

        protected override void WriteData()
        {
            var bytes = GetValueBytes();
            if (bytes == null)
            {
                SetLength(0);
                return;
            }

            SetLength(bytes.Length);
            BaseStream.Write(bytes, 0, bytes.Length);
        }

        // Read

        protected override void ReadData()
        {
            var bytes = new byte[Header.DataByteSize];
            BaseStream.Read(bytes, 0, bytes.Length);
            ReadValueBytes(bytes);
        }
    }
}
