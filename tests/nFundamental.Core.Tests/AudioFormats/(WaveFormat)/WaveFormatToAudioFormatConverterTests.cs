using Fundamental.Core.AudioFormats;
using MiscUtil.Conversion;
using NUnit.Framework;
using Endianness = Fundamental.Core.AudioFormats.Endianness;

namespace Fundamental.Core.Tests.AudioFormats
{
    [TestFixture]
    public class WaveFormatToAudioFormatConverterTests
    {

        private WaveFormatToAudioFormatConverter GetTestFixture()
            => new WaveFormatToAudioFormatConverter();

        #region AudioFormat
 
        private AudioFormat Pcm16Bit44KhzSteroAudioFormat => new AudioFormat
        {
            { FormatKeys.Endianness,     Endianness.Little},
            { FormatKeys.Encoding,       FormatKeys.Pcm.Format},
            { FormatKeys.Pcm.Depth,      Depth.Bit16},
            { FormatKeys.Pcm.SampleRate, SampleRate.Khz44},
            { FormatKeys.Pcm.Channels,   Speakers.Stereo.ChannelCount()},
            { FormatKeys.Pcm.DataType,   PcmDataType.Int},
        };

        #endregion

        #region WaveFormatEx

        private static WaveFormat Pcm16Bit44KhzSteroLittleFormatEx =>
            WaveFormat.CreateFormatEx
            (
                WaveFormatTag.Pcm,
                SampleRate.Khz44,
                Depth.Bit16,
                Speakers.Stereo.ChannelCount(),
                EndianBitConverter.Little
            );

        private static WaveFormat Float32Bit88KhzMonoLittleFormatEx =>
            WaveFormat.CreateFormatEx
            (
                WaveFormatTag.IeeeFloat,
                SampleRate.Khz88,
                Depth.Bit32,
                Speakers.Mono.ChannelCount(),
                EndianBitConverter.Little
            );

        #endregion

        #region WaveFormatExtensible

        private static WaveFormat Pcm8Bit22Khz5Point1LittleFormatExtensible =>
            WaveFormat.CreateFormatExtensible
            (
                AudioMediaSubType.Pcm,
                SampleRate.Khz22,
                Depth.Bit8,
                Speakers.Surround5Point1,
                EndianBitConverter.Little
            );

        private WaveFormat Float24Bit192Khz7Point1LittleFormatExtensible =>
            WaveFormat.CreateFormatExtensible
            (
                AudioMediaSubType.IeeeFloat,
                SampleRate.Khz192,
                Depth.Bit24,
                Speakers.Surround7Point1,
                EndianBitConverter.Little
            );

        #endregion

        #region Convert WAVEFORMATEX stucts to Audio Format

        [Test]
        public void ConvertPcmWaveFormatExToAudioFormat()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            // -> ACT

            IAudioFormat result;
            var canConvert = fixture.TryConvert(Pcm16Bit44KhzSteroLittleFormatEx, out result);

            // -> ASSERT
            Assert.AreEqual(true, canConvert);

            Assert.AreEqual(Endianness.Little,     result[FormatKeys.Endianness]);
            Assert.AreEqual(FormatKeys.Pcm.Format, result[FormatKeys.Encoding]);

            Assert.AreEqual(16 /* Bits */ ,        result[FormatKeys.Pcm.Depth]);
            Assert.AreEqual(2  /* Channels */ ,    result[FormatKeys.Pcm.Channels]);
            Assert.AreEqual(44100  /* Hz */ ,      result[FormatKeys.Pcm.SampleRate]);
            Assert.AreEqual(PcmDataType.Int ,      result[FormatKeys.Pcm.DataType]);
        }

        [Test]
        public void ConvertIeeFloatWaveFormatExToAudioFormat()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            // -> ACT

            IAudioFormat result;
            var canConvert = fixture.TryConvert(Float32Bit88KhzMonoLittleFormatEx, out result);

            // -> ASSERT
            Assert.AreEqual(true, canConvert);

            Assert.AreEqual(Endianness.Little,     result[FormatKeys.Endianness]);
            Assert.AreEqual(FormatKeys.Pcm.Format, result[FormatKeys.Encoding]);

