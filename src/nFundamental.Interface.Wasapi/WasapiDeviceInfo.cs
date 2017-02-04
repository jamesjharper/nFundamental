using System;
using Fundamental.Interface.Wasapi.Internal;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Extentions;
using Fundamental.Interface.Wasapi.Win32;

namespace Fundamental.Interface.Wasapi
{
    public class WasapiDeviceInfo : IDeviceInfo
    {
        /// <summary>
        /// The WSAPI property name translator
        /// </summary>
        private readonly IWasapiPropertyNameTranslator _wasapiPropertyNameTranslator;

        /// <summary>
        /// The device property bag
        /// </summary>
        private WasapiPropertyBag _propertyBag;

        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiDeviceInfo" /> class.
        /// </summary>
        /// <param name="wasapiInterfaceNotifyClient">The WASAPI interface notify client.</param>
        /// <param name="wasapiPropertyNameTranslator">The WASAPI property name translator.</param>
        /// <param name="deviceToken">The device token.</param>
        public WasapiDeviceInfo(IWasapiInterfaceNotifyClient wasapiInterfaceNotifyClient,
                                IWasapiPropertyNameTranslator wasapiPropertyNameTranslator,
                                WasapiDeviceToken deviceToken)
        {
            _wasapiPropertyNameTranslator = wasapiPropertyNameTranslator;
            DeviceToken = deviceToken;

            wasapiInterfaceNotifyClient.DevicePropertyChanged += OnDevicePropertyChanged;
        }

        /// <summary>
        /// Occurs when a property value in the bag changes.
        /// </summary>
        public event EventHandler<DevicePropertyChangedEventArgs> PropertyValueChangedEvent;

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
        public WasapiPropertyBag Properties => _propertyBag ?? (_propertyBag = CreatePropertyBag());

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
        IPropertyBag IDeviceInfo.Properties => Properties;

        // Private Methods

        private WasapiPropertyBag CreatePropertyBag()
        {
            IPropertyStore propertyStore;
            DeviceToken.MmDevice.OpenPropertyStore(StorageAccess.Read, out propertyStore);
            return new WasapiPropertyBag(propertyStore, _wasapiPropertyNameTranslator);
        }

        private DeviceState GetState()
        {
            Interop.DeviceState deviceState;
            DeviceToken.MmDevice.GetState(out deviceState).ThrowIfFailed();
            return deviceState.ConvertToFundamentalDeviceState();
        }

        private void OnDevicePropertyChanged(object sender, Internal.WasapiDevicePropertyChangedEventArgs args)
        {

            // only do the work is anyone is listing
            var handler = PropertyValueChangedEvent;
            if (handler == null)
                return;

            // We will receive events for all devices, we want to ignore all but those destined for this device
            if (!Equals(args.DeviceToken, DeviceToken))
                return;

            var propertyName = _wasapiPropertyNameTranslator.ResolvePropertyKey(args.PropertyKey);
            handler.Invoke(this, new DevicePropertyChangedEventArgs(this, propertyName.Id));
        }
    }
}
