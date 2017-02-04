using System;
using System.IO;
using Fundamental.Core.AudioFormats;
using MiscUtil.Conversion;
using NUnit.Framework;

namespace Fundamental.Core.Tests.AudioFormats
{
    [TestFixture]
    public class WaveFormatExTests
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

            var formatBytes = WaveFormatHelper.CreateFormatEx(endianess, formatTag, numberOfChannels, samplesPerSec, bitsPerSample, extended);
            fixed (byte* pFormat = formatBytes)
            {
                // -> ACT
                var waveFormat = new WaveFormatEx((IntPtr)pFormat, endianess);

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

            var exectedFormatBytes = WaveFormatHelper.CreateFormatEx(endianess, formatTag, numberOfChannels, samplesPerSec, bitsPerSample, extended);

            // -> ACT
            var actualFormatBytes = new WaveFormatEx(endianess)
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

      
    }
}
