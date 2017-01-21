using Fundamental.Interface;
using Fundamental.Interface.Wasapi;
using Fundamental.Interface.Wasapi.Interop;
using NSubstitute;
using NUnit.Framework;

namespace nFundamental.Interface.Wasapi.Tests
{
    [TestFixture]
    public class WasapiInterfaceProviderTests
    {

        public class WasapiInterfaceProviderTestFixture : WasapiInterfaceProvider
        {
            protected override IMMDeviceEnumerator FactoryWasapiDeviceEnumerator()
            {
                return Substitute.For<IMMDeviceEnumerator>();
            }

            protected override IWasapiDeviceTokenFactory FactoryDeviceTokenFactory()
            {
                return Substitute.For<IWasapiDeviceTokenFactory>();
            }
        }

        private static WasapiInterfaceProviderTestFixture GetTestFixture() => new WasapiInterfaceProviderTestFixture();


        [Test]
        public void CanFactoryKnowsItSupportsIDefaultDeviceProvider()
        {
            // -> ARRANGE:
            var factory = GetTestFixture();

            // -> ACT:
            var isSupported = factory.IsAudioInterfaceSupported<IDefaultDeviceProvider>();

            // -> ASSET:
            Assert.IsTrue( isSupported);
        }

        [Test]
        public void CanFactoryIDefaultDeviceProvider()
        {
            // -> ARRANGE:
            var factory = GetTestFixture();

            // -> ACT:
            var interfaceInstance = factory.GetAudioInterface<IDefaultDeviceProvider>();

            // -> ASSERT:
            Assert.IsNotNull(interfaceInstance);
        }

        [Test]
        public void CanFactoryKnowsItSupportsIDeviceEnumerator()
        {
            // -> ARRANGE:
            var factory = GetTestFixture();

            // -> ACT:
            var isSupported = factory.IsAudioInterfaceSupported<IDeviceEnumerator>();

            // -> ASSET:
            Assert.IsTrue(isSupported);
        }



        [Test]
        public void CanFactoryIDeviceEnumerator()
        {
            // -> ARRANGE:
            var factory = GetTestFixture();

            // -> ACT:
            var interfaceInstance = factory.GetAudioInterface<IDeviceEnumerator>();

            // -> ASSERT:
            Assert.IsNotNull(interfaceInstance);
        }
    }
}
