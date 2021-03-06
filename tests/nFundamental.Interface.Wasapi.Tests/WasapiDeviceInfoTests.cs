﻿using System;
using Fundamental.Core;
using Fundamental.Wave.Format;
using Fundamental.Interface.Wasapi.Internal;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Win32;
using NSubstitute;
using NUnit.Framework;

namespace Fundamental.Interface.Wasapi.Tests
{
    [TestFixture]
    public class WasapiDeviceInfoTests
    {

        public IWasapiInterfaceNotifyClient WasapiInterfaceNotifyClient { get; set; }

        public IMMDevice ImmDevice { get; set; }

        public IWasapiPropertyNameTranslator WasapiPropertyNameTranslatorTestFixture { get; set; }

        public IAudioFormatConverter<WaveFormat> AudioFormatConverterWaveFormat { get; set; }

        public WasapiDeviceToken WasapiDeviceToken { get; set; }


        [SetUp]
        public void SetUp()
        {
            ImmDevice = Substitute.For<IMMDevice>();
            WasapiDeviceToken = new WasapiDeviceToken("A63439A9-5928-4BEC-99EC-F39B0414278B", ImmDevice);
            WasapiInterfaceNotifyClient = Substitute.For<IWasapiInterfaceNotifyClient>();
            WasapiPropertyNameTranslatorTestFixture = Substitute.For<IWasapiPropertyNameTranslator>();
            AudioFormatConverterWaveFormat = Substitute.For<IAudioFormatConverter<WaveFormat>>();
        }

        private WasapiDeviceInfo GetTestFixture()
            => new WasapiDeviceInfo(WasapiInterfaceNotifyClient, WasapiPropertyNameTranslatorTestFixture, AudioFormatConverterWaveFormat, WasapiDeviceToken);


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


        [Test]
        public void CanRaisePropertyChangedEventForOwnDeivce()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();

            var keyRaisedInEvent = new PropertyKey(new Guid("A838D5FC-6880-4F15-BEFF-FB760CEE3857"), 2);
            var tokenRaisedInEvent = WasapiDeviceToken;

            var returnedPropertyBagKey = new WasapiPropertyBagKey("key9", "name1", "category2", 9, new Guid("AAEE33E2-48BA-44F3-8923-F7C3DAA8B17A"), /* is known */ true); ;

            bool eventWasCalled = false;
            fixture.PropertyValueChangedEvent += (sender, args) =>
                {
                    eventWasCalled = true;
                    Assert.AreEqual("key9", args.PropertyKey);
                    Assert.AreEqual(fixture, args.DeviceInfo);
                };


            WasapiPropertyNameTranslatorTestFixture
                .ResolvePropertyKey(keyRaisedInEvent)
                .Returns(returnedPropertyBagKey);

            // -> ACT
            WasapiInterfaceNotifyClient.DevicePropertyChanged 
                += Raise.Event< EventHandler<WasapiDevicePropertyChangedEventArgs>>(this, new WasapiDevicePropertyChangedEventArgs(tokenRaisedInEvent, keyRaisedInEvent));

            // -> ASSERT
            Assert.That(eventWasCalled);
        }

        [Test]
        public void ShouldNotRaisePropertyChangedEventForOtherDevice()
        {
            // -> ARRANGE:
            var fixture = GetTestFixture();
            
            var keyRaisedInEvent = new PropertyKey(new Guid("A838D5FC-6880-4F15-BEFF-FB760CEE3857"), 2);
            var tokenRaisedInEvent = new WasapiDeviceToken("5ED59CEA-0E93-4731-A32D-5E43F77D6E30", Substitute.For<IMMDevice>());

            var wasNotCalled = true;
            fixture.PropertyValueChangedEvent += (sender, args) =>
                {
                    wasNotCalled = false;
                };

            // -> ACT
            WasapiInterfaceNotifyClient.DevicePropertyChanged
                += Raise.Event<EventHandler<WasapiDevicePropertyChangedEventArgs>>(this, new WasapiDevicePropertyChangedEventArgs(tokenRaisedInEvent, keyRaisedInEvent));

            // -> ASSERT
            Assert.That(wasNotCalled);
        }
    }
}
