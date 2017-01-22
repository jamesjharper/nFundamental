using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fundamental.Interface;
using Fundamental.Interface.Wasapi;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Win32;
using NSubstitute;
using NUnit.Framework;

namespace nFundamental.Interface.Wasapi.Tests
{

    [TestFixture]
    public class WasapiDeviceInfoFactoryTests
    {

        private IMMDeviceEnumerator ImmDeviceEnumeratorTestFixture { get; set; }

        private IWasapiInterfaceNotifyClient WasapiInterfaceNotifyClient { get; set; }

        [SetUp]
        public void SetUp()
        {
            WasapiInterfaceNotifyClient = Substitute.For<IWasapiInterfaceNotifyClient>();
            ImmDeviceEnumeratorTestFixture = Substitute.For<IMMDeviceEnumerator>();
        }

        private WasapiDeviceInfoFactory GetTestFixture() 
            => new WasapiDeviceInfoFactory(WasapiInterfaceNotifyClient, ImmDeviceEnumeratorTestFixture);



        [Test]
        public void CanGetDeviceInfo()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();
            var expectedDeviceId = "115977BC-AB6A-4892-9CD6-CE2C6B287109";
            var inputToken = new WasapiDeviceToken(expectedDeviceId);

            // Expect a call to ImmDeviceEnumerator
            IMMDevice emptyDevice;
            ImmDeviceEnumeratorTestFixture
                .GetDevice(expectedDeviceId, out emptyDevice)
                .Returns((param) =>
                {
                    param[1] = Substitute.For<IMMDevice>();
                    return HResult.S_OK;
                });

            // -> ACT
            var deviceInfo = fixture.GetInfoDevice(inputToken);

            // -> ASSERT
            Assert.AreEqual(expectedDeviceId, deviceInfo.DeviceToken.Id);
        }
    }
}
