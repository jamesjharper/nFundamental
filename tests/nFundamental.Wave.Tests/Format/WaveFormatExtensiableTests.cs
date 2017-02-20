using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiscUtil.Conversion;
using NUnit.Framework;
using Fundamental.Core.AudioFormats;

namespace Fundamental.Wave.Format
{
    [TestFixture]
    public class WaveFormatExtensiableTests
    {


        #region Test PCM Formats

        [Test]
        public void CanReadLittleEndianExtensibalePcmFormatFromPointer()
        {
            // -> ASSERT
            AssertCanReadFormatFromPointer
            (
                /* Endianess        */ EndianBitConverter.Little,
                /* Channels         */ 6,
                /* Sample Rate      */ 44100 /* Hz */,
                /* Bit rate         */ 24    /* bit */,
                /* Valid Bit rate   */ 32    /* bit */,
                /* Speakers         */ Speakers.Surround5Point1,
                /* Format Id        */ AudioMediaSubType.Pcm
            );
        }


        [Test]
        public void CanReadBigEndianExtensibalePcmFormatFromPointer()
        {
            // -> ASSERT
            AssertCanReadFormatFromPointer
            (
                /* Endianess        */ EndianBitConverter.Big,
                /* Channels         */ 6,
                /* Sample Rate      */ 44100 /* Hz */,
                /* Bit rate         */ 24    /* bit */,
                /* Valid Bit rate   */ 32    /* bit */,
                /* Speakers         */ Speakers.Surround5Point1,
                /* Format Id        */ AudioMediaSubType.Pcm
            );
        }

        [Test]
        public void CanWriteLittleEndianBytesOfExtensibalePcmFormat()
        {
            // -> ASSERT
            AssertCanReadFormatFromPointer
            (
                /* Endianess        */ EndianBitConverter.Little,
                /* Channels         */ 6,
                /* Sample Rate      */ 44100 /* Hz */,
                /* Bit rate         */ 24    /* bit */,
                /* Valid Bit rate   */ 32    /* bit */,
                /* Speakers         */ Speakers.Surround5Point1,
                /* Format Id        */ AudioMediaSubType.Pcm
            );
        }

        [Test]
        public void CanWriteBigEndianBytesOfExtensibalePcmFormat()
        {
            // -> ASSERT
            AssertCanReadFormatFromPointer
            (
                /* Endianess        */ EndianBitConverter.Big,
                /* Channels         */ 6,
                /* Sample Rate      */ 44100 /* Hz */,
                /* Bit rate         */ 24    /* bit */,
                /* Valid Bit rate   */ 32    /* bit */,
                /* Speakers         */ Speakers.Surround5Point1,
                /* Format Id        */ AudioMediaSubType.Pcm
            );
        }

        #endregion

        #region Test Ieee Float Formats

        [Test]
        public void CanReadLittleEndianExtensibaleIeeeFloatFormatFromPointer()
        {
            // -> ASSERT
            AssertCanReadFormatFromPointer
            (
                /* Endianess        */ EndianBitConverter.Little,
                /* Channels         */ 8,
                /* Sample Rate      */ 88200 /* Hz */,
                /* Bit rate         */ 32    /* bit */,
                /* Valid Bit rate   */ 32    /* bit */,
                /* Speakers         */ Speakers.Surround7Point1,
                /* Format Id        */ AudioMediaSubType.IeeeFloat
            );
        }


        [Test]
        public void CanReadBigEndianExtensibaleIeeeFloatFormatFromPointer()
        {
            // -> ASSERT
            AssertCanReadFormatFromPointer
            (
                /* Endianess        */ EndianBitConverter.Big,
                /* Channels         */ 8,
                /* Sample Rate      */ 88200 /* Hz */,
                /* Bit rate         */ 32    /* bit */,
                /* Valid Bit rate   */ 32    /* bit */,
                /* Speakers         */ Speakers.Surround7Point1,
                /* Format Id        */ AudioMediaSubType.IeeeFloat
            );
        }

