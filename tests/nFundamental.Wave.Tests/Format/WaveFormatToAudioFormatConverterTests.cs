using System;
using Fundamental.Core.AudioFormats;
using MiscUtil.Conversion;
using NUnit.Framework;
using Endianness = Fundamental.Core.Memory.Endianness;

namespace Fundamental.Wave.Format
{
    [TestFixture]
    public class WaveFormatToAudioFormatConverterTests
    {

        private WaveFormatToAudioFormatConverter GetTestFixture()
            => new WaveFormatToAudioFormatConverter();

        // Tests

        // WAVEFORMATEX -> IAudioFormat

        #region Convert WAVEFORMATEX stucts to Audio Format

        // WAVEFORMATEX 16 Bit int / 44Khz / Stereo / Little

        [Test]
        public void ConvertPcmWaveFormatExToAudioFormat_16Bit44KhzStereoLittle()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            var audioFormat =
                WaveFormat.CreateFormatEx
                (
                    WaveFormatTag.Pcm,
                    SampleRate.Khz44,
                    Depth.Bit16,
                    Speakers.Stereo.ChannelCount(),
                    EndianBitConverter.Little
                );

            // -> ACT
            IAudioFormat result;
            var canConvert = fixture.TryConvert(audioFormat, out result);

            // -> ASSERT
            Assert.AreEqual(true, canConvert);

            Assert.AreEqual(Endianness.Little,      result[FormatKeys.Endianness]);
            Assert.AreEqual(FormatKeys.Pcm.Format,  result[FormatKeys.Encoding]);

            Assert.AreEqual(16 /* Bits */ ,         result[FormatKeys.Pcm.Depth]);
            Assert.AreEqual(2  /* Channels */ ,     result[FormatKeys.Pcm.Channels]);
            Assert.AreEqual(44100  /* Hz */ ,       result[FormatKeys.Pcm.SampleRate]);
            Assert.AreEqual(PcmDataType.Int,        result[FormatKeys.Pcm.DataType]);

            // Should not have speakers attribute as it can not be described by wave format ex
            Assert.AreEqual(false, result.ContainsKey(FormatKeys.Pcm.Speakers));
        }

        // WAVEFORMATEX 8 Bit int / 22Khz / Stereo / Big

        [Test]
        public void ConvertPcmWaveFormatExToAudioFormat_8Bit22KhzSteroBig()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            var audioFormat =
                WaveFormat.CreateFormatEx
                (
                    WaveFormatTag.Pcm,
                    SampleRate.Khz22,
                    Depth.Bit8,
                    Speakers.Stereo.ChannelCount(),
                    EndianBitConverter.Big
                );


            // -> ACT
            IAudioFormat result;
            var canConvert = fixture.TryConvert(audioFormat, out result);

            // -> ASSERT
            Assert.AreEqual(true, canConvert);

            Assert.AreEqual(Endianness.Big,        result[FormatKeys.Endianness]);
            Assert.AreEqual(FormatKeys.Pcm.Format,  result[FormatKeys.Encoding]);

            Assert.AreEqual(8 /* Bits */ ,          result[FormatKeys.Pcm.Depth]);
            Assert.AreEqual(2  /* Channels */ ,     result[FormatKeys.Pcm.Channels]);
            Assert.AreEqual(22050  /* Hz */ ,       result[FormatKeys.Pcm.SampleRate]);
            Assert.AreEqual(PcmDataType.Int,        result[FormatKeys.Pcm.DataType]);

