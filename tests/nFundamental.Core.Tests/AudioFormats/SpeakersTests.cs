
using Fundamental.Core.AudioFormats;
using NUnit.Framework;

namespace Fundamental.Core.Tests.AudioFormats
{
    [TestFixture]
    public class SpeakersTests
    {


        [Test]
        public void CanDetectTheNumberOfChannelsInMono()
        {
            // -> ARRANGE:
           var speakerConfig = Speakers.Mono;

           // -> ACT
           var numberOfChannels = speakerConfig.ChannelCount();

           // -> ASSERT
            Assert.AreEqual(1, numberOfChannels);
        }

        [Test]
        public void CanDetectTheNumberOfChannelsInStereo()
        {
            // -> ARRANGE:
           var speakerConfig = Speakers.Stereo;

           // -> ACT
           var numberOfChannels = speakerConfig.ChannelCount();

           // -> ASSERT
            Assert.AreEqual(2, numberOfChannels);
        }  

  [Test]
        public void CanDetectTheNumberOfChannelsInQuad()
        {
            // -> ARRANGE:
           var speakerConfig = Speakers.Quad;

           // -> ACT
           var numberOfChannels = speakerConfig.ChannelCount();

           // -> ASSERT
            Assert.AreEqual(4, numberOfChannels);
        }

        [Test]
        public void CanDetectTheNumberOfChannelsInSurround()
        {
            // -> ARRANGE:
           var speakerConfig = Speakers.Surround;

           // -> ACT
           var numberOfChannels = speakerConfig.ChannelCount();

           // -> ASSERT
            Assert.AreEqual(4, numberOfChannels);
        }

        [Test]
        public void CanDetectTheNumberOfChannelsInSurround5Point1()
        {
            // -> ARRANGE:
           var speakerConfig = Speakers.Surround5Point1;

           // -> ACT
           var numberOfChannels = speakerConfig.ChannelCount();

           // -> ASSERT
            Assert.AreEqual(6, numberOfChannels);
        }

        [Test]
        public void CanDetectTheNumberOfChannelsInSurround7Point1()
        {
            // -> ARRANGE:
           var speakerConfig = Speakers.Surround7Point1;

           // -> ACT
           var numberOfChannels = speakerConfig.ChannelCount();

           // -> ASSERT
            Assert.AreEqual(8, numberOfChannels);
        }
    }
}
