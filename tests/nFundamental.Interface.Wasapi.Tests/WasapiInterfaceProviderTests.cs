using Fundamental.Interface.Wasapi.Internal;
using Fundamental.Interface.Wasapi.Interop;
using NSubstitute;
using NUnit.Framework;

namespace Fundamental.Interface.Wasapi.Tests
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
            var isSupported = factory.IsSupported<IDefaultDeviceProvider>();

            // -> ASSET:
            Assert.IsTrue( isSupported);
        }

        [Test]
        public void CanFactoryIDefaultDeviceProvider()
        {
            // -> ARRANGE:
            var factory = GetTestFixture();

            // -> ACT:
            var interfaceInstance = factory.Get<IDefaultDeviceProvider>();

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
            var isSupported = factory.IsSupported<IDeviceEnumerator>();

            // -> ASSET:
            Assert.IsTrue(isSupported);
        }

        [Test]
        public void CanFactoryIDeviceEnumerator()
        {
            // -> ARRANGE:
            var factory = GetTestFixture();

            // -> ACT:
            var interfaceInstance = factory.Get<IDeviceEnumerator>();

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
            var isSupported = factory.IsSupported<IDefaultDeviceStatusNotifier>();

            // -> ASSET:
            Assert.IsTrue(isSupported);
        }

        [Test]
        public void CanFactoryIDefaultDeviceStatusNotifier()
        {
            // -> ARRANGE:
            var factory = GetTestFixture();

            // -> ACT:
            var interfaceInstance = factory.Get<IDefaultDeviceStatusNotifier>();

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
            var isSupported = factory.IsSupported<IDeviceAvailabilityNotifier>();

            // -> ASSET:
            Assert.IsTrue(isSupported);
        }

        [Test]
        public void CanFactoryIDeviceAvailabilityNotifier()
        {
            // -> ARRANGE:
            var factory = GetTestFixture();

            // -> ACT:
            var interfaceInstance = factory.Get<IDeviceAvailabilityNotifier>();

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
            var isSupported = factory.IsSupported<IDeviceStatusNotifier>();

            // -> ASSET:
            Assert.IsTrue(isSupported);
        }

        [Test]
        public void CanFactoryIDeviceStatusNotifier()
        {
            // -> ARRANGE:
            var factory = GetTestFixture();

            // -> ACT:
            var interfaceInstance = factory.Get<IDeviceStatusNotifier>();

            // -> ASSERT:
            factory.ImmDeviceEnumerator.RegisterEndpointNotificationCallback((IMMNotificationClient)interfaceInstance);
            Assert.IsNotNull(interfaceInstance);
        }

        #endregion

        #region IDeviceInfoFactory support

        [Test]
        public void CanFactoryKnowsItSupportsIDeviceInfoFactory()
        {
            // -> ARRANGE:
            var factory = GetTestFixture();

            // -> ACT:
            var isSupported = factory.IsSupported<IDeviceInfoFactory>();

            // -> ASSET:
            Assert.IsTrue(isSupported);
        }

        [Test]
        public void CanFactoryIDeviceInfoFactory()
        {
            // -> ARRANGE:
            var factory = GetTestFixture();

            // -> ACT:
            var interfaceInstance = factory.Get<IDeviceInfoFactory>();

            // -> ASSERT:
            Assert.IsNotNull(interfaceInstance);
        }

        #endregion
    }
}
