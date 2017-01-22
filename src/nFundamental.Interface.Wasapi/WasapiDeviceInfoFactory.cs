using System;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Win32;

namespace Fundamental.Interface.Wasapi
{
    public class WasapiDeviceInfoFactory : IDeviceInfoFactory
    {
        /// <summary>
        /// The WASAPI interface notify client
        /// </summary>
        private readonly IWasapiInterfaceNotifyClient _wasapiInterfaceNotifyClient;

        /// <summary>
        /// The device enumerator
        /// </summary>
        private readonly IMMDeviceEnumerator _deviceEnumerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiDeviceInfoFactory"/> class.
        /// </summary>
        /// <param name="wasapiInterfaceNotifyClient">The WASAPI interface notify client.</param>
        /// <param name="deviceEnumerator"></param>
        public WasapiDeviceInfoFactory(IWasapiInterfaceNotifyClient wasapiInterfaceNotifyClient,
                                       IMMDeviceEnumerator deviceEnumerator)
        {
            _wasapiInterfaceNotifyClient = wasapiInterfaceNotifyClient;
            _deviceEnumerator = deviceEnumerator;
        }

        /// <summary>
        /// Gets a information device instance.
        /// </summary>
        /// <param name="deviceToken">The device token.</param>
        /// <returns></returns>
        public WasapiDeviceInfo GetInfoDevice(WasapiDeviceToken deviceToken)
        {
            IMMDevice mmdevice;
           _deviceEnumerator.GetDevice(deviceToken.Id, out mmdevice).ThrowIfFailed();
            return new WasapiDeviceInfo(_wasapiInterfaceNotifyClient, deviceToken, mmdevice);
        }

        /// <summary>
        /// Gets the information device.
        /// </summary>
        /// <param name="deviceToken">The device handle.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">WasapiDeviceInfoFactory can only except tokens of type WasapiDeviceToken</exception>
        IDeviceInfo IDeviceInfoFactory.GetInfoDevice(IDeviceToken deviceToken)
        {
            if(!(deviceToken is WasapiDeviceToken))
                throw new InvalidOperationException("WasapiDeviceInfoFactory can only except tokens of type WasapiDeviceToken");
            return GetInfoDevice((WasapiDeviceToken)deviceToken);
        }
    }
}
