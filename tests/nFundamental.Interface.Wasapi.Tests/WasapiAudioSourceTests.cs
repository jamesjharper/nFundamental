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
    public class WasapiAudioSourceTests
    {

        public class WasapiAudioSourceTestFixture : WasapiAudioSource
        {
            public WasapiAudioSourceTestFixture
                (
                    IDeviceToken wasapiDeviceToken,
                    IDeviceInfo deviceInfoFixture,
                    IOptions<WasapiOptions> wasapiOptions,
                    IWasapiAudioClientInteropFactory wasapiAudioClientInteropFactory,
                    IWasapiAudioClientInterop wasapiAudioClientInterop
                )
                : base(wasapiDeviceToken, deviceInfoFixture, wasapiOptions, wasapiAudioClientInteropFactory)
            {
                AudioClientInterop = wasapiAudioClientInterop;
            }

            protected override IWasapiAudioClientInterop AudioClientInterop { get; }

            public IAudioFormat DesiredAudioFormatOverride
            {
                set { DesiredAudioFormat = value; }
            }

            public IWasapiAudioClientInterop ActualAudioClientInterop => base.AudioClientInterop;
        }


        private Options<WasapiOptions> WasapiOptionsFixture { get; set; }

        private IDeviceToken DeviceTokenFixture { get; set; }

        private IWasapiAudioClientInteropFactory WasapiAudioClientInteropFactoryFixture { get; set; }

        private IWasapiAudioClientInterop WasapiAudioClientInteropFixture { get; set; }

        private IDeviceInfo DeviceInfoFixture { get; set; }

        [SetUp]
        public void SetUp()
        {
            WasapiOptionsFixture = Options<WasapiOptions>.Default;

            DeviceTokenFixture = Substitute.For<IDeviceToken>();
            WasapiAudioClientInteropFactoryFixture = Substitute.For<IWasapiAudioClientInteropFactory>();
            WasapiAudioClientInteropFixture = Substitute.For<IWasapiAudioClientInterop>();
            DeviceInfoFixture = Substitute.For<IDeviceInfo>();
        }

        private WasapiAudioSourceTestFixture GetTestFixture()
            => new WasapiAudioSourceTestFixture(DeviceTokenFixture, DeviceInfoFixture, WasapiOptionsFixture, WasapiAudioClientInteropFactoryFixture, WasapiAudioClientInteropFixture);

        //#region Factory Audio Client tests
 
        //[Test]
        //public void CanFactoryAudioClient()
        //{
        //    // -> ARRANGE:
        //    var fixture = GetTestFixture();

        //    var expectedAudioClient = Substitute.For<IWasapiAudioClientInterop>();

        //    WasapiAudioClientInteropFactoryFixture
        //        .FactoryAudioClient(DeviceTokenFixture)
        //        .Returns(expectedAudioClient);

        //    // -> ACT:
        //    var audioClient = fixture.ActualAudioClientInterop;

        //    // -> ASSERT
        //    Assert.AreEqual(expectedAudioClient, audioClient);
        //}

        //#endregion

    }
}
