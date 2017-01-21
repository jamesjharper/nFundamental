using System.Linq;
using Fundamental.Interface;
using Fundamental.Interface.Wasapi;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Win32;
using NSubstitute;
using NUnit.Framework;
using DeviceState = Fundamental.Interface.DeviceState;

namespace nFundamental.Interface.Wasapi.Tests
{

    [TestFixture]
    public class WasapiDeviceEnumeratorTests
    {
        private IMMDeviceEnumerator ImmDeviceEnumeratorTestFixture { get; set; }
        private IWasapiDeviceTokenFactory WasapiDeviceTokenFactoryTestFixture { get; set; }

        [SetUp]
        public void SetUp()
        {
            ImmDeviceEnumeratorTestFixture = Substitute.For<IMMDeviceEnumerator>();
            WasapiDeviceTokenFactoryTestFixture = Substitute.For<IWasapiDeviceTokenFactory>();
        }

        private WasapiDeviceEnumerator GetTestFixture() => new WasapiDeviceEnumerator(ImmDeviceEnumeratorTestFixture, WasapiDeviceTokenFactoryTestFixture);


        [Test]
        public void CanGetAEmptyListOfAllDevices()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            // The device returned from the COM interface
            var immDeviceCollection = Substitute.For<IMMDeviceCollection>();

            // Call to GetDefaultAudioEndpoint returns mock IMMDevice
            IMMDeviceCollection outImmDeviceCollection;
            ImmDeviceEnumeratorTestFixture
                .EnumAudioEndpoints(DataFlow.All, Fundamental.Interface.Wasapi.Interop.DeviceState.All, out outImmDeviceCollection)
                .Returns(
                    param =>
                    {
                        param[2] = immDeviceCollection;
                        return HResult.S_OK;
                    });


            // The Device Collection should be called to find the device count
            int outCount;
            immDeviceCollection
                .GetCount(out outCount)
                .Returns(
                    param =>
                    {
                        param[0] = 0; // Count of 0
                        return HResult.S_OK;
                    });

            // -> ACT
            var result = fixture.GetDevices();

            // -> ASSERT
            Assert.IsEmpty(result);

        }

        [Test]
        public void CanGetAEmptyListOfRenderingDevices()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            // The device returned from the COM interface
            var immDeviceCollection = Substitute.For<IMMDeviceCollection>();


            // Call to GetDefaultAudioEndpoint returns mock IMMDevice
            IMMDeviceCollection outImmDeviceCollection;
            ImmDeviceEnumeratorTestFixture
                .EnumAudioEndpoints(DataFlow.Render, Fundamental.Interface.Wasapi.Interop.DeviceState.All, out outImmDeviceCollection)
                .Returns(
                    param =>
                    {
                        param[2] = immDeviceCollection;
                        return HResult.S_OK;
                    });

            // The Device Collection should be called to find the device count
            int outCount;
            immDeviceCollection
                .GetCount(out outCount)
                .Returns(
                    param =>
                    {
                        param[0] = 0; // Count of 0
                        return HResult.S_OK;
                    });

            // -> ACT
            var result = fixture.GetDevices(DeviceType.Render);

