using System;

namespace Fundamental.Core.AudioFormats
{
    public class WaveFormatExtensiable : WaveFormatDecorator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WaveFormatExtensiable"/> class.
        /// </summary>
        /// <param name="waveFormatInner">The wave format instance to be wrapped by the decorator.</param>
        public WaveFormatExtensiable(WaveFormat waveFormatInner) : base(waveFormatInner)
        {
        }
        
        /// <summary>
        /// Gets or sets the valid bits per sample.
        /// Number of bits of precision in the signal. Usually equal to WAVEFORMATEX.wBitsPerSample. However, 
        /// wBitsPerSample is the container size and must be a multiple of 8, whereas wValidBitsPerSample can be 
        /// any value not exceeding the container size. For example, if the format uses 20-bit samples, 
        /// wBitsPerSample must be at least 24, but wValidBitsPerSample is 20.
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
            get { return (Speakers)BitConverter.ToUInt16(ExtendedBytes,   2 /* offset */); }
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
