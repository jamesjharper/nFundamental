using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fundamental.Core;
using Fundamental.Core.AudioFormats;
using Fundamental.Core.Tests.AudioFormats;
using Fundamental.Interface.Wasapi.Internal;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Options;
using NSubstitute;
using NUnit.Framework;

namespace Fundamental.Interface.Wasapi.Tests
{
    [TestFixture]
    public class WasapiAudioClientTests
    {

        public class WasapiAudioSourceTestFixture : WasapiAudioClient
        {
            public WasapiAudioSourceTestFixture
                (
                    IDeviceToken wasapiDeviceToken,
                    IDeviceInfo deviceInfo,
                    IWasapiAudioClientInteropFactory wasapiAudioClientInteropFactory,
                    IWasapiAudioClientInterop wasapiAudioClientInterop
                )
                : base(wasapiDeviceToken, deviceInfo, wasapiAudioClientInteropFactory)
            {
                AudioClientInterop = wasapiAudioClientInterop;
            }

            protected override IWasapiAudioClientInterop AudioClientInterop { get; }

            public IAudioFormat DesiredAudioFormatOverride
            {
                set { DesiredAudioFormat = value; }
            }

            protected override AudioClientShareMode DeviceAccessMode => WasapiDeviceAccessOverride;

            public AudioClientShareMode WasapiDeviceAccessOverride { get; set; }

            public IWasapiAudioClientInterop ActualAudioClientInterop => base.AudioClientInterop;

            protected override TimeSpan ManualSyncLatency => ManualSyncLatencyOverride;

            public TimeSpan ManualSyncLatencyOverride { get; }

            protected override bool UseHardwareSync => UseHardwareSyncOverride;

            protected bool UseHardwareSyncOverride { get; set; }

            protected override bool PreferDeviceNativeFormat => PreferDeviceNativeFormatOverride;

            protected bool PreferDeviceNativeFormatOverride { get; set; }


            protected override void HardwareSyncAudioPump() { }

            protected override void ManualSyncAudioPump() { }
        }


        private IDeviceToken DeviceTokenFixture { get; set; }

        private IWasapiAudioClientInteropFactory WasapiAudioClientInteropFactoryFixture { get; set; }

        private IWasapiAudioClientInterop WasapiAudioClientInteropFixture { get; set; }

        private IDeviceInfo DeviceInfoFixture { get; set; }

        [SetUp]
        public void SetUp()
        {
            DeviceTokenFixture = Substitute.For<IDeviceToken>();
            WasapiAudioClientInteropFactoryFixture = Substitute.For<IWasapiAudioClientInteropFactory>();
            WasapiAudioClientInteropFixture = Substitute.For<IWasapiAudioClientInterop>();
            DeviceInfoFixture = Substitute.For<IDeviceInfo>();
        }

        private WasapiAudioSourceTestFixture GetTestFixture()
            => new WasapiAudioSourceTestFixture(DeviceTokenFixture, DeviceInfoFixture, WasapiAudioClientInteropFactoryFixture, WasapiAudioClientInteropFixture);

        #region Factory Audio Client tests

        [Test]
        public void CanFactoryAudioClient()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            var expectedAudioClient = Substitute.For<IWasapiAudioClientInterop>();

            WasapiAudioClientInteropFactoryFixture
                .FactoryAudioClient(DeviceTokenFixture)
                .Returns(expectedAudioClient);

            // -> ACT:
            var audioClient = fixture.ActualAudioClientInterop;

            // -> ASSERT
            Assert.AreEqual(expectedAudioClient, audioClient);
        }

        #endregion

        #region  Is Format Supported tests

        [Test]
        public void CanDetechSupportedFormatWhenUsingSharedMode()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            var format = AudioFormatHelper.Pcm16Bit44KhzMonoBig;
            var accessmode = AudioClientShareMode.Shared;

            IAudioFormat outFormat; // We don't care what the output is
            WasapiAudioClientInteropFixture
                .IsFormatSupported(accessmode, format, out outFormat)
                .Returns(true);

            // -> ACT:

            // Make sure the setting is set to Shared mode
            fixture.WasapiDeviceAccessOverride = accessmode;

            var isAudioFormatSupported = fixture.IsAudioFormatSupported(format);

