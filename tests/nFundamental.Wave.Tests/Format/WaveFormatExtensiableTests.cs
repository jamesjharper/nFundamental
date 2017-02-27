using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

using Fundamental.Core.Memory;
using Fundamental.Core.AudioFormats;

namespace Fundamental.Wave.Format
{
    [TestFixture]
    public class WaveFormatExtensiableTests
    {

        static readonly object[] TestFormats =
        {
            // -------------------------------------------
            // Little Endian, PCM
            // -------------------------------------------
            new object[]
            {
                /* Endianess        */ Endianness.Little,
                /* Channels         */ 6,
                /* Sample Rate      */ 44100 /* Hz */,
                /* Bit rate         */ 24    /* bit */,
                /* Valid Bit rate   */ 32    /* bit */,
                /* Speakers         */ Speakers.Surround5Point1,
                /* Format Id        */ AudioMediaSubType.Pcm
            },
            // -------------------------------------------
            // Big Endian, PCM
            // -------------------------------------------
            new object[]
            {
                /* Endianess        */ Endianness.Big,
                /* Channels         */ 6,
                /* Sample Rate      */ 44100 /* Hz */,
                /* Bit rate         */ 24    /* bit */,
                /* Valid Bit rate   */ 32    /* bit */,
                /* Speakers         */ Speakers.Surround5Point1,
                /* Format Id        */ AudioMediaSubType.Pcm
            },
            // -------------------------------------------
            // Little Endian, IEEE Float
            // -------------------------------------------
            new object[]
            {
                /* Endianess        */ Endianness.Little,
                /* Channels         */ 6,
                /* Sample Rate      */ 44100 /* Hz */,
                /* Bit rate         */ 24    /* bit */,
                /* Valid Bit rate   */ 32    /* bit */,
                /* Speakers         */ Speakers.Surround5Point1,
                /* Format Id        */ AudioMediaSubType.IeeeFloat
            },
            // -------------------------------------------
            // Big Endian, IEEE Float
            // -------------------------------------------
            new object[]
            {
                /* Endianess        */ Endianness.Big,
                /* Channels         */ 6,
                /* Sample Rate      */ 44100 /* Hz */,
                /* Bit rate         */ 24    /* bit */,
                /* Valid Bit rate   */ 32    /* bit */,
                /* Speakers         */ Speakers.Surround5Point1,
                /* Format Id        */ AudioMediaSubType.IeeeFloat
            },
        };

        [Test, TestCaseSource(nameof(TestFormats))]
        public void CanReadExtensibalePcmFormatFromPointer(
            Endianness endianess,
            int numberOfChannels,
            int samplesPerSec,
            int bitsPerSample,
            int samplesUnion,
            Speakers channelMask,
            Guid subFormat)
        {
            // -> ARRANGE:
            var blockAlign = numberOfChannels * (bitsPerSample / 8);
            var avgBytesPerSec = blockAlign * samplesPerSec;

            var formatBytes = WaveFormatHelper.CreateFormatExtensiable(endianess, numberOfChannels, samplesPerSec, bitsPerSample, blockAlign, avgBytesPerSec, samplesUnion, channelMask, subFormat);
            unsafe
            {
                fixed (byte* pFormat = formatBytes)
                {
                    // -> ACT
                    var waveFormat = new WaveFormatExtensible((IntPtr)pFormat, endianess.AsConverter());

                    // -> ASSERT
                    Assert.AreEqual(WaveFormatTag.Extensible, waveFormat.FormatTag);
                    Assert.AreEqual(numberOfChannels, waveFormat.Channels);
                    Assert.AreEqual(samplesPerSec, waveFormat.SamplesPerSec);
                    Assert.AreEqual(bitsPerSample, waveFormat.BitsPerSample);
                    Assert.AreEqual(blockAlign, waveFormat.BlockAlign);
                    Assert.AreEqual(avgBytesPerSec, waveFormat.AvgBytesPerSec);
                    Assert.AreEqual(samplesUnion, waveFormat.ValidBitsPerSample);
                    Assert.AreEqual(samplesUnion, waveFormat.SamplesPerBlock);
                    Assert.AreEqual(channelMask, waveFormat.ChannelMask);
                    Assert.AreEqual(subFormat, waveFormat.SubFormat);
                }
            }
        }

        [Test, TestCaseSource(nameof(TestFormats))]
        public void CanWriteBytesOfExtensibaleFormat(
                    Endianness endianess,
                    int numberOfChannels,
                    int samplesPerSec,
                    int bitsPerSample,
                    int samplesUnion,
                    Speakers channelMask,
                    Guid subFormat)
        {
            // -> ARRANGE:
            var blockAlign = numberOfChannels * (bitsPerSample / 8);
            var avgBytesPerSec = blockAlign * samplesPerSec;

            var expectedFormatBytes = WaveFormatHelper.CreateFormatExtensiable(endianess, numberOfChannels, samplesPerSec, bitsPerSample, blockAlign, avgBytesPerSec, samplesUnion, channelMask, subFormat);

            // -> ACT
            var actualFormatBytes = new WaveFormatExtensible(subFormat, endianess.AsConverter())
            {
                Channels = (ushort)numberOfChannels,
                SamplesPerSec = (uint)samplesPerSec,
                AvgBytesPerSec = (uint)avgBytesPerSec,
                BlockAlign = (ushort)blockAlign,
                BitsPerSample = (ushort)bitsPerSample,
                ValidBitsPerSample = (short)samplesUnion,
                SamplesPerBlock = (short)samplesUnion,
                ChannelMask = channelMask,
            }.ToBytes();

            // -> ASSERT
            Assert.AreEqual(expectedFormatBytes, actualFormatBytes);
        }
    }
}
