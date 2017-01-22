using System;
using Fundamental.Interface.Wasapi.Interop;

namespace Fundamental.Interface.Wasapi
{
    public class WasapiDeviceInfo : IDeviceInfo
    {
        /// <summary>
        /// The WASAPI interface notify client
        /// </summary>
        private IWasapiInterfaceNotifyClient _wasapiInterfaceNotifyClient;

        /// <summary>
        /// The device
        /// </summary>
        private readonly IMMDevice _immDevice;

        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiDeviceInfo"/> class.
        /// </summary>
        /// <param name="wasapiInterfaceNotifyClient">The WASAPI interface notify client.</param>
        /// <param name="deviceToken">The device token.</param>
        /// <param name="immDevice">The device.</param>
        public WasapiDeviceInfo(IWasapiInterfaceNotifyClient wasapiInterfaceNotifyClient,
                                WasapiDeviceToken deviceToken,
                                IMMDevice immDevice )
        {
            _wasapiInterfaceNotifyClient = wasapiInterfaceNotifyClient;
            _immDevice = immDevice;
            DeviceToken = deviceToken;
        }

        public event EventHandler<Interface.DevicePropertyChangedEventArgs> PropertyValueChangedEvent;

        IDeviceToken IDeviceInfo.DeviceToken => DeviceToken;

        public WasapiDeviceToken DeviceToken { get; }
        public IDevicePropertyBag Properties { get; }
    }
}
