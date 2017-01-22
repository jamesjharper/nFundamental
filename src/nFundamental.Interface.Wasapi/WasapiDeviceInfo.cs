using System;
using Fundamental.Interface.Wasapi.Extentions;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Win32;

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
        /// The device property bag
        /// </summary>
        private WasapiDevicePropertyBag _devicePropertyBag;

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

        /// <summary>
        /// Occurs when a property value in the bag changes.
        /// </summary>
        public event EventHandler<Interface.DevicePropertyChangedEventArgs> PropertyValueChangedEvent;

        /// <summary>
        /// Gets the device handle.
        /// </summary>
        /// <value>
        /// The device handle.
        /// </value>
        public WasapiDeviceToken DeviceToken { get; }

        /// <summary>
        /// Gets the state of the device.
        /// </summary>
        /// <value>
        /// The state of the device.
        /// </value>
        public DeviceState DeviceState => GetState();

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        public WasapiDevicePropertyBag Properties => _devicePropertyBag ?? (_devicePropertyBag = CreatePropertyBag());

        /// <summary>
        /// Gets the device handle.
        /// </summary>
        /// <value>
        /// The device handle.
        /// </value>
        IDeviceToken IDeviceInfo.DeviceToken => DeviceToken;

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        IDevicePropertyBag IDeviceInfo.Properties => Properties;

        // Private Methods

        /// <summary>
        /// Creates the property bag.
        /// </summary>
        /// <returns></returns>
        private WasapiDevicePropertyBag CreatePropertyBag()
        {
            IPropertyStore propertyStore;
            _immDevice.OpenPropertyStore(StorageAccess.Read, out propertyStore);

            return new WasapiDevicePropertyBag(propertyStore);
        }

        private DeviceState GetState()
        {
            Interop.DeviceState deviceState;
            _immDevice.GetState(out deviceState).ThrowIfFailed();
            return deviceState.ConvertToFundamentalDeviceState();
        }
    }
}
