using Fundamental.Interface;
using Fundamental.Interface.Wasapi;
using Fundamental.Interface.Wasapi.Internal;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Win32;
using NSubstitute;
using NUnit.Framework;

namespace nFundamental.Interface.Wasapi.Tests
{
    [TestFixture]
    public class WasapiDefaultDeviceProviderTests
    {
        private IMMDeviceEnumerator ImmDeviceEnumeratorTestFixture { get; set; }
        private IWasapiDeviceTokenFactory WasapiDeviceTokenFactoryTestFixture { get; set; }

        [SetUp]
        public void SetUp()
        {
            ImmDeviceEnumeratorTestFixture = Substitute.For<IMMDeviceEnumerator>();
            WasapiDeviceTokenFactoryTestFixture = Substitute.For<IWasapiDeviceTokenFactory>();
        }


        private WasapiDefaultDeviceProvider GetTestFixture() => new WasapiDefaultDeviceProvider(ImmDeviceEnumeratorTestFixture, WasapiDeviceTokenFactoryTestFixture);


        [Test]
        public void CanGetDefaultCapatureDevice()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            // The device returned from the COM interface
            var immDevice = Substitute.For<IMMDevice>();

            // The expected token to be returned from the token factory
            var expectedToken = new WasapiDeviceToken("A3923E50-80C1-420B-BC33-7B8427E448B7", immDevice);

            // Call to GetDefaultAudioEndpoint returns mock IMMDevice
            IMMDevice outImmDevice;
            ImmDeviceEnumeratorTestFixture
                .GetDefaultAudioEndpoint(DataFlow.Capture, Role.Console, out outImmDevice)
                .Returns(
                    param =>
                    {
                        param[2] = immDevice;
                        return HResult.S_OK;
                    });

            // Call to WasapiDeviceTokenFactory.GetToken returns dummy token
            WasapiDeviceTokenFactoryTestFixture
                .GetToken(immDevice)
                .Returns(expectedToken);


            // -> ACT:
            var handle = fixture.GetDefaultDevice(DeviceType.Capture, DeviceRole.Console);

            // -> ASSERT
            Assert.AreEqual(expectedToken , handle);
        }

        [Test]
        public void CanGetDefaultRenderDevice()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();


            // The device returned from the COM interface
            var immDevice = Substitute.For<IMMDevice>();

            // The expected token to be returned from the token factory
            var expectedToken = new WasapiDeviceToken("A3923E50-80C1-420B-BC33-7B8427E448B7", immDevice);

            // Call to GetDefaultAudioEndpoint returns mock IMMDevice
            IMMDevice outImmDevice;
            ImmDeviceEnumeratorTestFixture
                .GetDefaultAudioEndpoint(DataFlow.Render, Role.Communications, out outImmDevice)
                .Returns(
                    param =>
                    {
                        param[2] = immDevice;
                        return HResult.S_OK;
                    });

            // Call to WasapiDeviceTokenFactory.GetToken returns dummy token
            WasapiDeviceTokenFactoryTestFixture
                .GetToken(immDevice)
                .Returns(expectedToken);


            // -> ACT:
            var handle = fixture.GetDefaultDevice(DeviceType.Render, DeviceRole.Communications);

            // -> ASSERT
            Assert.AreEqual(expectedToken, handle);
        }

    }
}
