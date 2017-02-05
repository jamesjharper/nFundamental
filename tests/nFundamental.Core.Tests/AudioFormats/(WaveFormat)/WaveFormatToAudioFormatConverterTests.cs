using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fundamental.Core.AudioFormats;
using MiscUtil.Conversion;
using NUnit.Framework;

namespace Fundamental.Core.Tests.AudioFormats
{
    [TestFixture]
    public class WaveFormatToAudioFormatConverterTests
    {

        private WaveFormatToAudioFormatConverter GetTestFixture()
            => new WaveFormatToAudioFormatConverter();


        [Test]
        public void GetConvertPcmWaveFormatExToAudioFormat()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();


            var pcmWaveFormatEx = WaveFormat.CreateFormatEx
                (
                    WaveFormatTag.Pcm,
                    SampleRate.Khz44,
                    Depth.Bit16,
                    Speakers.Stereo.ChannelCount()
            );

            // -> ACT

            IAudioFormat result;
            var canConvert = fixture.TryConvert(pcmWaveFormatEx, out result);


            // -> ASSERT

        }
    }
}