            // Should not have speakers attribute as it can not be described by wave format ex
            Assert.AreEqual(false, result.ContainsKey(FormatKeys.Pcm.Speakers));
        }

        // WAVEFORMATEX 32 Bit float / 88Khz / Mono / Little

        [Test]
        public void ConvertPcmWaveFormatExToAudioFormat_32BitFloat88KhzMonoLittle()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            var audioFormat =
                WaveFormat.CreateFormatEx
                (
                    WaveFormatTag.IeeeFloat,
                    SampleRate.Khz88,
                    Depth.Bit32,
                    Speakers.Mono.ChannelCount(),
                    EndianBitConverter.Little
                );

            // -> ACT
            IAudioFormat result;
            var canConvert = fixture.TryConvert(audioFormat, out result);

            // -> ASSERT
            Assert.AreEqual(true, canConvert);

            Assert.AreEqual(Endianness.Little,      result[FormatKeys.Endianness]);
            Assert.AreEqual(FormatKeys.Pcm.Format,  result[FormatKeys.Encoding]);

            Assert.AreEqual(32 /* Bits */ ,         result[FormatKeys.Pcm.Depth]);
            Assert.AreEqual(1  /* Channels */ ,     result[FormatKeys.Pcm.Channels]);
            Assert.AreEqual(88200  /* Hz */ ,       result[FormatKeys.Pcm.SampleRate]);
            Assert.AreEqual(PcmDataType.Ieee754,    result[FormatKeys.Pcm.DataType]);

            // Should not have speakers attribute as it can not be described by wave format ex
            Assert.AreEqual(false, result.ContainsKey(FormatKeys.Pcm.Speakers));
        }

        // WAVEFORMATEX 64 Bit float / 96Khz / Mono / Big

        [Test]
        public void ConvertPcmWaveFormatExToAudioFormat_64BitFloat96KhzMonoBig()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            var audioFormat =
                WaveFormat.CreateFormatEx
                (
                    WaveFormatTag.IeeeFloat,
                    SampleRate.Khz96,
                    Depth.Bit64,
                    Speakers.Mono.ChannelCount(),
                    EndianBitConverter.Big
                );

            // -> ACT
            IAudioFormat result;
            var canConvert = fixture.TryConvert(audioFormat, out result);

            // -> ASSERT
            Assert.AreEqual(true, canConvert);

            Assert.AreEqual(Endianness.Big,         result[FormatKeys.Endianness]);
            Assert.AreEqual(FormatKeys.Pcm.Format,  result[FormatKeys.Encoding]);

            Assert.AreEqual(64 /* Bits */ ,         result[FormatKeys.Pcm.Depth]);
            Assert.AreEqual(1  /* Channels */ ,     result[FormatKeys.Pcm.Channels]);
            Assert.AreEqual(96000  /* Hz */ ,       result[FormatKeys.Pcm.SampleRate]);
            Assert.AreEqual(PcmDataType.Ieee754,    result[FormatKeys.Pcm.DataType]);

            // Should not have speakers attribute as it can not be described by wave format ex
            Assert.AreEqual(false, result.ContainsKey(FormatKeys.Pcm.Speakers));
        }

        // non supported WAVEFORMATEX

        [Test]
        public void CanNotConvertPcmWaveFormatExToAudioFormat_nonSupported()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            var audioFormat =
                WaveFormat.CreateFormatEx
                (
                    WaveFormatTag.Unknown,
                    SampleRate.Khz96,
                    Depth.Bit64,
                    Speakers.Mono.ChannelCount(),
                    EndianBitConverter.Big
                );

            // -> ACT
            IAudioFormat result;
            var canConvert = fixture.TryConvert(audioFormat, out result);

            // -> ASSERT
            Assert.AreEqual(false, canConvert);
        }

        #endregion

        // WAVEFORMATEXTENSIBLE -> IAudioFormat

        #region Convert WAVEFORMATEXTENSIBLE stucts to Audio Format

        // WAVEFORMATEXTENSIBLE 16 Bit int / 44Khz / 5.1 / Little

        [Test]
        public void ConvertPcmWaveFormatExtensibleToAudioFormat_16Bit44Khz5Point1Little()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            var audioFormat =
                WaveFormat.CreateFormatExtensible
                (
                    AudioMediaSubType.Pcm,
                    SampleRate.Khz44,
                    Depth.Bit16,
                    Speakers.Surround5Point1,
                    EndianBitConverter.Little
                );

            // -> ACT
            IAudioFormat result;
            var canConvert = fixture.TryConvert(audioFormat, out result);

            // -> ASSERT
            Assert.AreEqual(true, canConvert);

            Assert.AreEqual(Endianness.Little,        result[FormatKeys.Endianness]);
            Assert.AreEqual(FormatKeys.Pcm.Format,    result[FormatKeys.Encoding]);

            Assert.AreEqual(16 /* Bits */ ,           result[FormatKeys.Pcm.Depth]);
            Assert.AreEqual(6  /* Channels */ ,       result[FormatKeys.Pcm.Channels]);
            Assert.AreEqual(44100  /* Hz */ ,         result[FormatKeys.Pcm.SampleRate]);
            Assert.AreEqual(PcmDataType.Int,          result[FormatKeys.Pcm.DataType]);
            Assert.AreEqual(Speakers.Surround5Point1, result[FormatKeys.Pcm.Speakers]);
        }

        // WAVEFORMATEXTENSIBLE 8 Bit int / 44Khz / 5.1 / Big

        [Test]
        public void ConvertPcmWaveFormatExtensibleToAudioFormat_8Bit44Khz5Point1Little()
        {
                       // -> ARRANGE:
            var fixture = GetTestFixture();

            var audioFormat =
                WaveFormat.CreateFormatExtensible
                (
                    AudioMediaSubType.Pcm,
                    SampleRate.Khz44,
                    Depth.Bit8,
                    Speakers.Surround5Point1,
                    EndianBitConverter.Little
                );

            // -> ACT
            IAudioFormat result;
            var canConvert = fixture.TryConvert(audioFormat, out result);

            // -> ASSERT
            Assert.AreEqual(true, canConvert);

            Assert.AreEqual(Endianness.Little,        result[FormatKeys.Endianness]);
            Assert.AreEqual(FormatKeys.Pcm.Format,    result[FormatKeys.Encoding]);

            Assert.AreEqual(8  /* Bits */ ,           result[FormatKeys.Pcm.Depth]);
            Assert.AreEqual(6  /* Channels */ ,       result[FormatKeys.Pcm.Channels]);
            Assert.AreEqual(44100  /* Hz */ ,         result[FormatKeys.Pcm.SampleRate]);
            Assert.AreEqual(PcmDataType.Int,          result[FormatKeys.Pcm.DataType]);
            Assert.AreEqual(Speakers.Surround5Point1, result[FormatKeys.Pcm.Speakers]);
        }


        // WAVEFORMATEXTENSIBLE 16 Bit float / 44Khz / 5.1 / Little

        [Test]
        public void ConvertPcmWaveFormatExtensibleToAudioFormat_16BitFloat44Khz5Point1Little()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            var audioFormat =
                WaveFormat.CreateFormatExtensible
                (
                    AudioMediaSubType.IeeeFloat,
                    SampleRate.Khz44,
                    Depth.Bit16,
                    Speakers.Surround5Point1,
                    EndianBitConverter.Little
                );

            // -> ACT
            IAudioFormat result;
            var canConvert = fixture.TryConvert(audioFormat, out result);

            // -> ASSERT
            Assert.AreEqual(true, canConvert);

            Assert.AreEqual(Endianness.Little,        result[FormatKeys.Endianness]);
            Assert.AreEqual(FormatKeys.Pcm.Format,    result[FormatKeys.Encoding]);

            Assert.AreEqual(16 /* Bits */ ,           result[FormatKeys.Pcm.Depth]);
            Assert.AreEqual(6  /* Channels */ ,       result[FormatKeys.Pcm.Channels]);
            Assert.AreEqual(44100  /* Hz */ ,         result[FormatKeys.Pcm.SampleRate]);
            Assert.AreEqual(PcmDataType.Ieee754,      result[FormatKeys.Pcm.DataType]);
            Assert.AreEqual(Speakers.Surround5Point1, result[FormatKeys.Pcm.Speakers]);
        }

        // WAVEFORMATEXTENSIBLE 64 Bit float / 44Khz / 5.1 / big

        [Test]
        public void ConvertPcmWaveFormatExtensibleToAudioFormat_64BitFloat44Khz5Point1Big()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            var audioFormat =
                WaveFormat.CreateFormatExtensible
                (
                    AudioMediaSubType.IeeeFloat,
                    SampleRate.Khz44,
                    Depth.Bit64,
                    Speakers.Surround5Point1,
                    EndianBitConverter.Big
                );

            // -> ACT
            IAudioFormat result;
            var canConvert = fixture.TryConvert(audioFormat, out result);

            // -> ASSERT
            Assert.AreEqual(true, canConvert);

            Assert.AreEqual(Endianness.Big,             result[FormatKeys.Endianness]);
            Assert.AreEqual(FormatKeys.Pcm.Format,      result[FormatKeys.Encoding]);

            Assert.AreEqual(64 /* Bits */ ,             result[FormatKeys.Pcm.Depth]);
            Assert.AreEqual(6  /* Channels */ ,         result[FormatKeys.Pcm.Channels]);
            Assert.AreEqual(44100  /* Hz */ ,           result[FormatKeys.Pcm.SampleRate]);
            Assert.AreEqual(PcmDataType.Ieee754,        result[FormatKeys.Pcm.DataType]);
            Assert.AreEqual(Speakers.Surround5Point1,   result[FormatKeys.Pcm.Speakers]);
        }

        // non supported WAVEFORMATEXTENSIBLE

        [Test]
        public void CanNotConvertPcmWaveFormatExtensibleToAudioFormat_nonSupported()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            var audioFormat =
                WaveFormat.CreateFormatExtensible
                (
                    new Guid("EE27FA75-5B33-468D-AB76-B56DD2775D7B"),  // Random guid
                    SampleRate.Khz44,
                    Depth.Bit16,
                    Speakers.Surround5Point1,
                    EndianBitConverter.Little
                );

            // -> ACT
            IAudioFormat result;
            var canConvert = fixture.TryConvert(audioFormat, out result);

            // -> ASSERT
            Assert.AreEqual(false, canConvert);
        }

        #endregion


        // IAudioFormat -> WAVEFORMATEX 

        #region Convert Audio Format to WAVEFORMATEX

        // Audio Format 16 Bit int / 44Khz / Stereo / Little

        [Test]
        public void ConvertPcmAudioFormatToWaveFormatEx_16Bit44KhzStereoLittle()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();
            var format = new AudioFormat
            {
                { FormatKeys.Endianness,     Endianness.Little},
                { FormatKeys.Encoding,       FormatKeys.Pcm.Format},
                { FormatKeys.Pcm.Depth,      Depth.Bit16},
                { FormatKeys.Pcm.SampleRate, SampleRate.Khz44},
                { FormatKeys.Pcm.Channels,   Speakers.Stereo.ChannelCount()},
                { FormatKeys.Pcm.DataType,   PcmDataType.Int},
            };

            var expectedBlockAlign     = format.Value<int>(FormatKeys.Pcm.Channels) * format.Value<int>(FormatKeys.Pcm.Depth) / 8;
            var expectedAvgBytesPerSec = expectedBlockAlign * format.Value<int>(FormatKeys.Pcm.SampleRate);

            // -> ACT

            WaveFormat result;
            var canConvert = fixture.TryConvert(format, out result);

            // -> ASSERT
            Assert.AreEqual(true, canConvert);

            Assert.AreEqual(EndianBitConverter.Little,  result.BitConverter);
            Assert.AreEqual(16    /* Bits */ ,          result.BitsPerSample);
            Assert.AreEqual(2     /* Channels */ ,      result.Channels);
            Assert.AreEqual(44100 /* Hz */ ,            result.SamplesPerSec);
            Assert.AreEqual(WaveFormatTag.Pcm,          result.FormatTag);

            Assert.AreEqual(expectedBlockAlign,         result.BlockAlign);
            Assert.AreEqual(expectedAvgBytesPerSec,     result.AvgBytesPerSec);

            // Make sure the result is not of type WaveFormatExtensible
            Assert.IsNotAssignableFrom<WaveFormatExtensible>(result);
        }


        // Audio Format 32 Bit int / 88Khz / Mono / Big

        [Test]
        public void ConvertPcmAudioFormatToWaveFormatEx_32Bit88KhzMonoBig()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();
            var format = new AudioFormat
            {
                { FormatKeys.Endianness,     Endianness.Big},
                { FormatKeys.Encoding,       FormatKeys.Pcm.Format},
                { FormatKeys.Pcm.Depth,      Depth.Bit32},
                { FormatKeys.Pcm.SampleRate, SampleRate.Khz88},
                { FormatKeys.Pcm.Channels,   Speakers.Mono.ChannelCount()},
                { FormatKeys.Pcm.DataType,   PcmDataType.Int},
            };

            var expectedBlockAlign    = format.Value<int>(FormatKeys.Pcm.Channels) * format.Value<int>(FormatKeys.Pcm.Depth) / 8;
            var expectedAvgBytesPerSec = expectedBlockAlign * format.Value<int>(FormatKeys.Pcm.SampleRate);

            // -> ACT

            WaveFormat result;
            var canConvert = fixture.TryConvert(format, out result);

            // -> ASSERT
            Assert.AreEqual(true, canConvert);

            Assert.AreEqual(EndianBitConverter.Big,     result.BitConverter);
            Assert.AreEqual(32    /* Bits */ ,          result.BitsPerSample);
            Assert.AreEqual(1     /* Channels */ ,      result.Channels);
            Assert.AreEqual(88200 /* Hz */ ,            result.SamplesPerSec);
            Assert.AreEqual(WaveFormatTag.Pcm,          result.FormatTag);

            Assert.AreEqual(expectedBlockAlign,     result.BlockAlign);
            Assert.AreEqual(expectedAvgBytesPerSec, result.AvgBytesPerSec);

            // Make sure the result is not of type WaveFormatExtensible
            Assert.IsNotAssignableFrom<WaveFormatExtensible>(result);
        }


        // Audio Format 16 Bit Float / 44Khz / Stereo / Little

        [Test]
        public void ConvertPcmAudioFormatToWaveFormatEx_16BitFloat44KhzStereoLittle()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();
            var format = new AudioFormat
            {
                { FormatKeys.Endianness,     Endianness.Little},
                { FormatKeys.Encoding,       FormatKeys.Pcm.Format},
                { FormatKeys.Pcm.Depth,      Depth.Bit16},
                { FormatKeys.Pcm.SampleRate, SampleRate.Khz44},
                { FormatKeys.Pcm.Channels,   Speakers.Stereo.ChannelCount()},
                { FormatKeys.Pcm.DataType,   PcmDataType.Ieee754},
            };

            var expectedBlockAlign = format.Value<int>(FormatKeys.Pcm.Channels) * format.Value<int>(FormatKeys.Pcm.Depth) / 8;
            var expectedAvgBytesPerSec = expectedBlockAlign * format.Value<int>(FormatKeys.Pcm.SampleRate);

            // -> ACT

            WaveFormat result;
            var canConvert = fixture.TryConvert(format, out result);

            // -> ASSERT
            Assert.AreEqual(true, canConvert);

            Assert.AreEqual(EndianBitConverter.Little,  result.BitConverter);
            Assert.AreEqual(16    /* Bits */ ,          result.BitsPerSample);
            Assert.AreEqual(2     /* Channels */ ,      result.Channels);
            Assert.AreEqual(44100 /* Hz */ ,            result.SamplesPerSec);
            Assert.AreEqual(WaveFormatTag.IeeeFloat,    result.FormatTag);

            Assert.AreEqual(expectedBlockAlign, result.BlockAlign);
            Assert.AreEqual(expectedAvgBytesPerSec, result.AvgBytesPerSec);

            // Make sure the result is not of type WaveFormatExtensible
            Assert.IsNotAssignableFrom<WaveFormatExtensible>(result);
        }

         // Audio Format 16 Bit Float / 8Khz / Stereo / Big

        [Test]
        public void ConvertPcmAudioFormatToWaveFormatEx_16BitFloat8KhzStereoBig()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();
            var format = new AudioFormat
            {
                { FormatKeys.Endianness,     Endianness.Big},
                { FormatKeys.Encoding,       FormatKeys.Pcm.Format},
                { FormatKeys.Pcm.Depth,      Depth.Bit16},
                { FormatKeys.Pcm.SampleRate, SampleRate.Khz8},
                { FormatKeys.Pcm.Channels,   Speakers.Stereo.ChannelCount()},
                { FormatKeys.Pcm.DataType,   PcmDataType.Ieee754},
            };

            var expectedBlockAlign = format.Value<int>(FormatKeys.Pcm.Channels) * format.Value<int>(FormatKeys.Pcm.Depth) / 8;
            var expectedAvgBytesPerSec = expectedBlockAlign * format.Value<int>(FormatKeys.Pcm.SampleRate);

            // -> ACT

            WaveFormat result;
            var canConvert = fixture.TryConvert(format, out result);

            // -> ASSERT
            Assert.AreEqual(true, canConvert);

            Assert.AreEqual(EndianBitConverter.Big,     result.BitConverter);
            Assert.AreEqual(16    /* Bits */ ,          result.BitsPerSample);
            Assert.AreEqual(2     /* Channels */ ,      result.Channels);
            Assert.AreEqual(8000 /* Hz */ ,             result.SamplesPerSec);
            Assert.AreEqual(WaveFormatTag.IeeeFloat,    result.FormatTag);

            Assert.AreEqual(expectedBlockAlign, result.BlockAlign);
            Assert.AreEqual(expectedAvgBytesPerSec, result.AvgBytesPerSec);

            // Make sure the result is not of type WaveFormatExtensible
            Assert.IsNotAssignableFrom<WaveFormatExtensible>(result);
        }

        #endregion

        #region Convert Audio Format to WAVEFORMATEXTENSIBLE

        // Audio Format 16 Bit int / 44Khz / 5.1 / Little

        [Test]
        public void ConvertPcmAudioFormatToWaveFormatEx_16Bit44Khz5Point1Little()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();
            var format = new AudioFormat
            {
                { FormatKeys.Endianness,     Endianness.Little},
                { FormatKeys.Encoding,       FormatKeys.Pcm.Format},
                { FormatKeys.Pcm.Depth,      Depth.Bit16},
                { FormatKeys.Pcm.Packing,    Depth.Bit32},
                { FormatKeys.Pcm.SampleRate, SampleRate.Khz44},
                { FormatKeys.Pcm.Channels,   Speakers.Surround5Point1.ChannelCount()},
                { FormatKeys.Pcm.Speakers,   Speakers.Surround5Point1},
                { FormatKeys.Pcm.DataType,   PcmDataType.Int},
            };

            var expectedBlockAlign     = format.Value<int>(FormatKeys.Pcm.Channels) * format.Value<int>(FormatKeys.Pcm.Packing) / 8;
            var expectedAvgBytesPerSec = expectedBlockAlign * format.Value<int>(FormatKeys.Pcm.SampleRate);

            // -> ACT

            WaveFormat result;
            var canConvert = fixture.TryConvert(format, out result);

            // -> ASSERT
            Assert.AreEqual(true, canConvert);

            Assert.AreEqual(EndianBitConverter.Little,  result.BitConverter);
            Assert.AreEqual(32    /* Bits */ ,          result.BitsPerSample);
            Assert.AreEqual(6     /* Channels */ ,      result.Channels);
            Assert.AreEqual(44100 /* Hz */ ,            result.SamplesPerSec);
            Assert.AreEqual(WaveFormatTag.Extensible,   result.FormatTag);

            Assert.AreEqual(expectedBlockAlign, result.BlockAlign);
            Assert.AreEqual(expectedAvgBytesPerSec, result.AvgBytesPerSec);

            // Make sure the result is of type WaveFormatExtensible
            Assert.IsAssignableFrom<WaveFormatExtensible>(result);
            var resultExensible = (WaveFormatExtensible) result;

            Assert.AreEqual(Speakers.Surround5Point1, resultExensible.ChannelMask);
            Assert.AreEqual(16,                       resultExensible.ValidBitsPerSample);
            Assert.AreEqual(AudioMediaSubType.Pcm,    resultExensible.SubFormat);
        }

         // Audio Format 16 Bit int / 44Khz / 5.1 / Big

        [Test]
        public void ConvertPcmAudioFormatToWaveFormatEx_16Bit44Khz5Point1Big()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();
            var format = new AudioFormat
            {
                { FormatKeys.Endianness,     Endianness.Big},
                { FormatKeys.Encoding,       FormatKeys.Pcm.Format},
                { FormatKeys.Pcm.Depth,      Depth.Bit16},
                { FormatKeys.Pcm.Packing,    Depth.Bit16},
                { FormatKeys.Pcm.SampleRate, SampleRate.Khz44},
                { FormatKeys.Pcm.Channels,   Speakers.Surround5Point1.ChannelCount()},
                { FormatKeys.Pcm.Speakers,   Speakers.Surround5Point1},
                { FormatKeys.Pcm.DataType,   PcmDataType.Int},
            };

            var expectedBlockAlign = format.Value<int>(FormatKeys.Pcm.Channels) * format.Value<int>(FormatKeys.Pcm.Packing) / 8;
            var expectedAvgBytesPerSec = expectedBlockAlign * format.Value<int>(FormatKeys.Pcm.SampleRate);

            // -> ACT

            WaveFormat result;
            var canConvert = fixture.TryConvert(format, out result);

            // -> ASSERT
            Assert.AreEqual(true, canConvert);

            Assert.AreEqual(EndianBitConverter.Big,     result.BitConverter);
            Assert.AreEqual(16    /* Bits */ ,          result.BitsPerSample);
            Assert.AreEqual(6     /* Channels */ ,      result.Channels);
            Assert.AreEqual(44100 /* Hz */ ,            result.SamplesPerSec);
            Assert.AreEqual(WaveFormatTag.Extensible,   result.FormatTag);

            Assert.AreEqual(expectedBlockAlign, result.BlockAlign);
            Assert.AreEqual(expectedAvgBytesPerSec, result.AvgBytesPerSec);

            // Make sure the result is of type WaveFormatExtensible
            Assert.IsAssignableFrom<WaveFormatExtensible>(result);
            var resultExensible = (WaveFormatExtensible) result;

            Assert.AreEqual(Speakers.Surround5Point1, resultExensible.ChannelMask);
            Assert.AreEqual(16,                       resultExensible.ValidBitsPerSample);
            Assert.AreEqual(AudioMediaSubType.Pcm,    resultExensible.SubFormat);
        }


        // Audio Format 32 Bit float / 44Khz / 7.1 / Little

        [Test]
        public void ConvertPcmAudioFormatToWaveFormatEx_32BitFloat44Khz7Point1Little()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();
            var format = new AudioFormat
            {
                { FormatKeys.Endianness,     Endianness.Little},
                { FormatKeys.Encoding,       FormatKeys.Pcm.Format},
                { FormatKeys.Pcm.Depth,      Depth.Bit32},
                { FormatKeys.Pcm.Packing,    Depth.Bit64},
                { FormatKeys.Pcm.SampleRate, SampleRate.Khz44},
                { FormatKeys.Pcm.Channels,   Speakers.Surround7Point1.ChannelCount()},
                { FormatKeys.Pcm.Speakers,   Speakers.Surround7Point1},
                { FormatKeys.Pcm.DataType,   PcmDataType.Ieee754},
            };

            var expectedBlockAlign = format.Value<int>(FormatKeys.Pcm.Channels) * format.Value<int>(FormatKeys.Pcm.Packing) / 8;
            var expectedAvgBytesPerSec = expectedBlockAlign * format.Value<int>(FormatKeys.Pcm.SampleRate);

            // -> ACT

            WaveFormat result;
            var canConvert = fixture.TryConvert(format, out result);

            // -> ASSERT
            Assert.AreEqual(true, canConvert);

            Assert.AreEqual(EndianBitConverter.Little,  result.BitConverter);
            Assert.AreEqual(64    /* Bits */ ,          result.BitsPerSample);
            Assert.AreEqual(8     /* Channels */ ,      result.Channels);
            Assert.AreEqual(44100 /* Hz */ ,            result.SamplesPerSec);
            Assert.AreEqual(WaveFormatTag.Extensible,   result.FormatTag);

            Assert.AreEqual(expectedBlockAlign, result.BlockAlign);
            Assert.AreEqual(expectedAvgBytesPerSec, result.AvgBytesPerSec);

            // Make sure the result is of type WaveFormatExtensible
            Assert.IsAssignableFrom<WaveFormatExtensible>(result);
            var resultExensible = (WaveFormatExtensible) result;

            Assert.AreEqual(Speakers.Surround7Point1,       resultExensible.ChannelMask);
            Assert.AreEqual(32,                             resultExensible.ValidBitsPerSample);
            Assert.AreEqual(AudioMediaSubType.IeeeFloat,    resultExensible.SubFormat);
        }

        // Audio Format 64 Bit float / 44Khz / 7.1 / Big

        [Test]
        public void ConvertPcmAudioFormatToWaveFormatEx_32BitFloat44Khz7Point1Big()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();
            var format = new AudioFormat
            {
                { FormatKeys.Endianness,     Endianness.Big},
                { FormatKeys.Encoding,       FormatKeys.Pcm.Format},
                { FormatKeys.Pcm.Depth,      Depth.Bit64},
                { FormatKeys.Pcm.Packing,    Depth.Bit64},
                { FormatKeys.Pcm.SampleRate, SampleRate.Khz44},
                { FormatKeys.Pcm.Channels,   Speakers.Surround7Point1.ChannelCount()},
                { FormatKeys.Pcm.Speakers,   Speakers.Surround7Point1},
                { FormatKeys.Pcm.DataType,   PcmDataType.Ieee754},
            };

            var expectedBlockAlign = format.Value<int>(FormatKeys.Pcm.Channels) * format.Value<int>(FormatKeys.Pcm.Packing) / 8;
            var expectedAvgBytesPerSec = expectedBlockAlign * format.Value<int>(FormatKeys.Pcm.SampleRate);

            // -> ACT

            WaveFormat result;
            var canConvert = fixture.TryConvert(format, out result);

            // -> ASSERT
            Assert.AreEqual(true, canConvert);

            Assert.AreEqual(EndianBitConverter.Big,     result.BitConverter);
            Assert.AreEqual(64    /* Bits */ ,          result.BitsPerSample);
            Assert.AreEqual(8     /* Channels */ ,      result.Channels);
            Assert.AreEqual(44100 /* Hz */ ,            result.SamplesPerSec);
            Assert.AreEqual(WaveFormatTag.Extensible,   result.FormatTag);

            Assert.AreEqual(expectedBlockAlign, result.BlockAlign);
            Assert.AreEqual(expectedAvgBytesPerSec, result.AvgBytesPerSec);

            // Make sure the result is of type WaveFormatExtensible
            Assert.IsAssignableFrom<WaveFormatExtensible>(result);
            var resultExensible = (WaveFormatExtensible) result;

            Assert.AreEqual(Speakers.Surround7Point1,       resultExensible.ChannelMask);
            Assert.AreEqual(64,                             resultExensible.ValidBitsPerSample);
            Assert.AreEqual(AudioMediaSubType.IeeeFloat,    resultExensible.SubFormat);
        }

        #endregion

    }
}
