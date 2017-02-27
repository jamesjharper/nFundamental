using System;
using System.IO;
using Fundamental.Core.Memory;
using Fundamental.Core.AudioFormats;

namespace Fundamental.Wave.Format
{
    public static class WaveFormatHelper
    {
        public static byte[] CreateFormatEx(
            Endianness endianness,
            WaveFormatTag waveFormatTag,
            int numberOfChannels,
            int samplesPerSec,
            int bitsPerSample,
            byte[] extended)
        {
            var blockAlign = numberOfChannels * (bitsPerSample / 8);
            var avgBytesPerSec = (blockAlign * samplesPerSec);

            return CreateFormatEx
            (
                endianness, 
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
           Endianness endianness,
           WaveFormatTag waveFormatTag,
           int numberOfChannels,
           int samplesPerSec,
           int bitsPerSample,
           int blockAlign,
           int avgBytesPerSec,
           byte[] extended)
        {
            var ms = new MemoryStream();

            var writer = ms.AsEndianWriter(endianness);
            writer.Write((ushort)waveFormatTag);
            writer.Write((ushort)numberOfChannels);
            writer.Write((uint)samplesPerSec);
            writer.Write((uint)avgBytesPerSec);
            writer.Write((ushort)blockAlign);
            writer.Write((ushort)bitsPerSample);
            writer.Write((ushort)extended.Length);
            ms.Write(extended, 0, extended.Length);

            return ms.ToArray();
        }

        public static byte[] CreateFormatExtensiable(
            Endianness endianness,
            int numberOfChannels,
            int samplesPerSec,
            int bitsPerSample,
            int blockAlign,
            int avgBytesPerSec,
            int sampleUnion,
            Speakers channelMask,
            Guid subFormat)
        {
            var ms = new MemoryStream();
            var writer = ms.AsEndianWriter(endianness);
            writer.Write((short)sampleUnion);
            writer.Write((uint)channelMask);

            var subFormatBytes = subFormat.ToByteArray();
            writer.Write(subFormatBytes, 0, subFormatBytes.Length);

            return CreateFormatEx(endianness, WaveFormatTag.Extensible, numberOfChannels, samplesPerSec, bitsPerSample, blockAlign, avgBytesPerSec, ms.ToArray());
        }
    }
}