        [Test]
        public void CanWriteLittleEndianBytesOfExtensibaleIeeeFloatFormat()
        {
            // -> ASSERT
            AssertCanReadFormatFromPointer
            (
                /* Endianess        */ EndianBitConverter.Little,
                /* Channels         */ 8,
                /* Sample Rate      */ 88200 /* Hz */,
                /* Bit rate         */ 32    /* bit */,
                /* Valid Bit rate   */ 32    /* bit */,
                /* Speakers         */ Speakers.Surround7Point1,
                /* Format Id        */ AudioMediaSubType.IeeeFloat
            );
        }

        [Test]
        public void CanWriteBigEndianBytesOfExtensibaleIeeeFloatFormat()
        {
            // -> ASSERT
            AssertCanReadFormatFromPointer
            (
                /* Endianess        */ EndianBitConverter.Big,
                /* Channels         */ 8,
                /* Sample Rate      */ 88200 /* Hz */,
                /* Bit rate         */ 32    /* bit */,
                /* Valid Bit rate   */ 32    /* bit */,
                /* Speakers         */ Speakers.Surround7Point1,
                /* Format Id        */ AudioMediaSubType.IeeeFloat
            );
        }

        #endregion

        public unsafe void AssertCanReadFormatFromPointer(
            EndianBitConverter endianess,
            ushort numberOfChannels,
            uint samplesPerSec,
            ushort bitsPerSample,
            short samplesUnion,
            Speakers channelMask,
            Guid subFormat)
        {
            // -> ARRANGE:
            var blockAlign = (ushort)(numberOfChannels * (bitsPerSample / 8));
            var avgBytesPerSec = (uint)(blockAlign * samplesPerSec);

            var formatBytes = WaveFormatHelper.CreateFormatExtensiable(endianess, numberOfChannels, samplesPerSec, bitsPerSample, blockAlign, avgBytesPerSec, samplesUnion, channelMask, subFormat);

            fixed (byte* pFormat = formatBytes)
            {
                // -> ACT
                var waveFormat = new WaveFormatExtensible((IntPtr)pFormat, endianess);

                // -> ASSERT
                Assert.AreEqual(WaveFormatTag.Extensible, waveFormat.FormatTag);
                Assert.AreEqual(numberOfChannels,         waveFormat.Channels);
                Assert.AreEqual(samplesPerSec,            waveFormat.SamplesPerSec);
                Assert.AreEqual(bitsPerSample,            waveFormat.BitsPerSample);
                Assert.AreEqual(blockAlign,               waveFormat.BlockAlign);
                Assert.AreEqual(avgBytesPerSec,           waveFormat.AvgBytesPerSec);
                Assert.AreEqual(samplesUnion,             waveFormat.ValidBitsPerSample);
                Assert.AreEqual(samplesUnion,             waveFormat.SamplesPerBlock);
                Assert.AreEqual(channelMask,              waveFormat.ChannelMask);
                Assert.AreEqual(subFormat,                waveFormat.SubFormat);
            }
        }

        public void AssertCanGetBytesFromFormat(
            EndianBitConverter endianess,
            ushort numberOfChannels,
            uint samplesPerSec,
            ushort bitsPerSample,
            short samplesUnion,
            Speakers channelMask,
            Guid subFormat)
        {
            // -> ARRANGE:
            var blockAlign = (ushort)(numberOfChannels * (bitsPerSample / 8));
            var avgBytesPerSec = (uint)(blockAlign * samplesPerSec);

            var expectedFormatBytes = WaveFormatHelper.CreateFormatExtensiable(endianess, numberOfChannels, samplesPerSec, bitsPerSample, blockAlign, avgBytesPerSec, samplesUnion, channelMask, subFormat);

            // -> ACT
            var actualFormatBytes = new WaveFormatExtensible(subFormat, endianess)
            {
                Channels = numberOfChannels,
                SamplesPerSec = samplesPerSec,
                AvgBytesPerSec = avgBytesPerSec,
                BlockAlign = blockAlign,
                BitsPerSample = bitsPerSample,
                ValidBitsPerSample = samplesUnion,
                SamplesPerBlock = samplesUnion,
                ChannelMask = channelMask,
            }.ToBytes();

            // -> ASSERT
            Assert.AreEqual(expectedFormatBytes, actualFormatBytes);
        }
    }
}
