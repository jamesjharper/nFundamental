using Fundamental.Core.AudioFormats;

namespace Fundamental.Core.Tests.AudioFormats
{
    public static class AudioFormatHelper
    {

        public static AudioFormat Pcm16Bit44KhzMonoLittle
            => new AudioFormat
            {
                {FormatKeys.Endianness, Endianness.Little},
                {FormatKeys.Encoding, FormatKeys.Pcm.Format},
                {FormatKeys.Pcm.Depth, Depth.Bit16},
                {FormatKeys.Pcm.Packing, Depth.Bit16},
                {FormatKeys.Pcm.SampleRate, SampleRate.Khz44},
                {FormatKeys.Pcm.Channels, Speakers.Mono.ChannelCount()},
                {FormatKeys.Pcm.Speakers, Speakers.Mono},
                {FormatKeys.Pcm.DataType, PcmDataType.Int},
            };

        public static AudioFormat Pcm16Bit44KhzMonoBig
            => new AudioFormat
            {
                {FormatKeys.Endianness, Endianness.Big},
                {FormatKeys.Encoding, FormatKeys.Pcm.Format},
                {FormatKeys.Pcm.Depth, Depth.Bit16},
                {FormatKeys.Pcm.Packing, Depth.Bit16},
                {FormatKeys.Pcm.SampleRate, SampleRate.Khz44},
                {FormatKeys.Pcm.Channels, Speakers.Mono.ChannelCount()},
                {FormatKeys.Pcm.Speakers, Speakers.Mono},
                {FormatKeys.Pcm.DataType, PcmDataType.Int},
            };


        public static AudioFormat Pcm32BitFloat96KhzSurround5Point1Little
            => new AudioFormat
            {
                {FormatKeys.Endianness, Endianness.Little},
                {FormatKeys.Encoding, FormatKeys.Pcm.Format},
                {FormatKeys.Pcm.Depth, Depth.Bit16},
                {FormatKeys.Pcm.Packing, Depth.Bit16},
                {FormatKeys.Pcm.SampleRate, SampleRate.Khz96},
                {FormatKeys.Pcm.Channels, Speakers.Surround5Point1.ChannelCount()},
                {FormatKeys.Pcm.Speakers, Speakers.Surround5Point1},
                {FormatKeys.Pcm.DataType, PcmDataType.Ieee754},
            };

        public static AudioFormat Pcm32BitFloat96KhzSurround5Point1Big
            => new AudioFormat
            {
                {FormatKeys.Endianness, Endianness.Big},
                {FormatKeys.Encoding, FormatKeys.Pcm.Format},
                {FormatKeys.Pcm.Depth, Depth.Bit16},
                {FormatKeys.Pcm.Packing, Depth.Bit16},
                {FormatKeys.Pcm.SampleRate, SampleRate.Khz96},
                {FormatKeys.Pcm.Channels, Speakers.Surround5Point1.ChannelCount()},
                {FormatKeys.Pcm.Speakers, Speakers.Surround5Point1},
                {FormatKeys.Pcm.DataType, PcmDataType.Ieee754},
            };
    }
}
