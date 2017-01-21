using System;
using System.Runtime.InteropServices;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Win32;

namespace Fundamental.Interface.Wasapi
{
    [Guid("DDE8D8D4-055B-4FB1-BE59-6889F70EF9D1")]
    public class WasapiInterfaceNotifyClient :
       IMMNotificationClient,
       IDeviceStatusNotifier,
       IDefaultDeviceStatusNotifier,
       IDeviceAvailabilityNotifier
    {

        /// <summary>
        /// The wasapi device token factory
        /// </summary>
        private readonly IWasapiDeviceTokenFactory _wasapiDeviceTokenFactory;

        #region IDeviceStatusNotifier

        /// <summary>
        /// Occurs when a device status changes
        /// </summary>
        public event EventHandler<DeviceStatusChangedEvent> DeviceStatusChanged;

        #endregion

        #region IDefaultDeviceStatusNotifier

        /// <summary>
        /// Occurs when default device changed.
        /// </summary>
        public event EventHandler<DefaultDeviceChangedEventArgs> DefaultDeviceChanged;

        #endregion

        #region IDefaultDeviceStatusNotifier

        /// <summary>
        /// Occurs when a device added is to the system.
        /// </summary>
        public event EventHandler<DeviceAddedEventArgs> DeviceAdded;

        /// <summary>
        /// Occurs when a device removed is to the system.
        /// </summary>
        public event EventHandler<DeviceRemovedEventArgs> DeviceRemoved;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiInterfaceNotifyClient" /> class.
        /// </summary>
        /// <param name="wasapiDeviceTokenFactory">The WASAPI Device Token factory.</param>
        public WasapiInterfaceNotifyClient(IWasapiDeviceTokenFactory wasapiDeviceTokenFactory)
        {
            _wasapiDeviceTokenFactory = wasapiDeviceTokenFactory;
        }

        #region IMMNotificationClient

        HResult IMMNotificationClient.OnDeviceStateChanged(string deviceId, Interop.DeviceState deviceState)
        {
            //var deviceToken = _wasapiDeviceTokenFactory.GetToken(deviceId);
            //var deviceState = deviceState.
            ////   DeviceStateChanged?.Invoke(this, new DeviceStateChangedEventArgs(deviceId, deviceState));

            //DeviceStatusChanged?.Invoke(this, new DeviceStatusChangedEvent());
            return HResult.S_OK;
        }

        HResult IMMNotificationClient.OnDeviceAdded(string deviceId)
        {
            var deviceToken = _wasapiDeviceTokenFactory.GetToken(deviceId);
            //DeviceAdded?.Invoke(this, new DeviceAddedEventArgs(deviceId));
            return HResult.S_OK;
        }

        HResult IMMNotificationClient.OnDeviceRemoved(string deviceId)
        {
            var deviceToken = _wasapiDeviceTokenFactory.GetToken(deviceId);
            // DeviceRemoved?.Invoke(this, new DeviceRemovedEventArgs(deviceId));
            return HResult.S_OK;
        }

        HResult IMMNotificationClient.OnDefaultDeviceChanged(DataFlow dataFlow, Role role, string deviceId)
        {
            var deviceToken = _wasapiDeviceTokenFactory.GetToken(deviceId);
            // DefaultDeviceChanged?.Invoke(this, new DefaultDeviceChangedEventArgs(dataFlow, role, deviceId));
            return HResult.S_OK;
        }

        HResult IMMNotificationClient.OnPropertyValueChanged(string deviceId, PropertyKey key)
        {
            //var deviceToken = _wasapiDeviceTokenFactory.GetToken(deviceId);
            // PropertyValueChanged?.Invoke(this, new DevicePropertyChangedEventArgs(deviceId, key));
            return HResult.S_OK;
        }

        #endregion

    }
}
