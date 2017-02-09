using System;
using Fundamental.Core.Math;
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
        /// Writes wave format ex stuct to the given pointer
        /// </summary>
        /// <param name="pTarget">The target pointer.</param>
        public abstract void Write(IntPtr pTarget);


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
        /// Reads the Wave format Ex from a bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public static WaveFormat FromBytes(byte[] bytes)
        {
            return WaveFormatEx.FromBytes(bytes);
        }

        /// <summary>
        /// Reads the Wave format Ex from a bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="bitConverter">The bit converter.</param>
        /// <returns></returns>
        public static WaveFormat FromBytes(byte[] bytes, EndianBitConverter bitConverter)
        {
            return WaveFormatEx.FromBytes(bytes);
        }

        /// <summary>
        /// Creates the PCM format.
        /// </summary>
        /// <param name="sampleRate">The sample rate.</param>
        /// <param name="bitsPerSample">The bits per sample.</param>
        /// <param name="speakerConfiguration">The speaker configuration.</param>
        /// <param name="bitConverter">The bit converter.</param>
        /// <returns></returns>
        public static WaveFormat CreatePcm(int sampleRate, 
                                           int bitsPerSample, 
                                           Speakers speakerConfiguration,
                                           EndianBitConverter bitConverter)
        {
            if (UseOlderFormatType(speakerConfiguration, bitsPerSample))
            {
                return CreateFormatEx
                (
                    WaveFormatTag.Pcm,
                    sampleRate,
                    bitsPerSample,
                    speakerConfiguration.ChannelCount(),
                    bitConverter
                );
            }

            return CreateFormatExtensible
            (
                AudioMediaSubType.Pcm,
                sampleRate,
                bitsPerSample,
                speakerConfiguration,
                bitConverter
            );
        }


        /// <summary>
        /// Creates the IEEE float format.
        /// </summary>
        /// <param name="sampleRate">The sample rate.</param>
        /// <param name="bitsPerSample">The bits per sample.</param>
        /// <param name="speakerConfiguration">The speaker configuration.</param>
        /// <param name="bitConverter">The bit converter.</param>
        /// <returns></returns>
        public static WaveFormat CreateIeeeFloat(int sampleRate, 
                                                 int bitsPerSample, 
                                                 Speakers speakerConfiguration,
                                                 EndianBitConverter bitConverter)
        {
            if (UseOlderFormatType(speakerConfiguration, bitsPerSample))
            {
                return CreateFormatEx
                (
                    WaveFormatTag.IeeeFloat,
                    sampleRate,
                    bitsPerSample,
                    speakerConfiguration.ChannelCount(),
                    bitConverter
                );
            }

            return CreateFormatExtensible
            (
                AudioMediaSubType.IeeeFloat,
                sampleRate,
                bitsPerSample,
                speakerConfiguration,
                bitConverter
            );
        }


        /// <summary>
        /// Creates the format ex.
        /// </summary>
        /// <param name="waveFormatTag">The wave format tag.</param>
        /// <param name="sampleRate">The sample rate.</param>
        /// <param name="bitsPerSample">The bits per sample.</param>
        /// <param name="numberOfChannels">The number of channels.</param>
        /// <param name="bitConverter">The bit converter.</param>
        /// <returns></returns>
        public static WaveFormat CreateFormatEx(WaveFormatTag waveFormatTag,
                                                int sampleRate,
                                                int bitsPerSample,
                                                int numberOfChannels,
                                                EndianBitConverter bitConverter)
        {
            // Calculate contain restricted block size (must be of 2^x)
            var blockAlign = numberOfChannels * bitsPerSample / 8;
            var avgBytesPerSec = blockAlign * sampleRate;

            return CreateFormatEx(waveFormatTag, sampleRate, bitsPerSample, blockAlign, avgBytesPerSec, numberOfChannels, bitConverter);
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
        /// <param name="bitConverter">The bit converter.</param>
        /// <returns></returns>
        public static WaveFormat CreateFormatEx(WaveFormatTag waveFormatTag,
                                                int sampleRate,
                                                int bitsPerSample,
                                                int blockAlign,
                                                int avgBytesPerSec,
                                                int numberOfChannels,
                                                EndianBitConverter bitConverter)
        {
            return new WaveFormatEx(bitConverter)
            {
                FormatTag       = waveFormatTag,
                SamplesPerSec   = checked((uint)sampleRate),
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
        /// <param name="bitConverter">The bit converter.</param>
        /// <returns></returns>
        public static WaveFormat CreateFormatExtensible(Guid mediaSubType,
                                                        int sampleRate,
                                                        int bitsPerSample, 
                                                        Speakers speakerConfig,
                                                        EndianBitConverter bitConverter)
        {
            return CreateFormatExtensible(mediaSubType, sampleRate, bitsPerSample, speakerConfig.ChannelCount(), speakerConfig, bitConverter);
        }

        /// <summary>
        /// Creates the format extensible.
        /// </summary>
        /// <param name="mediaSubType">Type of the media sub.</param>
        /// <param name="sampleRate">The sample rate.</param>
        /// <param name="bitsPerSample">The bits per sample.</param>
        /// <param name="numberOfChannels">The number of channels.</param>
        /// <param name="speakerConfig">The speaker configuration.</param>
        /// <param name="bitConverter">The bit converter.</param>
        /// <returns></returns>
        public static WaveFormat CreateFormatExtensible(Guid mediaSubType,
                                                        int sampleRate,
                                                        int bitsPerSample,
                                                        int numberOfChannels,
                                                        Speakers speakerConfig,
                                                        EndianBitConverter bitConverter)
        {
            // Calculate container restricted block size (must be of 2^x)
            var samplePacking = GetContainerPackingSize(bitsPerSample);

            return CreateFormatExtensible(mediaSubType, sampleRate, bitsPerSample, samplePacking, numberOfChannels, speakerConfig, bitConverter);
        }


        /// <summary>
        /// Creates the format extensible.
        /// </summary>
        /// <param name="mediaSubType">Type of the media sub.</param>
        /// <param name="sampleRate">The sample rate.</param>
        /// <param name="bitsPerSample">The bits per sample.</param>
        /// <param name="samplePacking">The sample packing.</param>
        /// <param name="numberOfChannels">The number of channels.</param>
        /// <param name="speakerConfig">The speaker configuration.</param>
        /// <param name="bitConverter">The bit converter.</param>
        /// <returns></returns>
        public static WaveFormat CreateFormatExtensible(Guid mediaSubType,
                                                        int sampleRate,
                                                        int bitsPerSample,
                                                        int samplePacking,
                                                        int numberOfChannels,
                                                        Speakers speakerConfig,
                                                        EndianBitConverter bitConverter)
        {
            
            var blockAlign = numberOfChannels * samplePacking / 8;
            var avgBytesPerSec = blockAlign * sampleRate;

            return new WaveFormatExtensible(mediaSubType, bitConverter)
            {
                SamplesPerSec       = checked((uint)sampleRate),
                AvgBytesPerSec      = checked((uint)avgBytesPerSec),
                BitsPerSample       = checked((ushort)samplePacking),
                BlockAlign          = checked((ushort)blockAlign),
                Channels            = checked((ushort)numberOfChannels),
                ValidBitsPerSample  = checked((short)bitsPerSample),
                ChannelMask         = speakerConfig
            };
        }

        // Private Methods

        private static int GetContainerPackingSize( int bitsPerSample)
        {           
             // If the number of set bits is not equal to 1 then the bits per sample is not
            // of 2^x, which means it does not pack completely into its container, which means
            // we can not use the old wave format for storing this format  
            return Bitwise.IsSquareOf2(bitsPerSample)
                ? Bitwise.RoundDownToNearestBase2Power(bitsPerSample) * 2
                : bitsPerSample;
        }

        private static bool UseOlderFormatType(Speakers speakerConfiguration, int bitsPerSample)
        {
            // If the number of set bits is not equal to 1 then the bits per sample is not
            // of 2^x, which means it does not pack completely into its container, which means
            // we can not use the old wave format for storing this format           
            if (Bitwise.IsSquareOf2(bitsPerSample))
                return false;
            
            // To improve backwards compatibility Mono and Stereo sources are created using the older WaveFormatEx stuct. 
           return speakerConfiguration == Speakers.Mono || speakerConfiguration == Speakers.Stereo;
        }

      

    }
}
