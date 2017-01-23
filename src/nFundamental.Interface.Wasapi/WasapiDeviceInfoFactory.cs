using System;
using Fundamental.Interface.Wasapi.Internal;
using Fundamental.Interface.Wasapi.Interop;

namespace Fundamental.Interface.Wasapi
{
    public class WasapiDeviceInfoFactory : IDeviceInfoFactory
    {
        /// <summary>
        /// The WASAPI interface notify client
        /// </summary>
        private readonly IWasapiInterfaceNotifyClient _wasapiInterfaceNotifyClient;

        /// <summary>
        /// The WASAPI property name translator
        /// </summary>
        private readonly IWasapiPropertyNameTranslator _wasapiPropertyNameTranslator;

        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiDeviceInfoFactory" /> class.
        /// </summary>
        /// <param name="wasapiInterfaceNotifyClient">The WASAPI interface notify client.</param>
        /// <param name="wasapiPropertyNameTranslator">The WASAPI property name translator.</param>
        public WasapiDeviceInfoFactory(IWasapiInterfaceNotifyClient wasapiInterfaceNotifyClient,
                                       IWasapiPropertyNameTranslator wasapiPropertyNameTranslator)
        {
            _wasapiInterfaceNotifyClient = wasapiInterfaceNotifyClient;
            _wasapiPropertyNameTranslator = wasapiPropertyNameTranslator;
        }

        /// <summary>
        /// Gets a information device instance.
        /// </summary>
        /// <param name="deviceToken">The device token.</param>
        /// <returns></returns>
        public WasapiDeviceInfo GetInfoDevice(WasapiDeviceToken deviceToken)
        {
            return new WasapiDeviceInfo(_wasapiInterfaceNotifyClient, _wasapiPropertyNameTranslator, deviceToken);
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
                throw new InvalidOperationException($"{nameof(WasapiDeviceInfoFactory)} can only except tokens of type {nameof(WasapiDeviceToken)}");
            return GetInfoDevice((WasapiDeviceToken)deviceToken);
        }
    }
}