            // -> ASSERT
            Assert.AreEqual(true, isAudioFormatSupported);

        }


        [Test]
        public void CanDetechSupportedFormatWhenUsingExclusiveMode()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            var format = AudioFormatHelper.Pcm32BitFloat96KhzSurround5Point1Big;
            var accessmode = AudioClientShareMode.Exclusive;

            IAudioFormat outFormat; // We don't care what the output is
            WasapiAudioClientInteropFixture
                .IsFormatSupported(accessmode, format, out outFormat)
                .Returns(true);

            // -> ACT:

            // Make sure the setting is set to Shared mode
            fixture.WasapiDeviceAccessOverride = accessmode;

            var isAudioFormatSupported = fixture.IsAudioFormatSupported(format);

            // -> ASSERT
            Assert.AreEqual(true, isAudioFormatSupported);

        }


        [Test]
        public void CanDetechNonSupportedFormat()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            var format = AudioFormatHelper.Pcm16Bit44KhzMonoLittle;

            IAudioFormat outFormat; // We don't care what the output is
            WasapiAudioClientInteropFixture
                .IsFormatSupported(Arg.Any<AudioClientShareMode>(), format, out outFormat)
                .Returns(false);

            // -> ACT:

            // Make sure the setting is set to Shared mode
            var isAudioFormatSupported = fixture.IsAudioFormatSupported(format);

            // -> ASSERT
            Assert.AreEqual(false, isAudioFormatSupported);
        }


        [Test]
        public void CanGetClosestMatchFormatsForNonSupportedFormatWhenUsingSharedMode()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            var format = AudioFormatHelper.Pcm16Bit44KhzMonoBig;
            var suggestedFormat = AudioFormatHelper.Pcm16Bit44KhzMonoLittle;
            var accessmode = AudioClientShareMode.Shared;

            IAudioFormat outFormat;
            WasapiAudioClientInteropFixture
                .IsFormatSupported(accessmode, format, out outFormat)
                .Returns((p) =>
                {
                    p[2] = suggestedFormat;
                    return false;
                });

            // -> ACT:

            // Make sure the setting is set to Shared mode
            fixture.WasapiDeviceAccessOverride = accessmode;

            IEnumerable<IAudioFormat> outSuggestions;
            var isAudioFormatSupported = fixture.IsAudioFormatSupported(format, out outSuggestions);
            var closestMatchingFormats = outSuggestions.ToList();

            // -> ASSERT
            Assert.AreEqual(false, isAudioFormatSupported);

            Assert.AreEqual(1, closestMatchingFormats.Count);
            Assert.Contains(suggestedFormat, closestMatchingFormats);
        }


        [Test]
        public void CanGetClosestMatchFormatsForNonSupportedFormatWhenUsingExclusiveMode()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            var format = AudioFormatHelper.Pcm16Bit44KhzMonoBig;
            var suggestedFormat = AudioFormatHelper.Pcm16Bit44KhzMonoLittle;
            var accessmode = AudioClientShareMode.Exclusive;

            IAudioFormat outFormat;
            WasapiAudioClientInteropFixture
                .IsFormatSupported(accessmode, format, out outFormat)
                .Returns((p) =>
                {
                    p[2] = suggestedFormat;
                    return false;
                });

            // -> ACT:

            // Make sure the setting is set to Shared mode
            fixture.WasapiDeviceAccessOverride = accessmode;

            IEnumerable<IAudioFormat> outSuggestions;
            var isAudioFormatSupported = fixture.IsAudioFormatSupported(format, out outSuggestions);
            var closestMatchingFormats = outSuggestions.ToList();

            // -> ASSERT
            Assert.AreEqual(false, isAudioFormatSupported);

            Assert.AreEqual(1, closestMatchingFormats.Count);
            Assert.Contains(suggestedFormat, closestMatchingFormats);
        }



        [Test]
        public void CanGetClosestMatchFormatsForSupportedFormat()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            var format = AudioFormatHelper.Pcm16Bit44KhzMonoBig;
            var suggestedFormat = AudioFormatHelper.Pcm16Bit44KhzMonoLittle;


            IAudioFormat outFormat;
            WasapiAudioClientInteropFixture
                .IsFormatSupported(Arg.Any<AudioClientShareMode>(), format, out outFormat)
                .Returns(true);

            // -> ACT:

            // Make sure the setting is set to Shared mode
            IEnumerable<IAudioFormat> outSuggestions;
            var isAudioFormatSupported = fixture.IsAudioFormatSupported(format, out outSuggestions);
            var closestMatchingFormats = outSuggestions.ToList();

            // -> ASSERT
            Assert.AreEqual(true, isAudioFormatSupported);

            Assert.AreEqual(0, closestMatchingFormats.Count);
        }

        #endregion

        #region  Suggest Format

        [Test]
        public void SuggestSupportedFormatWhenUsingExclusiveModeWithSupportedMixerFormat()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            var format = AudioFormatHelper.Pcm16Bit44KhzMonoBig;
            var mixerFormat = AudioFormatHelper.Pcm32BitFloat96KhzSurround5Point1Big;
            var accessmode = AudioClientShareMode.Exclusive;

            // Expect a call to get the mixer format
            WasapiAudioClientInteropFixture
                .GetMixFormat()
                .Returns(mixerFormat);

            // Expect a call to check that is format is supported in the current mode 
            IAudioFormat outFormat;
            WasapiAudioClientInteropFixture
                .IsFormatSupported(accessmode, mixerFormat, out outFormat)
                .Returns((p) =>
                {
                    return true;
                });

            // -> ACT:

            // Make sure the setting is set to Shared mode
            fixture.WasapiDeviceAccessOverride = accessmode;

            IEnumerable<IAudioFormat> outSuggestions = fixture.SuggestFormats(format);
            var closestMatchingFormats = outSuggestions.ToList();

            // -> ASSERT
            Assert.AreEqual(1, closestMatchingFormats.Count);
            Assert.Contains(mixerFormat, closestMatchingFormats);
        }


        [Test]
        public void SuggestSupportedFormatWhenUsingShardedModeWithSupportedMixerFormat()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            var format = AudioFormatHelper.Pcm16Bit44KhzMonoBig;
            var mixerFormat = AudioFormatHelper.Pcm32BitFloat96KhzSurround5Point1Big;
            var accessmode = AudioClientShareMode.Shared;

            // Expect a call to get the mixer format
            WasapiAudioClientInteropFixture
                .GetMixFormat()
                .Returns(mixerFormat);

            // Expect a call to check that is format is supported in the current mode 
            IAudioFormat outFormat;
            WasapiAudioClientInteropFixture
                .IsFormatSupported(accessmode, mixerFormat, out outFormat)
                .Returns((p) =>
                {
                    return true;
                });

            // -> ACT:

            // Make sure the setting is set to Shared mode
            fixture.WasapiDeviceAccessOverride = accessmode;

            IEnumerable<IAudioFormat> outSuggestions = fixture.SuggestFormats(format);
            var closestMatchingFormats = outSuggestions.ToList();

            // -> ASSERT
            Assert.AreEqual(1, closestMatchingFormats.Count);
            Assert.Contains(mixerFormat, closestMatchingFormats);
        }


        [Test]
        public void SuggestSupportedFormatWithNonSupportedMixerFormat()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            var format = AudioFormatHelper.Pcm16Bit44KhzMonoBig;

            var suggestedFormat = AudioFormatHelper.Pcm16Bit44KhzMonoBig;
            var mixerFormat = AudioFormatHelper.Pcm32BitFloat96KhzSurround5Point1Big;

            // Expect a call to get the mixer format
            WasapiAudioClientInteropFixture
                .GetMixFormat()
                .Returns(mixerFormat);

            // Epect a call to check that is format is supported in the current mode 
            IAudioFormat outFormat;
            WasapiAudioClientInteropFixture
                .IsFormatSupported(Arg.Any<AudioClientShareMode>(), mixerFormat, out outFormat)
                .Returns((p) =>
                {
                    p[2] = suggestedFormat;
                    return false;
                });

            // -> ACT:

            // Make sure the setting is set to Shared mode
            IEnumerable<IAudioFormat> outSuggestions = fixture.SuggestFormats(format);
            var closestMatchingFormats = outSuggestions.ToList();

            // -> ASSERT
            Assert.AreEqual(1, closestMatchingFormats.Count);
            Assert.Contains(suggestedFormat, closestMatchingFormats);
        }



        [Test]
        public void SuggestSupportedFormatWithIgnoreFormatParam()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            var format = AudioFormatHelper.Pcm16Bit44KhzMonoBig;
            var mixerFormat = AudioFormatHelper.Pcm32BitFloat96KhzSurround5Point1Big;
            var ignoreFormat = mixerFormat;


            // Expect a call to get the mixer format
            WasapiAudioClientInteropFixture
                .GetMixFormat()
                .Returns(mixerFormat);

            // Expect a call to check that is format is supported in the current mode 
            IAudioFormat outFormat;
            WasapiAudioClientInteropFixture
                .IsFormatSupported(Arg.Any<AudioClientShareMode>(), mixerFormat, out outFormat)
                .Returns(true);

            // -> ACT:

            // Make sure the setting is set to Shared mode
            IEnumerable<IAudioFormat> outSuggestions = fixture.SuggestFormats(format, ignoreFormat);
            var closestMatchingFormats = outSuggestions.ToList();

            // -> ASSERT
            Assert.AreEqual(0, closestMatchingFormats.Count); //
        }


        #endregion

        #region Set Format

        [Test]
        public void CanSetSupportedFormat()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            var desiredFormat = AudioFormatHelper.Pcm16Bit44KhzMonoBig;

            // Epect a call to check that is format is supported in the current mode 
            IAudioFormat outFormat;
            WasapiAudioClientInteropFixture
                .IsFormatSupported(Arg.Any<AudioClientShareMode>(), desiredFormat, out outFormat)
                .Returns(true);

            // -> ACT:

            fixture.SetFormat(desiredFormat);

            // -> ASSERT

            // All good if no exceptions thrown
        }


        [Test]
        public void ShouldThrowExceptionWhenSettingUnsupportedFormat()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            var desiredFormat = AudioFormatHelper.Pcm16Bit44KhzMonoBig;

            // Epect a call to check that is format is supported in the current mode 
            IAudioFormat outFormat;
            WasapiAudioClientInteropFixture
                .IsFormatSupported(Arg.Any<AudioClientShareMode>(), desiredFormat, out outFormat)
                .Returns(false);

            // -> ACT:
            Assert.Throws<FormatNotSupportedException>(() => fixture.SetFormat(desiredFormat));
        }

        #endregion

        #region Get Format

        [Test]
        public void CanGetFormatWhenNoFormatHasBeenSet()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            var suggestedFormat = AudioFormatHelper.Pcm16Bit44KhzMonoBig;
            var mixerFormat = AudioFormatHelper.Pcm32BitFloat96KhzSurround5Point1Big;

            // As the format has never been set, we Expect the first suggested format to be resolve as the default format


            // Expect a call to get the mixer format
            WasapiAudioClientInteropFixture
                .GetMixFormat()
                .Returns(mixerFormat);

            // Expect a call to check that is format is supported in the current mode 
            IAudioFormat outFormat;
            WasapiAudioClientInteropFixture
                .IsFormatSupported(Arg.Any<AudioClientShareMode>(), mixerFormat, out outFormat)
                .Returns((p) =>
                {
                    p[2] = suggestedFormat;
                    return false;
                });

            // -> ACT:

            var format = fixture.GetFormat();

            // -> ASSERT
            Assert.AreEqual(suggestedFormat, format);
        }

        [Test]
        public void CanGetFormatWhenFormatHasBeenSet()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            // Hack in the value, as if it had be set naturally 
            var setFormat = AudioFormatHelper.Pcm16Bit44KhzMonoBig;
            fixture.DesiredAudioFormatOverride = setFormat;

            // -> ACT:
            var format = fixture.GetFormat();

            // -> ASSERT
            Assert.AreEqual(setFormat, format);
        }

        [Test]
        public void CanGetDefaultFormat()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            var suggestedFormat = AudioFormatHelper.Pcm16Bit44KhzMonoBig;
            var mixerFormat = AudioFormatHelper.Pcm32BitFloat96KhzSurround5Point1Big;

            // As the format has never been set, we Expect the first suggested format to be resolve as the default format


            // Expect a call to get the mixer format
            WasapiAudioClientInteropFixture
                .GetMixFormat()
                .Returns(mixerFormat);

            // Expect a call to check that is format is supported in the current mode 
            IAudioFormat outFormat;
            WasapiAudioClientInteropFixture
                .IsFormatSupported(Arg.Any<AudioClientShareMode>(), mixerFormat, out outFormat)
                .Returns((p) =>
                {
                    p[2] = suggestedFormat;
                    return false;
                });

            // -> ACT:
            var format = fixture.GetDefaultFormat();

            // -> ASSERT
            Assert.AreEqual(suggestedFormat, format);
        }



        #endregion

    }
}
