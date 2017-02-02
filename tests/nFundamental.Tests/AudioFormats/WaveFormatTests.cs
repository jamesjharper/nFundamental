
using System;
using System.IO;
using MiscUtil.Conversion;
using NUnit.Framework;
using Fundamental.AudioFormats;

namespace Fundamental.Tests.AudioFormats
{
    [TestFixture]
    public class WaveFormatTests
    {

        #region Test PCM Formats

        [Test]
        public void CanReadLittleEndianPcm()
        {
            // -> ASSERT
            AssertCanReadFormatFromPointer
            (
                /* Endianess      */ EndianBitConverter.Little,
                /* Format Tag     */ WaveFormatTag.Pcm,
                /* Channels       */ 2,
                /* Sample Rate    */ 44100 /* Hz */,
                /* Bit rate       */ 16 /* bit */,
                /* Extended bytes */ new byte[0]
            );
        }

        [Test]
        public void CanReadBigEndianPcm()
        {
            // -> ASSERT
            AssertCanReadFormatFromPointer
            (
                /* Endianess      */ EndianBitConverter.Big,
                /* Format Tag     */ WaveFormatTag.Pcm,
                /* Channels       */ 2,
                /* Sample Rate    */ 44100 /* Hz */,
                /* Bit rate       */ 16 /* bit */,
                /* Extended bytes */ new byte[0]
            );
        }

        [Test]
        public void CanGetLittleEndianPcmBytes()
        {
            // -> ASSERT
            AssertCanGetBytesFromFormat
            (
                /* Endianess      */ EndianBitConverter.Little,
                /* Format Tag     */ WaveFormatTag.Pcm,
                /* Channels       */ 2,
                /* Sample Rate    */ 44100 /* Hz */,
                /* Bit rate       */ 16 /* bit */,
                /* Extended bytes */ new byte[0]
            );
        }

        [Test]
        public void CanGetBigEndianPcmBytes()
        {
            // -> ASSERT
            AssertCanGetBytesFromFormat
            (
                /* Endianess      */ EndianBitConverter.Big,
                /* Format Tag     */ WaveFormatTag.Pcm,
                /* Channels       */ 2,
                /* Sample Rate    */ 44100 /* Hz */,
                /* Bit rate       */ 16 /* bit */,
                /* Extended bytes */ new byte[0]
            );
        }

        #endregion

        #region Test Ieee Float Formats

        [Test]
        public void CanReadBigEndianIeee()
        {
            // -> ASSERT
            AssertCanReadFormatFromPointer
            (
                /* Endianess      */ EndianBitConverter.Big,
                /* Format Tag     */ WaveFormatTag.IeeeFloat,
                /* Channels       */ 1,
                /* Sample Rate    */ 44100 /* Hz */,
                /* Bit rate       */ 32 /* bit */,
                /* Extended bytes */ new byte[0]
            );
        }

        [Test]
        public void CanReadBigLittleIeee()
        {
            // -> ASSERT
            AssertCanReadFormatFromPointer
            (
                /* Endianess      */ EndianBitConverter.Little,
                /* Format Tag     */ WaveFormatTag.IeeeFloat,
                /* Channels       */ 1,
                /* Sample Rate    */ 44100 /* Hz */,
                /* Bit rate       */ 32 /* bit */,
                /* Extended bytes */ new byte[0]
            );
        }

        [Test]
        public void CanGetLittleEndianIeeeBytes()
        {
            // -> ASSERT
            AssertCanReadFormatFromPointer
            (
                /* Endianess      */ EndianBitConverter.Little,
                /* Format Tag     */ WaveFormatTag.IeeeFloat,
                /* Channels       */ 1,
                /* Sample Rate    */ 44100 /* Hz */,
                /* Bit rate       */ 32 /* bit */,
                /* Extended bytes */ new byte[0]
            );
        }

        [Test]
        public void CanGetBigEndianIeeeBytes()
        {
            // -> ASSERT
            AssertCanReadFormatFromPointer
            (
                /* Endianess      */ EndianBitConverter.Big,
                /* Format Tag     */ WaveFormatTag.IeeeFloat,
                /* Channels       */ 1,
                /* Sample Rate    */ 44100 /* Hz */,
                /* Bit rate       */ 32 /* bit */,
                /* Extended bytes */ new byte[0]
            );
        }

        #endregion

        #region Test Extended Formats

        [Test]
        public void CanReadBigEndianExtended()
        {
            // -> ASSERT
            AssertCanReadFormatFromPointer
            (
                /* Endianess      */ EndianBitConverter.Big,
                /* Format Tag     */ WaveFormatTag.Extensible,
                /* Channels       */ 1,
                /* Sample Rate    */ 44100 /* Hz */,
                /* Bit rate       */ 32 /* bit */,
                /* Extended bytes */ new byte[] {1, 2 , 3 , 4}
            );
        }

