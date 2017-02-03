using Fundamental.Interface.Wasapi.Internal;
using Fundamental.Interface.Wasapi.Interop;
using NSubstitute;
using NUnit.Framework;

namespace Fundamental.Interface.Wasapi.Tests.Internal
{
    [TestFixture]
    public class WasapiInterfaceNotifyClientTests
    {
        private IWasapiDeviceTokenFactory WasapiDeviceTokenFactoryTestFixture { get; set; }

        [SetUp]
        public void SetUp()
        {
            WasapiDeviceTokenFactoryTestFixture = Substitute.For<IWasapiDeviceTokenFactory>();
        }

        private WasapiInterfaceNotifyClient GetTestFixture() => new WasapiInterfaceNotifyClient(WasapiDeviceTokenFactoryTestFixture);

        [Test]
        public void CanRaiseDeviceStatusChangedEvent()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();
            var expectedDeviceId = "B36C8273-EB04-4131-9EE0-EF5E2FE51BC8";
            var exepectedToken = new WasapiDeviceToken(expectedDeviceId, Substitute.For<IMMDevice>());

            DeviceStatusChangedEvent resultAgs = null;
            fixture.DeviceStatusChanged += (sender, args) => { resultAgs = args;  };

            // We expect a call to the token factory to get a new token from the id
            WasapiDeviceTokenFactoryTestFixture
                .GetToken(expectedDeviceId)
                .Returns(exepectedToken);

            // -> ACT
            ((IMMNotificationClient) fixture)
                .OnDeviceStateChanged(expectedDeviceId, Fundamental.Interface.Wasapi.Interop.DeviceState.Disabled);

            // -> ASSERT
            Assert.AreEqual(resultAgs?.DeviceToken, exepectedToken);
            Assert.AreEqual(resultAgs?.DeviceState, DeviceState.Disabled);
        }

        [Test]
        public void CanRaiseDefaultDeviceChangedEvent()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();
            var expectedDeviceId = "115977BC-AB6A-4892-9CD6-CE2C6B287109";
            var exepectedToken = new WasapiDeviceToken(expectedDeviceId, Substitute.For<IMMDevice>());

            DefaultDeviceChangedEventArgs resultAgs = null;
            fixture.DefaultDeviceChanged += (sender, args) => { resultAgs = args; };

            // We expect a call to the token factory to get a new token from the id
            WasapiDeviceTokenFactoryTestFixture
                .GetToken(expectedDeviceId)
                .Returns(exepectedToken);


            // -> ACT
            ((IMMNotificationClient)fixture)
                .OnDefaultDeviceChanged(DataFlow.Capture, Role.Console, expectedDeviceId);

            // -> ASSERT
            Assert.AreEqual(resultAgs?.DeviceToken, exepectedToken);
            Assert.AreEqual(resultAgs?.DeviceRole, DeviceRole.Console);
            Assert.AreEqual(resultAgs?.DeviceType, DeviceType.Capture);
        }

        [Test]
        public void CanRaiseOnDeviceAddedEvent()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();
            var expectedDeviceId = "115977BC-AB6A-4892-9CD6-CE2C6B287109";
            var exepectedToken = new WasapiDeviceToken(expectedDeviceId, Substitute.For<IMMDevice>());

            DeviceAddedEventArgs resultAgs = null;
            fixture.DeviceAdded += (sender, args) => { resultAgs = args; };

            // We expect a call to the token factory to get a new token from the id
            WasapiDeviceTokenFactoryTestFixture
                .GetToken(expectedDeviceId)
                .Returns(exepectedToken);


            // -> ACT
            ((IMMNotificationClient)fixture).OnDeviceAdded(expectedDeviceId);

            // -> ASSERT
            Assert.AreEqual(resultAgs?.DeviceToken, exepectedToken);
        }

        [Test]
        public void CanRaiseOnDeviceRemovedEvent()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();
            var expectedDeviceId = "115977BC-AB6A-4892-9CD6-CE2C6B287109";
            var exepectedToken = new WasapiDeviceToken(expectedDeviceId, Substitute.For<IMMDevice>());

            DeviceRemovedEventArgs resultAgs = null;
            fixture.DeviceRemoved += (sender, args) => { resultAgs = args; };

            // We expect a call to the token factory to get a new token from the id
            WasapiDeviceTokenFactoryTestFixture
                .GetToken(expectedDeviceId)
                .Returns(exepectedToken);


            // -> ACT
            ((IMMNotificationClient)fixture).OnDeviceRemoved(expectedDeviceId);

            // -> ASSERT
            Assert.AreEqual(resultAgs?.DeviceToken, exepectedToken);
        }
    }
}
