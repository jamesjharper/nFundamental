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
            public IWasapiDeviceTokenFactory WasapiDeviceTokenFactory { get;  } = Substitute.For<IWasapiDeviceTokenFactory>();
            public IMMDeviceEnumerator ImmDeviceEnumerator { get; } = Substitute.For<IMMDeviceEnumerator>();

            protected override IMMDeviceEnumerator FactoryWasapiDeviceEnumerator()
            {
                return ImmDeviceEnumerator;
            }

            protected override IWasapiDeviceTokenFactory FactoryDeviceTokenFactory()
            {
                return WasapiDeviceTokenFactory;
            }
        }

        private static WasapiInterfaceProviderTestFixture GetTestFixture() => new WasapiInterfaceProviderTestFixture();


        #region IDefaultDeviceProvider support

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

        #endregion

        #region IDeviceEnumerator support

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


        #endregion

        #region IDefaultDeviceStatusNotifier support

        [Test]
        public void CanFactoryKnowsItSupportsIDefaultDeviceStatusNotifier()
        {
            // -> ARRANGE:
            var factory = GetTestFixture();

            // -> ACT:
            var isSupported = factory.IsAudioInterfaceSupported<IDefaultDeviceStatusNotifier>();

            // -> ASSET:
            Assert.IsTrue(isSupported);
        }

        [Test]
        public void CanFactoryIDefaultDeviceStatusNotifier()
        {
            // -> ARRANGE:
            var factory = GetTestFixture();

            // -> ACT:
            var interfaceInstance = factory.GetAudioInterface<IDefaultDeviceStatusNotifier>();

            // -> ASSERT:
            factory.ImmDeviceEnumerator.RegisterEndpointNotificationCallback((IMMNotificationClient)interfaceInstance);
            Assert.IsNotNull(interfaceInstance);
        }


        #endregion

        #region IDeviceAvailabilityNotifier support

        [Test]
        public void CanFactoryKnowsItSupportsIDeviceAvailabilityNotifier()
        {
            // -> ARRANGE:
            var factory = GetTestFixture();

            // -> ACT:
            var isSupported = factory.IsAudioInterfaceSupported<IDeviceAvailabilityNotifier>();

            // -> ASSET:
            Assert.IsTrue(isSupported);
        }

        [Test]
        public void CanFactoryIDeviceAvailabilityNotifier()
        {
            // -> ARRANGE:
            var factory = GetTestFixture();

            // -> ACT:
            var interfaceInstance = factory.GetAudioInterface<IDeviceAvailabilityNotifier>();

            // -> ASSERT:
            factory.ImmDeviceEnumerator.RegisterEndpointNotificationCallback((IMMNotificationClient)interfaceInstance);
            Assert.IsNotNull(interfaceInstance);
        }

        #endregion

        #region IDeviceStatusNotifier support

        [Test]
        public void CanFactoryKnowsItSupportsIDeviceStatusNotifier()
        {
            // -> ARRANGE:
            var factory = GetTestFixture();

            // -> ACT:
            var isSupported = factory.IsAudioInterfaceSupported<IDeviceStatusNotifier>();

            // -> ASSET:
            Assert.IsTrue(isSupported);
        }

        [Test]
        public void CanFactoryIDeviceStatusNotifier()
        {
            // -> ARRANGE:
            var factory = GetTestFixture();

            // -> ACT:
            var interfaceInstance = factory.GetAudioInterface<IDeviceStatusNotifier>();

            // -> ASSERT:
            factory.ImmDeviceEnumerator.RegisterEndpointNotificationCallback((IMMNotificationClient)interfaceInstance);
            Assert.IsNotNull(interfaceInstance);
        }

        #endregion
    }
}