        [Test]
        public void CanReadLittleEndianExtended()
        {
            // -> ASSERT
            AssertCanReadFormatFromPointer
            (
                /* Endianess      */ EndianBitConverter.Little,
                /* Format Tag     */ WaveFormatTag.Extensible,
                /* Channels       */ 1,
                /* Sample Rate    */ 44100 /* Hz */,
                /* Bit rate       */ 32 /* bit */,
                /* Extended bytes */ new byte[] { 1, 2, 3, 4 }
            );
        }

        [Test]
        public void CanGetBigEndianExtendedBytes()
        {
            // -> ASSERT
            AssertCanGetBytesFromFormat
            (
                /* Endianess      */ EndianBitConverter.Big,
                /* Format Tag     */ WaveFormatTag.Extensible,
                /* Channels       */ 1,
                /* Sample Rate    */ 44100 /* Hz */,
                /* Bit rate       */ 32 /* bit */,
                /* Extended bytes */ new byte[] { 1, 2, 3, 4 }
            );
        }

        [Test]
        public void CanGetLittleEndianExtendedBytes()
        {
            // -> ASSERT
            AssertCanGetBytesFromFormat
            (
                /* Endianess      */ EndianBitConverter.Little,
                /* Format Tag     */ WaveFormatTag.Extensible,
                /* Channels       */ 1,
                /* Sample Rate    */ 44100 /* Hz */,
                /* Bit rate       */ 32 /* bit */,
                /* Extended bytes */ new byte[] { 1, 2, 3, 4 }
            );
        }

        #endregion

        // Helper Methods 

        public unsafe void AssertCanReadFormatFromPointer(
            EndianBitConverter endianess,
            WaveFormatTag formatTag,
            ushort numberOfChannels,
            uint samplesPerSec,
            ushort bitsPerSample,
            byte[] extended)
        {
            // -> ARRANGE:
            var blockAlign = (ushort)(numberOfChannels * (bitsPerSample / 8));
            var avgBytesPerSec = (uint)(blockAlign * samplesPerSec);

            var formatBytes = GetFormat (endianess, formatTag, numberOfChannels, samplesPerSec, bitsPerSample, extended);
            fixed (byte* pFormat = formatBytes)
            {
                // -> ACT
                var waveFormat = new WaveFormat((IntPtr)pFormat, endianess);

                // -> ASSERT
                Assert.AreEqual(formatTag, waveFormat.FormatTag);
                Assert.AreEqual(numberOfChannels, waveFormat.Channels);
                Assert.AreEqual(samplesPerSec, waveFormat.SamplesPerSec);
                Assert.AreEqual(bitsPerSample, waveFormat.BitsPerSample);
                Assert.AreEqual(blockAlign, waveFormat.BlockAlign);
                Assert.AreEqual(avgBytesPerSec, waveFormat.AvgBytesPerSec);
                Assert.AreEqual(extended, waveFormat.ExtendedBytes);
            }
        }

        public void AssertCanGetBytesFromFormat(
                EndianBitConverter endianess,
                WaveFormatTag formatTag,
                ushort numberOfChannels,
                uint samplesPerSec,
                ushort bitsPerSample,
                byte[] extended)
        {
            // -> ARRANGE:
            var blockAlign = (ushort)(numberOfChannels * (bitsPerSample / 8));
            var avgBytesPerSec = (uint)(blockAlign * samplesPerSec);

            var exectedFormatBytes = GetFormat(endianess, formatTag, numberOfChannels, samplesPerSec, bitsPerSample, extended);

            // -> ACT
            var actualFormatBytes = new WaveFormat(endianess)
                {
                    FormatTag = formatTag,
                    Channels = numberOfChannels,
                    SamplesPerSec = samplesPerSec,
                    AvgBytesPerSec = avgBytesPerSec,
                    BlockAlign = blockAlign,
                    BitsPerSample = bitsPerSample,
                    ExtendedBytes = extended
                }.ToBytes();

            // -> ASSERT
            Assert.AreEqual(exectedFormatBytes, actualFormatBytes);
        }

        byte[] GetFormat(EndianBitConverter endianBitConverter,
                         WaveFormatTag waveFormatTag,
                         ushort numberOfChannels,
                         uint samplesPerSec,
                         ushort bitsPerSample,
                         byte[] extended)
        {
            var blockAlign = (ushort)(numberOfChannels * (bitsPerSample / 8));
            var avgBytesPerSec = (uint)(blockAlign * samplesPerSec);

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
    }
}