            Assert.AreEqual(32 /* Bits */ ,        result[FormatKeys.Pcm.Depth]);
            Assert.AreEqual(1  /* Channels */ ,    result[FormatKeys.Pcm.Channels]);
            Assert.AreEqual(88200  /* Hz */ ,      result[FormatKeys.Pcm.SampleRate]);
            Assert.AreEqual(PcmDataType.Ieee754,   result[FormatKeys.Pcm.DataType]);
        }

        #endregion

        // Tests

        #region Convert WAVEFORMATEXTENSIBLE stucts to Audio Format


        [Test]
        public void ConvertPcmWaveFormatExtensibleToAudioFormat()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            // -> ACT

            IAudioFormat result;
            var canConvert = fixture.TryConvert(Pcm8Bit22Khz5Point1LittleFormatExtensible, out result);

            // -> ASSERT
            Assert.AreEqual(true, canConvert);

            Assert.AreEqual(Endianness.Little,        result[FormatKeys.Endianness]);
            Assert.AreEqual(FormatKeys.Pcm.Format,    result[FormatKeys.Encoding]);

            Assert.AreEqual(8 /* Bits */ ,            result[FormatKeys.Pcm.Depth]);
            Assert.AreEqual(6  /* Channels */ ,       result[FormatKeys.Pcm.Channels]);
            Assert.AreEqual(22050  /* Hz */ ,         result[FormatKeys.Pcm.SampleRate]);

            //
            Assert.AreEqual(PcmDataType.Int,          result[FormatKeys.Pcm.DataType]);
            Assert.AreEqual(Speakers.Surround5Point1, result[FormatKeys.Pcm.Speakers]);
        }

        [Test]
        public void ConvertIeeFloatWaveFormatExtensibleToAudioFormat()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            // -> ACT
            IAudioFormat result;
            var canConvert = fixture.TryConvert(Float24Bit192Khz7Point1LittleFormatExtensible, out result);

            // -> ASSERT
            Assert.AreEqual(true, canConvert);

            Assert.AreEqual(Endianness.Little,        result[FormatKeys.Endianness]);
            Assert.AreEqual(FormatKeys.Pcm.Format,    result[FormatKeys.Encoding]);

            Assert.AreEqual(24 /* Bits */ ,           result[FormatKeys.Pcm.Depth]);
            Assert.AreEqual(8  /* Channels */ ,       result[FormatKeys.Pcm.Channels]);
            Assert.AreEqual(192000  /* Hz */ ,        result[FormatKeys.Pcm.SampleRate]);
            Assert.AreEqual(PcmDataType.Ieee754,      result[FormatKeys.Pcm.DataType]);
            Assert.AreEqual(Speakers.Surround7Point1, result[FormatKeys.Pcm.Speakers]);
        }

        #endregion

        #region Convert Audio Format to WAVEFORMATEX

        [Test]
        public void ConvertPcmAudioFormatToWaveFormatEx()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();
            var format = Pcm16Bit44KhzSteroAudioFormat;

            var expectedBlockAlign = (int)format[FormatKeys.Pcm.Channels] * (int)format[FormatKeys.Pcm.Depth] / 8;
            var expectedAvgBytesPerSec = expectedBlockAlign * (int)format[FormatKeys.Pcm.SampleRate];
            // -> ACT

            WaveFormat result;
            var canConvert = fixture.TryConvert(Pcm16Bit44KhzSteroAudioFormat, out result);

            // -> ASSERT
            Assert.AreEqual(true, canConvert);

            Assert.AreEqual(EndianBitConverter.Little,  result.BitConverter);
            Assert.AreEqual(16    /* Bits */ ,          result.BitsPerSample);
            Assert.AreEqual(2     /* Channels */ ,      result.Channels);
            Assert.AreEqual(44100 /* Hz */ ,            result.SamplesPerSec);
            Assert.AreEqual(WaveFormatTag.Pcm,          result.FormatTag);

            Assert.AreEqual(expectedBlockAlign,      result.BlockAlign);
            Assert.AreEqual(expectedAvgBytesPerSec , result.AvgBytesPerSec);
        }


        #endregion
    }
}
