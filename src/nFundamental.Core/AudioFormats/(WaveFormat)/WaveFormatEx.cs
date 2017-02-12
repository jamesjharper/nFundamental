using System;
using System.Runtime.InteropServices;
using MiscUtil.Conversion;

namespace Fundamental.Core.AudioFormats
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class WaveFormatEx : WaveFormat
    {

        // Defined as per:
        /*

        typedef struct 
        {
            WORD  wFormatTag;
            WORD  nChannels;
            DWORD nSamplesPerSec;
            DWORD nAvgBytesPerSec;
            WORD  nBlockAlign;
            WORD  wBitsPerSample;
            WORD  cbSize;
        } WAVEFORMATEX;

        */

        /// <summary>
        /// The wave format ex stuct size
        /// </summary>
        private const int WaveFormatExSize = 18 /* Bytes*/;

        /// <summary>
        /// The default endianness
        /// </summary>
        public static EndianBitConverter DefaultEndianness = EndianBitConverter.Little;

        /// <summary>
        /// The bytes
        /// </summary>
        private readonly byte[] _waveformatBytes = new byte[WaveFormatExSize];

        /// <summary>
        /// The wave format extended bytes
        /// </summary>
        private byte[] _waveFormatExBytes = new byte[0];

        /// <summary>
        /// Gets or sets the bit converter used to read and write this format instance.
        /// </summary>
        /// <value>
        /// The bit converter.
        /// </value>
        public override EndianBitConverter BitConverter { get; }

        /// <summary>
        /// Gets or sets the format tag.
        /// </summary>
        /// <value>
        /// The format tag.
        /// </value>
        public override WaveFormatTag FormatTag
        {
            get { return (WaveFormatTag)BitConverter.ToUInt16(_waveformatBytes, 0 /* offset */); }
            set { BitConverter.CopyBytes((ushort)value, _waveformatBytes,       0 /* offset */); }
        }

        /// <summary>
        /// Gets or sets the number channels.
        /// </summary>
        /// <value>
        /// The channels.
        /// </value>
        public override ushort Channels
        {
            get { return BitConverter.ToUInt16(_waveformatBytes,  2 /* offset */); }
            set { BitConverter.CopyBytes(value, _waveformatBytes, 2 /* offset */); }
        }

        /// <summary>
        /// Gets or sets the samples per sec.
        /// </summary>
        /// <value>
        /// The samples per sec.
        /// </value>
        public override uint SamplesPerSec
        {
            get { return BitConverter.ToUInt32(_waveformatBytes,  4  /* offset */); }
            set { BitConverter.CopyBytes(value, _waveformatBytes, 4 /* offset */); }
        }

        /// <summary>
        /// Gets or sets the average bytes per sec.
        /// </summary>
        /// <value>
        /// The average bytes per sec.
        /// </value>
        public override uint AvgBytesPerSec
        {
            get { return BitConverter.ToUInt32(_waveformatBytes,  8  /* offset */); }
            set { BitConverter.CopyBytes(value, _waveformatBytes, 8 /* offset */); }
        }

        /// <summary>
        /// Gets or sets the block alignment.
        /// </summary>
        /// <value>
        /// The block align.
        /// </value>
        public override ushort BlockAlign
        {
            get { return BitConverter.ToUInt16(_waveformatBytes,  12  /* offset */); }
            set { BitConverter.CopyBytes(value, _waveformatBytes, 12 /* offset */); }
        }

        /// <summary>
        /// Gets or sets the bits per sample.
        /// </summary>
        /// <value>
        /// The bits per sample.
        /// </value>
        public override ushort BitsPerSample
        {
            get { return BitConverter.ToUInt16(_waveformatBytes,  14  /* offset */); }
            set { BitConverter.CopyBytes(value, _waveformatBytes, 14 /* offset */); }
        }

        /// <summary>
        /// Gets or sets the extended byte segment.
        /// </summary>
        /// <value>
        /// The extended.
        /// </value>
        public override byte[] ExtendedBytes
        {
            get { return _waveFormatExBytes; }
            set
            {
                // Ensure the extended size matches
                checked { ExtendedSize = (ushort)value.Length; }
                _waveFormatExBytes = value;
            }
        }


        /// <summary>
        /// Bytes the size.
        /// </summary>
        /// <returns></returns>
        public override int ByteSize => _waveformatBytes.Length + ExtendedBytes.Length;

        /// <summary>
        /// Gets or sets the size of the extended segment.
        /// </summary>
        /// <value>
        /// The size of the extended.
        /// </value>
        private ushort ExtendedSize
        {
            get { return BitConverter.ToUInt16(_waveformatBytes,  16 /* offset */); }
            set { BitConverter.CopyBytes(value, _waveformatBytes, 16 /* offset */); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaveFormatEx"/> class.
        /// </summary>
        public WaveFormatEx() :
            this(DefaultEndianness)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaveFormatEx" /> class.
        /// </summary>
        /// <param name="bitConverter">The bit converter.</param>
        public WaveFormatEx(EndianBitConverter bitConverter) :
            this(bitConverter, /* extended byte size */0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaveFormatEx" /> class.
        /// </summary>
        /// <param name="sbSize">Size of the extended portion of the wave format.</param>
        public WaveFormatEx(int sbSize) :
           this(DefaultEndianness, sbSize)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaveFormatEx"/> class.
        /// </summary>
        /// <param name="bitConverter">The bit converter.</param>
        /// <param name="sbSize">Size of the extended portion of the wave format.</param>
        public WaveFormatEx(EndianBitConverter bitConverter, int sbSize)
        {
            BitConverter = bitConverter;
            _waveFormatExBytes = new byte[sbSize];
            ExtendedSize = checked((ushort)sbSize);
        }

     

        /// <summary>
        /// To the bytes.
        /// </summary>
        /// <returns></returns>
        public override byte[] ToBytes()
        {
            var bytes = new byte[ByteSize];
            Write(bytes, 0);
            return bytes;
        }

        /// <summary>
        /// Writes the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="offset">The offset.</param>
        public override void Write(byte[] target, int offset)
        {
            Array.Copy(_waveformatBytes, 0, target, offset, _waveformatBytes.Length);
            offset += _waveformatBytes.Length;
            Array.Copy(_waveFormatExBytes, 0, target, offset, _waveFormatExBytes.Length);
        }

        /// <summary>
        /// Writes wave format ex stuct to the given pointer
        /// </summary>
        /// <param name="pTarget">The target pointer.</param>
        public override void Write(IntPtr pTarget)
        {
            Marshal.Copy(_waveformatBytes, 0, pTarget, _waveformatBytes.Length);
            var extendedPostion = pTarget + _waveformatBytes.Length;
            Marshal.Copy(_waveFormatExBytes, 0, extendedPostion, _waveFormatExBytes.Length);
        }

        /// <summary>
        /// Reads the Wave format Ex from a bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public new static WaveFormatEx FromBytes(byte[] bytes)
        {
            return FromBytes(bytes, DefaultEndianness);
        }

        /// <summary>
        /// Reads the Wave format Ex from a bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="bitConverter">The bit converter.</param>
        /// <returns></returns>
        public new static WaveFormatEx FromBytes(byte[] bytes, EndianBitConverter bitConverter)
        {
            return new WaveFormatEx(bytes, bitConverter);
        }

        /// <summary>
        /// Reads the Wave format Ex from a pointer.
        /// </summary>
        /// <param name="ptr">The source pointer.</param>
        /// <returns></returns>
        public new static WaveFormatEx FromPointer(IntPtr ptr)
        {
            return FromPointer(ptr, DefaultEndianness);
        }

        /// <summary>
        /// Reads the Wave format Ex from a pointer.
        /// </summary>
        /// <param name="ptr">The source pointer.</param>
        /// <param name="bitConverter">The bit converter.</param>
        /// <returns></returns>
        public new static WaveFormatEx FromPointer(IntPtr ptr, EndianBitConverter bitConverter)
        {
            return new WaveFormatEx(ptr, bitConverter);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaveFormatEx" /> class.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="bitConverter">The bit converter.</param>
        private WaveFormatEx(byte[] bytes, EndianBitConverter bitConverter)
        {
            // TODO: dont repeat your self, find a way of refactoring these two methods together

            BitConverter = bitConverter;

            var pcmWaveFormatSize = _waveformatBytes.Length - sizeof(ushort);

            // Read all but the last 2 bytes of the pointer.
            // This is to allow for support for formats written a very long time ago
            // using the older PCMWAVEFORMAT struct, which is missing the
            // extended bytes region
            Array.Copy(bytes, 0, _waveformatBytes, 0, pcmWaveFormatSize);

            // "wFormatTag = WAVE_FORMAT_PCM (because cbSize is implicitly zero)."
            if (FormatTag == WaveFormatTag.Pcm)
            {
                ExtendedSize = 0;
                return;
            }

            // Read the CbSize Value
            Array.Copy(bytes, pcmWaveFormatSize, _waveformatBytes, pcmWaveFormatSize, sizeof(ushort));


            if (ExtendedSize == 0)
            {
                // Make sure the waveformatEx struct is clean
                ExtendedSize = 0;
                return;
            }

            ExtendedBytes = new byte[ExtendedSize];
            Array.Copy(bytes, WaveFormatExSize, ExtendedBytes, 0, ExtendedBytes.Length);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaveFormatEx"/> class.
        /// </summary>
        /// <param name="ptr">The PTR.</param>
        /// <param name="bitConverter">The bit converter.</param>
        private WaveFormatEx(IntPtr ptr, EndianBitConverter bitConverter)
        {
            BitConverter = bitConverter;

            // Read all but the last 2 bytes of the pointer.
            // This is to allow for support for formats written a very long time ago
            // using the older PCMWAVEFORMAT struct, which is missing the
            // extended bytes region

            var pcmWaveFormatSize = _waveformatBytes.Length - sizeof(ushort);
            Marshal.Copy(ptr, _waveformatBytes, 0, pcmWaveFormatSize);

            // "wFormatTag = WAVE_FORMAT_PCM (because cbSize is implicitly zero)."
            if (FormatTag == WaveFormatTag.Pcm)
            {
                ExtendedSize = 0;
                return;
            }

            // Read the CbSize Value
            Marshal.Copy(ptr + pcmWaveFormatSize, _waveformatBytes, pcmWaveFormatSize, sizeof(ushort));
            if (ExtendedSize == 0)
            {
                // Make sure the waveformatEx struct is clean
                ExtendedSize = 0;
                return;
            }

            ExtendedBytes = new byte[ExtendedSize];
            Marshal.Copy(ptr + WaveFormatExSize, ExtendedBytes, 0, ExtendedBytes.Length);
        }
    }
}
