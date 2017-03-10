using System;
using Fundamental.Core;
using Fundamental.Wave.Format;
using Fundamental.Interface.Wasapi.Internal;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Win32;
using NSubstitute;
using NUnit.Framework;

namespace Fundamental.Interface.Wasapi.Tests.Internal
{
    [TestFixture]
    public class WasapiAudioClientFactoryTests
    {
        
        private IMMDevice ImmDevice { get; set; }


        private IAudioFormatConverter<WaveFormat> AudioFormatConverterWaveFormatFixture { get; set; }

        private IComThreadInteropStrategy ComThreadInterpoStrategyFixture { get; set; }




        [SetUp]
        public void SetUp()
        {
            ImmDevice = Substitute.For<IMMDevice>();
            AudioFormatConverterWaveFormatFixture = Substitute.For<IAudioFormatConverter<WaveFormat>>();
            ComThreadInterpoStrategyFixture = Substitute.For<IComThreadInteropStrategy>();
        }

        private WasapiAudioClientInteropFactory GetTestFixture() => new WasapiAudioClientInteropFactory(ComThreadInterpoStrategyFixture, AudioFormatConverterWaveFormatFixture);


        [Test]
        public void CanCreateAudioClientInstance()
        {
            // -> ARRANGE:
            var factory = GetTestFixture();

            // Expected returned COM instance
            var expectedAudioClient = Substitute.For<IAudioClient>();

            var token = new WasapiDeviceToken("72348FA2-4420-4E81-964F-8FF1D7C00881", ImmDevice);

            // Expect a call to ask if we need to be creating this object was any other thread 
            // The answer is ....No
            ComThreadInterpoStrategyFixture.RequiresInvoke().Returns(false);

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
            var asWasapiAudioClient = audioClient as WasapiAudioClientInterop;


            Assert.AreEqual(expectedAudioClient, asWasapiAudioClient.ComInstance);
        }


        [Test]
        public void CanCreateAudioClientInstanceUsingThreadStrategy()
        {
            // -> ARRANGE:
            var factory = GetTestFixture();

            // Expected returned COM instance
            var expectedAudioClient = Substitute.For<IAudioClient>();

            var token = new WasapiDeviceToken("72348FA2-4420-4E81-964F-8FF1D7C00881", ImmDevice);

            // Expect a call to ask if we need to be creating this object was any 
            // particular thread.
            // The answer is ....yes!
            ComThreadInterpoStrategyFixture
                .RequiresInvoke()
                .Returns
                (
                    /* First time called */ true,
                    /* Second time called */ false
                );

            // Expect a call to dispatch to a different thread 

            ComThreadInterpoStrategyFixture
                .InvokeOnTargetThread(Arg.Any<Delegate>(), Arg.Any<object[]>())
                .Returns((p) =>
                {
                    // Internally we just call the method 
                    var method = (Delegate)p[0];
                    var args = (object[])p[1];
                    return method.DynamicInvoke(args);
                });


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
            var asWasapiAudioClient = audioClient as WasapiAudioClientInterop;


            Assert.AreEqual(expectedAudioClient, asWasapiAudioClient.ComInstance);
        }
    }
}
