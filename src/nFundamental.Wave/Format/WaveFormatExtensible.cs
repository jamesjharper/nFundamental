using System;
using Fundamental.Core.AudioFormats;
using MiscUtil.Conversion;

namespace Fundamental.Wave.Format
{
    public sealed class WaveFormatExtensible : WaveFormatDecorator
    {
        // Defined as per:
        /*
  
        typedef struct {
            WAVEFORMATEX Format;
            union 
            {
                WORD wValidBitsPerSample;
                WORD wSamplesPerBlock;
                WORD wReserved;
            } Samples;
          DWORD        dwChannelMask;
          GUID         SubFormat;
        } WAVEFORMATEXTENSIBLE, *PWAVEFORMATEXTENSIBLE;

        */

        /// <summary>
        /// The wave format ex stuct size
        /// </summary>
        private const int WaveFormatExtensibleCbSize = 22 /* Bytes*/;



        /// <summary>
        /// Initializes a new instance of the <see cref="WaveFormatExtensible" /> class.
        /// </summary>
        /// <param name="subFormatType">Type of the sub format.</param>
        public WaveFormatExtensible(Guid subFormatType)
            : base(WaveFormatTag.Extensible, WaveFormatExtensibleCbSize)
        {
            SubFormat = subFormatType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaveFormatExtensible" /> class.
        /// </summary>
        /// <param name="subFormatType">Type of the sub format.</param>
        /// <param name="bitConverter">The bit converter.</param>
        public WaveFormatExtensible(Guid subFormatType, EndianBitConverter bitConverter) 
            : base(bitConverter, WaveFormatTag.Extensible, WaveFormatExtensibleCbSize)
        {
            SubFormat = subFormatType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaveFormatExtensible"/> class.
        /// </summary>
        /// <param name="ptr">The PTR.</param>
        /// <param name="bitConverter">The bit converter.</param>
        public WaveFormatExtensible(IntPtr ptr, EndianBitConverter bitConverter) 
            : base(ptr, bitConverter)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaveFormatExtensible"/> class.
        /// </summary>
        /// <param name="waveFormatInner">The wave format instance to be wrapped by the decorator.</param>
        public WaveFormatExtensible(WaveFormat waveFormatInner) 
            : base(waveFormatInner)
        {

        }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        protected override void Vaidate()
        {
            if(WaveFormatExtensibleCbSize < ExtendedBytes.Length)
                throw new FormatException("Given pointer did not de-serialize to a valid WAVEFORMATEXTENTSIBLE struct.");
        }

        /// <summary>
        /// Hide the Format tag field as it cant be changed and
        /// is not relevant to this format type
        /// </summary>
        /// <value>
        /// The format tag.
        /// </value>
        private new WaveFormatTag FormatTag
        {
            get { return base.FormatTag; }
            set { base.FormatTag = value; }
        }

        /// <summary>
        /// Gets or sets the valid bits per sample.
        /// NOTE: THIS FIELD IS A UNION OF THE SamplesPerBlock FIELD.
        /// 
        /// Extract from: https://msdn.microsoft.com/en-us/library/windows/desktop/dd757714(v=vs.85).aspx
        /// "Number of bits of precision in the signal. Usually equal to WAVEFORMATEX.wBitsPerSample. However, 
        ///  wBitsPerSample is the container size and must be a multiple of 8, whereas wValidBitsPerSample can be 
        ///  any value not exceeding the container size. For example, if the format uses 20-bit samples, 
        ///  wBitsPerSample must be at least 24, but wValidBitsPerSample is 20."
        /// </summary>
        /// <value>
        /// The valid bits per sample.
        /// </value>
        public short ValidBitsPerSample
        {
            get { return BitConverter.ToInt16(ExtendedBytes,   0 /* offset */); }
            set { BitConverter.CopyBytes(value, ExtendedBytes, 0 /* offset */); }
        }

        /// <summary>
        /// Gets or sets the samples per block.
        /// NOTE: THIS FIELD IS A UNION OF THE ValidBitsPerSample FIELD.
        /// Typically this value is only observed if the BlockAlign field is equal to 0
        /// 
        /// Extract from: https://msdn.microsoft.com/en-us/library/windows/desktop/dd757714(v=vs.85).aspx
        /// "Number of samples contained in one compressed block of audio data. This value is used in buffer 
        ///  estimation. This value is used with compressed formats that have a fixed number of samples 
        ///  within each block. This value can be set to 0 if a variable number of samples is contained in 
        ///  each block of compressed audio data. In this case, buffer estimation and position information 
        ///  needs to be obtained in other ways."
        /// </summary>
        /// <value>
        /// The samples per block.
        /// </value>
        public short SamplesPerBlock
        {
            get { return BitConverter.ToInt16(ExtendedBytes,   0 /* offset */); }
            set { BitConverter.CopyBytes(value, ExtendedBytes, 0 /* offset */); }
        }

        /// <summary>
        /// Gets or sets the channel mask.
        /// </summary>
        /// <value>
        /// The channel mask.
        /// </value>
        public Speakers ChannelMask
        {
            get { return (Speakers)BitConverter.ToUInt32(ExtendedBytes,   2 /* offset */); }
            set { BitConverter.CopyBytes((uint)value, ExtendedBytes,      2 /* offset */); }
        }


        /// <summary>
        /// Gets or sets the sub format.
        /// </summary>
        /// <value>
        /// The sub format.
        /// </value>
        public Guid SubFormat
        {
            get { return ToGuid(ExtendedBytes,   6 /* offset */); }
            set { ToBytes(value, ExtendedBytes,   6 /* offset */); }
        }

        // Private methods

        private static void ToBytes(Guid value, byte[] b, int offset)
        {
            var guidBytes = value.ToByteArray();
            Array.Copy(/* source */ guidBytes,  0, 
                       /* target */ b, offset, 
                       /* Length */ guidBytes.Length);

        }


        private static Guid ToGuid(byte[] b, int offset)
        {
            var guidBytes = new byte[16];
            Array.Copy(/* source */ b,  offset, 
                       /* target */ guidBytes, 0, 
                       /* Length */16);
            return new Guid(guidBytes);
        }
    }
}
