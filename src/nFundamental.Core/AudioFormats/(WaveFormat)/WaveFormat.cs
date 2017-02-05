using System;
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


        /// <summary>
        /// Reads the Wave format Ex from a pointer.
        /// </summary>
        /// <param name="ptr">The source pointer.</param>
        /// <returns></returns>
        public static WaveFormatEx FromPointer(IntPtr ptr)
        {
            return WaveFormatEx.FromPointer(ptr);
        }

        /// <summary>
        /// Reads the Wave format Ex from a pointer.
        /// </summary>
        /// <param name="ptr">The source pointer.</param>
        /// <param name="bitConverter">The bit converter.</param>
        /// <returns></returns>
        public static WaveFormatEx FromPointer(IntPtr ptr, EndianBitConverter bitConverter)
        {
            return WaveFormatEx.FromPointer(ptr, bitConverter);
        }

        /// <summary>
        /// Creates the PCM format.
        /// </summary>
        /// <param name="sampleRate">The sample rate.</param>
        /// <param name="bitsPerSample">The bits per sample.</param>
        /// <param name="speakerConfiguration">The speaker configuration.</param>
        /// <returns></returns>
        public static WaveFormat CreatePcm(int sampleRate, int bitsPerSample, Speakers speakerConfiguration)
        {
            if (UseOlderFormatType(speakerConfiguration))
            {
                return CreateFormatEx
                (
                    WaveFormatTag.Pcm,
                    sampleRate,
                    bitsPerSample,
                    speakerConfiguration.ChannelCount()
                );
            }

            return CreateFormatExtensible
            (
                AudioMediaSubType.Pcm,
                sampleRate,
                bitsPerSample,
                speakerConfiguration
            );
        }


        /// <summary>
        /// Creates the IEEE float format.
        /// </summary>
        /// <param name="sampleRate">The sample rate.</param>
        /// <param name="bitsPerSample">The bits per sample.</param>
        /// <param name="speakerConfiguration">The speaker configuration.</param>
        /// <returns></returns>
        public static WaveFormat CreateIeeeFloat(int sampleRate, int bitsPerSample, Speakers speakerConfiguration)
        {
            if (UseOlderFormatType(speakerConfiguration))
            {
                return CreateFormatEx
                (
                    WaveFormatTag.IeeeFloat,
                    sampleRate,
                    bitsPerSample,
                    speakerConfiguration.ChannelCount()
                );

            }

            return CreateFormatExtensible
            (
                AudioMediaSubType.IeeeFloat,
                sampleRate,
                bitsPerSample,
                speakerConfiguration
            );
        }


        /// <summary>
        /// Creates the format ex.
        /// </summary>
        /// <param name="waveFormatTag">The wave format tag.</param>
        /// <param name="sampleRate">The sample rate.</param>
        /// <param name="bitsPerSample">The bits per sample.</param>
        /// <param name="numberOfChannels">The number of channels.</param>
        /// <returns></returns>
        public static WaveFormat CreateFormatEx(WaveFormatTag waveFormatTag,
                                                 int sampleRate,
                                                 int bitsPerSample,
                                                 int numberOfChannels)
        {
            // Calculate contain restricted block size (must be of 2^x)
            var blockAlign = numberOfChannels * bitsPerSample / 8;
            var avgBytesPerSec = blockAlign * sampleRate;

            return CreateFormatEx(waveFormatTag, sampleRate, bitsPerSample, blockAlign, avgBytesPerSec, numberOfChannels);
        }

        /// <summary>
        /// Creates the format ex.
        /// </summary>
        /// <param name="waveFormatTag">The wave format tag.</param>
        /// <param name="sampleRate">The sample rate.</param>
        /// <param name="bitsPerSample">The bits per sample.</param>
        /// <param name="blockAlign">The block align.</param>
        /// <param name="avgBytesPerSec">The average bytes per sec.</param>
        /// <param name="numberOfChannels">The number of channels.</param>
        /// <returns></returns>
        public static WaveFormat CreateFormatEx(WaveFormatTag waveFormatTag,
                                                int sampleRate,
                                                int bitsPerSample,
                                                int blockAlign,
                                                int avgBytesPerSec,
                                                int numberOfChannels)
        {
            return new WaveFormatEx
            {
                FormatTag       = waveFormatTag,
                SamplesPerSec   = checked((uint)bitsPerSample),
                AvgBytesPerSec  = checked((uint)avgBytesPerSec),
                BitsPerSample   = checked((ushort)bitsPerSample),
                BlockAlign      = checked((ushort)blockAlign),
                Channels        = checked((ushort)numberOfChannels)
            };
        }


        /// <summary>
        /// Creates the format extensible.
        /// </summary>
        /// <param name="mediaSubType">Type of the media sub.</param>
        /// <param name="sampleRate">The sample rate.</param>
        /// <param name="bitsPerSample">The bits per sample.</param>
        /// <param name="speakerConfig">The speaker configuration.</param>
        /// <returns></returns>
        public static WaveFormat CreateFormatExtensible(Guid mediaSubType,
                                                         int sampleRate,
                                                         int bitsPerSample,
                                                         Speakers speakerConfig)
        {
            var numberOfChannels = speakerConfig.ChannelCount();

            // Calculate contain restricted block size (must be of 2^x)
            var blockAlign = numberOfChannels * bitsPerSample / 8;
            var avgBytesPerSec = blockAlign * sampleRate;

            return new WaveFormatExtensible(mediaSubType)
            {
                SamplesPerSec = checked((uint)bitsPerSample),
                AvgBytesPerSec = checked((uint)avgBytesPerSec),
                BitsPerSample = checked((ushort)bitsPerSample),
                BlockAlign = checked((ushort)blockAlign),
                Channels = checked((ushort)numberOfChannels),
                ValidBitsPerSample = checked((short)bitsPerSample),
                ChannelMask = speakerConfig
            };
        }

        // Private Methods

        private static bool UseOlderFormatType(Speakers speakerConfiguration)
        {
            // To improve backwards compatibility Mono and Stereo sources are created using the older WaveFormatEx Stuct. 
           return speakerConfiguration == Speakers.Mono || speakerConfiguration == Speakers.Stereo;
        }

      

    }
}
