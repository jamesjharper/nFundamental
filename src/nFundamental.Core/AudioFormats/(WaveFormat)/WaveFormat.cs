using MiscUtil.Conversion;

namespace Fundamental.Core.AudioFormats
{
    public abstract class WaveFormat
    {

        /// <summary>
        /// Gets or sets the bit converter used to read and write this format instance.
        /// </summary>
        /// <value>
        /// The bit converter.
        /// </value>
        public abstract EndianBitConverter BitConverter { get;  }

        /// <summary>
        /// Gets or sets the format tag.
        /// </summary>
        /// <value>
        /// The format tag.
        /// </value>
        public abstract WaveFormatTag FormatTag { get; set;  }

        /// <summary>
        /// Gets or sets the number channels.
        /// </summary>
        /// <value>
        /// The channels.
        /// </value>
        public abstract ushort Channels { get; set; }

        /// <summary>
        /// Gets or sets the samples per sec.
        /// </summary>
        /// <value>
        /// The samples per sec.
        /// </value>
        public abstract uint SamplesPerSec { get; set; }

        /// <summary>
        /// Gets or sets the average bytes per sec.
        /// </summary>
        /// <value>
        /// The average bytes per sec.
        /// </value>
        public abstract uint AvgBytesPerSec { get; set; }

        /// <summary>
        /// Gets or sets the block alignment.
        /// </summary>
        /// <value>
        /// The block align.
        /// </value>
        public abstract ushort BlockAlign { get; set; }

        /// <summary>
        /// Gets or sets the bits per sample.
        /// </summary>
        /// <value>
        /// The bits per sample.
        /// </value>
        public abstract ushort BitsPerSample { get; set; }

        /// <summary>
        /// Gets or sets the extended byte segment.
        /// </summary>
        /// <value>
        /// The extended.
        /// </value>
        public abstract byte[] ExtendedBytes { get; set; }

        /// <summary>
        /// Bytes the size.
        /// </summary>
        /// <returns></returns>
        public abstract int ByteSize { get;  }

        /// <summary>
        /// To the bytes.
        /// </summary>
        /// <returns></returns>
        public abstract byte[] ToBytes();

        /// <summary>
        /// Writes the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="offset">The offset.</param>
        public abstract void Write(byte[] target, int offset);


        public static WaveFormat CreatePcm(int sampleRate, int bitsPerSameple, int numberOfChannels, EndianBitConverter endianess)
        {
            var blockAlign = numberOfChannels * bitsPerSameple / 8;
            var avgBytesPerSec = blockAlign * sampleRate;

            return new WaveFormatEx(endianess)
            {
                SamplesPerSec   = checked ((uint)bitsPerSameple),
                AvgBytesPerSec  = checked ((uint)avgBytesPerSec),
                BitsPerSample   = checked ((ushort)bitsPerSameple),
                BlockAlign      = checked ((ushort)blockAlign),
                Channels        = checked ((ushort)numberOfChannels)
            };
        }

    }
}
