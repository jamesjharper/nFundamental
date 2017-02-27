using System;
using System.Text;

using Fundamental.Wave.Format;
using Fundamental.Core.Memory;

using NUnit.Framework;

namespace Fundamental.Core.Tests.Format
{
    [TestFixture]
    public class WaveFormatExTests
    {

        static readonly object[] TestFormats =
        {
            // -------------------------------------------
            // Big Endian, PCM
            // -------------------------------------------
            new object[]
            {
                /* Endianess      */ Endianness.Big,
                /* Format Tag     */ WaveFormatTag.Pcm,
                /* Channels       */ 2,
                /* Sample Rate    */ 44100 /* Hz */,
                /* Bit rate       */ 16, /* bit */
                /* extended       */ ""
            },

            // -------------------------------------------
            // Little Endian, PCM
            // -------------------------------------------
            new object[]
            {
                /* Endianess      */ Endianness.Little,
                /* Format Tag     */ WaveFormatTag.Pcm,
                /* Channels       */ 2,
                /* Sample Rate    */ 44100 /* Hz */,
                /* Bit rate       */ 16, /* bit */
                /* extended       */ ""
            },

            // -------------------------------------------
            // Big Endian, IEEE Float
            // -------------------------------------------
            new object[]
            {
                /* Endianess      */ Endianness.Big,
                /* Format Tag     */ WaveFormatTag.IeeeFloat,
                /* Channels       */ 2,
                /* Sample Rate    */ 44100 /* Hz */,
                /* Bit rate       */ 32, /* bit */
                /* extended       */ ""
            },

            // -------------------------------------------
            // Little Endian, IEEE Float
            // -------------------------------------------
            new object[]
            {
                /* Endianess      */ Endianness.Little,
                /* Format Tag     */ WaveFormatTag.IeeeFloat,
                /* Channels       */ 2,
                /* Sample Rate    */ 44100 /* Hz */,
                /* Bit rate       */ 32, /* bit */
                /* extended       */ ""
            }, 
            
            // -------------------------------------------
            // Big Endian, IEEE Float with extended bytes
            // -------------------------------------------
            new object[]
            {
                /* Endianess      */ Endianness.Big,
                /* Format Tag     */ WaveFormatTag.IeeeFloat,
                /* Channels       */ 2,
                /* Sample Rate    */ 44100 /* Hz */,
                /* Bit rate       */ 32, /* bit */
                /* extended       */ "123456789"
            },

            // -------------------------------------------
            // Little Endian,IEEE Float with extended bytes
            // -------------------------------------------
            new object[]
            {
                /* Endianess      */ Endianness.Little,
                /* Format Tag     */ WaveFormatTag.IeeeFloat,
                /* Channels       */ 2,
                /* Sample Rate    */ 44100 /* Hz */,
                /* Bit rate       */ 32, /* bit */
                /* extended       */ "123456789"
            },
        };

        [Test, TestCaseSource(nameof(TestFormats))]
        public void CanReadPcmFromPointer(
            Endianness endianess,
            WaveFormatTag formatTag,
            int numberOfChannels,
            int samplesPerSec,
            int bitsPerSample,
            string extendedContent)
        {
            // -> ARRANGE:
            var blockAlign = numberOfChannels * (bitsPerSample / 8);
            var avgBytesPerSec = blockAlign * samplesPerSec;
            var extendedBytes = Encoding.UTF8.GetBytes(extendedContent);

            var formatBytes = WaveFormatHelper.CreateFormatEx(endianess, formatTag, numberOfChannels, samplesPerSec, bitsPerSample, extendedBytes);
            unsafe
            {
                fixed (byte* pFormat = formatBytes)
                {
                    // -> ACT
                    var waveFormat = WaveFormat.FromPointer((IntPtr)pFormat, endianess.AsConverter());

                    // -> ASSERT
                    Assert.AreEqual(formatTag, waveFormat.FormatTag);
                    Assert.AreEqual((ushort)numberOfChannels, waveFormat.Channels);
                    Assert.AreEqual((uint)samplesPerSec, waveFormat.SamplesPerSec);
                    Assert.AreEqual((ushort)bitsPerSample, waveFormat.BitsPerSample);
                    Assert.AreEqual((ushort)blockAlign, waveFormat.BlockAlign);
                    Assert.AreEqual((uint)avgBytesPerSec, waveFormat.AvgBytesPerSec);
                    Assert.AreEqual(extendedBytes, waveFormat.ExtendedBytes);
                }
            }
        }

        [Test, TestCaseSource(nameof(TestFormats))]
        public void CanWriteBytesOfPcmFormat(
                Endianness endianess,
                WaveFormatTag formatTag,
                int numberOfChannels,
                int samplesPerSec,
                int bitsPerSample,
                string extendedContent)
        {
            // -> ARRANGE:
            var blockAlign = numberOfChannels * (bitsPerSample / 8);
            var avgBytesPerSec = blockAlign * samplesPerSec;
            var extendedBytes = Encoding.UTF8.GetBytes(extendedContent);

            var exectedFormatBytes = WaveFormatHelper.CreateFormatEx(endianess, formatTag, numberOfChannels, samplesPerSec, bitsPerSample, extendedBytes);

            // -> ACT
            var actualFormatBytes = new WaveFormatEx(endianess.AsConverter())
            {
                FormatTag = formatTag,
                Channels = (ushort)numberOfChannels,
                SamplesPerSec = (uint)samplesPerSec,
                AvgBytesPerSec = (uint)avgBytesPerSec,
                BlockAlign = (ushort)blockAlign,
                BitsPerSample = (ushort)bitsPerSample,
                ExtendedBytes = extendedBytes
            }.ToBytes();

            // -> ASSERT
            Assert.AreEqual(exectedFormatBytes, actualFormatBytes);
        }

    }
}
