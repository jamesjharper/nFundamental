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

        // private methods

        private void CalculateBytes()
        {
            if (_dataNeedsCalculating)
                RawBytes = GetValueBytes();
            _dataNeedsCalculating = false;
        }

        // Write

        protected override void WriteData()
        {
            CalculateBytes();
            Write(RawBytes, 0, RawBytes.Length);
        }

        // Read

        protected override void ReadData()
        {
            if(RawBytes.Length != DataByteSize)
                RawBytes = new byte[DataByteSize];

            Read(RawBytes, 0, (int)DataByteSize);
            ReadValueBytes(RawBytes);
            _dataNeedsCalculating = false;
        }
    }
}
