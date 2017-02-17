using System;
using NUnit.Framework;

using System.Collections.Generic;
using System.Linq;
using Fundamental.Interface.Wasapi.Internal;
using Fundamental.Core.AudioFormats;

namespace Fundamental.Interface.Wasapi.Tests.Internal
{
    [TestFixture]
    public class PcmLatencyCalculatorTests
    {
        [TestCase(/* At */ SampleRate.Khz96,  /* with frames */  2, /* expect */ 1920000u /* bytes */, /* To Be*/   10000 * TimeSpan.TicksPerMillisecond)]
        [TestCase(/* At */ SampleRate.Khz96,  /* with frames */  2, /* expect */ 192000u  /* bytes */,  /* To Be*/   1000 * TimeSpan.TicksPerMillisecond)]
        [TestCase(/* At */ SampleRate.Khz96,  /* with frames */  4, /* expect */ 192000u  /* bytes */,  /* To Be*/    500 * TimeSpan.TicksPerMillisecond)]
        [TestCase(/* At */ SampleRate.Khz96,  /* with frames */  2, /* expect */ 96000u   /* bytes */,  /* To Be*/    500 * TimeSpan.TicksPerMillisecond)]
        [TestCase(/* At */ SampleRate.Khz96,  /* with frames */  2, /* expect */ 960u     /* bytes */,  /* To Be*/      5 * TimeSpan.TicksPerMillisecond)]
        [TestCase(/* At */ SampleRate.Khz96,  /* with frames */  2, /* expect */ 96u      /* bytes */,  /* To Be*/   5000 /* Ticks */)]
        [TestCase(/* At */ SampleRate.Khz96,  /* with frames */  2, /* expect */ 96000u   /* bytes */,  /* To Be*/    500 * TimeSpan.TicksPerMillisecond)]
        [TestCase(/* At */ SampleRate.Khz8,   /* with frames */  2, /* expect */ 16000u   /* bytes */,  /* To Be*/   1000 * TimeSpan.TicksPerMillisecond)]
        [TestCase(/* At */ SampleRate.Khz8,   /* with frames */  2, /* expect */ 160u     /* bytes */,  /* To Be*/ 100000  /* Ticks */)]
        public void CanConvertBytesToLatency(int sampleRate, int frameSize, ulong numberOfBytes, long expectedTimeSpanTicks)
        {
            // -> ARRANGE:
            var expectedTimeSpan = TimeSpan.FromTicks(expectedTimeSpanTicks);
            var fixture = new PcmLatencyCalculator(frameSize, sampleRate);

            // -> ACT
            var lantency = fixture.BytesToLatency(numberOfBytes);

            // -> ASSERT
            Assert.AreEqual(expectedTimeSpan, lantency);
        }


        [TestCase(/* At */ SampleRate.Khz96,  /* with frames */  2, /* expect */ 1920000u /* samples */,  /* To Be */    20000 * TimeSpan.TicksPerMillisecond)]
        [TestCase(/* At */ SampleRate.Khz96,  /* with frames */  2, /* expect */ 192000u  /* samples */,  /* To Be */    2000 * TimeSpan.TicksPerMillisecond)]
        [TestCase(/* At */ SampleRate.Khz96,  /* with frames */  4, /* expect */ 192000u  /* samples */,  /* To Be */    2000 * TimeSpan.TicksPerMillisecond)]
        [TestCase(/* At */ SampleRate.Khz96,  /* with frames */  2, /* expect */ 96000u   /* samples */,  /* To Be */    1000 * TimeSpan.TicksPerMillisecond)]
        [TestCase(/* At */ SampleRate.Khz96,  /* with frames */  2, /* expect */ 960u     /* samples */,  /* To Be */      10 * TimeSpan.TicksPerMillisecond)]
        [TestCase(/* At */ SampleRate.Khz96,  /* with frames */  2, /* expect */ 96u      /* samples */,  /* To Be */   10000 /* Ticks */)]
        [TestCase(/* At */ SampleRate.Khz96,  /* with frames */  2, /* expect */ 96000u   /* samples */,  /* To Be */    1000 * TimeSpan.TicksPerMillisecond)]
        [TestCase(/* At */ SampleRate.Khz8,   /* with frames */  2, /* expect */ 16000u   /* samples */,  /* To Be */    2000 * TimeSpan.TicksPerMillisecond)]
        [TestCase(/* At */ SampleRate.Khz8,   /* with frames */  2, /* expect */ 160u     /* samples */,  /* To Be */  200000  /* Ticks */)]
        public void CanConvertSamplesToLatency(int sampleRate, int frameSize, ulong numberOfSamples, long expectedTimeSpanTicks)
        {
            // -> ARRANGE:
            var expectedTimeSpan = TimeSpan.FromTicks(expectedTimeSpanTicks);
            var fixture = new PcmLatencyCalculator(frameSize, sampleRate);

            // -> ACT
            var lantency = fixture.FramesToLatency(numberOfSamples);

            // -> ASSERT
            Assert.AreEqual(expectedTimeSpan, lantency);
        }

        [TestCase(/* At */ SampleRate.Khz96,  /* with frames */  2,  /* expect */   20000 * TimeSpan.TicksPerMillisecond, /* To Be */  1920000u /* samples */)]
        [TestCase(/* At */ SampleRate.Khz96,  /* with frames */  2,  /* expect */    2000 * TimeSpan.TicksPerMillisecond, /* To Be */  192000u  /* samples */)]
        [TestCase(/* At */ SampleRate.Khz96,  /* with frames */  4,  /* expect */    2000 * TimeSpan.TicksPerMillisecond, /* To Be */  192000u  /* samples */)]
        [TestCase(/* At */ SampleRate.Khz96,  /* with frames */  2,  /* expect */    1000 * TimeSpan.TicksPerMillisecond, /* To Be */  96000u   /* samples */)]
        [TestCase(/* At */ SampleRate.Khz96,  /* with frames */  2,  /* expect */      10 * TimeSpan.TicksPerMillisecond, /* To Be */  960u     /* samples */)]
        [TestCase(/* At */ SampleRate.Khz96,  /* with frames */  2,  /* expect */   10000 /* Ticks */                   , /* To Be */  96u      /* samples */)]
        [TestCase(/* At */ SampleRate.Khz96,  /* with frames */  2,  /* expect */    1000 * TimeSpan.TicksPerMillisecond, /* To Be */  96000u   /* samples */)]
        [TestCase(/* At */ SampleRate.Khz8,   /* with frames */  2,  /* expect */    2000 * TimeSpan.TicksPerMillisecond, /* To Be */  16000u   /* samples */)]
        [TestCase(/* At */ SampleRate.Khz8,   /* with frames */  2,  /* expect */  200000  /* Ticks */                  , /* To Be */  160u     /* samples */)]
        public void CanConvertLatencyToSamples(int sampleRate, int frameSize, long timeSpanTicks, ulong expectedNumberOfSamples)
        {
            // -> ARRANGE:
            var timeSpan = TimeSpan.FromTicks(timeSpanTicks);
            var fixture = new PcmLatencyCalculator(frameSize, sampleRate);

            // -> ACT
            var sampleCount = fixture.LatencyToFrames(timeSpan);

            // -> ASSERT
            Assert.AreEqual(expectedNumberOfSamples, sampleCount);
        }
    }
}