            // -> ASSERT
            Assert.IsEmpty(result);

        }


        [Test]
        public void CanGetAEmptyListOfCapatureDevices()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            // The device returned from the COM interface
            var immDeviceCollection = Substitute.For<IMMDeviceCollection>();

            // Call to GetDefaultAudioEndpoint returns mock IMMDevice
            IMMDeviceCollection outImmDeviceCollection;
            ImmDeviceEnumeratorTestFixture
                .EnumAudioEndpoints(DataFlow.Capture, Fundamental.Interface.Wasapi.Interop.DeviceState.All, out outImmDeviceCollection)
                .Returns(
                    param =>
                    {
                        param[2] = immDeviceCollection;
                        return HResult.S_OK;
                    });

            // The Device Collection should be called to find the device count
            int outCount;
            immDeviceCollection
                .GetCount(out outCount)
                .Returns(
                    param =>
                    {
                        param[0] = 0; // Count of 0
                        return HResult.S_OK;
                    });

            // -> ACT
            var result = fixture.GetDevices(DeviceType.Capature);

            // -> ASSERT
            Assert.IsEmpty(result);

        }

        [Test]
        public void CanGetAEmptyListOfAvailableRenderDevices()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            // The device returned from the COM interface
            var immDeviceCollection = Substitute.For<IMMDeviceCollection>();

            // Call to GetDefaultAudioEndpoint returns mock IMMDevice
            IMMDeviceCollection outImmDeviceCollection;
            ImmDeviceEnumeratorTestFixture
                .EnumAudioEndpoints(DataFlow.Render, Fundamental.Interface.Wasapi.Interop.DeviceState.Active, out outImmDeviceCollection)
                .Returns(
                    param =>
                    {
                        param[2] = immDeviceCollection;
                        return HResult.S_OK;
                    });

            // The Device Collection should be called to find the device count
            int outCount;
            immDeviceCollection
                .GetCount(out outCount)
                .Returns(
                    param =>
                    {
                        param[0] = 0; // Count of 0
                        return HResult.S_OK;
                    });

            // -> ACT
            var result = fixture.GetDevices(DeviceType.Render, DeviceState.Available);

            // -> ASSERT
            Assert.IsEmpty(result);

        }

        [Test]
        public void CanGetAEmptyListOfActiveOrUnpluggedCapatureDevices()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            // The device returned from the COM interface
            var immDeviceCollection = Substitute.For<IMMDeviceCollection>();


            // Call to GetDefaultAudioEndpoint returns mock IMMDevice

            var stateMask = Fundamental.Interface.Wasapi.Interop.DeviceState.Active |
                            Fundamental.Interface.Wasapi.Interop.DeviceState.Unplugged;

            IMMDeviceCollection outImmDeviceCollection;
            ImmDeviceEnumeratorTestFixture
                .EnumAudioEndpoints(DataFlow.Capture, stateMask, out outImmDeviceCollection)
                .Returns(
                    param =>
                    {
                        param[2] = immDeviceCollection;
                        return HResult.S_OK;
                    });


            // The Device Collection should be called to find the device count
            int outCount;
            immDeviceCollection
                .GetCount(out outCount)
                .Returns(
                    param =>
                    {
                        param[0] = 0; // Count of 0
                        return HResult.S_OK;
                    });

            // -> ACT
            var result = fixture.GetDevices(DeviceType.Capature, DeviceState.Available, DeviceState.Unplugged);

            // -> ASSERT
            Assert.IsEmpty(result);

        }



        [Test]
        public void CanGetAEmptyListOfActiveOrUnpluggedCapatureAndRenderDevices()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            // The device returned from the COM interface
            var immDeviceCollection = Substitute.For<IMMDeviceCollection>();


            // Call to GetDefaultAudioEndpoint returns mock IMMDevice
            var stateMask = Fundamental.Interface.Wasapi.Interop.DeviceState.Active |
                            Fundamental.Interface.Wasapi.Interop.DeviceState.Unplugged;

            var dataFlow = DataFlow.All;

            IMMDeviceCollection outImmDeviceCollection;
            ImmDeviceEnumeratorTestFixture
                .EnumAudioEndpoints(dataFlow, stateMask, out outImmDeviceCollection)
                .Returns(
                    param =>
                    {
                        param[2] = immDeviceCollection;
                        return HResult.S_OK;
                    });


            // The Device Collection should be called to find the device count
            int outCount;
            immDeviceCollection
                .GetCount(out outCount)
                .Returns(
                    param =>
                    {
                        param[0] = 0; // Count of 0
                        return HResult.S_OK;
                    });

            // -> ACT
            var result = fixture.GetDevices(new [] { DeviceType.Capature, DeviceType.Render}, 
                                            new [] { DeviceState.Available, DeviceState.Unplugged});

            // -> ASSERT
            Assert.IsEmpty(result);

        }


        [Test]
        public void CanGetListOfaSingleDevice()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            // The device returned from the COM interface
            var immDeviceCollection = Substitute.For<IMMDeviceCollection>();
            var immDevice1 = Substitute.For<IMMDevice>();


            // The Token for device 1
            var device1Token = new WasapiDeviceToken("E5E0D331-50A1-4020-AD36-491F554B92FF");

            // Call to GetDefaultAudioEndpoint returns mock IMMDevice
            IMMDeviceCollection outImmDeviceCollection;
            ImmDeviceEnumeratorTestFixture
                .EnumAudioEndpoints(Arg.Any<DataFlow>(), Arg.Any<Fundamental.Interface.Wasapi.Interop.DeviceState>(), out outImmDeviceCollection)
                .Returns(
                    param =>
                    {
                        param[2] = immDeviceCollection;
                        return HResult.S_OK;
                    });

            // The Device Collection should be called to find the device count
            int outCount;
            immDeviceCollection
                .GetCount(out outCount)
                .Returns(
                    param =>
                    {
                        param[0] = 1; // Count of 1
                        return HResult.S_OK;
                    });


            // Expect the first device to be yielded
            IMMDevice outImmDevice;
            immDeviceCollection
                .Item(0, out outImmDevice)
                .Returns(param =>
                {
                    param[1] = immDevice1;
                    return HResult.S_OK;
                });

            // Expect the token factory to receive the returned device 1
            WasapiDeviceTokenFactoryTestFixture
                .GetToken(immDevice1)
                .Returns(device1Token);

            // -> ACT
            var result = fixture.GetDevices().ToList();

            // -> ASSERT
            Assert.Contains(device1Token, result);
            Assert.AreEqual(1, result.Count);
        }


        [Test]
        public void CanGetListOfaTwoDevices()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            // The device returned from the COM interface
            var immDeviceCollection = Substitute.For<IMMDeviceCollection>();
            var immDevice1 = Substitute.For<IMMDevice>();
            var immDevice2 = Substitute.For<IMMDevice>();
     
            // The Token for device 1
            var device1Token = new WasapiDeviceToken("E5E0D331-50A1-4020-AD36-491F554B92FF");
            var device2Token = new WasapiDeviceToken("E5E0D331-50A1-4020-AD36-491F554B92FF");

            // Call to GetDefaultAudioEndpoint returns mock IMMDevice
            IMMDeviceCollection outImmDeviceCollection;
            ImmDeviceEnumeratorTestFixture
                .EnumAudioEndpoints(Arg.Any<DataFlow>(), Arg.Any<Fundamental.Interface.Wasapi.Interop.DeviceState>(), out outImmDeviceCollection)
                .Returns(
                    param =>
                    {
                        param[2] = immDeviceCollection;
                        return HResult.S_OK;
                    });

            // The Device Collection should be called to find the device count
            int outCount;
            immDeviceCollection
                .GetCount(out outCount)
                .Returns(
                    param =>
                    {
                        param[0] = 2; // Count of 2
                        return HResult.S_OK;
                    });


            // Expect the first device to be yielded
            IMMDevice outImmDevice1;
            immDeviceCollection
                .Item(0, out outImmDevice1)
                .Returns(param =>
                {
                    param[1] = immDevice1;
                    return HResult.S_OK;
                });

            // Expect the token factory to receive the returned device 1
            WasapiDeviceTokenFactoryTestFixture
                .GetToken(immDevice1)
                .Returns(device1Token);


            // Expect the second device to be yielded
            IMMDevice outImmDevice2;
            immDeviceCollection
                .Item(1, out outImmDevice2)
                .Returns(param =>
                {
                    param[1] = immDevice2;
                    return HResult.S_OK;
                });

            // Expect the token factory to receive the returned device 1
            WasapiDeviceTokenFactoryTestFixture
                .GetToken(immDevice2)
                .Returns(device1Token);


            // -> ACT
            var result = fixture.GetDevices().ToList();

            // -> ASSERT
            Assert.Contains(device1Token, result);
            Assert.Contains(device2Token, result);
            Assert.AreEqual(2, result.Count);
        }


    }
}
