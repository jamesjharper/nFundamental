using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fundamental.Interface.Wasapi;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Win32;
using NSubstitute;
using NUnit.Framework;
using DeviceState = Fundamental.Interface.DeviceState;

namespace nFundamental.Interface.Wasapi.Tests
{
    [TestFixture]
    public class WasapiDeviceInfoTests
    {

        public IWasapiInterfaceNotifyClient WasapiInterfaceNotifyClient { get; set; }

        public Fundamental.Interface.Wasapi.Interop.IMMDevice ImmDevice { get; set; }

        public WasapiDeviceToken WasapiDeviceToken { get; } =
            new WasapiDeviceToken("A63439A9-5928-4BEC-99EC-F39B0414278B");

        [SetUp]
        public void SetUp()
        {
            ImmDevice = Substitute.For<Fundamental.Interface.Wasapi.Interop.IMMDevice>();
            WasapiInterfaceNotifyClient = Substitute.For<IWasapiInterfaceNotifyClient>();
        }

        private WasapiDeviceInfo GetTestFixture()
            => new WasapiDeviceInfo(WasapiInterfaceNotifyClient, WasapiDeviceToken, ImmDevice);


        [Test]
        public void CanGetDeviceToken()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            // -> ACT
            var deviceToken = fixture.DeviceToken;

            // -> ASSERT
            Assert.AreEqual(WasapiDeviceToken, deviceToken);
        }

        [Test]
        public void CanGetDeviceState()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            // Expect a call to IMMDevice.GetState
            Fundamental.Interface.Wasapi.Interop.DeviceState deviceStateOut;
            ImmDevice
                .GetState(out deviceStateOut)
                .Returns(param =>
                {
                    param[0] = Fundamental.Interface.Wasapi.Interop.DeviceState.Disabled;
                    return HResult.S_OK;
                });

            // -> ACT
            var deviceState = fixture.DeviceState;

            // -> ASSERT
            Assert.AreEqual(DeviceState.Disabled, deviceState);
        }

        [Test]
        public void CanGetDeviceProperties()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            var propertyStoreFixture = Substitute.For<IPropertyStore>();

            // Expect a call to IMMDevice.GetState
            IPropertyStore propertyStoreOut;
            ImmDevice
                .OpenPropertyStore(StorageAccess.Read, out propertyStoreOut)
                .Returns(param =>
                {
                    param[1] = propertyStoreFixture;
                    return HResult.S_OK;
                });

            // -> ACT
            var properties = fixture.Properties;

            // -> ASSERT
            Assert.AreEqual(propertyStoreFixture, properties.PropertyStore);
        }
    }
}
