using System;
using MiscUtil.Conversion;

namespace Fundamental.Core.AudioFormats
{
    public class WaveFormatDecorator : WaveFormat
    {
        /// <summary>
        /// The wave format inner
        /// </summary>
        private readonly WaveFormat _waveFormatInner;

        /// <summary>
        /// Initializes a new instance of the <see cref="WaveFormatDecorator" /> class.
        /// </summary>
        /// <param name="bitConverter">The bit converter.</param>
        /// <param name="waveFormatTag">The wave format tag.</param>
        /// <param name="sbSize">Size of the extended segment.</param>
        protected WaveFormatDecorator(EndianBitConverter bitConverter, WaveFormatTag waveFormatTag, int sbSize) :
            this(new WaveFormatEx(bitConverter, sbSize))
        {
            _waveFormatInner.FormatTag = waveFormatTag;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaveFormatDecorator" /> class.
        /// </summary>
        /// <param name="waveFormatTag">The wave format tag.</param>
        /// <param name="sbSize">Size of the sb.</param>
        protected WaveFormatDecorator(WaveFormatTag waveFormatTag, int sbSize) :
            this(new WaveFormatEx(sbSize))
        {
            _waveFormatInner.FormatTag = waveFormatTag;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaveFormatDecorator"/> class.
        /// </summary>
        /// <param name="ptr">The PTR.</param>
        /// <param name="bitConverter">The bit converter.</param>
        public WaveFormatDecorator(IntPtr ptr, EndianBitConverter bitConverter) :
            this( WaveFormatEx.FromPointer(ptr, bitConverter))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaveFormatDecorator"/> class.
        /// </summary>
        /// <param name="waveFormatInner">The wave format instance to be wrapped by the decorator.</param>
        protected WaveFormatDecorator(WaveFormat waveFormatInner)
        {
            _waveFormatInner = waveFormatInner;
            // ReSharper disable once VirtualMemberCallInConstructor
            Vaidate();
        }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        protected virtual void Vaidate()
        {
            
        }


        /// <summary>
        /// Gets or sets the bit converter used to read and write this format instance.
        /// </summary>
        /// <value>
        /// The bit converter.
        /// </value>
        public override EndianBitConverter BitConverter => _waveFormatInner.BitConverter;

        /// <summary>
        /// Gets or sets the format tag.
        /// </summary>
        /// <value>
        /// The format tag.
        /// </value>
        public override WaveFormatTag FormatTag
        {
            get { return _waveFormatInner.FormatTag; }
            set { _waveFormatInner.FormatTag = value; }
        }

        /// <summary>
        /// Gets or sets the number channels.
        /// </summary>
        /// <value>
        /// The channels.
        /// </value>
        public override ushort Channels
        {
            get { return _waveFormatInner.Channels; }
            set { _waveFormatInner.Channels = value; }
        }

        /// <summary>
        /// Gets or sets the samples per sec.
        /// </summary>
        /// <value>
        /// The samples per sec.
        /// </value>
        public override uint SamplesPerSec
        {
            get { return _waveFormatInner.SamplesPerSec; }
            set { _waveFormatInner.SamplesPerSec = value; }
        }

        /// <summary>
        /// Gets or sets the average bytes per sec.
        /// </summary>
        /// <value>
        /// The average bytes per sec.
        /// </value>
        public override uint AvgBytesPerSec
        {
            get { return _waveFormatInner.AvgBytesPerSec; }
            set { _waveFormatInner.AvgBytesPerSec = value; }
        }

        /// <summary>
        /// Gets or sets the block alignment.
        /// </summary>
        /// <value>
        /// The block align.
        /// </value>
        public override ushort BlockAlign
        {
            get { return _waveFormatInner.BlockAlign; }
            set { _waveFormatInner.BlockAlign = value; }
        }

        /// <summary>
        /// Gets or sets the bits per sample.
        /// </summary>
        /// <value>
        /// The bits per sample.
        /// </value>
        public override ushort BitsPerSample
        {
            get { return _waveFormatInner.BitsPerSample; }
            set { _waveFormatInner.BitsPerSample = value; }
        }

        /// <summary>
        /// Gets or sets the extended byte segment.
        /// </summary>
        /// <value>
        /// The extended.
        /// </value>
        public override byte[] ExtendedBytes
        {
            get { return _waveFormatInner.ExtendedBytes; }
            set { _waveFormatInner.ExtendedBytes = value; }
        }

        /// <summary>
        /// Bytes the size.
        /// </summary>
        public override int ByteSize => _waveFormatInner.ByteSize;

        /// <summary>
        /// To the bytes.
        /// </summary>
        /// <returns></returns>
        public override byte[] ToBytes() => _waveFormatInner.ToBytes();

        /// <summary>
        /// Writes the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="offset">The offset.</param>
        public override void Write(byte[] target, int offset) => _waveFormatInner.Write(target, offset);
    }
}
