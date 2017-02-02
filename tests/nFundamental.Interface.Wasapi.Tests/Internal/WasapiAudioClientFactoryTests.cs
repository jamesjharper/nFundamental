using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Fundamental.Interface.Wasapi;
using Fundamental.Interface.Wasapi.Internal;
using Fundamental.Interface.Wasapi.Win32;
using Fundamental.Interface.Wasapi.Interop;

namespace nFundamental.Interface.Wasapi.Tests.Internal
{
    [TestFixture]
    public class WasapiAudioClientFactoryTests
    {
        private WasapiAudioClientFactory GetTestFixture() => new WasapiAudioClientFactory();

        private IMMDevice ImmDevice { get; set; }
        

        [SetUp]
        public void SetUp()
        {
            ImmDevice = Substitute.For<IMMDevice>();
        }

        [Test]
        public void CanCreateAudioClientInstance()
        {
            // -> ARRANGE:
            var factory = GetTestFixture();

            // Expected returned COM instance
            var expectedAudioClient = Substitute.For<IAudioClient>();

            var token = new WasapiDeviceToken("72348FA2-4420-4E81-964F-8FF1D7C00881", ImmDevice);

            // Expect a call to activate the IAudioClient Instance
            object outObject;
            ImmDevice.Activate(
                /* Com interface requested    */ IIds.IAudioClientGuid,
                /* Com Server type            */ ClsCtx.LocalServer,
                /* Activation Flags           */ IntPtr.Zero,
                /* [out] resulting com object */ out outObject)
                .Returns((param) =>
                {
                    param[3] = expectedAudioClient;
                    return HResult.S_OK;
                });


            // -> ACT
            var audioClient = factory.FactoryAudioClient(token);

            // -> ASSERT
            Assert.AreEqual(expectedAudioClient, audioClient);
        }
    }
}
