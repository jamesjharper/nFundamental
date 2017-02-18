using System;
using System.IO;
using MiscUtil.Conversion;
using Fundamental.Core.AudioFormats;

namespace Fundamental.Wave.Format
{
    public static class WaveFormatHelper
    {
        public static byte[] CreateFormatEx(
            EndianBitConverter endianBitConverter,
            WaveFormatTag waveFormatTag,
            ushort numberOfChannels,
            uint samplesPerSec,
            ushort bitsPerSample,
            byte[] extended)
        {
            var blockAlign = (ushort)(numberOfChannels * (bitsPerSample / 8));
            var avgBytesPerSec = (uint)(blockAlign * samplesPerSec);

            return CreateFormatEx
            (
                endianBitConverter, 
                waveFormatTag, 
                numberOfChannels,
                samplesPerSec, 
                bitsPerSample,
                blockAlign, 
                avgBytesPerSec, 
                extended
            );
        }

        public static byte[] CreateFormatEx(
           EndianBitConverter endianBitConverter,
           WaveFormatTag waveFormatTag,
           ushort numberOfChannels,
           uint samplesPerSec,
           ushort bitsPerSample,
           ushort blockAlign,
           uint avgBytesPerSec,
           byte[] extended)
        {
            var ms = new MemoryStream();

            var formatTagBytes = endianBitConverter.GetBytes((ushort)waveFormatTag);
            ms.Write(formatTagBytes, 0, formatTagBytes.Length);

            var numberOfChannelBytes = endianBitConverter.GetBytes(numberOfChannels);
            ms.Write(numberOfChannelBytes, 0, numberOfChannelBytes.Length);

            var samplesPerSecBytes = endianBitConverter.GetBytes(samplesPerSec);
            ms.Write(samplesPerSecBytes, 0, samplesPerSecBytes.Length);

            var avgBytesPerSecBytes = endianBitConverter.GetBytes(avgBytesPerSec);
            ms.Write(avgBytesPerSecBytes, 0, avgBytesPerSecBytes.Length);

            var blockAlignBytes = endianBitConverter.GetBytes(blockAlign);
            ms.Write(blockAlignBytes, 0, blockAlignBytes.Length);

            var bitsPerSampleBytes = endianBitConverter.GetBytes(bitsPerSample);
            ms.Write(bitsPerSampleBytes, 0, bitsPerSampleBytes.Length);

            var extendedSizeBytes = endianBitConverter.GetBytes((ushort)extended.Length);
            ms.Write(extendedSizeBytes, 0, extendedSizeBytes.Length);
            ms.Write(extended, 0, extended.Length);

            return ms.ToArray();
        }

        public static byte[] CreateFormatExtensiable(
            EndianBitConverter endianBitConverter,
            ushort numberOfChannels,
            uint samplesPerSec,
            ushort bitsPerSample,
            ushort blockAlign,
            uint avgBytesPerSec,
            short sampleUnion,
            Speakers channelMask,
            Guid subFormat)
        {
            var ms = new MemoryStream();

            var validBitsPerSampleBytes = endianBitConverter.GetBytes(sampleUnion);
            ms.Write(validBitsPerSampleBytes, 0, validBitsPerSampleBytes.Length);

            var channelMaskBytes = endianBitConverter.GetBytes((uint)channelMask);
            ms.Write(channelMaskBytes, 0, channelMaskBytes.Length);

            var subFormatBytes = subFormat.ToByteArray();
            ms.Write(subFormatBytes, 0, subFormatBytes.Length);

            return CreateFormatEx(endianBitConverter, WaveFormatTag.Extensible, numberOfChannels, samplesPerSec, bitsPerSample, blockAlign, avgBytesPerSec, ms.ToArray());
        }
    }
}
