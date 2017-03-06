namespace Fundamental.Wave.Container.Iff
{
    public abstract class ValueChunk : Chunk
    {
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
            RawBytes = GetValueBytes();
            return RawBytes.Length;
        }

        // Write

        protected override void WriteData()
        {
            BaseStream.Write(RawBytes, 0, RawBytes.Length);
        }

        // Read

        protected override void ReadData()
        {
            if(RawBytes.Length != DataByteSize)
                RawBytes = new byte[DataByteSize];

            BaseStream.Read(RawBytes, 0, (int)DataByteSize);
            ReadValueBytes(RawBytes);
        }
    }
}
